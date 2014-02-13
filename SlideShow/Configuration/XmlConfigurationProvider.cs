using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Represents an XmlConfigurationProvider
	/// </summary>
	public class XmlConfigurationProvider : ConfigurationProvider
	{
		/// <summary>
		/// Occurs when [options set].
		/// </summary>
		protected override event EventHandler OptionsSet;

		private new Dictionary<string, Dictionary<string, string>> initParams;

		/// <summary>
		/// Validates the specified init params.
		/// </summary>
		/// <param name="initParams">The init params.</param>
		public void Validate(Dictionary<string, Dictionary<string, string>> initParams)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlConfigurationProvider"/> class.
		/// </summary>
		/// <param name="initParams">The init params.</param>
		public XmlConfigurationProvider(Dictionary<string, Dictionary<string, string>> initParams)
		{
			Validate(initParams);
			this.initParams = initParams;
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		protected override void GetConfiguration()
		{
			string configurationFile = initParams.ContainsKey("ConfigurationProvider") ?
											initParams["ConfigurationProvider"].ContainsKey("Path") ?
											initParams["ConfigurationProvider"]["Path"] :
											"../Configuration.xml" :
											"../Configuration.xml";

			WebClient client = new WebClient();
			client.DownloadStringCompleted += client_DownloadStringCompleted;
			client.DownloadStringAsync(new Uri(configurationFile, UriKind.RelativeOrAbsolute));
		}

		/// <summary>
		/// Handles the DownloadStringCompleted event of the getConfigurationClient control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		public void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Dictionary<string, Dictionary<string, string>> xmlOptions = new Dictionary<string, Dictionary<string, string>>();
			using (XmlReader reader = XmlReader.Create(new StringReader(e.Result)))
			{
				while (reader.Read())
				{
					if (reader.IsStartElement())
					{
						switch (reader.Name)
						{
							case "configuration":
								xmlOptions.Add(
									string.Empty,
									ParseRoot(reader));
								break;
							case "module":
								xmlOptions.Add(
									reader.GetAttribute("name"),
									ParseOptions(reader.ReadSubtree()));
								break;
						}
					}
				}
			}

			Options defaultOptions = DefaultTheme.Options;
            Options = DefaultTheme.Options;
			
			#region [ General ]
			Dictionary<string, string> generalOptions = xmlOptions[string.Empty];
			Options.General = new Options.GeneralOptions()
			{
			    Background = generalOptions.ContainsKey("background") ? ParseBrush(generalOptions["background"]) : defaultOptions.General.Background,
				StartInAlbumView = ParseBool(generalOptions, "StartInAlbumView") ?? defaultOptions.General.StartInAlbumView,
				AutoStart = ParseBool(generalOptions, "AutoStart") ?? defaultOptions.General.AutoStart
			};
			#endregion

			#region [ Transition ]

			if (xmlOptions.ContainsKey("Transition"))
			{
				Dictionary<string, string> d = xmlOptions["Transition"];
				Options.Transition = new Options.TransitionOptions()
				{
					TransitionDuration =
						ParseInt(d, "TransitionDuration") ?? defaultOptions.Transition.TransitionDuration,
					WaitTime =
						ParseInt(d, "WaitTime") ?? defaultOptions.Transition.WaitTime,
				};
			}
			#endregion

			#region [ DataProvider ]
			if (xmlOptions.ContainsKey("DataProvider"))
			{
				Dictionary<string, string> d = xmlOptions["DataProvider"];
				Options.DataProvider = new Options.DataProviderOptions()
				{
					Type =
						d.ContainsKey("Type") ?
						d["Type"] :
						defaultOptions.DataProvider.Type,
					ApiKey = 
						d.ContainsKey("ApiKey") ?
						d["ApiKey"] :
						defaultOptions.DataProvider.ApiKey,
					Uri =
						d.ContainsKey("Uri") ?
						d["Uri"] :
						defaultOptions.DataProvider.Uri,
					UserName =
						d.ContainsKey("UserName") ?
						d["UserName"] :
						defaultOptions.DataProvider.UserName 
				};
			}
			#endregion

			#region [ Preloader ]
			if (xmlOptions.ContainsKey("Preloader"))
			{
				Dictionary<string, string> d = xmlOptions["Preloader"];
				Options.Preloader = new Options.PreloaderOptions()
				{
					PreloadAhead =
						ParseInt(d, "PreloadAhead") ?? defaultOptions.Preloader.PreloadAhead
				};
			}
			#endregion

			#region [ LoadingProgressIndicator ]
			if (xmlOptions.ContainsKey("LoadingProgressIndicator"))
			{
				Dictionary<string, string> d = xmlOptions["LoadingProgressIndicator"];
				Options.LoadingProgressIndicator = new Options.LoadingProgressIndicatorOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.LoadingProgressIndicator.Enabled,					
					Type =
						d.ContainsKey("Type") ?
						d["Type"] :
						defaultOptions.LoadingProgressIndicator.Type,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.LoadingProgressIndicator.Height,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.LoadingProgressIndicator.Width,
					Foreground =
						d.ContainsKey("Foreground") ?
						ParseBrush(d["Foreground"]) :
						defaultOptions.LoadingProgressIndicator.Foreground,
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.LoadingProgressIndicator.Background
				};
			}
			#endregion

			#region [ SlideNavigation ]
			if (xmlOptions.ContainsKey("SlideNavigation"))
			{
				Dictionary<string, string> d = xmlOptions["SlideNavigation"];
				Options.SlideNavigation = new Options.SlideNavigationOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.SlideNavigation.Enabled,
					PlayPathData =
						d.ContainsKey("PlayPathData") ?
						d["PlayPathData"] :
						defaultOptions.SlideNavigation.PlayPathData,
					PausePathData =
						d.ContainsKey("PausePathData") ?
						d["PausePathData"] :
						defaultOptions.SlideNavigation.PausePathData,
					PreviousPathData =
						d.ContainsKey("PreviousPathData") ?
						d["PreviousPathData"] :
						defaultOptions.SlideNavigation.PreviousPathData,
					NextPathData =
						d.ContainsKey("NextPathData") ?
						d["NextPathData"] :
						defaultOptions.SlideNavigation.NextPathData,
					Foreground =
						d.ContainsKey("Foreground") ?
						ParseBrush(d["Foreground"]) :
						defaultOptions.SlideNavigation.Foreground,
					ForegroundHover =
						d.ContainsKey("ForegroundHover") ?
						ParseBrush(d["ForegroundHover"]) :
						defaultOptions.SlideNavigation.ForegroundHover,
					Background1Brush =
						d.ContainsKey("Background1Brush") ?
						ParseBrush(d["Background1Brush"]) :
						defaultOptions.SlideNavigation.Background1Brush,
					Background2Brush =
						d.ContainsKey("Background2Brush") ?
						ParseBrush(d["Background2Brush"]) :
						defaultOptions.SlideNavigation.Background2Brush,
					RadiusX =
						ParseDouble(d, "RadiusX") ?? defaultOptions.SlideNavigation.RadiusX,
					RadiusY =
						ParseDouble(d, "RadiusY") ?? defaultOptions.SlideNavigation.RadiusY,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.SlideNavigation.Height,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.SlideNavigation.Width,
					PlayPauseButtonHeight =
						ParseDouble(d, "PlayPauseButtonHeight") ?? defaultOptions.SlideNavigation.PlayPauseButtonHeight,
					PlayPauseButtonWidth =
						ParseDouble(d, "PlayPauseButtonWidth") ?? defaultOptions.SlideNavigation.PlayPauseButtonWidth,
					NextButtonWidth =
						ParseDouble(d, "NextButtonWidth") ?? defaultOptions.SlideNavigation.NextButtonWidth,
					NextButtonHeight =
						ParseDouble(d, "NextButtonHeight") ?? defaultOptions.SlideNavigation.NextButtonHeight,
					PreviousButtonWidth =
						ParseDouble(d, "PreviousButtonWidth") ?? defaultOptions.SlideNavigation.PreviousButtonWidth,
					PreviousButtonHeight =
						ParseDouble(d, "PreviousButtonHeight") ?? defaultOptions.SlideNavigation.PreviousButtonHeight,
					PlayPauseButtonPathHeight =
						ParseDouble(d, "PlayPauseButtonPathHeight") ?? defaultOptions.SlideNavigation.PlayPauseButtonPathHeight,
					PlayPauseButtonPathWidth =
						ParseDouble(d, "PlayPauseButtonPathWidth") ?? defaultOptions.SlideNavigation.PlayPauseButtonPathWidth,
					NextButtonPathWidth =
						ParseDouble(d, "NextButtonPathWidth") ?? defaultOptions.SlideNavigation.NextButtonPathWidth,
					NextButtonPathHeight =
						ParseDouble(d, "NextButtonPathHeight") ?? defaultOptions.SlideNavigation.NextButtonPathHeight,
					PreviousButtonPathWidth =
						ParseDouble(d, "PreviousButtonPathWidth") ?? defaultOptions.SlideNavigation.PreviousButtonPathWidth,
					PreviousButtonPathHeight =
						ParseDouble(d, "PreviousButtonPathHeight") ?? defaultOptions.SlideNavigation.PreviousButtonPathHeight
				};
			}
			#endregion

			#region [ EmbedViewer ]
			if (xmlOptions.ContainsKey("EmbedViewer"))
			{
				Dictionary<string, string> d = xmlOptions["EmbedViewer"];
				Options.EmbedViewer = new Options.EmbedViewerOptions()
				{
                    Enabled = ParseBool(d, "Enabled") ?? defaultOptions.EmbedViewer.Enabled,
                    Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.EmbedViewer.Background,
					BackgroundOpacity =
						ParseDouble(d, "BackgroundOpacity") ?? defaultOptions.EmbedViewer.BackgroundOpacity,
                    CornerRadius =
                        ParseCornerRadius(d, "CornerRadius") ?? defaultOptions.EmbedViewer.CornerRadius,
					DialogBackground =
						d.ContainsKey("DialogBackground") ?
						ParseBrush(d["DialogBackground"]) :
						defaultOptions.EmbedViewer.DialogBackground,
					DialogBackgroundOpacity =
						ParseDouble(d, "DialogBackgroundOpacity") ?? defaultOptions.EmbedViewer.DialogBackgroundOpacity,
					DialogBorderBrush =
						d.ContainsKey("DialogBorderBrush") ?
						ParseBrush(d["DialogBorderBrush"]) :
						defaultOptions.EmbedViewer.DialogBorderBrush,
					DialogBorderThickness =
						ParseThickness(d, "DialogBorderThickness") ?? defaultOptions.EmbedViewer.DialogBorderThickness,
					DialogWidth =
						ParseDouble(d, "DialogWidth") ?? defaultOptions.EmbedViewer.DialogWidth,
					DialogHeight =
						ParseDouble(d, "DialogHeight") ?? defaultOptions.EmbedViewer.DialogHeight,
					CloseButtonBackground1Brush =
						d.ContainsKey("CloseButtonBackground1Brush") ?
						ParseBrush(d["CloseButtonBackground1Brush"]) :
						defaultOptions.EmbedViewer.CloseButtonBackground1Brush,
					CloseButtonBackground2Brush =
						d.ContainsKey("CloseButtonBackground2Brush") ?
						ParseBrush(d["CloseButtonBackground2Brush"]) :
						defaultOptions.EmbedViewer.CloseButtonBackground2Brush,
					CloseButtonForegroundBrush =
						d.ContainsKey("CloseButtonForegroundBrush") ?
						ParseBrush(d["CloseButtonForegroundBrush"]) :
						defaultOptions.EmbedViewer.CloseButtonForegroundBrush,
					CloseButtonForegroundHoverBrush =
						d.ContainsKey("CloseButtonForegroundHoverBrush") ?
						ParseBrush(d["CloseButtonForegroundHoverBrush"]) :
						defaultOptions.EmbedViewer.CloseButtonForegroundHoverBrush,
					CloseButtonRadiusX =
						ParseDouble(d, "CloseButtonRadiusX") ?? defaultOptions.EmbedViewer.CloseButtonRadiusX,
					CloseButtonRadiusY =
						ParseDouble(d, "CloseButtonRadiusY") ?? defaultOptions.EmbedViewer.CloseButtonRadiusY,
					CloseButtonMargin =
						ParseThickness(d, "CloseButtonMargin") ?? defaultOptions.EmbedViewer.CloseButtonMargin,
					CloseButtonWidth =
						ParseDouble(d, "CloseButtonWidth") ?? defaultOptions.EmbedViewer.CloseButtonWidth,
					CloseButtonHeight =
						ParseDouble(d, "CloseButtonHeight") ?? defaultOptions.EmbedViewer.CloseButtonHeight,
					CloseButtonPathWidth =
						ParseDouble(d, "CloseButtonPathWidth") ?? defaultOptions.EmbedViewer.CloseButtonPathWidth,
					CloseButtonPathHeight =
						ParseDouble(d, "CloseButtonPathHeight") ?? defaultOptions.EmbedViewer.CloseButtonPathHeight,
					CloseButtonPathData =
						d.ContainsKey("CloseButtonPathData") ?
						d["CloseButtonPathData"] :
						defaultOptions.EmbedViewer.CloseButtonPathData,
					HyperlinkButtonText =
						d.ContainsKey("HyperlinkButtonText") ?
						d["HyperlinkButtonText"] :
						defaultOptions.EmbedViewer.HyperlinkButtonText,
					HyperlinkButtonForeground =
						d.ContainsKey("HyperlinkButtonForeground") ?
						ParseBrush(d["HyperlinkButtonForeground"]) :
						defaultOptions.EmbedViewer.HyperlinkButtonForeground,
					HyperlinkButtonBackground =
						d.ContainsKey("HyperlinkButtonBackground") ?
						ParseBrush(d["HyperlinkButtonBackground"]) :
						defaultOptions.EmbedViewer.HyperlinkButtonBackground,
					HyperlinkButtonWidth =
						ParseDouble(d, "HyperlinkButtonWidth") ?? defaultOptions.EmbedViewer.HyperlinkButtonWidth,
					HyperlinkButtonHeight =
						ParseDouble(d, "HyperlinkButtonHeight") ?? defaultOptions.EmbedViewer.HyperlinkButtonHeight,
					HyperlinkButtonMargin =
						ParseThickness(d, "HyperlinkButtonMargin") ?? defaultOptions.EmbedViewer.HyperlinkButtonMargin,
					HyperlinkButtonFontFamily = ParseFontFamily(d, "HyperlinkButtonFontFamily", defaultOptions.EmbedViewer.HyperlinkButtonFontFamily),
					HyperlinkButtonFontSize =
						ParseDouble(d, "HyperlinkButtonFontSize") ?? defaultOptions.EmbedViewer.HyperlinkButtonFontSize,
					TextBlockFontFamily = ParseFontFamily(d, "TextBlockFontFamily", defaultOptions.EmbedViewer.TextBlockFontFamily),
					TextBlockFontSize =
						ParseDouble(d, "TextBlockFontSize") ?? defaultOptions.EmbedViewer.TextBlockFontSize,
					TitleText =
						d.ContainsKey("TitleText") ?
						d["TitleText"] :
						defaultOptions.EmbedViewer.TitleText,
					TextBlockMargin =
						ParseThickness(d, "TextBlockMargin") ?? defaultOptions.EmbedViewer.TextBlockMargin,
					TextBlockForeground =
						d.ContainsKey("TextBlockForeground") ?
						ParseBrush(d["TextBlockForeground"]) :
						defaultOptions.EmbedViewer.TextBlockForeground,
					TextBoxWidth =
						ParseDouble(d, "TextBoxWidth") ?? defaultOptions.EmbedViewer.TextBoxWidth,
					TextBoxHeight =
						ParseDouble(d, "TextBoxHeight") ?? defaultOptions.EmbedViewer.TextBoxHeight,
					TextBoxBackground =
						d.ContainsKey("TextBoxBackground") ?
						ParseBrush(d["TextBoxBackground"]) :
						defaultOptions.EmbedViewer.TextBoxBackground,
					TextBoxForeground =
						d.ContainsKey("TextBoxForeground") ?
						ParseBrush(d["TextBoxForeground"]) :
						defaultOptions.EmbedViewer.TextBoxForeground,
					TextBoxFontFamily = ParseFontFamily(d, "TextBoxFontFamily", defaultOptions.EmbedViewer.TextBoxFontFamily),
					TextBoxFontSize =
						ParseDouble(d, "TextBoxFontSize") ?? defaultOptions.EmbedViewer.TextBoxFontSize
				};
			}
			#endregion

			#region [ NavigationTray ]
			if (xmlOptions.ContainsKey("NavigationTray"))
			{
				Dictionary<string, string> d = xmlOptions["NavigationTray"];
				Options.NavigationTray = new Options.NavigationTrayOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.NavigationTray.Enabled,
					BackgroundOpacity =
						ParseDouble(d, "BackgroundOpacity") ?? defaultOptions.NavigationTray.BackgroundOpacity,
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.NavigationTray.Background
				};				
			}
			#endregion

			#region [ ToggleAlbumViewButton ]
			if (xmlOptions.ContainsKey("ToggleAlbumViewButton"))
			{
				Dictionary<string, string> d = xmlOptions["ToggleAlbumViewButton"];
				Options.ToggleAlbumViewButton = new Options.ToggleAlbumViewButtonOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.ToggleAlbumViewButton.Enabled,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.ToggleAlbumViewButton.Width,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.ToggleAlbumViewButton.Height,
					PathWidth =
						ParseDouble(d, "PathWidth") ?? defaultOptions.ToggleAlbumViewButton.PathWidth,
					PathHeight =
						ParseDouble(d, "PathHeight") ?? defaultOptions.ToggleAlbumViewButton.PathHeight,
					PathData =
						d.ContainsKey("PathData") ?
						d["PathData"] :
						defaultOptions.ToggleAlbumViewButton.PathData,
					Background1Brush =
						d.ContainsKey("Background1Brush") ?
						ParseBrush(d["Background1Brush"]) :
						defaultOptions.ToggleAlbumViewButton.Background1Brush,
					Background2Brush =
						d.ContainsKey("Background2Brush") ?
						ParseBrush(d["Background2Brush"]) :
						defaultOptions.ToggleAlbumViewButton.Background2Brush,
					ForegroundBrush =
						d.ContainsKey("ForegroundBrush") ?
						ParseBrush(d["ForegroundBrush"]) :
						defaultOptions.ToggleAlbumViewButton.ForegroundBrush,
					ForegroundHoverBrush =
						d.ContainsKey("ForegroundHoverBrush") ?
						ParseBrush(d["ForegroundHoverBrush"]) :
						defaultOptions.ToggleAlbumViewButton.ForegroundHoverBrush,
					RadiusX =
						ParseDouble(d, "RadiusX") ?? defaultOptions.ToggleAlbumViewButton.RadiusX,
					RadiusY =
						ParseDouble(d, "RadiusY") ?? defaultOptions.ToggleAlbumViewButton.RadiusY,
					Margin =
						ParseThickness(d, "Margin") ?? defaultOptions.ToggleAlbumViewButton.Margin
				};
			}
			#endregion

            #region [ ToggleSaveButton ]
            if (xmlOptions.ContainsKey("ToggleSaveButton"))
            {
                Dictionary<string, string> d = xmlOptions["ToggleSaveButton"];
                Options.ToggleSaveButton = new Options.ToggleSaveButtonOptions()
                {
                    Enabled =
                        ParseBool(d, "Enabled") ?? defaultOptions.ToggleSaveButton.Enabled,
                    Width =
                        ParseDouble(d, "Width") ?? defaultOptions.ToggleSaveButton.Width,
                    Height =
                        ParseDouble(d, "Height") ?? defaultOptions.ToggleSaveButton.Height,
                    PathWidth =
                        ParseDouble(d, "PathWidth") ?? defaultOptions.ToggleSaveButton.PathWidth,
                    PathHeight =
                        ParseDouble(d, "PathHeight") ?? defaultOptions.ToggleSaveButton.PathHeight,
                    PathData =
                        d.ContainsKey("GoToFullPathData") ?
                        d["GoToFullPathData"] :
                        Options.ToggleSaveButton.PathData,
                    Background1Brush =
                        d.ContainsKey("Background1Brush") ?
                        ParseBrush(d["Background1Brush"]) :
                        Options.ToggleSaveButton.Background1Brush,
                    Background2Brush =
                        d.ContainsKey("Background2Brush") ?
                        ParseBrush(d["Background2Brush"]) :
                        Options.ToggleSaveButton.Background2Brush,
                    ForegroundBrush =
                        d.ContainsKey("ForegroundBrush") ?
                        ParseBrush(d["ForegroundBrush"]) :
                        Options.ToggleSaveButton.ForegroundBrush,
                    ForegroundHoverBrush =
                        d.ContainsKey("ForegroundHoverBrush") ?
                        ParseBrush(d["ForegroundHoverBrush"]) :
                        Options.ToggleSaveButton.ForegroundHoverBrush,
                    RadiusX =
                        ParseDouble(d, "RadiusX") ?? defaultOptions.ToggleSaveButton.RadiusX,
                    RadiusY =
                        ParseDouble(d, "RadiusY") ?? defaultOptions.ToggleSaveButton.RadiusY,
                    Margin =
                        ParseThickness(d, "Margin") ?? defaultOptions.ToggleSaveButton.Margin,
					DownloadImageHandler = d.ContainsKey("DownloadImageHandler") ?
						d["DownloadImageHandler"] :
						Options.ToggleSaveButton.DownloadImageHandler,
                };
            }
            #endregion

			#region [ ToggleFullScreenButton ]
			if (xmlOptions.ContainsKey("ToggleFullScreenButton"))
			{
				Dictionary<string, string> d = xmlOptions["ToggleFullScreenButton"];
				Options.ToggleFullScreenButton = new Options.ToggleFullScreenButtonOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.ToggleFullScreenButton.Enabled,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.ToggleFullScreenButton.Width,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.ToggleFullScreenButton.Height,
					PathWidth =
						ParseDouble(d, "PathWidth") ?? defaultOptions.ToggleFullScreenButton.PathWidth,
					PathHeight =
						ParseDouble(d, "PathHeight") ?? defaultOptions.ToggleFullScreenButton.PathHeight,
					GoToFullPathData =
						d.ContainsKey("GoToFullPathData") ?
						d["GoToFullPathData"] :
						defaultOptions.ToggleFullScreenButton.GoToFullPathData,
					EscapeFullPathData =
						d.ContainsKey("EscapeFullPathData") ?
						d["EscapeFullPathData"] :
						defaultOptions.ToggleFullScreenButton.EscapeFullPathData,
					Background1Brush =
						d.ContainsKey("Background1Brush") ?
						ParseBrush(d["Background1Brush"]) :
						defaultOptions.ToggleFullScreenButton.Background1Brush,
					Background2Brush =
						d.ContainsKey("Background2Brush") ?
						ParseBrush(d["Background2Brush"]) :
						defaultOptions.ToggleFullScreenButton.Background2Brush,
					ForegroundBrush =
						d.ContainsKey("ForegroundBrush") ?
						ParseBrush(d["ForegroundBrush"]) :
						defaultOptions.ToggleFullScreenButton.ForegroundBrush,
					ForegroundHoverBrush =
						d.ContainsKey("ForegroundHoverBrush") ?
						ParseBrush(d["ForegroundHoverBrush"]) :
						defaultOptions.ToggleFullScreenButton.ForegroundHoverBrush,
					RadiusX =
						ParseDouble(d, "RadiusX") ?? defaultOptions.ToggleFullScreenButton.RadiusX,
					RadiusY =
						ParseDouble(d, "RadiusY") ?? defaultOptions.ToggleFullScreenButton.RadiusY,
					Margin =
						ParseThickness(d, "Margin") ?? defaultOptions.ToggleFullScreenButton.Margin
				};
			}
			#endregion

			#region [ ToggleEmbedViewButton ]
			if (xmlOptions.ContainsKey("ToggleEmbedViewButton"))
			{
				Dictionary<string, string> d = xmlOptions["ToggleEmbedViewButton"];
				Options.ToggleEmbedViewButton = new Options.ToggleEmbedViewButtonOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.ToggleEmbedViewButton.Enabled,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.ToggleEmbedViewButton.Width,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.ToggleEmbedViewButton.Height,
					PathWidth =
						ParseDouble(d, "PathWidth") ?? defaultOptions.ToggleEmbedViewButton.PathWidth,
					PathHeight =
						ParseDouble(d, "PathHeight") ?? defaultOptions.ToggleEmbedViewButton.PathHeight,
					PathData =
						d.ContainsKey("PathData") ?
						d["PathData"] :
						defaultOptions.ToggleEmbedViewButton.PathData,
					Background1Brush =
						d.ContainsKey("Background1Brush") ?
						ParseBrush(d["Background1Brush"]) :
						defaultOptions.ToggleEmbedViewButton.Background1Brush,
					Background2Brush =
						d.ContainsKey("Background2Brush") ?
						ParseBrush(d["Background2Brush"]) :
						defaultOptions.ToggleEmbedViewButton.Background2Brush,
					ForegroundBrush =
						d.ContainsKey("ForegroundBrush") ?
						ParseBrush(d["ForegroundBrush"]) :
						defaultOptions.ToggleEmbedViewButton.ForegroundBrush,
					ForegroundHoverBrush =
						d.ContainsKey("ForegroundHoverBrush") ?
						ParseBrush(d["ForegroundHoverBrush"]) :
						defaultOptions.ToggleEmbedViewButton.ForegroundHoverBrush,
					RadiusX =
						ParseDouble(d, "RadiusX") ?? defaultOptions.ToggleEmbedViewButton.RadiusX,
					RadiusY =
						ParseDouble(d, "RadiusY") ?? defaultOptions.ToggleEmbedViewButton.RadiusY,
					Margin =
						ParseThickness(d, "Margin") ?? defaultOptions.ToggleEmbedViewButton.Margin
				};
			}
			#endregion

			#region [ SlideDescription ]
			if (xmlOptions.ContainsKey("SlideDescription"))
			{
				Dictionary<string, string> d = xmlOptions["SlideDescription"];
				Options.SlideDescription = new Options.SlideDescriptionOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.SlideDescription.Enabled,
					TitleHeight =
						ParseDouble(d, "TitleHeight") ?? defaultOptions.SlideDescription.TitleHeight,
					TitleForeground =
						d.ContainsKey("TitleForeground") ?
						ParseBrush(d["TitleForeground"]) :
						defaultOptions.SlideDescription.TitleForeground,
					TitleBackground =
						d.ContainsKey("TitleBackground") ?
						ParseBrush(d["TitleBackground"]) :
						defaultOptions.SlideDescription.TitleBackground,
					TitleBackgroundOpacity =
						ParseDouble(d, "TitleBackgroundOpacity") ?? defaultOptions.SlideDescription.TitleBackgroundOpacity,
					TitleFontFamily = ParseFontFamily(d, "TitleFontFamily", defaultOptions.SlideDescription.TitleFontFamily),
					TitleFontSize =
						ParseDouble(d, "TitleFontSize") ?? defaultOptions.SlideDescription.TitleFontSize,
					TitleMargin =
						ParseThickness(d, "TitleMargin") ?? defaultOptions.SlideDescription.TitleMargin,
					DescriptionHeight =
						ParseDouble(d, "DescriptionHeight") ?? defaultOptions.SlideDescription.DescriptionHeight,
					DescriptionForeground =
						d.ContainsKey("DescriptionForeground") ?
						ParseBrush(d["DescriptionForeground"]) :
						defaultOptions.SlideDescription.DescriptionForeground,
					DescriptionBackground =
						d.ContainsKey("DescriptionBackground") ?
						ParseBrush(d["DescriptionBackground"]) :
						defaultOptions.SlideDescription.DescriptionBackground,
					DescriptionBackgroundOpacity =
						ParseDouble(d, "DescriptionBackgroundOpacity") ?? defaultOptions.SlideDescription.DescriptionBackgroundOpacity,
					DescriptionFontFamily = ParseFontFamily(d, "DescriptionFontFamily", defaultOptions.SlideDescription.DescriptionFontFamily),
					DescriptionFontSize =
						ParseDouble(d, "DescriptionFontSize") ?? defaultOptions.SlideDescription.DescriptionFontSize,
					DescriptionMargin =
						ParseThickness(d, "DescriptionMargin") ?? defaultOptions.SlideDescription.DescriptionMargin,
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.SlideDescription.Background,
					BackgroundOpacity =
						ParseDouble(d, "BackgroundOpacity") ?? defaultOptions.SlideDescription.BackgroundOpacity,
					BackgroundRadiusX =
						ParseDouble(d, "BackgroundRadiusX") ?? defaultOptions.SlideDescription.BackgroundRadiusX,
					BackgroundRadiusY =
						ParseDouble(d, "BackgroundRadiusY") ?? defaultOptions.SlideDescription.BackgroundRadiusY,
					BackgroundMargin =
						ParseThickness(d, "BackgroundMargin") ?? defaultOptions.SlideDescription.BackgroundMargin,
                    CloseButtonBackground1Brush =
                        d.ContainsKey("CloseButtonBackground1Brush") ?
                        ParseBrush(d["CloseButtonBackground1Brush"]) :
                        defaultOptions.SlideDescription.CloseButtonBackground1Brush,
                    CloseButtonBackground2Brush =
                        d.ContainsKey("CloseButtonBackground2Brush") ?
                        ParseBrush(d["CloseButtonBackground2Brush"]) :
                        defaultOptions.SlideDescription.CloseButtonBackground2Brush,
                    CloseButtonForegroundBrush =
                        d.ContainsKey("CloseButtonForegroundBrush") ?
                        ParseBrush(d["CloseButtonForegroundBrush"]) :
                        defaultOptions.SlideDescription.CloseButtonForegroundBrush,
                    CloseButtonForegroundHoverBrush =
                        d.ContainsKey("CloseButtonForegroundHoverBrush") ?
                        ParseBrush(d["CloseButtonForegroundHoverBrush"]) :
                        defaultOptions.SlideDescription.CloseButtonForegroundHoverBrush,
                    CloseButtonRadiusX =
                        ParseDouble(d, "CloseButtonRadiusX") ?? defaultOptions.SlideDescription.CloseButtonRadiusX,
                    CloseButtonRadiusY =
                        ParseDouble(d, "CloseButtonRadiusY") ?? defaultOptions.SlideDescription.CloseButtonRadiusY,
					CloseButtonMargin =
						ParseThickness(d, "CloseButtonMargin") ?? defaultOptions.SlideDescription.CloseButtonMargin,
                    CloseButtonWidth =
                        ParseDouble(d, "CloseButtonWidth") ?? defaultOptions.SlideDescription.CloseButtonWidth,
                    CloseButtonHeight =
                        ParseDouble(d, "CloseButtonHeight") ?? defaultOptions.SlideDescription.CloseButtonHeight,
                    CloseButtonPathWidth =
                        ParseDouble(d, "CloseButtonPathWidth") ?? defaultOptions.SlideDescription.CloseButtonPathWidth,
                    CloseButtonPathHeight =
                        ParseDouble(d, "CloseButtonPathHeight") ?? defaultOptions.SlideDescription.CloseButtonPathHeight,
                    CloseButtonPathData =
                        d.ContainsKey("CloseButtonPathData") ?
                        d["CloseButtonPathData"] :
                        defaultOptions.SlideDescription.CloseButtonPathData
				};
			}
			#endregion

			#region [ AlbumViewer ]
			if (xmlOptions.ContainsKey("AlbumViewer"))
			{
				Dictionary<string, string> d = xmlOptions["AlbumViewer"];
				Options.AlbumViewer = new Options.AlbumViewerOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.AlbumViewer.Enabled,
					ScrollButtonForeground =
						d.ContainsKey("ScrollButtonForeground") ?
						ParseBrush(d["ScrollButtonForeground"]) :
						defaultOptions.AlbumViewer.ScrollButtonForeground,
					ScrollButtonForegroundHover =
						d.ContainsKey("ScrollButtonForegroundHover") ?
						ParseBrush(d["ScrollButtonForegroundHover"]) :
						defaultOptions.AlbumViewer.ScrollButtonForegroundHover,
					ScrollButtonWidth =
						ParseDouble(d, "ScrollButtonWidth") ?? defaultOptions.AlbumViewer.ScrollButtonWidth,
					ScrollButtonHeight =
						ParseDouble(d, "ScrollButtonHeight") ?? defaultOptions.AlbumViewer.ScrollButtonHeight,
					ScrollButtonMargin =
						ParseThickness(d, "ScrollButtonMargin") ?? defaultOptions.AlbumViewer.ScrollButtonMargin,
					LeftScrollButtonData =
						d.ContainsKey("LeftScrollButtonData") ?
						d["LeftScrollButtonData"] :
						defaultOptions.AlbumViewer.LeftScrollButtonData,
					RightScrollButtonData =
						d.ContainsKey("RightScrollButtonData") ?
						d["RightScrollButtonData"] :
						defaultOptions.AlbumViewer.RightScrollButtonData,
					PageNumberFontFamily = ParseFontFamily(d, "PageNumberFontFamily", defaultOptions.AlbumViewer.PageNumberFontFamily),
					PageNumberFontSize =
						ParseDouble(d, "PageNumberFontSize") ?? defaultOptions.AlbumViewer.PageNumberFontSize,
					PageNumberForeground =
						d.ContainsKey("PageNumberForeground") ?
						ParseBrush(d["PageNumberForeground"]) :
						defaultOptions.AlbumViewer.PageNumberForeground
				};
			}
			#endregion

			#region [ ThumbnailViewer ]
			if (xmlOptions.ContainsKey("ThumbnailViewer"))
			{
				Dictionary<string, string> d = xmlOptions["ThumbnailViewer"];
				Options.ThumbnailViewer = new Options.ThumbnailViewerOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.ThumbnailViewer.Enabled,
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.ThumbnailViewer.Background,
					BackgroundOpacity =
						ParseDouble(d, "BackgroundOpacity") ?? defaultOptions.ThumbnailViewer.BackgroundOpacity,
					BackgroundRadiusX =
						ParseDouble(d, "BackgroundRadiusX") ?? defaultOptions.ThumbnailViewer.BackgroundRadiusX,
					BackgroundRadiusY =
						ParseDouble(d, "BackgroundRadiusY") ?? defaultOptions.ThumbnailViewer.BackgroundRadiusY,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.ThumbnailViewer.Width,
					Margin =
						ParseThickness(d, "Margin") ?? defaultOptions.ThumbnailViewer.Margin,
					ThumbSpacing =
						ParseInt(d, "ThumbSpacing") ?? defaultOptions.ThumbnailViewer.ThumbSpacing,
					ScrollIncrement =
						ParseDouble(d, "ScrollIncrement") ?? defaultOptions.ThumbnailViewer.ScrollIncrement,
					ScrollButtonBrush =
						d.ContainsKey("ScrollButtonBrush") ?
						ParseBrush(d["ScrollButtonBrush"]) :
						defaultOptions.ThumbnailViewer.ScrollButtonBrush,
					ScrollButtonHoverBrush =
						d.ContainsKey("ScrollButtonHoverBrush") ?
						ParseBrush(d["ScrollButtonHoverBrush"]) :
						defaultOptions.ThumbnailViewer.ScrollButtonHoverBrush,
					ScrollButtonWidth =
						ParseDouble(d, "ScrollButtonWidth") ?? defaultOptions.ThumbnailViewer.ScrollButtonWidth,
					ScrollButtonHeight =
						ParseDouble(d, "ScrollButtonHeight") ?? defaultOptions.ThumbnailViewer.ScrollButtonHeight,
					ScrollButtonMargin =
						ParseThickness(d, "ScrollButtonMargin") ?? defaultOptions.ThumbnailViewer.ScrollButtonMargin,
					ScrollRepeatButtonInterval =
						ParseInt(d, "ScrollRepeatButtonInterval") ?? defaultOptions.ThumbnailViewer.ScrollRepeatButtonInterval,
					LeftScrollButtonData =
						d.ContainsKey("LeftScrollButtonData") ?
						d["LeftScrollButtonData"] :
						defaultOptions.ThumbnailViewer.LeftScrollButtonData,
					RightScrollButtonData =
						d.ContainsKey("RightScrollButtonData") ?
						d["RightScrollButtonData"] :
						defaultOptions.ThumbnailViewer.RightScrollButtonData
				};
			}
			#endregion

			#region [ SlideThumbnail ]
			if (xmlOptions.ContainsKey("SlideThumbnail"))
			{
				Dictionary<string, string> d = xmlOptions["SlideThumbnail"];
				Options.SlideThumbnail = new Options.SlideThumbnailOptions()
				{
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.SlideThumbnail.Height,
					BorderBrush =
						d.ContainsKey("BorderBrush") ?
						ParseBrush(d["BorderBrush"]) :
						defaultOptions.SlideThumbnail.BorderBrush,
					BorderHighlightBrush =
						d.ContainsKey("BorderHighlightBrush") ?
						ParseBrush(d["BorderHighlightBrush"]) :
						defaultOptions.SlideThumbnail.BorderHighlightBrush,
					BorderThickness =
						ParseThickness(d, "BorderThickness") ?? defaultOptions.SlideThumbnail.BorderThickness
				};
			}
			#endregion

			#region [ AlbumPage ]
			if (xmlOptions.ContainsKey("AlbumPage"))
			{
				Dictionary<string, string> d = xmlOptions["AlbumPage"];
				Options.AlbumPage = new Options.AlbumPageOptions()
				{
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
                        defaultOptions.AlbumPage.Background 
				};
			}
			#endregion

			#region [ AlbumButton ]
			if (xmlOptions.ContainsKey("AlbumButton"))
			{
				Dictionary<string, string> d = xmlOptions["AlbumButton"];
				Options.AlbumButton = new Options.AlbumButtonOptions()
				{
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.AlbumButton.Background,
					BackgroundHover =
						d.ContainsKey("BackgroundHover") ?
						ParseBrush(d["BackgroundHover"]) :
						defaultOptions.AlbumButton.BackgroundHover,
					TitleFontFamily = ParseFontFamily(d, "TitleFontFamily", defaultOptions.AlbumButton.TitleFontFamily),
					TitleFontSize =
						ParseDouble(d, "TitleFontSize") ?? defaultOptions.AlbumButton.TitleFontSize,
					TitleForeground =
						d.ContainsKey("TitleForeground") ?
						ParseBrush(d["TitleForeground"]) :
						defaultOptions.AlbumButton.TitleForeground,
					TitleMargin =
						ParseThickness(d, "TitleMargin") ?? defaultOptions.AlbumButton.TitleMargin,
					DescriptionFontFamily = ParseFontFamily(d, "DescriptionFontFamily", defaultOptions.AlbumButton.DescriptionFontFamily),
					DescriptionFontSize =
						ParseDouble(d, "DescriptionFontSize") ?? defaultOptions.AlbumButton.DescriptionFontSize,
					DescriptionForeground =
						d.ContainsKey("DescriptionForeground") ?
						ParseBrush(d["DescriptionForeground"]) :
						defaultOptions.AlbumButton.DescriptionForeground,
					DescriptionMargin =
						ParseThickness(d, "DescriptionMargin") ?? defaultOptions.AlbumButton.DescriptionMargin,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.AlbumButton.Width,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.AlbumButton.Height,
					Padding =
						ParseDouble(d, "Padding") ?? defaultOptions.AlbumButton.Padding,
					BackgroundRadiusX =
						ParseDouble(d, "BackgroundRadiusX") ?? defaultOptions.AlbumButton.BackgroundRadiusX,
					BackgroundRadiusY =
						ParseDouble(d, "BackgroundRadiusY") ?? defaultOptions.AlbumButton.BackgroundRadiusY,
					ThumbnailWidth =
						ParseDouble(d, "ThumbnailWidth") ?? defaultOptions.AlbumButton.ThumbnailWidth,
					ThumbnailHeight =
						ParseDouble(d, "ThumbnailHeight") ?? defaultOptions.AlbumButton.ThumbnailHeight,
					ThumbnailRadiusX =
						ParseDouble(d, "ThumbnailRadiusX") ?? defaultOptions.AlbumButton.ThumbnailRadiusX,
					ThumbnailRadiusY =
						ParseDouble(d, "ThumbnailRadiusY") ?? defaultOptions.AlbumButton.ThumbnailRadiusY,
					ThumbnailMargin =
						ParseThickness(d, "ThumbnailMargin") ?? defaultOptions.AlbumButton.ThumbnailMargin,
					ThumbnailBorderStroke =
						d.ContainsKey("ThumbnailBorderStroke") ?
						ParseBrush(d["ThumbnailBorderStroke"]) :
						defaultOptions.AlbumButton.ThumbnailBorderStroke,
					ThumbnailBorderThickness =
						ParseThickness(d, "ThumbnailBorderThickness") ?? defaultOptions.AlbumButton.ThumbnailBorderThickness
				};
			}
			#endregion

			#region [ SlidePreview ]
			if (xmlOptions.ContainsKey("SlidePreview"))
			{
				Dictionary<string, string> d = xmlOptions["SlidePreview"];
				Options.SlidePreview = new Options.SlidePreviewOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.SlidePreview.Enabled,
					Height =
						ParseDouble(d, "Height") ?? defaultOptions.SlidePreview.Height,
					BorderBrush =
						d.ContainsKey("BorderBrush") ?
						ParseBrush(d["BorderBrush"]) :
						defaultOptions.SlidePreview.BorderBrush,
					BorderWidth =
						ParseDouble(d, "BorderWidth") ?? defaultOptions.SlidePreview.BorderWidth,
					RadiusX =
						ParseDouble(d, "RadiusX") ?? defaultOptions.SlidePreview.RadiusX,
					RadiusY =
						ParseDouble(d, "RadiusY") ?? defaultOptions.SlidePreview.RadiusY
				};
			}
			#endregion

			#region [ SlideViewer ]
			if (xmlOptions.ContainsKey("SlideViewer"))
			{
				Dictionary<string, string> d = xmlOptions["SlideViewer"];
				Options.SlideViewer = new Options.SlideViewerOptions()
				{
					Stretch = ParseStretch(d, "Stretch") ?? defaultOptions.SlideViewer.Stretch
				};
			}
			#endregion

			#region [ ImageViewer ]
			if (xmlOptions.ContainsKey("ImageViewer"))
			{
				Dictionary<string, string> d = xmlOptions["ImageViewer"];
				Options.ImageViewer = new Options.ImageViewerOptions()
				{
				};
			}
			#endregion

			#region [ VideoViewer ]
			if (xmlOptions.ContainsKey("VideoViewer"))
			{
				Dictionary<string, string> d = xmlOptions["VideoViewer"];
				Options.VideoViewer = new Options.VideoViewerOptions()
                {
                    AutoPlay = ParseBool(d, "AutoPlay") ?? defaultOptions.VideoViewer.AutoPlay
				};
			}
			#endregion

			#region [ VideoTray ]
			if (xmlOptions.ContainsKey("VideoTray"))
			{
				Dictionary<string, string> d = xmlOptions["VideoTray"];
				Options.VideoTray = new Options.VideoTrayOptions()
				{
					Enabled =
						ParseBool(d, "Enabled") ?? defaultOptions.VideoTray.Enabled,
					Height =
                        ParseDouble(d, "Height") ?? defaultOptions.VideoTray.Height,
                    Margin =
                        ParseThickness(d, "Margin") ?? defaultOptions.VideoTray.Margin,
					Width =
						ParseDouble(d, "Width") ?? defaultOptions.VideoTray.Width,
					PlayPauseButtonHeight =
						ParseDouble(d, "PlayPauseButtonHeight") ?? defaultOptions.VideoTray.PlayPauseButtonHeight,
					PlayPauseButtonWidth =
						ParseDouble(d, "PlayPauseButtonWidth") ?? defaultOptions.VideoTray.PlayPauseButtonWidth,
					PlayPauseButtonPathHeight =
						ParseDouble(d, "PlayPauseButtonPathHeight") ?? defaultOptions.VideoTray.PlayPauseButtonPathHeight,
					PlayPauseButtonPathWidth =
						ParseDouble(d, "PlayPauseButtonPathWidth") ?? defaultOptions.VideoTray.PlayPauseButtonPathWidth,
					RadiusX =
						ParseDouble(d, "RadiusX") ?? defaultOptions.VideoTray.RadiusX,
					RadiusY =
						ParseDouble(d, "RadiusY") ?? defaultOptions.VideoTray.RadiusY,
					TextFontFamily = ParseFontFamily(d, "TextFontFamily", defaultOptions.VideoTray.TextFontFamily),
					TextFontSize =
						ParseDouble(d, "TextFontSize") ?? defaultOptions.VideoTray.TextFontSize,
					BackgroundOpacity =
						ParseDouble(d, "BackgroundOpacity") ?? defaultOptions.VideoTray.BackgroundOpacity,
					PlayPathData =
						d.ContainsKey("PlayPathData") ?
						d["PlayPathData"] :
						defaultOptions.VideoTray.PlayPathData,
					PausePathData =
						d.ContainsKey("PausePathData") ?
						d["PausePathData"] :
						defaultOptions.VideoTray.PausePathData,
					Foreground =
						d.ContainsKey("Foreground") ?
						ParseBrush(d["Foreground"]) :
						defaultOptions.VideoTray.Foreground,
					ForegroundHover =
						d.ContainsKey("ForegroundHover") ?
						ParseBrush(d["ForegroundHover"]) :
						defaultOptions.VideoTray.ForegroundHover,
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.VideoTray.Background,
					TextForegroundBrush =
						d.ContainsKey("TextForegroundBrush") ?
						ParseBrush(d["TextForegroundBrush"]) :
						defaultOptions.VideoTray.TextForegroundBrush,
					VolumeWidth =
						ParseDouble(d, "VolumeWidth") ?? defaultOptions.VideoTray.VolumeWidth
				};
			}
			#endregion

			#region [ PeopleTag ]
			if (xmlOptions.ContainsKey("PeopleTag"))
			{
				Dictionary<string, string> d = xmlOptions["PeopleTag"];
				Options.PeopleTag = new Options.PeopleTagOptions()
				{
					Enabled = ParseBool(d, "Enabled") ?? defaultOptions.PeopleTag.Enabled,
					PersonNameMargin = ParseThickness(d, "PersonNameMargin") ?? defaultOptions.PeopleTag.PersonNameMargin,
					PersonNameForeground = 
						d.ContainsKey("PersonNameForeground") ?
						ParseBrush(d["PersonNameForeground"]) :
						defaultOptions.PeopleTag.PersonNameForeground,
					PersonNameFontFamily = ParseFontFamily(d, "PersonNameFontFamily", defaultOptions.PeopleTag.PersonNameFontFamily),
					PersonNameFontSize = ParseDouble(d, "PersonNameFontSize") ?? defaultOptions.PeopleTag.PersonNameFontSize,

					BackgroundBorderBrush =
						d.ContainsKey("BackgroundBorderBrush") ?
						ParseBrush(d["BackgroundBorderBrush"]) :
						defaultOptions.PeopleTag.BackgroundBorderBrush,
					Background =
						d.ContainsKey("Background") ?
						ParseBrush(d["Background"]) :
						defaultOptions.PeopleTag.Background,
					BackgroundBorderThickness = ParseThickness(d, "BackgroundBorderThickness") ?? defaultOptions.PeopleTag.BackgroundBorderThickness,
					BackgroundCornerRadius = ParseCornerRadius(d, "BackgroundCornerRadius") ?? defaultOptions.PeopleTag.BackgroundCornerRadius,
					BackgroundOpacity = ParseDouble(d, "BackgroundOpacity") ?? defaultOptions.PeopleTag.BackgroundOpacity
				};
			}
			#endregion

			if (OptionsSet != null)
			{
				OptionsSet(null, null);
			}
		}

		/// <summary>
		/// Parses the root.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>a double</returns>
		private Dictionary<string, string> ParseRoot(XmlReader reader)
		{
			Dictionary<string, string> options = new Dictionary<string, string>();
			options.Add("background", reader.GetAttribute("background"));
			options.Add("AutoStart", reader.GetAttribute("AutoStart") ?? "true");
			options.Add("StartInAlbumView", reader.GetAttribute("StartInAlbumView") ?? "false");
			return options;
		}

		/// <summary>
		/// Parses the options.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>a dictionary of strings</returns>
		private Dictionary<string, string> ParseOptions(XmlReader reader)
		{
			Dictionary<string, string> options = new Dictionary<string, string>();

			while (reader.Read())
			{
				if (reader.IsStartElement() && reader.Name == "option")
				{
					options.Add(
						reader.GetAttribute("name"),
						reader.GetAttribute("value"));
				}
			}

			return options;
		}

		/// <summary>
		/// Parses the bool.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="key">The key.</param>
		/// <returns>a boolean if parse is successful, else returns null</returns>
		private bool? ParseBool(Dictionary<string, string> d, string key)
		{
			string input = d.ContainsKey(key) ?
				d[key] :
				null;

			bool result;
			if (bool.TryParse(input, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Parses the int.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="key">The key.</param>
		/// <returns>int if parse is successful, else returns null</returns>
		private int? ParseInt(Dictionary<string, string> d, string key)
		{
			string input = d.ContainsKey(key) ?
				d[key] :
				null;

			int result;
			if (int.TryParse(input, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Parses the double.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="key">The key.</param>
		/// <returns>double if parse is successful, else returns null</returns>
		private double? ParseDouble(Dictionary<string, string> d, string key)
		{
			string input = d.ContainsKey(key) ?
				d[key] :
				null;

			double result;
			System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowDecimalPoint;
			if (double.TryParse(input, style, System.Globalization.CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Parses the thickness.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="key">The key.</param>
		/// <returns>thickness if parse is successful, else returns null</returns>
		private Thickness? ParseThickness(Dictionary<string, string> d, string key)
		{
			string input = d.ContainsKey(key) ?
				d[key] :
				null;

			if (input == null)
			{
				return null;
			}

			string[] splitInput = input.Split(',');
			int length = splitInput.Count();
			double[] doubleArray = new double[length];

			for (int i = 0; i < length; i++)
			{ 
				double output;
				if (double.TryParse(splitInput[i], out output))
				{
					doubleArray[i] = output;
				}
			}

			if (length == 1)
			{
				return new Thickness(doubleArray[0]);
			}
			else if (length == 4)
			{
				return new Thickness(doubleArray[0], doubleArray[1], doubleArray[2], doubleArray[3]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Parses the corner radius.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="key">The key.</param>
		/// <returns>CornerRadius if parse is successful, else returns null</returns>
        private CornerRadius? ParseCornerRadius(Dictionary<string, string> d, string key)
        {
            string input = d.ContainsKey(key) ?
                d[key] :
                null;

			if (input == null)
			{
				return null;
			}

            string[] splitInput = input.Split(',');
            int length = splitInput.Count();
            double[] doubleArray = new double[length];

            for (int i = 0; i < length; i++)
            {
                double output;
				if (double.TryParse(splitInput[i], out output))
				{
					doubleArray[i] = output;
				}
            }

			if (length == 1)
			{
				return new CornerRadius(doubleArray[0]);
			}
			else if (length == 4)
			{
				return new CornerRadius(doubleArray[0], doubleArray[1], doubleArray[2], doubleArray[3]);
			}
			else
			{
				return null;
			}
        }

		private Stretch? ParseStretch(Dictionary<string, string> d, string key)
		{
			string input = d.ContainsKey(key) ?
				d[key] :
				null;

			if (input == null)
			{
				return null;
			}

			return (Stretch)Enum.Parse(typeof(Stretch), input, true);
		}

		/// <summary>
		/// Parses the FontFamily.
		/// </summary>
		/// <param name="d">The d.</param>
		/// <param name="key">The key.</param>
		/// <returns>FontFamily if parse is successful, else returns null</returns>
		private FontFamily ParseFontFamily(Dictionary<string, string> d, string key, FontFamily defaultFontFamily)
		{
			string input = d.ContainsKey(key) ?
				d[key] :
				null;

			if (input == null)
			{
				return defaultFontFamily;
			}

			return new FontFamily(input);
		}
	}
}