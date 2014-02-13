using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using Vertigo.SlideShow.Controls;
using Vertigo.SlideShow.Transitions;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Preloads slides into cache.
	/// </summary>
	public class DataPreloader
	{
		public bool PreloadAll { get; private set; }
		public bool PreloadCurrentAlbum { get; private set; }
		public int PreloadAhead { get; private set; }
		public static DownloadProgressIndicator ProgressIndicator { get; private set; }

		// State -1 indicates that the slideshow is not waiting for a slide to load
		private int toSlideIndex = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataPreloader"/> class.
		/// </summary>
		public DataPreloader()
		{
			PreloadAhead = Configuration.Options.Preloader.PreloadAhead;
		}

		/// <summary>
		/// Preloads slides based on configuration settings passed to the constructor.
		/// </summary>
		public void PreloadSlides()
		{
			AlbumHandler album = DataHandler.Albums[DataHandler.CurrentAlbumIndex];
			int toSlideIndex = album.TransitionManager.ToSlideIndex;

			for (int i = 0; i <= PreloadAhead; i++)
			{
				int slideIndex = TransitionManager.Mod(toSlideIndex + i, DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides.Length);
				if (!album.Slides[slideIndex].IsPreloaded)
				{
					Preload(album.Slides[slideIndex]);
				}
			}
		}

		/// <summary>
		/// Preloads the specified slide.
		/// </summary>
		/// <param name="slide">The slide.</param>
		private void Preload(SlideHandler slide)
		{
			PreloadClient client = new PreloadClient();
			client.PreloadProgressChanged += client_PreloadProgressChanged;
			client.PreloadCompleted += client_PreloadCompleted;
			client.Preload(slide);
		}

		/// <summary>
		/// Handles the PreloadProgressChanged event of the client control and updates the progress indicator.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Vertigo.SlideShow.Preloader.SlidePreloadProgressChangedEventArgs"/> instance containing the event data.</param>
		private void client_PreloadProgressChanged(object sender, SlidePreloadProgressChangedEventArgs e)
		{
			if (!e.PreloadSlide.IsPreloaded && toSlideIndex != -1)
			{
				if (e.PreloadSlide == DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides[toSlideIndex] && ProgressIndicator != null)
				{
					ProgressIndicator.UpdateProgress(e.Percent);
				}
			}
		}

		/// <summary>
		/// Handles the PreloadCompleted event of the client control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Vertigo.SlideShow.Preloader.SlidePreloadCompletedEventArgs"/> instance containing the event data.</param>
		private void client_PreloadCompleted(object sender, SlidePreloadCompletedEventArgs e)
		{
			if (!e.PreloadSlide.IsPreloaded)
			{
				if (toSlideIndex != -1)
				{
					if (e.PreloadSlide == DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides[toSlideIndex])
					{
						toSlideIndex = -1;
					}
				}

				e.PreloadSlide.TriggerToSlideFinishedLoadingEvent();
				e.PreloadSlide.IsPreloaded = true;
			}
		}

		/// <summary>
		/// Preloads the slide specified in the constructor. Supplies PreloadCompleted and PreloadProgressChanged events.
		/// </summary>
		public class PreloadClient
		{
			public SlideHandler PreloadSlide { get; private set; }
			public event PreloadProgressChangedHandler PreloadProgressChanged;
			public event PreloadCompletedHandler PreloadCompleted;

			public void Preload(SlideHandler slide)
			{
				PreloadSlide = slide;

				WebClient client = new WebClient();
				client.DownloadProgressChanged += client_DownloadProgressChanged;
				client.OpenReadCompleted += client_OpenReadCompleted;
				client.OpenReadAsync(slide.Source);
			}

			private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
			{
				PreloadProgressChanged(this, new SlidePreloadProgressChangedEventArgs(e.ProgressPercentage, PreloadSlide));
			}

			private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
			{
				if (e.Error == null)
				{
					ReadPeopleTagsFromXml(GetXmpXmlDocFromImage(e.Result));
				}

				PreloadCompleted(this, new SlidePreloadCompletedEventArgs(e.Error, PreloadSlide));
			}

			private string GetXmpXmlDocFromImage(Stream fileStream)
			{
				char contents;
				string beginCapture = "<MPRI:Regions";
				string endCapture = "</MPRI:Regions>";
				string collection = string.Empty;
				bool collecting = false;
				bool matching = false;
				int collectionCount = 0;

				using (StreamReader sr = new StreamReader(fileStream))
				{
					while (!sr.EndOfStream)
					{
						contents = (char)sr.Read();

						if (!matching && !collecting && contents == '<')
						{
							matching = true;
						}

						if (matching)
						{
							collection += contents;

							if (collection.Contains(beginCapture))
							{
								//found the begin element we can stop matching and start collecting
								matching = false;
								collecting = true;
							}
							else if (contents == beginCapture[collectionCount++])
							{
								//we are still looking, but on track to start collecting
								continue;
							}
							else
							{
								//false start reset everything
								collection = string.Empty;
								matching = false;
								collecting = false;
								collectionCount = 0;
							}

						}
						else if (collecting)
						{
							collection += contents;

							if (collection.Contains(endCapture))
							{
								//we are finished found the end of the XMP data
								break;
							}
						}
					}

					if (!String.IsNullOrEmpty(collection))
					{
						fileStream.Seek(0, SeekOrigin.Begin);
						sr.DiscardBufferedData();
						sr.BaseStream.Seek(0, SeekOrigin.Begin);
						sr.BaseStream.Position = 0;
						PreloadSlide.ImageSize = JPEGUtil.getJPEGDimension(fileStream);
					}
				}

				return collection;
			}

			/// <summary>
			/// Parses an image's XMP XML to pull out the PeopleTags 
			/// </summary>
			/// <param name="xmpXml"></param>
			private void ReadPeopleTagsFromXml(string xmpXml)
			{
				if (!String.IsNullOrEmpty(xmpXml))
				{
					XDocument doc = XDocument.Load(new System.IO.StringReader(xmpXml));
					XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
					XNamespace MPReg = "http://ns.microsoft.com/photo/1.2/t/Region#";

					foreach (XElement element in doc.Descendants(rdf + "Description"))
					{
						PeopleTagDefinition peopleTag = new PeopleTagDefinition();
						foreach (XElement rectangleElement in element.Descendants(MPReg + "Rectangle"))
						{
							string[] rectangleElements = rectangleElement.Value.Split(',');
							peopleTag.XPositionPercentage = Convert.ToDouble(rectangleElements[0]);
							peopleTag.YPositionPercentage = Convert.ToDouble(rectangleElements[1]);
							peopleTag.WidthPercentage = Convert.ToDouble(rectangleElements[2]);
							peopleTag.HeightPercentage = Convert.ToDouble(rectangleElements[3]);
						}
						foreach (XElement rectangleElement in element.Descendants(MPReg + "PersonDisplayName"))
							peopleTag.DisplayName = rectangleElement.Value;
						PreloadSlide.PeopleTagDefinitions.Add(peopleTag);
					}
				}
			}	
		}

		public delegate void PreloadProgressChangedHandler(object sender, SlidePreloadProgressChangedEventArgs e);

		/// <summary>
		/// Event arguments passed when a change in download progress has occured.
		/// </summary>
		public class SlidePreloadProgressChangedEventArgs : EventArgs
		{
			public int Percent { get; private set; }
			public SlideHandler PreloadSlide { get; private set; }

			public SlidePreloadProgressChangedEventArgs(int percent, SlideHandler preloadSlide)
			{
				Percent = percent;
				PreloadSlide = preloadSlide;
			}
		}

		public delegate void PreloadCompletedHandler(object sender, SlidePreloadCompletedEventArgs e);

		/// <summary>
		/// Event arguments passed when a download has finished.
		/// </summary>
		public class SlidePreloadCompletedEventArgs : EventArgs
		{
			public Exception Error { get; private set; }
			public SlideHandler PreloadSlide { get; private set; }
			public SlidePreloadCompletedEventArgs(Exception error, SlideHandler preloadSlide)
			{
				Error = error;
				PreloadSlide = preloadSlide;
			}
		}

		/// <summary>
		/// Displays the progress indicator.
		/// </summary>
		/// <param name="index">The index of the slide the indicator is to track.</param>
		public void DisplayProgressIndicator(int index)
		{
			toSlideIndex = index;

			// Look in configuration for indicator type, else default to bar progress indicator
			ProgressIndicator = DownloadProgressIndicator.Create(
				Configuration.Options.LoadingProgressIndicator.Type);

			DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.MediaRoot.Children.Add(ProgressIndicator);
		}

		/// <summary>
		/// Hides the progress indicator.
		/// </summary>
		public void HideProgressIndicator()
		{
			UIElementCollection children = DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.MediaRoot.Children;
			if (children.Contains(ProgressIndicator))
			{
				children.Remove(ProgressIndicator);
			}
		}
	}

	public class JPEGUtil
	{
		/// <summary>
		/// Based: http://forum.java.sun.com/thread.jspa?threadID=554733&messageID=2717319 and http://www.obrador.com/essentialjpeg/headerinfo.htm
		/// </summary>
		/// <param name="fileStream"></param>
		/// <returns></returns>
		public static Size getJPEGDimension(Stream fileStream)
		{
			byte[] buf = new byte[64 * 1024];

			// check for SOI marker
			if (0xFF != fileStream.ReadByte() || 0xD8 != fileStream.ReadByte())
			{
				return new Size(0, 0);
			}

			while (0xFF == fileStream.ReadByte())
			{
				int marker = fileStream.ReadByte();
				int len = (fileStream.ReadByte() << 8 | fileStream.ReadByte()) - 2;
				fileStream.Read(buf, 0, len);

				// Start of frame marker (FFC0)
				if (   /* baseline */0xC0 == marker
					|| /* non-baseline*/ 0xC1 == marker
					|| /* progressive Huffman */ 0xC2 == marker
					|| /* arithmetic coding */ 0xC9 == marker)
				{
					int height = buf[1] << 8 | buf[2];
					int width = buf[3] << 8 | buf[4];

					return new Size(width, height);
				}
				// JFIF marker (FFE0)
				else if (0xE0 == marker)
				{
					if (/* Units in pixel */ 0 == buf[7])
					{
						int width = buf[8] << 8 | buf[9];
						int height = buf[10] << 8 | buf[11];

						return new Size(width, height);
					}
				}
			}

			return new Size(0, 0);
		}
	}
}