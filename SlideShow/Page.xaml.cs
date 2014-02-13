using System;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vertigo.SlideShow.Controls;
using System.Windows;

namespace Vertigo.SlideShow
{
	public partial class Page : UserControl
	{
		public Page()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Applies configuration settings from the configuration provider.
		/// </summary>
		public void ApplyConfiguration()
		{
			WireEventHandlers();

			new KeyboardSupport();

			// If no background param is present, it will default to white.
			// If the param is not white, use it instead of the configured value.
			SlideShowRoot.Background = App.Current.Host.Background != Colors.White ?
				new SolidColorBrush(App.Current.Host.Background) :
				Configuration.Options.General.Background;
			navigationTray.Opacity = 1;
			slideDescription.Opacity = 1;
		}

		/// <summary>
		/// Wires up UI controls.
		/// </summary>
		private void WireEventHandlers()
		{
			App.Current.RootVisual.MouseEnter += RootVisual_MouseMove;
			App.Current.RootVisual.MouseLeave += RootVisual_MouseLeave;

			navigationTray.SlideNavigationElement.PreviousClick += delegate 
			{ 
			    Navigation.SkipToPreviousSlide(null);
			};

			navigationTray.SlideNavigationElement.PlayClick += delegate
			{
				Navigation.ToggleAlbumViewOff();
				Navigation.Play();
			};

			navigationTray.SlideNavigationElement.PauseClick += delegate
			{
				Navigation.Pause();
			};

			navigationTray.SlideNavigationElement.NextClick += delegate
			{
				Navigation.SkipToNextSlide(null);
			};

			navigationTray.GoToFullScreenButtonElement.Click += delegate
			{
				Navigation.ToggleFullScreenOn();
			};

			navigationTray.EscapeFullScreenButtonElement.Click += delegate
			{
				Navigation.ToggleFullScreenOff();
			};

			navigationTray.AlbumButtonElement.Click += delegate
			{
				Navigation.ToggleAlbumView();
			};

			navigationTray.EmbedButtonElement.Click += delegate
			{
				Navigation.ToggleEmbedView();
			};

			EmbedViewerElement.CloseButtonClicked += delegate
			{
				Navigation.ToggleEmbedView();
			};

			navigationTray.AlbumViewerElement.CurrentAlbumPage.AlbumButtonClicked += new AlbumPage.AlbumButtonClickedEventHandler(AlbumPage_AlbumButtonClicked);

			navigationTray.SaveHyperlinkButtonElement.MouseEnter += new MouseEventHandler(SaveHyperlinkButtonElement_MouseEnter);
			navigationTray.SaveHyperlinkButtonElement.MouseLeave += new MouseEventHandler(SaveHyperlinkButtonElement_MouseLeave);
		}

		void SaveHyperlinkButtonElement_MouseLeave(object sender, MouseEventArgs e)
		{
			VisualStateManager.GoToState(navigationTray.SaveButtonElement, "Normal", true);
		}

		void SaveHyperlinkButtonElement_MouseEnter(object sender, MouseEventArgs e)
		{
			VisualStateManager.GoToState(navigationTray.SaveButtonElement, "MouseOver", true);
		}

		private void RootVisual_MouseMove(object sender, MouseEventArgs e)
		{
			slideDescription.Show();
		}

		private void RootVisual_MouseLeave(object sender, MouseEventArgs e)
		{
			slideDescription.Hide();
		}

		private void AlbumPage_AlbumButtonClicked(object sender, EventArgs e)
		{
			if (sender is AlbumButton)
			{
				Navigation.SelectAlbum(((AlbumButton)sender).Index);
			}
		}

		/// <summary>
		/// Updates the play/pause button to display paused state.
		/// </summary>
		public void DisplayPausedState()
		{
			if (navigationTray != null)
			{
				if (navigationTray.SlideNavigationElement != null)
				{
					navigationTray.SlideNavigationElement.DisplayPauseState();
				}
			}
		}

		/// <summary>
		/// Updates the play/pause button to display playing state.
		/// </summary>
		public void DisplayPlayState()
		{
			if (navigationTray != null)
			{
				if (navigationTray.SlideNavigationElement != null)
				{
					navigationTray.SlideNavigationElement.DisplayPlayState();
				}
			}
		}

		/// <summary>
		/// Handles the MouseEnter event on the navigation tray.
		/// </summary>
		/// <param name="sender">the sending object</param>
		/// <param name="e">the event args</param>
		private void navigationTray_MouseEnter(object sender, MouseEventArgs e)
		{
			navigationTray.Show();
		}

		/// <summary>
		/// Handles the MouseLeave event on the navigation tray.
		/// </summary>
		/// <param name="sender">the sending object</param>
		/// <param name="e">the event args</param>
		private void navigationTray_MouseLeave(object sender, MouseEventArgs e)
		{
			navigationTray.Hide();
		}

		/// <summary>
		/// This method updates the Slide Description control with the description and title of the slide.
		/// </summary>
		/// <param name="Title">The title of the current slide.</param>
		/// <param name="Description">The description of the current slide.</param>
		public void UpdateSlideDescription(string Title, string Description)
		{
			slideDescription.Title = Title;
			slideDescription.Description = Description;
		}

		/// <summary>
		/// For testing purposes. Displays an alert message of the parameter.ToString()
		/// </summary>
		/// <param name="message">The object to display object.ToString() of.</param>
		public static void Alert(object message)
		{
			HtmlPage.Window.Alert(message.ToString());
		}
	}
}