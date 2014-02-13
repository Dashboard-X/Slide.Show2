using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Represents a Configuration
	/// </summary>
	public static class Configuration
	{
		/// <summary>
		/// Gets or sets the data provider.
		/// </summary>
		/// <value>The data provider.</value>
		public static IDataProvider DataProvider { get; set; }

		/// <summary>
		/// Gets the options.
		/// </summary>
		/// <value>The options.</value>
		public static Options Options { get; private set; }

		/// <summary>
		/// Sets the configuration.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public static void SetConfiguration(ConfigurationProvider provider)
		{
			Options = provider.Options;
		}
	}

	/// <summary>
	/// Represents a ConfigurationProvider
	/// </summary>
	public abstract class ConfigurationProvider
	{
		/// <summary>
		/// Occurs when [configuration finished loading].
		/// </summary>
		public static event EventHandler ConfigurationFinishedLoading;

		/// <summary>
		/// Occurs when [options set].
		/// </summary>
		protected abstract event EventHandler OptionsSet;

		/// <summary>
		/// Gets or sets the default theme.
		/// </summary>
		/// <value>The default theme.</value>
		public static ConfigurationProvider DefaultTheme { get; set; }

		/// <summary>
		/// Gets the init params.
		/// </summary>
		/// <value>The init params.</value>
		public static Dictionary<string, Dictionary<string, string>> initParams { get; private set; }

		/// <summary>
		/// Gets the options.
		/// </summary>
		/// <value>The options.</value>
		public Options Options { get; protected set; }

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		protected abstract void GetConfiguration();

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationProvider"/> class.
		/// </summary>
		protected ConfigurationProvider()
		{
			Options = new Options();
		}

		/// <summary>
		/// Called when loading is finished
		/// </summary>
		private void FinishedLoading()
		{
			Validate();
			Configuration.SetConfiguration(this);
			Options.DataProvider.Type =
								initParams.ContainsKey("DataProvider") ?
								initParams["DataProvider"]["DataProvider"] :
								Options.DataProvider.Type ?? string.Empty;

			Options.General.StartInAlbumView = initParams.ContainsKey("ConfigurationProvider") ? (initParams["ConfigurationProvider"].ContainsKey("StartInAlbumView") ? Convert.ToBoolean(initParams["ConfigurationProvider"]["StartInAlbumView"]) : Options.General.StartInAlbumView) : Options.General.StartInAlbumView;

			Type dataProviderType;
			switch (Options.DataProvider.Type.ToLower(CultureInfo.InvariantCulture))
			{
				case "xmldataprovider":
					dataProviderType = typeof(XmlDataProvider);
					break;
				case "flickrdataprovider":
					dataProviderType = typeof(FlickrDataProvider);
					break;
				default:
					dataProviderType = typeof(XmlDataProvider);
					break;
			}

			Configuration.DataProvider = (IDataProvider)Activator.CreateInstance(dataProviderType, initParams);

			if (ConfigurationFinishedLoading != null)
			{
				ConfigurationFinishedLoading(null, null);
			}
		}

		/// <summary>
		/// Loads the configuration provider.
		/// </summary>
		/// <param name="providerType">Type of the provider.</param>
		/// <param name="initParams">The init params.</param>
		public static void LoadConfigurationProvider(Type providerType, Dictionary<string, Dictionary<string, string>> initParams)
		{
			ConfigurationProvider.initParams = initParams;

			DefaultTheme = new LightTheme(initParams);
			DefaultTheme.GetConfiguration();

			ConfigurationProvider provider = Activator.CreateInstance(providerType, initParams) as ConfigurationProvider;
			if (provider == null)
				provider = Activator.CreateInstance(typeof(LightTheme), initParams) as ConfigurationProvider;

			provider.OptionsSet += delegate
			{
				provider.FinishedLoading();
			};

			provider.GetConfiguration();
		}

		/// <summary>
		/// Validates this instance.
		/// </summary>
		private void Validate()
		{ 
			// TODO #390 - validate is blank
		}

		/// <summary>
		/// Parses a brush from a string.
		/// </summary>
		/// <param name="value">The string (e.g. "Black", "#FF000000", etc.)</param>
		/// <returns>The brush.</returns>
		public static Brush ParseBrush(string value)
		{
			try
			{
				Canvas canvas = (Canvas)XamlReader.Load("<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Background=\"" + value + "\" />");
				return canvas.Background;
			}
			catch (XamlParseException)
			{
				return null;
			}
		}
	}

	/// <summary>
	/// Represents an Options
	/// </summary>
	public class Options
	{
		/// <summary>
		/// Gets or sets the default options.
		/// </summary>
		/// <value>The default options.</value>
		protected static Options DefaultOptions { get; set; }

		/// <summary>
		/// Gets or sets the general.
		/// </summary>
		/// <value>The general.</value>
		public GeneralOptions General { get; set; }

		/// <summary>
		/// Gets or sets the transition.
		/// </summary>
		/// <value>The transition.</value>
		public TransitionOptions Transition { get; set; }

		/// <summary>
		/// Gets or sets the data provider.
		/// </summary>
		/// <value>The data provider.</value>
		public DataProviderOptions DataProvider { get; set; }

		/// <summary>
		/// Gets or sets the preloader.
		/// </summary>
		/// <value>The preloader.</value>
		public PreloaderOptions Preloader { get; set; }

		/// <summary>
		/// Gets or sets the loading progress indicator.
		/// </summary>
		/// <value>The loading progress indicator.</value>
		public LoadingProgressIndicatorOptions LoadingProgressIndicator { get; set; }

		/// <summary>
		/// Gets or sets the slide navigation.
		/// </summary>
		/// <value>The slide navigation.</value>
		public SlideNavigationOptions SlideNavigation { get; set; }

		/// <summary>
		/// Gets or sets the embed viewer.
		/// </summary>
		/// <value>The embed viewer.</value>
		public EmbedViewerOptions EmbedViewer { get; set; }

		/// <summary>
		/// Gets or sets the navigation tray.
		/// </summary>
		/// <value>The navigation tray.</value>
		public NavigationTrayOptions NavigationTray { get; set; }

		/// <summary>
		/// Gets or sets the toggle album view button.
		/// </summary>
		/// <value>The toggle album view button.</value>
		public ToggleAlbumViewButtonOptions ToggleAlbumViewButton { get; set; }

		/// <summary>
        /// Gets or sets the save button.
        /// </summary>
        /// <value>The save button.</value>
        public ToggleSaveButtonOptions ToggleSaveButton { get; set; }

        /// <summary>
		/// Gets or sets the toggle full screen button.
		/// </summary>
		/// <value>The toggle full screen button.</value>
		public ToggleFullScreenButtonOptions ToggleFullScreenButton { get; set; }

		/// <summary>
		/// Gets or sets the toggle embed view button.
		/// </summary>
		/// <value>The toggle embed view button.</value>
		public ToggleEmbedViewButtonOptions ToggleEmbedViewButton { get; set; }

		/// <summary>
		/// Gets or sets the slide description.
		/// </summary>
		/// <value>The slide description.</value>
		public SlideDescriptionOptions SlideDescription { get; set; }

		/// <summary>
		/// Gets or sets the album viewer.
		/// </summary>
		/// <value>The album viewer.</value>
		public AlbumViewerOptions AlbumViewer { get; set; }

		/// <summary>
		/// Gets or sets the thumbnail viewer.
		/// </summary>
		/// <value>The thumbnail viewer.</value>
		public ThumbnailViewerOptions ThumbnailViewer { get; set; }

		/// <summary>
		/// Gets or sets the slide thumbnail.
		/// </summary>
		/// <value>The slide thumbnail.</value>
		public SlideThumbnailOptions SlideThumbnail { get; set; }

		/// <summary>
		/// Gets or sets the album page.
		/// </summary>
		/// <value>The album page.</value>
		public AlbumPageOptions AlbumPage { get; set; }

		/// <summary>
		/// Gets or sets the album button.
		/// </summary>
		/// <value>The album button.</value>
		public AlbumButtonOptions AlbumButton { get; set; }

		/// <summary>
		/// Gets or sets the slide preview.
		/// </summary>
		/// <value>The slide preview.</value>
		public SlidePreviewOptions SlidePreview { get; set; }

		/// <summary>
		/// Gets or sets the slide viewer.
		/// </summary>
		/// <value>The slide viewer.</value>
		public SlideViewerOptions SlideViewer { get; set; }

		/// <summary>
		/// Gets or sets the image viewer.
		/// </summary>
		/// <value>The image viewer.</value>
		public ImageViewerOptions ImageViewer { get; set; }

		/// <summary>
		/// Gets or sets the video viewer.
		/// </summary>
		/// <value>The video viewer.</value>
		public VideoViewerOptions VideoViewer { get; set; }

		/// <summary>
		/// Gets or sets the video tray.
		/// </summary>
		/// <value>The video tray.</value>
		public VideoTrayOptions VideoTray { get; set; }

		/// <summary>
		/// Gets or sets the slide description.
		/// </summary>
		/// <value>The slide description.</value>
		public PeopleTagOptions PeopleTag { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Options"/> class.
		/// </summary>
		public Options()
		{
			if (ConfigurationProvider.DefaultTheme != null)
			{
				if (ConfigurationProvider.DefaultTheme.Options != null)
				{
					DefaultOptions = ConfigurationProvider.DefaultTheme.Options;
				}
			}
		}

		/// <summary>
		/// Represents a GeneralOptions
		/// </summary>
		public class GeneralOptions
		{
			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

            /// <summary> 
            /// Gets or sets AlbumView startup mode 
            /// </summary> 
            /// <value><c>true</c> if start in album view, <c>false</c> otherwise.</value> 
            public bool StartInAlbumView { get; set; }

            /// <summary> 
            /// Gets or sets whether to auto start slide playback. 
            /// </summary> 
            /// <value><c>true</c> if auto start, <c>false</c> otherwise.</value> 
            public bool AutoStart { get; set; }

			/// <summary>
            /// Initializes a new instance of the <see cref="GeneralOptions"/> class.
			/// </summary>
            public GeneralOptions()
			{
				if (DefaultOptions != null)
				{
                    Background = DefaultOptions.General.Background;
                    StartInAlbumView = DefaultOptions.General.StartInAlbumView;
                    AutoStart = DefaultOptions.General.AutoStart;
                }
			}
		}

		/// <summary>
		/// Represents a TransitionOptions
		/// </summary>
		public class TransitionOptions
		{
			/// <summary>
			/// Gets or sets the wait time.
			/// </summary>
			/// <value>The wait time.</value>
			public int WaitTime { get; set; }

			/// <summary>
			/// Gets or sets the duration of the transition.
			/// </summary>
			/// <value>The duration of the transition.</value>
			public int TransitionDuration { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="TransitionOptions"/> class.
			/// </summary>
			public TransitionOptions()
			{
				if (DefaultOptions != null)
				{
					WaitTime = DefaultOptions.Transition.WaitTime;
					TransitionDuration = DefaultOptions.Transition.TransitionDuration;
				}
			}
		}

		/// <summary>
		/// Represents a DataProviderOptions
		/// </summary>
		public class DataProviderOptions
		{
			/// <summary>
			/// Gets or sets the type.
			/// </summary>
			/// <value>The type.</value>
			public string Type { get; set; }

			/// <summary>
			/// Gets or sets the username.
			/// </summary>
			/// <value>The username.</value>
			public string UserName { get; set; } // Flickr

			/// <summary>
			/// Gets or sets the API key.
			/// </summary>
			/// <value>The API key.</value>
			public string ApiKey { get; set; } // Flickr

			/// <summary>
			/// Gets or sets the URI.
			/// </summary>
			/// <value>The URI.</value>
			public string Uri { get; set; } // Xml
		}

		/// <summary>
		/// Represents a PreloaderOptions
		/// </summary>
		public class PreloaderOptions
		{
			/// <summary>
			/// Gets or sets the preload ahead.
			/// </summary>
			/// <value>The preload ahead.</value>
			public int PreloadAhead { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="PreloaderOptions"/> class.
			/// </summary>
			public PreloaderOptions()
			{
				if (DefaultOptions != null)
				{
					PreloadAhead = DefaultOptions.Preloader.PreloadAhead;
				}
			}
		}

		/// <summary>
		/// Represents a LoadingProgressIndicatorOptions
		/// </summary>
		public class LoadingProgressIndicatorOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="LoadingProgressIndicatorOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the type.
			/// </summary>
			/// <value>The type.</value>
			public string Type { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the foreground.
			/// </summary>
			/// <value>The foreground.</value>
			public Brush Foreground { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="LoadingProgressIndicatorOptions"/> class.
			/// </summary>
			public LoadingProgressIndicatorOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.LoadingProgressIndicator.Enabled;
					Height = DefaultOptions.LoadingProgressIndicator.Height;
					Width = DefaultOptions.LoadingProgressIndicator.Width;
				}
			}
		}

		/// <summary>
		/// Represents a SlideNavigationOptions
		/// </summary>
		public class SlideNavigationOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="SlideNavigationOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the height of the play pause button.
			/// </summary>
			/// <value>The height of the play pause button.</value>
			public double PlayPauseButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the play pause button.
			/// </summary>
			/// <value>The width of the play pause button.</value>
			public double PlayPauseButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the width of the next button.
			/// </summary>
			/// <value>The width of the next button.</value>
			public double NextButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the next button.
			/// </summary>
			/// <value>The height of the next button.</value>
			public double NextButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the previous button.
			/// </summary>
			/// <value>The width of the previous button.</value>
			public double PreviousButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the previous button.
			/// </summary>
			/// <value>The height of the previous button.</value>
			public double PreviousButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the height of the play pause button path.
			/// </summary>
			/// <value>The height of the play pause button path.</value>
			public double PlayPauseButtonPathHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the play pause button path.
			/// </summary>
			/// <value>The width of the play pause button path.</value>
			public double PlayPauseButtonPathWidth { get; set; }

			/// <summary>
			/// Gets or sets the width of the next button path.
			/// </summary>
			/// <value>The width of the next button path.</value>
			public double NextButtonPathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the next button path.
			/// </summary>
			/// <value>The height of the next button path.</value>
			public double NextButtonPathHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the previous button path.
			/// </summary>
			/// <value>The width of the previous button path.</value>
			public double PreviousButtonPathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the previous button path.
			/// </summary>
			/// <value>The height of the previous button path.</value>
			public double PreviousButtonPathHeight { get; set; }

			/// <summary>
			/// Gets or sets the play path data.
			/// </summary>
			/// <value>The play path data.</value>
			public string PlayPathData { get; set; }

			/// <summary>
			/// Gets or sets the pause path data.
			/// </summary>
			/// <value>The pause path data.</value>
			public string PausePathData { get; set; }

			/// <summary>
			/// Gets or sets the previous path data.
			/// </summary>
			/// <value>The previous path data.</value>
			public string PreviousPathData { get; set; }

			/// <summary>
			/// Gets or sets the next path data.
			/// </summary>
			/// <value>The next path data.</value>
			public string NextPathData { get; set; }

			/// <summary>
			/// Gets or sets the foreground.
			/// </summary>
			/// <value>The foreground.</value>
			public Brush Foreground { get; set; }

			/// <summary>
			/// Gets or sets the foreground hover.
			/// </summary>
			/// <value>The foreground hover.</value>
			public Brush ForegroundHover { get; set; }

			/// <summary>
			/// Gets or sets the background1 brush.
			/// </summary>
			/// <value>The background1 brush.</value>
			public Brush Background1Brush { get; set; }

			/// <summary>
			/// Gets or sets the background2 brush.
			/// </summary>
			/// <value>The background2 brush.</value>
			public Brush Background2Brush { get; set; }

			/// <summary>
			/// Gets or sets the radius X.
			/// </summary>
			/// <value>The radius X.</value>
			public double RadiusX { get; set; }

			/// <summary>
			/// Gets or sets the radius Y.
			/// </summary>
			/// <value>The radius Y.</value>
			public double RadiusY { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="SlideNavigationOptions"/> class.
			/// </summary>
			public SlideNavigationOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.SlideNavigation.Enabled;
					RadiusX = DefaultOptions.SlideNavigation.RadiusX;
					RadiusY = DefaultOptions.SlideNavigation.RadiusY;
					Height = DefaultOptions.SlideNavigation.Height;
					Width = DefaultOptions.SlideNavigation.Width;

					PlayPauseButtonHeight = DefaultOptions.SlideNavigation.PlayPauseButtonHeight;
					PlayPauseButtonWidth = DefaultOptions.SlideNavigation.PlayPauseButtonWidth;
					NextButtonWidth = DefaultOptions.SlideNavigation.NextButtonWidth;
					NextButtonHeight = DefaultOptions.SlideNavigation.NextButtonHeight;
					PreviousButtonWidth = DefaultOptions.SlideNavigation.PreviousButtonWidth;
					PreviousButtonHeight = DefaultOptions.SlideNavigation.PreviousButtonHeight;

					PlayPauseButtonPathHeight = DefaultOptions.SlideNavigation.PlayPauseButtonPathHeight;
					PlayPauseButtonPathWidth = DefaultOptions.SlideNavigation.PlayPauseButtonPathWidth;
					NextButtonPathWidth = DefaultOptions.SlideNavigation.NextButtonPathWidth;
					NextButtonPathHeight = DefaultOptions.SlideNavigation.NextButtonPathHeight;
					PreviousButtonPathWidth = DefaultOptions.SlideNavigation.PreviousButtonPathWidth;
					PreviousButtonPathHeight = DefaultOptions.SlideNavigation.PreviousButtonPathHeight;
				}
			}
		}

		/// <summary>
		/// Represents a EmbedViewerOptions
		/// </summary>
		public class EmbedViewerOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="EmbedViewerOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The background opacity.</value>
			public double BackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the corner radius.
			/// </summary>
			/// <value>The corner radius.</value>
            public CornerRadius CornerRadius { get; set; }

			/// <summary>
			/// Gets or sets the dialog background.
			/// </summary>
			/// <value>The dialog background.</value>
			public Brush DialogBackground { get; set; }

			/// <summary>
			/// Gets or sets the dialog background opacity.
			/// </summary>
			/// <value>The dialog background opacity.</value>
			public double DialogBackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the dialog border brush.
			/// </summary>
			/// <value>The dialog border brush.</value>
			public Brush DialogBorderBrush { get; set; }

			/// <summary>
			/// Gets or sets the dialog border thickness.
			/// </summary>
			/// <value>The dialog border thickness.</value>
			public Thickness DialogBorderThickness { get; set; }

			/// <summary>
			/// Gets or sets the width of the dialog.
			/// </summary>
			/// <value>The width of the dialog.</value>
			public double DialogWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the dialog.
			/// </summary>
			/// <value>The height of the dialog.</value>
			public double DialogHeight { get; set; }

			/// <summary>
			/// Gets or sets the close button background1 brush.
			/// </summary>
			/// <value>The close button background1 brush.</value>
			public Brush CloseButtonBackground1Brush { get; set; }

			/// <summary>
			/// Gets or sets the close button background2 brush.
			/// </summary>
			/// <value>The close button background2 brush.</value>
			public Brush CloseButtonBackground2Brush { get; set; }

			/// <summary>
			/// Gets or sets the close button foreground brush.
			/// </summary>
			/// <value>The close button foreground brush.</value>
			public Brush CloseButtonForegroundBrush { get; set; }

			/// <summary>
			/// Gets or sets the close button foreground hover brush.
			/// </summary>
			/// <value>The close button foreground hover brush.</value>
			public Brush CloseButtonForegroundHoverBrush { get; set; }

			/// <summary>
			/// Gets or sets the close button radius X.
			/// </summary>
			/// <value>The close button radius X.</value>
			public double CloseButtonRadiusX { get; set; }

			/// <summary>
			/// Gets or sets the close button radius Y.
			/// </summary>
			/// <value>The close button radius Y.</value>
			public double CloseButtonRadiusY { get; set; }

			/// <summary>
			/// Gets or sets the close button margin.
			/// </summary>
			/// <value>The close button margin.</value>
			public Thickness CloseButtonMargin { get; set; }

			/// <summary>
			/// Gets or sets the width of the close button.
			/// </summary>
			/// <value>The width of the close button.</value>
			public double CloseButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the close button.
			/// </summary>
			/// <value>The height of the close button.</value>
			public double CloseButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the close button path.
			/// </summary>
			/// <value>The width of the close button path.</value>
			public double CloseButtonPathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the close button path.
			/// </summary>
			/// <value>The height of the close button path.</value>
			public double CloseButtonPathHeight { get; set; }

			/// <summary>
			/// Gets or sets the close button path data.
			/// </summary>
			/// <value>The close button path data.</value>
			public string CloseButtonPathData { get; set; }

			/// <summary>
			/// Gets or sets the hyperlink button text.
			/// </summary>
			/// <value>The hyperlink button text.</value>
			public string HyperlinkButtonText { get; set; }

			/// <summary>
			/// Gets or sets the hyperlink button foreground.
			/// </summary>
			/// <value>The hyperlink button foreground.</value>
			public Brush HyperlinkButtonForeground { get; set; }

			/// <summary>
			/// Gets or sets the hyperlink button background.
			/// </summary>
			/// <value>The hyperlink button background.</value>
			public Brush HyperlinkButtonBackground { get; set; }

			/// <summary>
			/// Gets or sets the hyperlink button margin.
			/// </summary>
			/// <value>The hyperlink button margin.</value>
			public Thickness HyperlinkButtonMargin { get; set; }

			/// <summary>
			/// Gets or sets the width of the hyperlink button.
			/// </summary>
			/// <value>The width of the hyperlink button.</value>
			public double HyperlinkButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the hyperlink button.
			/// </summary>
			/// <value>The height of the hyperlink button.</value>
			public double HyperlinkButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the font of the hyperlink button.
			/// </summary>
			/// <value>The font of the hyperlink button.</value>
			public FontFamily HyperlinkButtonFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the hyperlink button font.
			/// </summary>
			/// <value>The size of the hyperlink button font.</value>
			public double HyperlinkButtonFontSize { get; set; }

			/// <summary>
			/// Gets or sets the font of the text block.
			/// </summary>
			/// <value>The font of the text block.</value>
			public FontFamily TextBlockFontFamily { get; set; }
	
			/// <summary>
			/// Gets or sets the size of the text block font.
			/// </summary>
			/// <value>The size of the text block font.</value>
			public double TextBlockFontSize { get; set; }

			/// <summary>
			/// Gets or sets the title text.
			/// </summary>
			/// <value>The title text.</value>
			public string TitleText { get; set; }

			/// <summary>
			/// Gets or sets the text block margin.
			/// </summary>
			/// <value>The text block margin.</value>
			public Thickness TextBlockMargin { get; set; }

			/// <summary>
			/// Gets or sets the text block foreground.
			/// </summary>
			/// <value>The text block foreground.</value>
			public Brush TextBlockForeground { get; set; }

			/// <summary>
			/// Gets or sets the width of the text box.
			/// </summary>
			/// <value>The width of the text box.</value>
			public double TextBoxWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the text box.
			/// </summary>
			/// <value>The height of the text box.</value>
			public double TextBoxHeight { get; set; }

			/// <summary>
			/// Gets or sets the text box background.
			/// </summary>
			/// <value>The text box background.</value>
			public Brush TextBoxBackground { get; set; }

			/// <summary>
			/// Gets or sets the text box foreground.
			/// </summary>
			/// <value>The text box foreground.</value>
			public Brush TextBoxForeground { get; set; }

			/// <summary>
			/// Gets or sets the font of the output text box.
			/// </summary>
			/// <value>The font of the output text box.</value>
			public FontFamily TextBoxFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the output text box font.
			/// </summary>
			/// <value>The size of the output text box font.</value>
			public double TextBoxFontSize { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="EmbedViewerOptions"/> class.
			/// </summary>
			public EmbedViewerOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.EmbedViewer.Enabled;
					BackgroundOpacity = DefaultOptions.EmbedViewer.BackgroundOpacity;
                    CornerRadius = DefaultOptions.EmbedViewer.CornerRadius;

					DialogBackgroundOpacity = DefaultOptions.EmbedViewer.DialogBackgroundOpacity;
					DialogBorderThickness = DefaultOptions.EmbedViewer.DialogBorderThickness;
					DialogWidth = DefaultOptions.EmbedViewer.DialogWidth;
					DialogHeight = DefaultOptions.EmbedViewer.DialogHeight;

					CloseButtonRadiusX = DefaultOptions.EmbedViewer.CloseButtonRadiusX;
					CloseButtonRadiusY = DefaultOptions.EmbedViewer.CloseButtonRadiusY;
					CloseButtonMargin = DefaultOptions.EmbedViewer.CloseButtonMargin;
					CloseButtonWidth = DefaultOptions.EmbedViewer.CloseButtonWidth;
					CloseButtonHeight = DefaultOptions.EmbedViewer.CloseButtonHeight;
					CloseButtonPathWidth = DefaultOptions.EmbedViewer.CloseButtonPathWidth;
					CloseButtonPathHeight = DefaultOptions.EmbedViewer.CloseButtonPathHeight;

					HyperlinkButtonText = DefaultOptions.EmbedViewer.HyperlinkButtonText;
					HyperlinkButtonBackground = DefaultOptions.EmbedViewer.HyperlinkButtonBackground;
					HyperlinkButtonFontFamily = DefaultOptions.EmbedViewer.HyperlinkButtonFontFamily;
					HyperlinkButtonFontSize = DefaultOptions.EmbedViewer.HyperlinkButtonFontSize;
					HyperlinkButtonForeground = DefaultOptions.EmbedViewer.HyperlinkButtonForeground;
					HyperlinkButtonHeight = DefaultOptions.EmbedViewer.HyperlinkButtonHeight;
					HyperlinkButtonMargin = DefaultOptions.EmbedViewer.HyperlinkButtonMargin;
					HyperlinkButtonWidth = DefaultOptions.EmbedViewer.HyperlinkButtonWidth;
					
					TextBlockFontFamily = DefaultOptions.EmbedViewer.TextBlockFontFamily;
					TextBlockFontSize = DefaultOptions.EmbedViewer.TextBlockFontSize;
					TextBlockMargin = DefaultOptions.EmbedViewer.TextBlockMargin;

					TextBoxWidth = DefaultOptions.EmbedViewer.TextBoxWidth;
					TextBoxHeight = DefaultOptions.EmbedViewer.TextBoxHeight;
					TextBoxFontFamily = DefaultOptions.EmbedViewer.TextBoxFontFamily;
					TextBoxFontSize = DefaultOptions.EmbedViewer.TextBoxFontSize;
				}
			}
		}

		/// <summary>
		/// Represents a NavigationTrayOptions
		/// </summary>
		public class NavigationTrayOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="NavigationTrayOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The background opacity.</value>
			public double BackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="NavigationTrayOptions"/> class.
			/// </summary>
			public NavigationTrayOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.NavigationTray.Enabled;
					BackgroundOpacity = DefaultOptions.NavigationTray.BackgroundOpacity;
				}
			}
		}

		/// <summary>
		/// Represents a ToggleAlbumViewButtonOptions
		/// </summary>
		public class ToggleAlbumViewButtonOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="ToggleAlbumViewButtonOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the width of the path.
			/// </summary>
			/// <value>The width of the path.</value>
			public double PathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the path.
			/// </summary>
			/// <value>The height of the path.</value>
			public double PathHeight { get; set; }

			/// <summary>
			/// Gets or sets the path data.
			/// </summary>
			/// <value>The path data.</value>
			public string PathData { get; set; }

			/// <summary>
			/// Gets or sets the background1 brush.
			/// </summary>
			/// <value>The background1 brush.</value>
			public Brush Background1Brush { get; set; }

			/// <summary>
			/// Gets or sets the background2 brush.
			/// </summary>
			/// <value>The background2 brush.</value>
			public Brush Background2Brush { get; set; }

			/// <summary>
			/// Gets or sets the foreground brush.
			/// </summary>
			/// <value>The foreground brush.</value>
			public Brush ForegroundBrush { get; set; }

			/// <summary>
			/// Gets or sets the foreground hover brush.
			/// </summary>
			/// <value>The foreground hover brush.</value>
			public Brush ForegroundHoverBrush { get; set; }

			/// <summary>
			/// Gets or sets the radius X.
			/// </summary>
			/// <value>The radius X.</value>
			public double RadiusX { get; set; }

			/// <summary>
			/// Gets or sets the radius Y.
			/// </summary>
			/// <value>The radius Y.</value>
			public double RadiusY { get; set; }

			/// <summary>
			/// Gets or sets the margin.
			/// </summary>
			/// <value>The margin.</value>
			public Thickness Margin { get; set; }

			/// <summary>
			/// Gets or sets the button tooltip.
			/// </summary>
			/// <value>The button tooltip.  Set to null or empty string to disable.</value>
			public string Tooltip { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="ToggleAlbumViewButtonOptions"/> class.
			/// </summary>
			public ToggleAlbumViewButtonOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.ToggleAlbumViewButton.Enabled;
					Width = DefaultOptions.ToggleAlbumViewButton.Width;
					Height = DefaultOptions.ToggleAlbumViewButton.Height;
					PathWidth = DefaultOptions.ToggleAlbumViewButton.PathWidth;
					PathHeight = DefaultOptions.ToggleAlbumViewButton.PathHeight;
					PathData = DefaultOptions.ToggleAlbumViewButton.PathData;
					RadiusX = DefaultOptions.ToggleAlbumViewButton.RadiusX;
					RadiusY = DefaultOptions.ToggleAlbumViewButton.RadiusY;
					Margin = DefaultOptions.ToggleAlbumViewButton.Margin;
					Tooltip = DefaultOptions.ToggleAlbumViewButton.Tooltip;
				}
			}
		}

		/// <summary>
        /// Represents a ToggleSaveButtonOptions
        /// </summary>
        public class ToggleSaveButtonOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="ToggleSaveButtonOptions"/> is enabled.
            /// </summary>
            /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
            public bool Enabled { get; set; }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public double Width { get; set; }

            /// <summary>
            /// Gets or sets the height.
            /// </summary>
            /// <value>The height.</value>
            public double Height { get; set; }

            /// <summary>
            /// Gets or sets the width of the path.
            /// </summary>
            /// <value>The width of the path.</value>
            public double PathWidth { get; set; }

            /// <summary>
            /// Gets or sets the height of the path.
            /// </summary>
            /// <value>The height of the path.</value>
            public double PathHeight { get; set; }

            /// <summary>
            /// Gets or sets the path data.
            /// </summary>
            /// <value>The path data.</value>
            public string PathData { get; set; }

            /// <summary>
            /// Gets or sets the background1 brush.
            /// </summary>
            /// <value>The background1 brush.</value>
            public Brush Background1Brush { get; set; }

            /// <summary>
            /// Gets or sets the background2 brush.
            /// </summary>
            /// <value>The background2 brush.</value>
            public Brush Background2Brush { get; set; }

            /// <summary>
            /// Gets or sets the foreground brush.
            /// </summary>
            /// <value>The foreground brush.</value>
            public Brush ForegroundBrush { get; set; }

            /// <summary>
            /// Gets or sets the foreground hover brush.
            /// </summary>
            /// <value>The foreground hover brush.</value>
            public Brush ForegroundHoverBrush { get; set; }

            /// <summary>
            /// Gets or sets the radius X.
            /// </summary>
            /// <value>The radius X.</value>
            public double RadiusX { get; set; }

            /// <summary>
            /// Gets or sets the radius Y.
            /// </summary>
            /// <value>The radius Y.</value>
            public double RadiusY { get; set; }

            /// <summary>
            /// Gets or sets the margin.
            /// </summary>
            /// <value>The margin.</value>
            public Thickness Margin { get; set; }

			/// <summary>
			/// Gets or sets the ashx file handler .
			/// </summary>
			/// <value>If DownloadImage=true, this specifies the ashx file handler used to download the image to the user's machine.  This is ignored if DownloadImage=false.</value>
			public string DownloadImageHandler { get; set; }

			/// <summary>
			/// Gets or sets the button tooltip.
			/// </summary>
			/// <value>The button tooltip.  Set to null or empty string to disable.</value>
			public string Tooltip { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ToggleSaveButtonOptions"/> class.
            /// </summary>
            public ToggleSaveButtonOptions()
            {
                if (DefaultOptions != null)
                {
                    Enabled = DefaultOptions.ToggleSaveButton.Enabled;
                    Width = DefaultOptions.ToggleSaveButton.Width;
                    Height = DefaultOptions.ToggleSaveButton.Height;
                    PathWidth = DefaultOptions.ToggleSaveButton.PathWidth;
                    PathHeight = DefaultOptions.ToggleSaveButton.PathHeight;
                    PathData = DefaultOptions.ToggleSaveButton.PathData;
                    RadiusX = DefaultOptions.ToggleSaveButton.RadiusX;
                    RadiusY = DefaultOptions.ToggleSaveButton.RadiusY;
                    Margin = DefaultOptions.ToggleSaveButton.Margin;
					DownloadImageHandler = DefaultOptions.ToggleSaveButton.DownloadImageHandler;
					Tooltip = DefaultOptions.ToggleSaveButton.Tooltip;
                }
            }
        }

        /// <summary>
		/// Represents a ToggleFullScreenButtonOptions
		/// </summary>
		public class ToggleFullScreenButtonOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="ToggleFullScreenButtonOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the width of the path.
			/// </summary>
			/// <value>The width of the path.</value>
			public double PathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the path.
			/// </summary>
			/// <value>The height of the path.</value>
			public double PathHeight { get; set; }

			/// <summary>
			/// Gets or sets the go to full path data.
			/// </summary>
			/// <value>The go to full path data.</value>
			public string GoToFullPathData { get; set; }

			/// <summary>
			/// Gets or sets the escape full path data.
			/// </summary>
			/// <value>The escape full path data.</value>
			public string EscapeFullPathData { get; set; }

			/// <summary>
			/// Gets or sets the background1 brush.
			/// </summary>
			/// <value>The background1 brush.</value>
			public Brush Background1Brush { get; set; }

			/// <summary>
			/// Gets or sets the background2 brush.
			/// </summary>
			/// <value>The background2 brush.</value>
			public Brush Background2Brush { get; set; }

			/// <summary>
			/// Gets or sets the foreground brush.
			/// </summary>
			/// <value>The foreground brush.</value>
			public Brush ForegroundBrush { get; set; }

			/// <summary>
			/// Gets or sets the foreground hover brush.
			/// </summary>
			/// <value>The foreground hover brush.</value>
			public Brush ForegroundHoverBrush { get; set; }

			/// <summary>
			/// Gets or sets the radius X.
			/// </summary>
			/// <value>The radius X.</value>
			public double RadiusX { get; set; }

			/// <summary>
			/// Gets or sets the radius Y.
			/// </summary>
			/// <value>The radius Y.</value>
			public double RadiusY { get; set; }

			/// <summary>
			/// Gets or sets the margin.
			/// </summary>
			/// <value>The margin.</value>
			public Thickness Margin { get; set; }

			/// <summary>
			/// Gets or sets the button tooltip to go to to full screen mode.
			/// </summary>
			/// <value>The button tooltip to go to to full screen mode.  Set to null or empty string to disable.</value>
			public string GoToFullTooltip { get; set; }

			/// <summary>
			/// Gets or sets the button tooltip to escape full screen mode.
			/// </summary>
			/// <value>The button tooltip to escape full screen mode.  Set to null or empty string to disable.</value>
			public string EscapeFullTooltip { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="ToggleFullScreenButtonOptions"/> class.
			/// </summary>
			public ToggleFullScreenButtonOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.ToggleFullScreenButton.Enabled;
					Width = DefaultOptions.ToggleFullScreenButton.Width;
					Height = DefaultOptions.ToggleFullScreenButton.Height;
					PathWidth = DefaultOptions.ToggleFullScreenButton.PathWidth;
					PathHeight = DefaultOptions.ToggleFullScreenButton.PathHeight;
					GoToFullPathData = DefaultOptions.ToggleFullScreenButton.GoToFullPathData;
					EscapeFullPathData = DefaultOptions.ToggleFullScreenButton.EscapeFullPathData;
					RadiusX = DefaultOptions.ToggleFullScreenButton.RadiusX;
					RadiusY = DefaultOptions.ToggleFullScreenButton.RadiusY;
					Margin = DefaultOptions.ToggleFullScreenButton.Margin;
					GoToFullTooltip = DefaultOptions.ToggleFullScreenButton.GoToFullTooltip;
					EscapeFullTooltip = DefaultOptions.ToggleFullScreenButton.EscapeFullTooltip;
				}
			}
		}

		/// <summary>
		/// Represents a ToggleEmbedViewButtonOptions
		/// </summary>
		public class ToggleEmbedViewButtonOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="ToggleEmbedViewButtonOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the width of the path.
			/// </summary>
			/// <value>The width of the path.</value>
			public double PathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the path.
			/// </summary>
			/// <value>The height of the path.</value>
			public double PathHeight { get; set; }

			/// <summary>
			/// Gets or sets the path data.
			/// </summary>
			/// <value>The path data.</value>
			public string PathData { get; set; }

			/// <summary>
			/// Gets or sets the background1 brush.
			/// </summary>
			/// <value>The background1 brush.</value>
			public Brush Background1Brush { get; set; }

			/// <summary>
			/// Gets or sets the background2 brush.
			/// </summary>
			/// <value>The background2 brush.</value>
			public Brush Background2Brush { get; set; }

			/// <summary>
			/// Gets or sets the foreground brush.
			/// </summary>
			/// <value>The foreground brush.</value>
			public Brush ForegroundBrush { get; set; }

			/// <summary>
			/// Gets or sets the foreground hover brush.
			/// </summary>
			/// <value>The foreground hover brush.</value>
			public Brush ForegroundHoverBrush { get; set; }

			/// <summary>
			/// Gets or sets the radius X.
			/// </summary>
			/// <value>The radius X.</value>
			public double RadiusX { get; set; }

			/// <summary>
			/// Gets or sets the radius Y.
			/// </summary>
			/// <value>The radius Y.</value>
			public double RadiusY { get; set; }

			/// <summary>
			/// Gets or sets the margin.
			/// </summary>
			/// <value>The margin.</value>
			public Thickness Margin { get; set; }

			/// <summary>
			/// Gets or sets the button tooltip.
			/// </summary>
			/// <value>The button tooltip.  Set to null or empty string to disable.</value>
			public string Tooltip { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="ToggleEmbedViewButtonOptions"/> class.
			/// </summary>
			public ToggleEmbedViewButtonOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.ToggleEmbedViewButton.Enabled;
					Width = DefaultOptions.ToggleEmbedViewButton.Width;
					Height = DefaultOptions.ToggleEmbedViewButton.Height;
					PathWidth = DefaultOptions.ToggleEmbedViewButton.PathWidth;
					PathHeight = DefaultOptions.ToggleEmbedViewButton.PathHeight;
					PathData = DefaultOptions.ToggleEmbedViewButton.PathData;
					RadiusX = DefaultOptions.ToggleEmbedViewButton.RadiusX;
					RadiusY = DefaultOptions.ToggleEmbedViewButton.RadiusY;
					Margin = DefaultOptions.ToggleEmbedViewButton.Margin;
					Tooltip = DefaultOptions.ToggleEmbedViewButton.Tooltip;
				}
			}
		}

		/// <summary>
		/// Represents a SlideDescriptionOptions
		/// </summary>
		public class SlideDescriptionOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="SlideDescriptionOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the title height, in increments of 30.
			/// </summary>
			/// <value>The height of the slide title, in increments of 30.</value>
			public double TitleHeight { get; set; }

			/// <summary>
			/// Gets or sets the title foreground.
			/// </summary>
			/// <value>The title foreground.</value>
			public Brush TitleForeground { get; set; }

			/// <summary>
			/// Gets or sets the title background.
			/// </summary>
			/// <value>The title background.</value>
			public Brush TitleBackground { get; set; }

			/// <summary>
			/// Gets or sets the title background opacity.
			/// </summary>
			/// <value>The title background opacity.</value>
			public double TitleBackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the font of the title.
			/// </summary>
			/// <value>The font of the title.</value>
			public FontFamily TitleFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the title font.
			/// </summary>
			/// <value>The size of the title font.</value>
			public double TitleFontSize { get; set; }

			/// <summary>
			/// Gets or sets the title margin.
			/// </summary>
			/// <value>The title margin.</value>
			public Thickness TitleMargin { get; set; }

			/// <summary>
			/// Gets or sets the description height, in increments of 30.
			/// </summary>
			/// <value>The height of the slide description, in increments of 30.</value>
			public double DescriptionHeight { get; set; }

			/// <summary>
			/// Gets or sets the description foreground.
			/// </summary>
			/// <value>The description foreground.</value>
			public Brush DescriptionForeground { get; set; }

			/// <summary>
			/// Gets or sets the description background.
			/// </summary>
			/// <value>The description background.</value>
			public Brush DescriptionBackground { get; set; }

			/// <summary>
			/// Gets or sets the description background opacity.
			/// </summary>
			/// <value>The description background opacity.</value>
			public double DescriptionBackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the font of the description.
			/// </summary>
			/// <value>The font of the description.</value>
			public FontFamily DescriptionFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the description font.
			/// </summary>
			/// <value>The size of the description font.</value>
			public double DescriptionFontSize { get; set; }

			/// <summary>
			/// Gets or sets the description margin.
			/// </summary>
			/// <value>The description margin.</value>
			public Thickness DescriptionMargin { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The background opacity.</value>
			public double BackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the background radius X.
			/// </summary>
			/// <value>The background radius X.</value>
			public double BackgroundRadiusX { get; set; }

			/// <summary>
			/// Gets or sets the background radius Y.
			/// </summary>
			/// <value>The background radius Y.</value>
			public double BackgroundRadiusY { get; set; }

			/// <summary>
			/// Gets or sets the background margin.
			/// </summary>
			/// <value>The background margin.</value>
			public Thickness BackgroundMargin { get; set; }

			/// <summary>
			/// Gets or sets the close button background1 brush.
			/// </summary>
			/// <value>The close button background1 brush.</value>
            public Brush CloseButtonBackground1Brush { get; set; }

			/// <summary>
			/// Gets or sets the close button background2 brush.
			/// </summary>
			/// <value>The close button background2 brush.</value>
            public Brush CloseButtonBackground2Brush { get; set; }

			/// <summary>
			/// Gets or sets the close button foreground brush.
			/// </summary>
			/// <value>The close button foreground brush.</value>
            public Brush CloseButtonForegroundBrush { get; set; }

			/// <summary>
			/// Gets or sets the close button foreground hover brush.
			/// </summary>
			/// <value>The close button foreground hover brush.</value>
            public Brush CloseButtonForegroundHoverBrush { get; set; }

			/// <summary>
			/// Gets or sets the close button radius X.
			/// </summary>
			/// <value>The close button radius X.</value>
            public double CloseButtonRadiusX { get; set; }

			/// <summary>
			/// Gets or sets the close button radius Y.
			/// </summary>
			/// <value>The close button radius Y.</value>
            public double CloseButtonRadiusY { get; set; }

			/// <summary>
			/// Gets or sets the close button margin.
			/// </summary>
			/// <value>The close button margin.</value>
            public Thickness CloseButtonMargin { get; set; }

			/// <summary>
			/// Gets or sets the width of the close button.
			/// </summary>
			/// <value>The width of the close button.</value>
            public double CloseButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the close button.
			/// </summary>
			/// <value>The height of the close button.</value>
            public double CloseButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the close button path.
			/// </summary>
			/// <value>The width of the close button path.</value>
            public double CloseButtonPathWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the close button path.
			/// </summary>
			/// <value>The height of the close button path.</value>
            public double CloseButtonPathHeight { get; set; }

			/// <summary>
			/// Gets or sets the close button path data.
			/// </summary>
			/// <value>The close button path data.</value>
            public string CloseButtonPathData { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="SlideDescriptionOptions"/> class.
			/// </summary>
			public SlideDescriptionOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.SlideDescription.Enabled;

					TitleHeight = DefaultOptions.SlideDescription.TitleHeight;
					TitleBackgroundOpacity = DefaultOptions.SlideDescription.TitleBackgroundOpacity;
					TitleFontFamily = DefaultOptions.SlideDescription.TitleFontFamily;
					TitleFontSize = DefaultOptions.SlideDescription.TitleFontSize;
					TitleMargin = DefaultOptions.SlideDescription.TitleMargin;

					DescriptionHeight = DefaultOptions.SlideDescription.DescriptionHeight;
					DescriptionBackgroundOpacity = DefaultOptions.SlideDescription.DescriptionBackgroundOpacity;
					DescriptionFontFamily = DefaultOptions.SlideDescription.DescriptionFontFamily;
					DescriptionFontSize = DefaultOptions.SlideDescription.DescriptionFontSize;
					DescriptionMargin = DefaultOptions.SlideDescription.DescriptionMargin;

					BackgroundOpacity = DefaultOptions.SlideDescription.BackgroundOpacity;
					BackgroundRadiusX = DefaultOptions.SlideDescription.BackgroundRadiusX;
					BackgroundRadiusY = DefaultOptions.SlideDescription.BackgroundRadiusY;
					BackgroundMargin = DefaultOptions.SlideDescription.BackgroundMargin;

                    CloseButtonRadiusX = DefaultOptions.EmbedViewer.CloseButtonRadiusX;
                    CloseButtonRadiusY = DefaultOptions.EmbedViewer.CloseButtonRadiusY;
                    CloseButtonMargin = DefaultOptions.EmbedViewer.CloseButtonMargin;
                    CloseButtonWidth = DefaultOptions.EmbedViewer.CloseButtonWidth;
                    CloseButtonHeight = DefaultOptions.EmbedViewer.CloseButtonHeight;
                    CloseButtonPathWidth = DefaultOptions.EmbedViewer.CloseButtonPathWidth;
                    CloseButtonPathHeight = DefaultOptions.EmbedViewer.CloseButtonPathHeight;
				}
			}
		}

		/// <summary>
		/// Represents an AlbumViewerOptions
		/// </summary>
		public class AlbumViewerOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="AlbumViewerOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the scroll button foreground.
			/// </summary>
			/// <value>The scroll button foreground.</value>
			public Brush ScrollButtonForeground { get; set; }

			/// <summary>
			/// Gets or sets the scroll button foreground hover.
			/// </summary>
			/// <value>The scroll button foreground hover.</value>
			public Brush ScrollButtonForegroundHover { get; set; }

			/// <summary>
			/// Gets or sets the width of the scroll button.
			/// </summary>
			/// <value>The width of the scroll button.</value>
			public double ScrollButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the scroll button.
			/// </summary>
			/// <value>The height of the scroll button.</value>
			public double ScrollButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the scroll button margin.
			/// </summary>
			/// <value>The scroll button margin.</value>
			public Thickness ScrollButtonMargin { get; set; }

			/// <summary>
			/// Gets or sets the left scroll button data.
			/// </summary>
			/// <value>The left scroll button data.</value>
			public string LeftScrollButtonData { get; set; }

			/// <summary>
			/// Gets or sets the right scroll button data.
			/// </summary>
			/// <value>The right scroll button data.</value>
			public string RightScrollButtonData { get; set; }

			/// <summary>
			/// Gets or sets the font of the page number.
			/// </summary>
			/// <value>The font of the page number.</value>
			public FontFamily PageNumberFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the page number font.
			/// </summary>
			/// <value>The size of the page number font.</value>
			public double PageNumberFontSize { get; set; }

			/// <summary>
			/// Gets or sets the page number foreground.
			/// </summary>
			/// <value>The page number foreground.</value>
			public Brush PageNumberForeground { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="AlbumViewerOptions"/> class.
			/// </summary>
			public AlbumViewerOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.AlbumViewer.Enabled;
					ScrollButtonWidth = DefaultOptions.AlbumViewer.ScrollButtonWidth;
					ScrollButtonHeight = DefaultOptions.AlbumViewer.ScrollButtonHeight;
					ScrollButtonMargin = DefaultOptions.AlbumViewer.ScrollButtonMargin;
					LeftScrollButtonData = DefaultOptions.AlbumViewer.LeftScrollButtonData;
					RightScrollButtonData = DefaultOptions.AlbumViewer.RightScrollButtonData;
					PageNumberFontFamily = DefaultOptions.AlbumViewer.PageNumberFontFamily;
					PageNumberFontSize = DefaultOptions.AlbumViewer.PageNumberFontSize;
				}
			}
		}

		/// <summary>
		/// Represents a ThumbnailViewerOptions
		/// </summary>
		public class ThumbnailViewerOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="ThumbnailViewerOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The background opacity.</value>
			public double BackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the background radius X.
			/// </summary>
			/// <value>The background radius X.</value>
			public double BackgroundRadiusX { get; set; }

			/// <summary>
			/// Gets or sets the background radius Y.
			/// </summary>
			/// <value>The background radius Y.</value>
			public double BackgroundRadiusY { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the margin.
			/// </summary>
			/// <value>The margin.</value>
			public Thickness Margin { get; set; }

			/// <summary>
			/// Gets or sets the thumb spacing.
			/// </summary>
			/// <value>The thumb spacing.</value>
			public int ThumbSpacing { get; set; }

			/// <summary>
			/// Gets or sets the scroll increment.
			/// </summary>
			/// <value>The scroll increment.</value>
			public double ScrollIncrement { get; set; }

			/// <summary>
			/// Gets or sets the scroll button brush.
			/// </summary>
			/// <value>The scroll button brush.</value>
			public Brush ScrollButtonBrush { get; set; }

			/// <summary>
			/// Gets or sets the scroll button hover brush.
			/// </summary>
			/// <value>The scroll button hover brush.</value>
			public Brush ScrollButtonHoverBrush { get; set; }

			/// <summary>
			/// Gets or sets the width of the scroll button.
			/// </summary>
			/// <value>The width of the scroll button.</value>
			public double ScrollButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the scroll button.
			/// </summary>
			/// <value>The height of the scroll button.</value>
			public double ScrollButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the scroll button margin.
			/// </summary>
			/// <value>The scroll button margin.</value>
			public Thickness ScrollButtonMargin { get; set; }

			/// <summary>
			/// Gets or sets the scroll repeat button interval.
			/// </summary>
			/// <value>The scroll repeat button interval.</value>
			public int ScrollRepeatButtonInterval { get; set; }

			/// <summary>
			/// Gets or sets the left scroll button data.
			/// </summary>
			/// <value>The left scroll button data.</value>
			public string LeftScrollButtonData { get; set; }

			/// <summary>
			/// Gets or sets the right scroll button data.
			/// </summary>
			/// <value>The right scroll button data.</value>
			public string RightScrollButtonData { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="ThumbnailViewerOptions"/> class.
			/// </summary>
			public ThumbnailViewerOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.ThumbnailViewer.Enabled;
					BackgroundOpacity = DefaultOptions.ThumbnailViewer.BackgroundOpacity;
					BackgroundRadiusX = DefaultOptions.ThumbnailViewer.BackgroundRadiusX;
					BackgroundRadiusY = DefaultOptions.ThumbnailViewer.BackgroundRadiusY;
					Width = DefaultOptions.ThumbnailViewer.Width;
					Margin = DefaultOptions.ThumbnailViewer.Margin;
					ThumbSpacing = DefaultOptions.ThumbnailViewer.ThumbSpacing;
					ScrollIncrement = DefaultOptions.ThumbnailViewer.ScrollIncrement;
					ScrollButtonWidth = DefaultOptions.ThumbnailViewer.ScrollButtonWidth;
					ScrollButtonHeight = DefaultOptions.ThumbnailViewer.ScrollButtonHeight; 
					ScrollButtonMargin = DefaultOptions.ThumbnailViewer.ScrollButtonMargin;
					ScrollRepeatButtonInterval = DefaultOptions.ThumbnailViewer.ScrollRepeatButtonInterval;
					LeftScrollButtonData = DefaultOptions.ThumbnailViewer.LeftScrollButtonData;
					RightScrollButtonData = DefaultOptions.ThumbnailViewer.RightScrollButtonData;
				}
			}
		}

		/// <summary>
		/// Represents a SlideThumbnailOptions
		/// </summary>
		public class SlideThumbnailOptions
		{
			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the border brush.
			/// </summary>
			/// <value>The border brush.</value>
			public Brush BorderBrush { get; set; }

			/// <summary>
			/// Gets or sets the border highlight brush.
			/// </summary>
			/// <value>The border highlight brush.</value>
			public Brush BorderHighlightBrush { get; set; }

			/// <summary>
			/// Gets or sets the border thickness.
			/// </summary>
			/// <value>The border thickness.</value>
			public Thickness BorderThickness { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="SlideThumbnailOptions"/> class.
			/// </summary>
			public SlideThumbnailOptions()
			{
				if (DefaultOptions != null)
				{
					Height = DefaultOptions.SlideThumbnail.Height;
					BorderThickness = DefaultOptions.SlideThumbnail.BorderThickness;
				}
			}
		}

		/// <summary>
		/// Represents an AlbumPageOptions
		/// </summary>
		public class AlbumPageOptions
		{
			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }
		}

		/// <summary>
		/// Represents an AlbumButtonOptions
		/// </summary>
		public class AlbumButtonOptions
		{
			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Gets or sets the background hover.
			/// </summary>
			/// <value>The background hover.</value>
			public Brush BackgroundHover { get; set; }

			/// <summary>
			/// Gets or sets the font of the title.
			/// </summary>
			/// <value>The font of the title.</value>
			public FontFamily TitleFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the title font.
			/// </summary>
			/// <value>The size of the title font.</value>
			public double TitleFontSize { get; set; }

			/// <summary>
			/// Gets or sets the title foreground.
			/// </summary>
			/// <value>The title foreground.</value>
			public Brush TitleForeground { get; set; }

			/// <summary>
			/// Gets or sets the title margin.
			/// </summary>
			/// <value>The title margin.</value>
			public Thickness TitleMargin { get; set; }

			/// <summary>
			/// Gets or sets the font of the description.
			/// </summary>
			/// <value>The font of the description.</value>
			public FontFamily DescriptionFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the description font.
			/// </summary>
			/// <value>The size of the description font.</value>
			public double DescriptionFontSize { get; set; }

			/// <summary>
			/// Gets or sets the description foreground.
			/// </summary>
			/// <value>The description foreground.</value>
			public Brush DescriptionForeground { get; set; }

			/// <summary>
			/// Gets or sets the description margin.
			/// </summary>
			/// <value>The description margin.</value>
			public Thickness DescriptionMargin { get; set; }

			/// <summary>
			/// Gets or sets the width.
			/// </summary>
			/// <value>The width.</value>
			public double Width { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the padding.
			/// </summary>
			/// <value>The padding.</value>
			public double Padding { get; set; }

			/// <summary>
			/// Gets or sets the background radius X.
			/// </summary>
			/// <value>The background radius X.</value>
			public double BackgroundRadiusX { get; set; }

			/// <summary>
			/// Gets or sets the background radius Y.
			/// </summary>
			/// <value>The background radius Y.</value>
			public double BackgroundRadiusY { get; set; }

			/// <summary>
			/// Gets or sets the width of the thumbnail.
			/// </summary>
			/// <value>The width of the thumbnail.</value>
			public double ThumbnailWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the thumbnail.
			/// </summary>
			/// <value>The height of the thumbnail.</value>
			public double ThumbnailHeight { get; set; }

			/// <summary>
			/// Gets or sets the thumbnail radius X.
			/// </summary>
			/// <value>The thumbnail radius X.</value>
			public double ThumbnailRadiusX { get; set; }

			/// <summary>
			/// Gets or sets the thumbnail radius Y.
			/// </summary>
			/// <value>The thumbnail radius Y.</value>
			public double ThumbnailRadiusY { get; set; }

			/// <summary>
			/// Gets or sets the thumbnail margin.
			/// </summary>
			/// <value>The thumbnail margin.</value>
			public Thickness ThumbnailMargin { get; set; }

			/// <summary>
			/// Gets or sets the thumbnail border stroke.
			/// </summary>
			/// <value>The thumbnail border stroke.</value>
			public Brush ThumbnailBorderStroke { get; set; }

			/// <summary>
			/// Gets or sets the thumbnail border thickness.
			/// </summary>
			/// <value>The thumbnail border thickness.</value>
			public Thickness ThumbnailBorderThickness { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="AlbumButtonOptions"/> class.
			/// </summary>
			public AlbumButtonOptions()
			{
				if (DefaultOptions != null)
				{
					Width = DefaultOptions.AlbumButton.Width;
					Height = DefaultOptions.AlbumButton.Height;

					TitleFontFamily = DefaultOptions.AlbumButton.TitleFontFamily;
					TitleFontSize = DefaultOptions.AlbumButton.TitleFontSize;
					TitleMargin = DefaultOptions.AlbumButton.TitleMargin;

					DescriptionFontFamily = DefaultOptions.AlbumButton.DescriptionFontFamily;
					DescriptionFontSize = DefaultOptions.AlbumButton.DescriptionFontSize;
					DescriptionMargin = DefaultOptions.AlbumButton.DescriptionMargin;

					Padding = DefaultOptions.AlbumButton.Padding;
					BackgroundRadiusX = DefaultOptions.AlbumButton.BackgroundRadiusX;
					BackgroundRadiusY = DefaultOptions.AlbumButton.BackgroundRadiusY;
					ThumbnailMargin = DefaultOptions.AlbumButton.ThumbnailMargin;

					ThumbnailRadiusX = DefaultOptions.AlbumButton.ThumbnailRadiusX;
					ThumbnailRadiusY = DefaultOptions.AlbumButton.ThumbnailRadiusY;
					ThumbnailWidth = DefaultOptions.AlbumButton.ThumbnailWidth;
					ThumbnailHeight = DefaultOptions.AlbumButton.ThumbnailHeight;
					ThumbnailBorderThickness = DefaultOptions.AlbumButton.ThumbnailBorderThickness;
				}
			}
		}

		/// <summary>
		/// Represents a SlidePreviewOptions
		/// </summary>
		public class SlidePreviewOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="SlidePreviewOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
			public double Height { get; set; }

			/// <summary>
			/// Gets or sets the border brush.
			/// </summary>
			/// <value>The border brush.</value>
			public Brush BorderBrush { get; set; }

			/// <summary>
			/// Gets or sets the width of the border.
			/// </summary>
			/// <value>The width of the border.</value>
			public double BorderWidth { get; set; }

			/// <summary>
			/// Gets or sets the radius X.
			/// </summary>
			/// <value>The radius X.</value>
			public double RadiusX { get; set; }

			/// <summary>
			/// Gets or sets the radius Y.
			/// </summary>
			/// <value>The radius Y.</value>
			public double RadiusY { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="SlidePreviewOptions"/> class.
			/// </summary>
			public SlidePreviewOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.SlidePreview.Enabled;
					Height = DefaultOptions.SlidePreview.Height;
					BorderWidth = DefaultOptions.SlidePreview.BorderWidth;
					RadiusX = DefaultOptions.SlidePreview.RadiusX;
					RadiusY = DefaultOptions.SlidePreview.RadiusY;
				}
			}
		}

		/// <summary>
		/// Represents a SlideViewerOptions
		/// </summary>
		public class SlideViewerOptions
		{
			public Stretch Stretch { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="SlideViewerOptions"/> class.
			/// </summary>
			public SlideViewerOptions()
			{
				if (DefaultOptions != null)
				{
                    Stretch = DefaultOptions.SlideViewer.Stretch;
				}
			}
		}

		/// <summary>
		/// Represents an ImageViewerOptions
		/// </summary>
		public class ImageViewerOptions
		{
		}

		/// <summary>
		/// Represents a VideoViewerOptions
		/// </summary>
		public class VideoViewerOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether videos will auto play <see cref="VideoTrayOptions"/>
            /// </summary>
            /// <value><c>true</c> if the video is to autoplay; otherwise, <c>false</c>.</value>
            public bool AutoPlay { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="VideoViewerOptions"/> class.
			/// </summary>
            public VideoViewerOptions()
			{
				if (DefaultOptions != null)
				{
                    AutoPlay = DefaultOptions.VideoViewer.AutoPlay;
				}
			}
		}

		/// <summary>
		/// Represents a VideoTrayOptions
		/// </summary>
		public class VideoTrayOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="VideoTrayOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the height.
			/// </summary>
			/// <value>The height.</value>
            public double Height { get; set; }

            /// <summary>
            /// Gets or sets the width.
            /// </summary>
            /// <value>The width.</value>
            public double Width { get; set; }

            /// <summary>
            /// Gets or sets the margin.
            /// </summary>
            /// <value>The margin.</value>
            public Thickness Margin { get; set; }

			/// <summary>
			/// Gets or sets the foreground.
			/// </summary>
			/// <value>The foreground.</value>
			public Brush Foreground { get; set; }

			/// <summary>
			/// Gets or sets the foreground hover.
			/// </summary>
			/// <value>The foreground hover.</value>
			public Brush ForegroundHover { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The background opacity.</value>
			public double BackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the radius X.
			/// </summary>
			/// <value>The radius X.</value>
			public double RadiusX { get; set; }

			/// <summary>
			/// Gets or sets the radius Y.
			/// </summary>
			/// <value>The radius Y.</value>
			public double RadiusY { get; set; }

			/// <summary>
			/// Gets or sets the width of the volume.
			/// </summary>
			/// <value>The width of the volume.</value>
			public double VolumeWidth { get; set; }

			/// <summary>
			/// Gets or sets the play path data.
			/// </summary>
			/// <value>The play path data.</value>
			public string PlayPathData { get; set; }

			/// <summary>
			/// Gets or sets the pause path data.
			/// </summary>
			/// <value>The pause path data.</value>
			public string PausePathData { get; set; }

			/// <summary>
			/// Gets or sets the height of the play pause button.
			/// </summary>
			/// <value>The height of the play pause button.</value>
			public double PlayPauseButtonHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the play pause button.
			/// </summary>
			/// <value>The width of the play pause button.</value>
			public double PlayPauseButtonWidth { get; set; }

			/// <summary>
			/// Gets or sets the height of the play pause button path.
			/// </summary>
			/// <value>The height of the play pause button path.</value>
			public double PlayPauseButtonPathHeight { get; set; }

			/// <summary>
			/// Gets or sets the width of the play pause button path.
			/// </summary>
			/// <value>The width of the play pause button path.</value>
			public double PlayPauseButtonPathWidth { get; set; }

			/// <summary>
			/// Gets or sets the font of the text.
			/// </summary>
			/// <value>The font of the text.</value>
			public FontFamily TextFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the text font.
			/// </summary>
			/// <value>The size of the text font.</value>
			public double TextFontSize { get; set; }

			/// <summary>
			/// Gets or sets the text foreground brush.
			/// </summary>
			/// <value>The text foreground brush.</value>
			public Brush TextForegroundBrush { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="VideoTrayOptions"/> class.
			/// </summary>
			public VideoTrayOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.VideoTray.Enabled;
                    Width = DefaultOptions.VideoTray.Width;
                    Height = DefaultOptions.VideoTray.Height;
                    Margin = DefaultOptions.VideoTray.Margin;
					PlayPauseButtonHeight = DefaultOptions.VideoTray.PlayPauseButtonHeight;
					PlayPauseButtonWidth = DefaultOptions.VideoTray.PlayPauseButtonWidth;
					PlayPauseButtonPathHeight = DefaultOptions.VideoTray.PlayPauseButtonPathHeight;
					PlayPauseButtonPathWidth = DefaultOptions.VideoTray.PlayPauseButtonPathWidth;
					BackgroundOpacity = DefaultOptions.VideoTray.BackgroundOpacity;
					RadiusX = DefaultOptions.VideoTray.RadiusX;
					RadiusY = DefaultOptions.VideoTray.RadiusY;
					TextFontFamily = DefaultOptions.VideoTray.TextFontFamily;
					TextFontSize = DefaultOptions.VideoTray.TextFontSize;
					VolumeWidth = DefaultOptions.VideoTray.VolumeWidth;
				}
			}
		}


		/// <summary>
		/// Represents a PeopleTagOptions
		/// </summary>
		public class PeopleTagOptions
		{
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="PeopleTagOptions"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get; set; }

			/// <summary>
			/// Gets or sets the border brush.
			/// </summary>
			/// <value>The border brush.</value>
			public Brush BackgroundBorderBrush { get; set; }

			/// <summary>
			/// Gets or sets the border thickness.
			/// </summary>
			/// <value>The border thickness.</value>
			public Thickness BackgroundBorderThickness { get; set; }

			/// <summary>
			/// Gets or sets the background.
			/// </summary>
			/// <value>The background.</value>
			public Brush Background { get; set; }

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The background opacity.</value>
			public double BackgroundOpacity { get; set; }

			/// <summary>
			/// Gets or sets the background corner radius.
			/// </summary>
			/// <value>The background corner radius.</value>
			public CornerRadius BackgroundCornerRadius { get; set; }

			/// <summary>
			/// Gets or sets the name foreground.
			/// </summary>
			/// <value>The name foreground.</value>
			public Brush PersonNameForeground { get; set; }

			/// <summary>
			/// Gets or sets the font of the name.
			/// </summary>
			/// <value>The font of the name.</value>
			public FontFamily PersonNameFontFamily { get; set; }

			/// <summary>
			/// Gets or sets the size of the name font.
			/// </summary>
			/// <value>The size of the name font.</value>
			public double PersonNameFontSize { get; set; }

			/// <summary>
			/// Gets or sets the name margin.
			/// </summary>
			/// <value>The name margin.</value>
			public Thickness PersonNameMargin { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="PeopleTagOptions"/> class.
			/// </summary>
			public PeopleTagOptions()
			{
				if (DefaultOptions != null)
				{
					Enabled = DefaultOptions.SlideDescription.Enabled;

					PersonNameForeground = DefaultOptions.PeopleTag.PersonNameForeground;
					PersonNameFontFamily = DefaultOptions.PeopleTag.PersonNameFontFamily;
					PersonNameFontSize = DefaultOptions.PeopleTag.PersonNameFontSize;
					PersonNameMargin = DefaultOptions.PeopleTag.PersonNameMargin;

					Background = DefaultOptions.PeopleTag.Background;
					BackgroundBorderBrush = DefaultOptions.PeopleTag.BackgroundBorderBrush;
					BackgroundBorderThickness = DefaultOptions.PeopleTag.BackgroundBorderThickness;
					BackgroundCornerRadius = DefaultOptions.PeopleTag.BackgroundCornerRadius;
					BackgroundOpacity = DefaultOptions.PeopleTag.BackgroundOpacity;
				}
			}
		}
	}		
}