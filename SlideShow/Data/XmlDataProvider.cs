using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace Vertigo.SlideShow
{
    /// <summary>
    /// Represents the XmlDataProvider class
    /// </summary>
	public class XmlDataProvider : IDataProvider
	{
        /// <summary>
        /// Occurs when [data finished loading].
        /// </summary>
		public event EventHandler DataFinishedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataProvider"/> class.
        /// </summary>
        /// <param name="initParams">The init params.</param>
		public XmlDataProvider(Dictionary<string, Dictionary<string, string>> initParams)
		{
			string xmlPath = initParams.ContainsKey("DataProvider") ?
									initParams["DataProvider"].ContainsKey("Path") ?
											initParams["DataProvider"]["Path"] :
											string.Empty :
											string.Empty;

			if (string.IsNullOrEmpty(xmlPath))
			{
				xmlPath = Configuration.Options.DataProvider.Uri ?? "../data.xml";
			}

			WebClient client = new WebClient();
			client.DownloadStringCompleted += client_DownloadStringCompleted;
			client.DownloadStringAsync(new Uri(xmlPath, UriKind.RelativeOrAbsolute));
		}

		/// <summary>
		/// Handles the DownloadStringCompleted event of the client control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			XmlReader reader = XmlReader.Create(new StringReader(e.Result));

			while (reader.Read())
			{
				if (reader.IsStartElement() && reader.Name == "data")
				{
					Data.StartAlbumIndex = Convert.ToInt32(reader.GetAttribute("startalbumindex"), CultureInfo.InvariantCulture);
					Data.Albums = ParseAlbums(reader.ReadSubtree(), reader.GetAttribute("transition"));
				}
			}

			DataFinishedLoading(this, EventArgs.Empty);
		}

		/// <summary>
		/// Parses the albums.
		/// </summary>
		/// <param name="reader">an xmlReader</param>
		/// <param name="dataTransition">a string representing the transition</param>
		/// <returns>an album array</returns>
		private Album[] ParseAlbums(XmlReader reader, string dataTransition)
		{
			List<Album> albums = new List<Album>();

			while (reader.Read())
			{
				if (reader.IsStartElement() && reader.Name == "album")
				{
					albums.Add(new Album()
					{
						Title = reader.GetAttribute("title"),
						Description = reader.GetAttribute("description"),
						Thumbnail = GetSource(reader),
						Slides = ParseSlides(reader.ReadSubtree(), reader.GetAttribute("transition") ?? dataTransition),
					});
				}
			}

			return albums.ToArray();
		}

		/// <summary>
		/// parses the slides
		/// </summary>
		/// <param name="reader">the xmlreader</param>
		/// <param name="inheritTransition">a string</param>
		/// <returns>a Slide array</returns>
		private Slide[] ParseSlides(XmlReader reader, string inheritTransition)
		{
			List<Slide> slides = new List<Slide>();

			while (reader.Read())
			{
				if (reader.IsStartElement() && reader.Name == "slide")
				{
					slides.Add(new Slide()
					{
						Title = reader.GetAttribute("title"),
						Description = reader.GetAttribute("description"),
						Source = GetSource(reader),
						Preview =
							reader.GetAttribute("preview") != null ?
							new Uri(reader.GetAttribute("preview"), UriKind.RelativeOrAbsolute) :
							null,
						Thumbnail =
							reader.GetAttribute("thumbnail") != null ?
							new Uri(reader.GetAttribute("thumbnail"), UriKind.RelativeOrAbsolute) :
							null,
						Link =
							reader.GetAttribute("link") != null ?
							new Uri(reader.GetAttribute("link"), UriKind.RelativeOrAbsolute) :
							null,
						Transition =
							(reader.GetAttribute("transition") != null ?
							reader.GetAttribute("transition") :
							inheritTransition) ?? Data.FALLBACK_TRANSITION
					});
				}
			}

			return slides.ToArray();
		}

		/// <summary>
		/// For backwards compatability with Slide.Show 1 data files the image property as 
		/// well as the source property are supported. Source takes precedence.
		/// </summary>
		/// <param name="reader">XmlReader containing the data elements</param>
		/// <returns>The Uri of the source image or video.</returns>
		private Uri GetSource(XmlReader reader)
		{
			// source and image is not supported
			if (reader.GetAttribute("source") != null && reader.GetAttribute("image") != null)
			{
				return null;
			}

			if (reader.GetAttribute("source") != null)
			{
				return new Uri(reader.GetAttribute("source"), UriKind.RelativeOrAbsolute);
			}

			if (reader.GetAttribute("image") != null)
			{
				return new Uri(reader.GetAttribute("image"), UriKind.RelativeOrAbsolute);
			}
						
			return null;
		}
	}
}