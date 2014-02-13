using Vertigo.SlideShow.Controls;
using Vertigo.SlideShow.Transitions;
using System;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Represents a Navigation
	/// </summary>
	public static class Navigation
	{
		/// <summary>
		/// Gets a value indicating whether this instance is embed view.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is embed view; otherwise, <c>false</c>.
		/// </value>
		public static bool IsEmbedView { get; private set; }

		/// <summary>
		/// Starts the slide show.
		/// </summary>
		public static void StartSlideShow()
		{
			//check to see if default album and slideindex were specified on querystring
            if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("album"))
            {
                string album = System.Windows.Browser.HtmlPage.Document.QueryString["album"];
				if (SelectDefaultAlbum(album))
				{
					if (System.Windows.Browser.HtmlPage.Document.QueryString.ContainsKey("slideIndex"))
					{
						string slideIndex = System.Windows.Browser.HtmlPage.Document.QueryString["slideIndex"];
						SelectDefaultSlide(slideIndex);
					}
				}
				else
					SelectAlbum(DataHandler.CurrentAlbumIndex);
			}
			//check to see if default album and slideindex were specified in initparams (note that querystring takes precedence)
			else if (ConfigurationProvider.initParams.ContainsKey("DataProvider") && ConfigurationProvider.initParams["DataProvider"].ContainsKey("Album") && !String.IsNullOrEmpty(ConfigurationProvider.initParams["DataProvider"]["Album"]))
			{
				string album = ConfigurationProvider.initParams["DataProvider"]["Album"];
				if (SelectDefaultAlbum(album))
				{
					if (ConfigurationProvider.initParams["DataProvider"].ContainsKey("SlideIndex") && !String.IsNullOrEmpty(ConfigurationProvider.initParams["DataProvider"]["SlideIndex"]))
					{
						string slideIndex = ConfigurationProvider.initParams["DataProvider"]["SlideIndex"];
						SelectDefaultSlide(slideIndex);
					}
				}
				else
					SelectAlbum(DataHandler.CurrentAlbumIndex);
			}
			else
				SelectAlbum(DataHandler.CurrentAlbumIndex);

			if (Configuration.Options.General.StartInAlbumView)
				ToggleAlbumViewOn();
			if (!Configuration.Options.General.AutoStart)
				Pause();
		}

		private static bool SelectDefaultAlbum(string album)
		{
			bool albumFound = false;
			if (!String.IsNullOrEmpty(album))
			{
				int i;
				if (int.TryParse(album, out i)) //album index provided?
				{
					SelectAlbum(i);
					albumFound = true;
				}
				else
				{
					//album by title?
					for (int a = 0; a < DataHandler.Albums.Length; a++)
					{
						if (DataHandler.Albums[a].Title == album)
						{
							SelectAlbum(a);
							albumFound = true;
						}
					}
				}
			}
			return albumFound;
		}


		private static bool SelectDefaultSlide(string slideIndex)
		{
			bool slideFound = false;
			if (!String.IsNullOrEmpty(slideIndex))
			{
				int i;
				if (int.TryParse(slideIndex, out i))
				{
					SkipToSlide(i, null);
					slideFound = true;
				}
			}
			return slideFound;
		}

		/// <summary>
		/// Resume slideshow playback. If currently playing, does nothing.
		/// </summary>
		public static void Play()
		{
			if (DataHandler.Albums != null)
			{
				if (DataHandler.Albums[DataHandler.CurrentAlbumIndex] != null)
				{
					if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager != null)
					{
						DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.Play();
					}
				}
			}
		}

		/// <summary>
		/// Pauses slideshow playback. If currently paused, does nothing.
		/// </summary>
		public static void Pause()
		{
			if (DataHandler.Albums != null)
			{
				if (DataHandler.Albums[DataHandler.CurrentAlbumIndex] != null)
				{
					if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager != null)
					{
						DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.Pause();
					}
				}
			}
		}

		/// <summary>
		/// Skips to next slide in slideshow.
		/// </summary>
		/// <param name="transition">Overrides the transition specified by the slide. To not override the the slide's transition, pass null.</param>
		public static void SkipToNextSlide(string transition)
		{
			if (DataHandler.Albums != null)
			{
				if (DataHandler.Albums[DataHandler.CurrentAlbumIndex] != null)
				{
					if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager != null)
					{
						ToggleAlbumViewOff();
						DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.SelectNextSlide(transition);
					}
				}
			}
		}

		/// <summary>
		/// Skips to previous slide.
		/// </summary>
		/// <param name="transition">Overrides the transition specified by the slide. To not override the the slide's transition, pass null.</param>
		public static void SkipToPreviousSlide(string transition)
		{
			if (DataHandler.Albums != null)
			{
				if (DataHandler.Albums[DataHandler.CurrentAlbumIndex] != null)
				{
					if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager != null)
					{
						ToggleAlbumViewOff();
						DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.SelectPreviousSlide(transition);
					}
				}
			}
		}

		/// <summary>
		/// Skips to specified slide.
		/// </summary>
		/// <param name="index">The index of the slide.</param>
		/// <param name="transition">Overrides the transition specified by the slide. To not override the the slide's transition, pass null.</param>
		public static void SkipToSlide(int index, string transition)
		{
			if (DataHandler.Albums != null)
			{
				if (DataHandler.Albums[DataHandler.CurrentAlbumIndex] != null)
				{
					if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager != null)
					{
						ToggleAlbumViewOff();
						DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.SelectSlide(index, transition);
					}
				}
			}

			Page page = App.Current.RootVisual as Page;
			if (page != null)
			{
				if (page.navigationTray != null)
				{
					ToggleAlbumViewOff();
				}
			}
		}

		/// <summary>
		/// Toggles album view on.
		/// </summary>
		public static void ToggleAlbumViewOn()
		{
			if (!NavigationTray.IsAlbumView)
			{
				ToggleAlbumView();
			}
		}

		/// <summary>
		/// Toggles album view off.
		/// </summary>
		public static void ToggleAlbumViewOff()
		{
			if (NavigationTray.IsAlbumView)
			{
				ToggleAlbumView();
			}
		}

		/// <summary>
		/// Toggles album view.
		/// </summary>
		public static void ToggleAlbumView()
		{
			Page page = App.Current.RootVisual as Page;
			if (page != null)
			{
				if (NavigationTray.IsAlbumView)
				{
					page.navigationTray.SaveHyperlinkButtonElement.IsEnabled = Configuration.Options.ToggleSaveButton.Enabled;
					Play();
				}
				else
				{
					page.navigationTray.SaveHyperlinkButtonElement.IsEnabled = false;
					Pause();
				}

				page.navigationTray.ToggleAlbumView();
			}
		}

		/// <summary>
		/// Toggles embed view.
		/// </summary>
		public static void ToggleEmbedView()
		{
			Page page = App.Current.RootVisual as Page;
			if (page != null)
			{
				if (!IsEmbedView)
				{
					page.EmbedViewerElement.Display();
					page.slideDescription.Opacity = 0;
					KeyboardSupport.ListenToInput = false;
					Pause();
				}
				else
				{
					page.EmbedViewerElement.Hide();
					page.slideDescription.Opacity = 1;
					KeyboardSupport.ListenToInput = true;
					Play();
				}

				IsEmbedView = !IsEmbedView;
			}
		}

		/// <summary>
		/// Toggles between Play/Pause states.
		/// </summary>
		public static void TogglePause()
		{
			if (DataHandler.Albums != null)
			{
				if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager != null)
				{
					if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.IsPaused)
					{
						Play();
					}
					else
					{
						Pause();
					}
				}
			}
		}

		/// <summary>
		/// Selects album of preceeding index.
		/// </summary>
		public static void SelectPreviousAlbum()
		{
			SelectAlbum(DataHandler.CurrentAlbumIndex - 1);
		}

		/// <summary>
		/// Selects album of following index.
		/// </summary>
		public static void SelectNextAlbum()
		{
			SelectAlbum(DataHandler.CurrentAlbumIndex + 1);
		}

		/// <summary>
		/// Selects the specified album.
		/// </summary>
		/// <param name="albumIndex">The index of the album to select.</param>
		public static void SelectAlbum(int albumIndex)
		{
			int previousIndex = DataHandler.CurrentAlbumIndex;
			DataHandler.CurrentAlbumIndex = TransitionManager.Mod(albumIndex, DataHandler.Albums.Length);

			if (DataHandler.Albums[previousIndex].TransitionManager != null)
			{
				DataHandler.Albums[previousIndex].TransitionManager.Hide();
			}

			if (DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager == null)
			{
				DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager = new TransitionManager();
			}
			
			DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.Show();
		
			Page page = App.Current.RootVisual as Page;
			if (page != null)
			{
				if (page.navigationTray != null)
				{
					page.navigationTray.DisplayThumbnailViewer(
						DataHandler.Albums[DataHandler.CurrentAlbumIndex].ThumbnailViewer);
					
					ToggleAlbumViewOff();
				}
			}

			DataHandler.Albums[DataHandler.CurrentAlbumIndex].ThumbnailViewer.ThumbnailClicked += delegate(object sender, int slideIndex)
			{
				Navigation.SkipToSlide(slideIndex, null);
			};

            if (!Configuration.Options.General.AutoStart)
                Pause();
		}

		/// <summary>
		/// Toggles full screen mode.
		/// </summary>
		public static void ToggleFullScreen()
		{
			App.Current.Host.Content.IsFullScreen = !App.Current.Host.Content.IsFullScreen;
		}

		/// <summary>
		/// Toggles full screen mode on.
		/// </summary>
		public static void ToggleFullScreenOn()
		{
			App.Current.Host.Content.IsFullScreen = true;
		}

		/// <summary>
		/// Toggles full screen mode off.
		/// </summary>
		public static void ToggleFullScreenOff()
		{
			App.Current.Host.Content.IsFullScreen = false;
		}
	}
}