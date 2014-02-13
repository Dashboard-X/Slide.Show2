using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Represents a DarkTheme
	/// </summary>
	public class DarkTheme : ConfigurationProvider
	{
		/// <summary>
		/// Occurs when [options set].
		/// </summary>
		protected override event EventHandler OptionsSet;

		/// <summary>
		/// Initializes a new instance of the <see cref="DarkTheme"/> class.
		/// </summary>
		/// <param name="initParams">The init params.</param>
		public DarkTheme(Dictionary<string, Dictionary<string, string>> initParams)
		{
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		protected override void GetConfiguration()
		{
			#region [ General ]
			Options.General = new Options.GeneralOptions()
			{
				Background = ParseBrush("Black"),
				AutoStart = true,
				StartInAlbumView = false
			};
			#endregion

			#region [ Transition ]
			Options.Transition = new Options.TransitionOptions()
			{
				TransitionDuration = 500,
				WaitTime = 3000
			};
			#endregion

			#region [ DataProvider ]
			Options.DataProvider = new Options.DataProviderOptions()
			{
			};
			#endregion

			#region [ Preloader ]
			Options.Preloader = new Options.PreloaderOptions()
			{
				PreloadAhead = 5
			};
			#endregion

			#region [ LoadingProgressIndicator ]
			Options.LoadingProgressIndicator = new Options.LoadingProgressIndicatorOptions()
			{
				Enabled = true,
				Type = "bar",
				Height = 3,
				Width = 75,
				Foreground = ParseBrush("White"),
				Background = ParseBrush("Black")
			};
			#endregion

			#region [ SlideNavigation ]
			Options.SlideNavigation = new Options.SlideNavigationOptions()
			{
				Enabled = true,
				PlayPathData = "F1 M 101.447,284.834L 101.447,274.714L 106.906,279.774L 101.447,284.834 Z",
				PausePathData = "M 100.048,736.889L 98.5482,736.889C 97.9959,736.889 97.5482,736.441 97.5482,735.889L 97.5482,727.889C 97.5482,727.337 97.9959,726.889 98.5482,726.889L 100.048,726.889C 100.6,726.889 101.048,727.337 101.048,727.889L 101.048,735.889C 101.048,736.441 100.6,736.889 100.048,736.889 Z M 106.922,736.889L 105.422,736.889C 104.87,736.889 104.422,736.441 104.422,735.889L 104.422,727.889C 104.422,727.337 104.87,726.889 105.422,726.889L 106.922,726.889C 107.475,726.889 107.922,727.337 107.922,727.889L 107.922,735.889C 107.922,736.441 107.475,736.889 106.922,736.889 Z",
				PreviousPathData = "F1 M6.0000005,1.473075E-06 L6.0000005,10.000002 5.9604696E-07,5.0000014 6.0000005,1.473075E-06 z M-1.3709068E-06,4.0019114E-07 L-1.3709068E-06,10.000006 -6.0000029,5.0000029 -1.3709068E-06,4.0019114E-07 Z",
				NextPathData = "F1 M-5.9999976,1.0552602E-05 L-1.9073478E-06,4.9999938 -1.9073478E-06,1.8062785E-05 5.9999981,5.0000033 -1.9073478E-06,10.000018 -1.9073478E-06,5.0000024 -5.9999976,9.9999857 -5.9999976,1.0552602E-05 Z",
				Foreground = ParseBrush("#F2C29F"),
				ForegroundHover = ParseBrush("White"),
				Background1Brush = ParseBrush("#7F2D12"),
				Background2Brush = ParseBrush("#E9A16B"),
				RadiusX = 4,
				RadiusY = 4,
				Width = 66,
				Height = 20,

				PlayPauseButtonWidth = 22,
				PlayPauseButtonHeight = 20,
				NextButtonWidth = 22,
				NextButtonHeight = 20,
				PreviousButtonWidth = 22,
				PreviousButtonHeight = 20,

				PlayPauseButtonPathWidth = 10,
				PlayPauseButtonPathHeight = 12,
				NextButtonPathWidth = 10,
				NextButtonPathHeight = 8,
				PreviousButtonPathWidth = 10,
				PreviousButtonPathHeight = 8
			};
			#endregion

			#region [ EmbedViewer ]
			Options.EmbedViewer = new Options.EmbedViewerOptions()
			{
				Enabled = true,
				Background = ParseBrush("Black"),
				BackgroundOpacity = .3,
                CornerRadius = new CornerRadius(0),

				DialogBackground = ParseBrush("#3C3C3C"),
				DialogBackgroundOpacity = .8,
				DialogBorderBrush = ParseBrush("Black"),
				DialogBorderThickness = new Thickness(2),
				DialogWidth = 620,
				DialogHeight = 225,

				CloseButtonBackground1Brush = ParseBrush("Transparent"),
				CloseButtonBackground2Brush = ParseBrush("Transparent"),
				CloseButtonForegroundBrush = ParseBrush("#8D9AB4"),
				CloseButtonForegroundHoverBrush = ParseBrush("White"),
				CloseButtonRadiusX = 4,
				CloseButtonRadiusY = 4,
				CloseButtonMargin = new Thickness(3),
				CloseButtonWidth = 15,
				CloseButtonHeight = 15,
				CloseButtonPathWidth = 10,
				CloseButtonPathHeight = 10,
				CloseButtonPathData = "M59.156,59.156 L81.073,81.073 C81.073,81.073 82.635,85.167 81.07,86.729 L26.573,141.228 L84.072,198.728 C85.634,200.290 85.634,202.822 84.072,204.384 L62.155,226.301 C60.593,227.863 58.061,227.863 56.499,226.301 L-1.000,168.801 L-57.499,225.301 C-59.061,226.863 -61.593,226.863 -63.155,225.301 L-85.072,203.384 C-86.634,201.822 -86.634,199.290 -85.072,197.728 L-28.573,141.228 L-82.073,87.729 C-83.635,86.167 -83.635,83.635 -82.073,82.073 L-60.156,60.156 C-58.594,58.594 -56.062,58.594 -54.500,60.156 L-1.000,113.655 L53.500,59.156 C55.062,57.594 57.594,57.594 59.156,59.156 z",

				HyperlinkButtonText = "Copy to clipboard",
				HyperlinkButtonForeground = ParseBrush("White"),
				HyperlinkButtonBackground = ParseBrush("#3C3C3C"),
				HyperlinkButtonMargin = new Thickness(12),
				HyperlinkButtonWidth = 200,
				HyperlinkButtonHeight = 20,
				HyperlinkButtonFontFamily = new FontFamily("Portable User Interface"),
				HyperlinkButtonFontSize = 14,

				TextBlockFontFamily = new FontFamily("Portable User Interface"),
				TextBlockFontSize = 14,
				TitleText = "Use this HTML to embed this slideshow into your webpage:",
				TextBlockMargin = new Thickness(12),
				TextBlockForeground = ParseBrush("White"),

				TextBoxWidth = 600,
				TextBoxHeight = 120,
				TextBoxBackground = ParseBrush("White"),
				TextBoxForeground = ParseBrush("Black"),
				TextBoxFontFamily = new FontFamily("Portable User Interface"),
				TextBoxFontSize = 11,
			};
			#endregion

			#region [ NavigationTray ]
			Options.NavigationTray = new Options.NavigationTrayOptions()
			{
				Enabled = true,
				BackgroundOpacity = 1,
				Background = ParseBrush("#3C3C3C")
			};
			#endregion

			#region [ ToggleAlbumViewButton ]
			Options.ToggleAlbumViewButton = new Options.ToggleAlbumViewButtonOptions()
			{
				Enabled = true,
				Width = 22,
				Height = 20,
				PathWidth = 11,
				PathHeight = 11,
				PathData = "M0.3068684,6.6970766E-23 L4.8691305,6.6970766E-23 C5.0380702,3.6356173E-08 5.1759998,0.13792887 5.1759998,0.30690225 L5.1759998,2.6368366 C5.1759998,2.8058099 5.0380702,2.9437565 4.8691305,2.9437565 L0.3068684,2.9437565 C0.13680684,2.9437565 1.2251062E-13,2.8058099 -3.3276147E-30,2.6368366 L-3.3276147E-30,0.30690225 C1.2251062E-13,0.13792887 0.13680684,3.6356173E-08 0.3068684,6.6970766E-23 z M6.5989051,3.4025645E-07 L11.162244,3.4025645E-07 C11.331214,2.5640611E-07 11.468,0.13793494 11.468,0.30690496 L11.468,2.6368515 C11.468,2.8058217 11.331214,2.9437562 11.162244,2.9437562 L6.5989051,2.9437562 C6.4299352,2.9437562 6.2920002,2.8058217 6.2920002,2.6368515 L6.2920002,0.30690496 C6.2920002,0.13793494 6.4299352,2.5640611E-07 6.5989051,3.4025645E-07 z M0.30690471,4.0560001 L4.8690952,4.0560001 C5.0380656,4.0560001 5.1760002,4.1927856 5.1760002,4.3617556 L5.1760002,6.6928522 C5.1760002,6.861822 5.0380656,6.9997566 4.8690952,6.9997566 L0.30690471,6.9997566 C0.1367853,6.9997566 4.0662732E-08,6.861822 -3.3276153E-30,6.6928522 L-3.3276153E-30,4.3617556 C4.0662732E-08,4.1927856 0.1367853,4.0560001 0.30690471,4.0560001 z M6.5989051,4.0560005 L11.162244,4.0560005 C11.331214,4.0560005 11.468,4.192786 11.468,4.3617559 L11.468,6.6928519 C11.468,6.8618216 11.331214,6.9997562 11.162244,6.9997562 L6.5989051,6.9997562 C6.4299352,6.9997562 6.2920002,6.8618216 6.2920002,6.6928519 L6.2920002,4.3617559 C6.2920002,4.192786 6.4299352,4.0560005 6.5989051,4.0560005 z M0.30690471,8.1120001 L4.8690952,8.1120001 C5.0380656,8.1120001 5.1760002,8.2487856 5.1760002,8.4177556 L5.1760002,10.748852 C5.1760002,10.917822 5.0380656,11.055757 4.8690952,11.055757 L0.30690471,11.055757 C0.1367853,11.055757 4.0662732E-08,10.917822 -3.3276153E-30,10.748852 L-3.3276153E-30,8.4177556 C4.0662732E-08,8.2487856 0.1367853,8.1120001 0.30690471,8.1120001 z M6.5989051,8.1120005 L11.162244,8.1120005 C11.331214,8.1120005 11.468,8.248786 11.468,8.4177559 L11.468,10.748852 C11.468,10.917822 11.331214,11.055756 11.162244,11.055756 L6.5989051,11.055756 C6.4299352,11.055756 6.2920002,10.917822 6.2920002,10.748852 L6.2920002,8.4177559 C6.2920002,8.248786 6.4299352,8.1120005 6.5989051,8.1120005 Z",
				Background1Brush = ParseBrush("#273B5B"),
				Background2Brush = ParseBrush("#6A75A2"),
				ForegroundBrush = ParseBrush("#8D9AB4"),
				ForegroundHoverBrush = ParseBrush("White"),
				RadiusX = 4,
				RadiusY = 4,
				Margin = new Thickness(0, 0, 5, 0),
				Tooltip = "Toggle album view"
			};
			#endregion

			#region [ ToggleFullScreenButton ]
			Options.ToggleFullScreenButton = new Options.ToggleFullScreenButtonOptions()
			{
				Enabled = true,
				Width = 22,
				Height = 20,
				PathWidth = 12,
				PathHeight = 10,
				GoToFullPathData = "M 7.90548,1.32341L 14.6458,1.32341L 14.6458,8.02661L 12.3811,5.78146L 8.43281,9.72004L 6.21009,7.42345L 10.1668,3.57935L 7.90548,1.32341 Z M -1.60064e-007,1.35265L 6.66667,1.35265L 6.66667,2.68583L 1.32284,2.68583L 1.32284,13.3317L 13.323,13.3317L 13.323,9.33174L 14.6562,9.33174L 14.6562,13.6585L 14.6458,13.6585L 14.6458,14.6963L -8.30604e-007,14.6963L -2.59563e-008,14.6651L -8.30604e-007,13.3317L -1.60064e-007,2.68583L -2.59563e-008,2.32351L -1.60064e-007,1.35265 Z",
				EscapeFullPathData = "M 12.9504,9.72004L 6.21009,9.72004L 6.21009,3.01684L 8.47481,5.26199L 12.4231,1.32341L 14.6458,3.62L 10.6891,7.4641L 12.9504,9.72004 Z M 3.51898e-007,1.35265L 6.66667,1.35265L 6.66667,2.68583L 1.32284,2.68583L 1.32284,13.3317L 13.323,13.3317L 13.323,9.33174L 14.6562,9.33174L 14.6562,13.6585L 14.6458,13.6585L 14.6458,14.6963L -1.79383e-006,14.6963L 3.51898e-007,14.6651L -1.79383e-006,13.3317L 3.51898e-007,2.68583L 3.51898e-007,2.32351L 3.51898e-007,1.35265 Z",
				Background1Brush = ParseBrush("#273B5B"),
				Background2Brush = ParseBrush("#6A75A2"),
				ForegroundBrush = ParseBrush("#8D9AB4"),
				ForegroundHoverBrush = ParseBrush("White"),
				RadiusX = 4,
				RadiusY = 4,
				Margin = new Thickness(0),
				GoToFullTooltip = "Switch to full-screen mode",
				EscapeFullTooltip = "Exit full-screen mode"
			};
			#endregion

			#region [ ToggleEmbedViewButton ]
			Options.ToggleEmbedViewButton = new Options.ToggleEmbedViewButtonOptions()
			{
				Enabled = true,
				Width = 22,
				Height = 20,
				PathWidth = 13,
				PathHeight = 11,
				PathData = "M14.863486,7.5777659 L17.268641,8.2222252 L14.359774,19.078266 L11.954618,18.433807 L14.863486,7.5777659 z M21.529634,7.5262961 L27.602283,13.599229 L26.256895,14.944682 L26.256739,14.944526 L21.529394,19.672092 L20.184,18.326633 L24.911343,13.599068 L20.184246,8.871748 L21.529634,7.5262961 z",
				Background1Brush = ParseBrush("#273B5B"),
				Background2Brush = ParseBrush("#6A75A2"),
				ForegroundBrush = ParseBrush("#8D9AB4"),
				ForegroundHoverBrush = ParseBrush("White"),
				RadiusX = 4,
				RadiusY = 4,
				Margin = new Thickness(0, 0, 5, 0),
				Tooltip = "View html to embed this slideshow into your webpage"
			};
			#endregion

			#region [ ToggleSaveButton ]
			Options.ToggleSaveButton = new Options.ToggleSaveButtonOptions()
			{
				Enabled = false,
				Width = 22,
				Height = 20,
				PathWidth = 11,
				PathHeight = 11,
				PathData = "M13.303991,13.31762 L1.3528529,13.379672 L1.3085877,2.6726847 L4.4928222,2.6820953 L4.5800614,8.2556801 L10.643193,8.1718283 L10.643193,2.6820953 L13.391231,2.6328566 L13.303991,7.8029037 z M3.51898E-07,1.35265 L13.378687,1.3034161 L13.378687,2.6365962 L1.32284,2.6858301 L1.3664342,13.469897 L5.4088349,13.416098 L5.4112124,10.070662 L7.8303814,10.067492 L7.895155,13.465336 L13.347611,13.514574 L13.317558,1.3034161 L14.650759,1.3034161 L14.6562,13.6585 L14.6458,13.6585 L14.6458,14.6963 L-1.7938301E-06,14.6963 L3.51898E-07,14.6651 L-1.7938301E-06,13.3317 L3.51898E-07,2.6858301 L3.51898E-07,2.3235099 L3.51898E-07,1.35265 z",
				Background1Brush = ParseBrush("#273B5B"),
				Background2Brush = ParseBrush("#6A75A2"),
				ForegroundBrush = ParseBrush("#8D9AB4"),
				ForegroundHoverBrush = ParseBrush("White"),
				RadiusX = 4,
				RadiusY = 4,
				Margin = new Thickness(0, 0, 5, 0),
				DownloadImageHandler = "http://www.yourDomainHere.com/your_Path_Here/DownloadImage.ashx?image=", //Absolute URI to image download handler
				Tooltip = "Download the current image"
			};
			#endregion

			#region [ SlideDescription ]
			Options.SlideDescription = new Options.SlideDescriptionOptions()
			{
				Enabled = true,

				TitleHeight = 30,
				TitleForeground = ParseBrush("White"),
				TitleBackground = ParseBrush("Black"),
				TitleFontFamily = new FontFamily("Portable User Interface"),
				TitleFontSize = 13,
				TitleBackgroundOpacity = .5,
				TitleMargin = new Thickness(5, 8, 0, 0),

				DescriptionHeight = 30,
				DescriptionForeground = ParseBrush("White"),
				DescriptionBackground = ParseBrush("Black"),
				DescriptionBackgroundOpacity = .5,
				DescriptionFontFamily = new FontFamily("Portable User Interface"),
				DescriptionFontSize = 11,
				DescriptionMargin = new Thickness(6.5, 8, 0, 0),

				Background = ParseBrush("Black"),
				BackgroundOpacity = .5,
				BackgroundRadiusX = 0,
				BackgroundRadiusY = 0,
				BackgroundMargin = new Thickness(0),

                CloseButtonBackground1Brush = ParseBrush("Transparent"),
                CloseButtonBackground2Brush = ParseBrush("Transparent"),
                CloseButtonForegroundBrush = ParseBrush("#8D9AB4"),
                CloseButtonForegroundHoverBrush = ParseBrush("White"),
                CloseButtonRadiusX = 0,
                CloseButtonRadiusY = 0,
                CloseButtonMargin = new Thickness()
                {
                    Left = 3,
                    Top = 3,
                    Right = 6,
                    Bottom = 3
                },
                CloseButtonWidth = 15,
                CloseButtonHeight = 15,
                CloseButtonPathWidth = 10,
                CloseButtonPathHeight = 10,
                CloseButtonPathData = "M59.156,59.156 L81.073,81.073 C81.073,81.073 82.635,85.167 81.07,86.729 L26.573,141.228 L84.072,198.728 C85.634,200.290 85.634,202.822 84.072,204.384 L62.155,226.301 C60.593,227.863 58.061,227.863 56.499,226.301 L-1.000,168.801 L-57.499,225.301 C-59.061,226.863 -61.593,226.863 -63.155,225.301 L-85.072,203.384 C-86.634,201.822 -86.634,199.290 -85.072,197.728 L-28.573,141.228 L-82.073,87.729 C-83.635,86.167 -83.635,83.635 -82.073,82.073 L-60.156,60.156 C-58.594,58.594 -56.062,58.594 -54.500,60.156 L-1.000,113.655 L53.500,59.156 C55.062,57.594 57.594,57.594 59.156,59.156 z"
			};
			#endregion

			#region [ AlbumViewer ]
			Options.AlbumViewer = new Options.AlbumViewerOptions()
			{
				Enabled = true,
				ScrollButtonForeground = ParseBrush("#777"),
				ScrollButtonForegroundHover = ParseBrush("White"),
				ScrollButtonWidth = 10,
				ScrollButtonHeight = 10,
				ScrollButtonMargin = new Thickness(5),
				LeftScrollButtonData = "M0.05,0.8 C0.05,0.4 0.4,0.05 0.8,0.05 L2.7,0 C3.2,0 3.5,0.4 3.5,0.8 L3.5,9.2 C3.5,9.6 3.2,10 2.7,10 L0.8,10 C0.4,10 0,9.6 0,9.2 z M-3.4,0 L-9.4,5 -3.4,10 Z",
				RightScrollButtonData = "M0,0.8 C0.05,0.4 0.4,0.05 0.8,0.05 L2.7,0 C3.2,0 3.5,0.4 3.5,0.8 L3.5,9.2 C3.5,9.6 3.2,10 2.7,10 L0.8,10 C0.4,10 0,9.6 0,9.2 z M6.9,0 L13,5 6.9,10 Z",
				PageNumberFontFamily = new FontFamily("Portable User Interface"),
				PageNumberFontSize = 14,
				PageNumberForeground = ParseBrush("White")
			};
			#endregion

			#region [ ThumbnailViewer ]
			Options.ThumbnailViewer = new Options.ThumbnailViewerOptions()
			{
				Enabled = true,
				Background = ParseBrush("Black"),
				BackgroundOpacity = 1,
				BackgroundRadiusX = 4,
				BackgroundRadiusY = 4,
				Width = 310,
				Margin = new Thickness(5),
				ThumbSpacing = 5,
				ScrollIncrement = 310,
				ScrollButtonBrush = ParseBrush("#8D9AB4"),
				ScrollButtonHoverBrush = ParseBrush("White"),
				ScrollButtonWidth = 10,
				ScrollButtonHeight = 10,
				ScrollButtonMargin = new Thickness(5),
				ScrollRepeatButtonInterval = 75,
				LeftScrollButtonData = "M0.05,0.8 C0.05,0.4 0.4,0.05 0.8,0.05 L2.7,0 C3.2,0 3.5,0.4 3.5,0.8 L3.5,9.2 C3.5,9.6 3.2,10 2.7,10 L0.8,10 C0.4,10 0,9.6 0,9.2 z M-3.4,0 L-9.4,5 -3.4,10 Z",
				RightScrollButtonData = "M0,0.8 C0.05,0.4 0.4,0.05 0.8,0.05 L2.7,0 C3.2,0 3.5,0.4 3.5,0.8 L3.5,9.2 C3.5,9.6 3.2,10 2.7,10 L0.8,10 C0.4,10 0,9.6 0,9.2 z M6.9,0 L13,5 6.9,10 Z"
			};
			#endregion

			#region [ SlideThumbnail ]
			Options.SlideThumbnail = new Options.SlideThumbnailOptions()
			{
				Height = 28,
				BorderBrush = ParseBrush("Transparent"),
				BorderHighlightBrush = ParseBrush("White"),
				BorderThickness = new Thickness(1)
			};
			#endregion

			#region [ AlbumPage ]
			Options.AlbumPage = new Options.AlbumPageOptions()
			{
				Background = ParseBrush("Black")
			};
			#endregion

			#region [ AlbumButton ]
			Options.AlbumButton = new Options.AlbumButtonOptions()
			{
				Background = ParseBrush("#333"),
				BackgroundHover = ParseBrush("#555"),

				TitleForeground = ParseBrush("#F3F4F8"),
				TitleFontFamily = new FontFamily("Portable User Interface"),
				TitleFontSize = 12,
				TitleMargin = new Thickness(0, 6, 0, 0),

				DescriptionForeground = ParseBrush("Silver"),
				DescriptionFontFamily = new FontFamily("Portable User Interface"),
				DescriptionFontSize = 10,
				DescriptionMargin = new Thickness(0),

				Width = 222,
				Height = 84,
				Padding = 5,
				BackgroundRadiusX = 6,
				BackgroundRadiusY = 6,

				ThumbnailWidth = 78,
				ThumbnailHeight = 68,
				ThumbnailRadiusX = 4,
				ThumbnailRadiusY = 4,
				ThumbnailMargin = new Thickness(6),
				ThumbnailBorderStroke = ParseBrush("Silver"),
				ThumbnailBorderThickness = new Thickness(1)
			};
			#endregion

			#region [ SlidePreview ]
			Options.SlidePreview = new Options.SlidePreviewOptions()
			{
				Enabled = true,
				Height = 150,
				BorderBrush = ParseBrush("White"),
				BorderWidth = 2,
				RadiusX = 4,
				RadiusY = 4
			};
			#endregion

			#region [ SlideViewer ]
			Options.SlideViewer = new Options.SlideViewerOptions()
			{
				Stretch = Stretch.Uniform
			};
			#endregion

			#region [ ImageViewer ]
			Options.ImageViewer = new Options.ImageViewerOptions()
			{
			};
			#endregion

			#region [ VideoViewer ]
			Options.VideoViewer = new Options.VideoViewerOptions()
            {
                AutoPlay = true
			};
			#endregion

			#region [ VideoTray ]
			Options.VideoTray = new Options.VideoTrayOptions()
			{
				Enabled = true,
				Height = 30,
                Width = 400,
                Margin = new Thickness(0, -6, 0, 0),
				PlayPathData = "F1 M 101.447,284.834L 101.447,274.714L 106.906,279.774L 101.447,284.834 Z",
				PausePathData = "M 100.048,736.889L 98.5482,736.889C 97.9959,736.889 97.5482,736.441 97.5482,735.889L 97.5482,727.889C 97.5482,727.337 97.9959,726.889 98.5482,726.889L 100.048,726.889C 100.6,726.889 101.048,727.337 101.048,727.889L 101.048,735.889C 101.048,736.441 100.6,736.889 100.048,736.889 Z M 106.922,736.889L 105.422,736.889C 104.87,736.889 104.422,736.441 104.422,735.889L 104.422,727.889C 104.422,727.337 104.87,726.889 105.422,726.889L 106.922,726.889C 107.475,726.889 107.922,727.337 107.922,727.889L 107.922,735.889C 107.922,736.441 107.475,736.889 106.922,736.889 Z",
				PlayPauseButtonWidth = 20,
				PlayPauseButtonHeight = 20,
				PlayPauseButtonPathWidth = 10,
				PlayPauseButtonPathHeight = 10,
				Foreground = ParseBrush("Black"),
				ForegroundHover = ParseBrush("White"),
				Background = ParseBrush("White"),
				BackgroundOpacity = 0.5,
				RadiusX = 4,
				RadiusY = 4,
				TextFontFamily = new FontFamily("Portable User Interface"),
				TextFontSize = 10,
				TextForegroundBrush = ParseBrush("Black"),
				VolumeWidth = 50
			};
			#endregion

			#region [ PeopleTag ]
			Options.PeopleTag = new Options.PeopleTagOptions()
			{
				Enabled = true,

				PersonNameMargin = new Thickness(30, 5, 30, 5),
				PersonNameForeground = ParseBrush("Black"),
				PersonNameFontFamily = new FontFamily("Portable User Interface"),
				PersonNameFontSize = 12,

				BackgroundBorderBrush = ParseBrush("Black"),
				Background = ParseBrush("WhiteSmoke"),
				BackgroundBorderThickness = new Thickness(1),
				BackgroundCornerRadius = new CornerRadius(12),
				BackgroundOpacity = .8
			};
			#endregion

			if (OptionsSet != null)
			{
				OptionsSet(this, null);
			}
		}
	}
}