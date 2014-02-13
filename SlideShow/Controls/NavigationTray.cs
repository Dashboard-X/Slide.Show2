using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the NavigationTray
	/// </summary>
	[TemplatePart(Name = NavigationTray.ElementRootName, Type = typeof(Panel))]
	[TemplatePart(Name = NavigationTray.ElementAlbumViewTranslationStoryboardName, Type = typeof(Storyboard))]
	[TemplatePart(Name = NavigationTray.ElementAlbumViewTranslationKeyFrameName, Type = typeof(SplineDoubleKeyFrame))]
	[TemplatePart(Name = NavigationTray.ElementAlbumViewOutTranslationStoryboardName, Type = typeof(Storyboard))]
	[TemplatePart(Name = NavigationTray.ElementAlbumViewOutTranslationKeyFrameName, Type = typeof(SplineDoubleKeyFrame))]
	[TemplatePart(Name = NavigationTray.ElementBackgroundName, Type = typeof(Rectangle))]
	[TemplatePart(Name = NavigationTray.ElementSlideNavigationName, Type = typeof(SlideNavigation))]
	[TemplatePart(Name = NavigationTray.ElementThumbnailViewerRootName, Type = typeof(Canvas))]
	[TemplatePart(Name = NavigationTray.ElementAlbumViewerName, Type = typeof(AlbumViewer))]
	[TemplatePart(Name = NavigationTray.ElementAlbumButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = NavigationTray.ElementSaveHyperlinkButtonName, Type = typeof(HyperlinkButton))]
	[TemplatePart(Name = NavigationTray.ElementSaveButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = NavigationTray.ElementGoToFullScreenButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = NavigationTray.ElementEscapeFullScreenButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = NavigationTray.ElementEmbedButtonName, Type = typeof(PathButton))]
	[TemplateVisualState(Name = "Show", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Hide", GroupName = "CommonStates")]
	
	public partial class NavigationTray : Control
	{
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationTray"/> class.
		/// </summary>
		public NavigationTray()
		{
			IsTabStop = false;
			DefaultStyleKey = typeof(NavigationTray);

			Options.NavigationTrayOptions navigationTrayOptions = Configuration.Options.NavigationTray;
			Options.ToggleAlbumViewButtonOptions toggleAlbumViewButtonOptions = Configuration.Options.ToggleAlbumViewButton;
            Options.ToggleSaveButtonOptions toggleSaveButtonOptions = Configuration.Options.ToggleSaveButton;
			Options.ToggleFullScreenButtonOptions toggleFullScreenButtonOptions = Configuration.Options.ToggleFullScreenButton;
			Options.ToggleEmbedViewButtonOptions toggleEmbedViewButtonOptions = Configuration.Options.ToggleEmbedViewButton;
			Options.ThumbnailViewerOptions thumbnailViewerOptions = Configuration.Options.ThumbnailViewer;
			Options.SlideThumbnailOptions slideThumbnailOptions = Configuration.Options.SlideThumbnail;

			Background = navigationTrayOptions.Background;
			BackgroundOpacity = navigationTrayOptions.BackgroundOpacity;

			AlbumButtonWidth = toggleAlbumViewButtonOptions.Width;
			AlbumButtonHeight = toggleAlbumViewButtonOptions.Height;
			AlbumButtonPathWidth = toggleAlbumViewButtonOptions.PathWidth;
			AlbumButtonPathHeight = toggleAlbumViewButtonOptions.PathHeight;
			AlbumButtonBackground1Brush = toggleAlbumViewButtonOptions.Background1Brush;
			AlbumButtonBackground2Brush = toggleAlbumViewButtonOptions.Background2Brush;
			AlbumButtonForegroundBrush = toggleAlbumViewButtonOptions.ForegroundBrush;
			AlbumButtonForegroundHoverBrush = toggleAlbumViewButtonOptions.ForegroundHoverBrush;
			AlbumButtonRadiusX = toggleAlbumViewButtonOptions.RadiusX;
			AlbumButtonRadiusY = toggleAlbumViewButtonOptions.RadiusY;
			AlbumButtonPathData = toggleAlbumViewButtonOptions.PathData;
			AlbumButtonMargin = toggleAlbumViewButtonOptions.Margin;
			AlbumViewButtonVisibility = toggleAlbumViewButtonOptions.Enabled ? Visibility.Visible : Visibility.Collapsed;

            SaveButtonWidth = toggleSaveButtonOptions.Width;
            SaveButtonHeight = toggleSaveButtonOptions.Height;
            SaveButtonPathWidth = toggleSaveButtonOptions.PathWidth;
            SaveButtonPathHeight = toggleSaveButtonOptions.PathHeight;
            SaveButtonBackground1Brush = toggleSaveButtonOptions.Background1Brush;
            SaveButtonBackground2Brush = toggleSaveButtonOptions.Background2Brush;
            SaveButtonForegroundBrush = toggleSaveButtonOptions.ForegroundBrush;
            SaveButtonForegroundHoverBrush = toggleSaveButtonOptions.ForegroundHoverBrush;
            SaveButtonRadiusX = toggleSaveButtonOptions.RadiusX;
            SaveButtonRadiusY = toggleSaveButtonOptions.RadiusY;
            SaveButtonMargin = toggleSaveButtonOptions.Margin;
            SaveButtonPathData = toggleSaveButtonOptions.PathData;
            SaveButtonVisibility = toggleSaveButtonOptions.Enabled ? Visibility.Visible : Visibility.Collapsed;

			FullScreenButtonWidth = toggleFullScreenButtonOptions.Width;
			FullScreenButtonHeight = toggleFullScreenButtonOptions.Height;
			FullScreenButtonPathWidth = toggleFullScreenButtonOptions.PathWidth;
			FullScreenButtonPathHeight = toggleFullScreenButtonOptions.PathHeight;
			FullScreenButtonBackground1Brush = toggleFullScreenButtonOptions.Background1Brush;
			FullScreenButtonBackground2Brush = toggleFullScreenButtonOptions.Background2Brush;
			FullScreenButtonForegroundBrush = toggleFullScreenButtonOptions.ForegroundBrush;
			FullScreenButtonForegroundHoverBrush = toggleFullScreenButtonOptions.ForegroundHoverBrush;
			FullScreenButtonRadiusX = toggleFullScreenButtonOptions.RadiusX;
			FullScreenButtonRadiusY = toggleFullScreenButtonOptions.RadiusY;
			FullScreenButtonMargin = toggleFullScreenButtonOptions.Margin;

			GoToFullScreenButtonPathData = toggleFullScreenButtonOptions.GoToFullPathData;
			GoToFullScreenButtonVisibility = toggleFullScreenButtonOptions.Enabled ? Visibility.Visible : Visibility.Collapsed;
			EscapeFullScreenButtonPathData = toggleFullScreenButtonOptions.EscapeFullPathData;
			EscapeFullScreenButtonVisibility = Visibility.Collapsed;

			EmbedButtonWidth = toggleEmbedViewButtonOptions.Width;
			EmbedButtonHeight = toggleEmbedViewButtonOptions.Height;
			EmbedButtonPathWidth = toggleEmbedViewButtonOptions.PathWidth;
			EmbedButtonPathHeight = toggleEmbedViewButtonOptions.PathHeight;
			EmbedButtonBackground1Brush = toggleEmbedViewButtonOptions.Background1Brush;
			EmbedButtonBackground2Brush = toggleEmbedViewButtonOptions.Background2Brush;
			EmbedButtonForegroundBrush = toggleEmbedViewButtonOptions.ForegroundBrush;
			EmbedButtonForegroundHoverBrush = toggleEmbedViewButtonOptions.ForegroundHoverBrush;
			EmbedButtonRadiusX = toggleEmbedViewButtonOptions.RadiusX;
			EmbedButtonRadiusY = toggleEmbedViewButtonOptions.RadiusY;
			EmbedButtonPathData = toggleEmbedViewButtonOptions.PathData;
			EmbedButtonMargin = toggleEmbedViewButtonOptions.Margin;
			EmbedViewButtonVisibility = toggleEmbedViewButtonOptions.Enabled && Configuration.Options.EmbedViewer.Enabled ? Visibility.Visible : Visibility.Collapsed;

			TrayHeight = slideThumbnailOptions.Height +
						 thumbnailViewerOptions.Margin.Top +
						 thumbnailViewerOptions.Margin.Bottom +
						 slideThumbnailOptions.BorderThickness.Top +
						 slideThumbnailOptions.BorderThickness.Bottom + thumbnailViewerOptions.BackgroundRadiusY;

			ThumbnailViewerWidth =
				thumbnailViewerOptions.Width
				+ thumbnailViewerOptions.ScrollButtonWidth * 2
				+ thumbnailViewerOptions.ScrollButtonMargin.Left * 2
				+ thumbnailViewerOptions.ScrollButtonMargin.Right * 2
				+ 4 * 2; // 4 = padding on each side
		}
		#endregion

		#region [ Template Parts ]
		internal Panel _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal Storyboard _elementAlbumViewTranslationStoryboard;
		internal const string ElementAlbumViewTranslationStoryboardName = "AlbumViewTranslationStoryboardElement";

		internal SplineDoubleKeyFrame _elementAlbumViewTranslationKeyFrame;
		internal const string ElementAlbumViewTranslationKeyFrameName = "AlbumViewTranslationKeyFrameElement";

		internal Storyboard _elementAlbumViewOutTranslationStoryboard;
		internal const string ElementAlbumViewOutTranslationStoryboardName = "AlbumViewOutTranslationStoryboardElement";

		internal SplineDoubleKeyFrame _elementAlbumViewOutTranslationKeyFrame;
		internal const string ElementAlbumViewOutTranslationKeyFrameName = "AlbumViewOutTranslationKeyFrameElement";

		internal Rectangle _elementBackground;
		internal const string ElementBackgroundName = "BackgroundElement";

		internal SlideNavigation _elementSlideNavigation;
		internal const string ElementSlideNavigationName = "SlideNavigationElement";

		internal Canvas _elementThumbnailViewerRoot;
		internal const string ElementThumbnailViewerRootName = "ThumbnailViewerRootElement";

		internal AlbumViewer _elementAlbumViewer;
		internal const string ElementAlbumViewerName = "AlbumViewerElement";

		internal PathButton _elementAlbumButton;
		internal const string ElementAlbumButtonName = "AlbumButtonElement";

		internal HyperlinkButton _elementSaveHyperlinkButton;
		internal const string ElementSaveHyperlinkButtonName = "SaveHyperlinkButtonElement";

        internal PathButton _elementSaveButton;
        internal const string ElementSaveButtonName = "SaveButtonElement";

		internal PathButton _elementGoToFullScreenButton;
		internal const string ElementGoToFullScreenButtonName = "GoToFullScreenButtonElement";

		internal PathButton _elementEscapeFullScreenButton;
		internal const string ElementEscapeFullScreenButtonName = "EscapeFullScreenButtonElement";

		internal PathButton _elementEmbedButton;
		internal const string ElementEmbedButtonName = "EmbedButtonElement";
		#endregion

		#region [ Private Properties ]
        /// <summary>
        /// Gets or sets the root element.
        /// </summary>
        /// <value>The root element.</value>
		private Panel RootElement
		{
			get { return _elementRoot; }
			set { _elementRoot = value; }
		}

		/// <summary>
		/// Gets or sets the AlbumViewTranslationStoryboard element.
		/// </summary>
		/// <value>The AlbumViewTranslationStoryboard element.</value>
		private Storyboard AlbumViewTranslationStoryboardElement
		{
			get { return _elementAlbumViewTranslationStoryboard; }
			set { _elementAlbumViewTranslationStoryboard = value; }
		}

		/// <summary>
		/// Gets or sets the AlbumViewTranslationKeyFrame element.
		/// </summary>
		/// <value>The AlbumViewTranslationKeyFrame element.</value>
		private SplineDoubleKeyFrame AlbumViewTranslationKeyFrameElement
		{
			get { return _elementAlbumViewTranslationKeyFrame; }
			set { _elementAlbumViewTranslationKeyFrame = value; }
		}

		/// <summary>
		/// Gets or sets the AlbumViewOutTranslationStoryboard element.
		/// </summary>
		/// <value>The AlbumViewOutTranslationStoryboard element.</value>
		private Storyboard AlbumViewOutTranslationStoryboardElement
		{
			get { return _elementAlbumViewOutTranslationStoryboard; }
			set { _elementAlbumViewOutTranslationStoryboard = value; }
		}

		/// <summary>
		/// Gets or sets the AlbumViewOutTranslationKeyFrame element.
		/// </summary>
		/// <value>The AlbumViewOutTranslationKeyFrame element.</value>
		private SplineDoubleKeyFrame AlbumViewOutTranslationKeyFrameElement
		{
			get { return _elementAlbumViewOutTranslationKeyFrame; }
			set { _elementAlbumViewOutTranslationKeyFrame = value; }
		}

		/// <summary>
		/// Gets or sets the background element.
		/// </summary>
		/// <value>The background element.</value>
		private Rectangle BackgroundElement
		{
			get { return _elementBackground; }
			set { _elementBackground = value; }
		}

		#endregion

		#region [ Public Properties ]

		/// <summary>
        /// Gets or sets the slide navigation element.
        /// </summary>
        /// <value>The slide navigation element.</value>
		public SlideNavigation SlideNavigationElement
		{
			get { return _elementSlideNavigation; }
			set { _elementSlideNavigation = value; }
		}

        /// <summary>
        /// Gets or sets the thumbnail viewer root element.
        /// </summary>
        /// <value>The thumbnail viewer root element.</value>
		public Canvas ThumbnailViewerRootElement
		{
			get { return _elementThumbnailViewerRoot; }
			set { _elementThumbnailViewerRoot = value; }
		}

        /// <summary>
        /// Gets or sets the album viewer element.
        /// </summary>
        /// <value>The album viewer element.</value>
		public AlbumViewer AlbumViewerElement
		{
			get { return _elementAlbumViewer; }
			set { _elementAlbumViewer = value; }
		}

        /// <summary>
        /// Gets or sets the album button element.
        /// </summary>
        /// <value>The album button element.</value>
		public PathButton AlbumButtonElement
		{
			get { return _elementAlbumButton; }
			set 
			{ 
				_elementAlbumButton = value;
				if (!String.IsNullOrEmpty(Configuration.Options.ToggleAlbumViewButton.Tooltip))
					ToolTipService.SetToolTip(_elementAlbumButton, Configuration.Options.ToggleAlbumViewButton.Tooltip);
			}
		}

		/// <summary>
		/// Gets or sets the save hyperlink button element.
		/// </summary>
		/// <value>The ave hyperlink button element.</value>
		public HyperlinkButton SaveHyperlinkButtonElement
		{
			get { return _elementSaveHyperlinkButton; }
			set 
			{ 
				_elementSaveHyperlinkButton = value;
				if (!String.IsNullOrEmpty(Configuration.Options.ToggleSaveButton.Tooltip))
					ToolTipService.SetToolTip(_elementSaveHyperlinkButton, Configuration.Options.ToggleSaveButton.Tooltip);
			}
		}

		/// <summary>
        /// Gets or sets the save button element.
        /// </summary>
        /// <value>The save button element.</value>
        public PathButton SaveButtonElement
        {
            get { return _elementSaveButton; }
            set { _elementSaveButton = value; }
        }

        /// <summary>
		/// Gets or sets the go to full screen button element.
		/// </summary>
		/// <value>The go to full screen button element.</value>
		public PathButton GoToFullScreenButtonElement
		{
			get { return _elementGoToFullScreenButton; }
			set 
			{ 
				_elementGoToFullScreenButton = value;
				if (!String.IsNullOrEmpty(Configuration.Options.ToggleFullScreenButton.GoToFullTooltip))
					ToolTipService.SetToolTip(_elementGoToFullScreenButton, Configuration.Options.ToggleFullScreenButton.GoToFullTooltip);
			}
		}

		/// <summary>
		/// Gets or sets the escape full screen button element.
		/// </summary>
		/// <value>The escape full screen button element.</value>
		public PathButton EscapeFullScreenButtonElement
		{
			get { return _elementEscapeFullScreenButton; }
			set 
			{ 
				_elementEscapeFullScreenButton = value;
				if (!String.IsNullOrEmpty(Configuration.Options.ToggleFullScreenButton.EscapeFullTooltip))
					ToolTipService.SetToolTip(_elementEscapeFullScreenButton, Configuration.Options.ToggleFullScreenButton.EscapeFullTooltip);
			}
		}

		/// <summary>
		/// Gets or sets the embed button element.
		/// </summary>
		/// <value>The embed button element.</value>
		public PathButton EmbedButtonElement
		{
			get { return _elementEmbedButton; }
			set 
			{ 
				_elementEmbedButton = value;
				if (!String.IsNullOrEmpty(Configuration.Options.ToggleEmbedViewButton.Tooltip))
					ToolTipService.SetToolTip(_elementEmbedButton, Configuration.Options.ToggleEmbedViewButton.Tooltip);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is album view.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is album view; otherwise, <c>false</c>.
		/// </value>
		public static bool IsAlbumView { get; private set; }

		#endregion

		#region [ Dependency Properties ]

			#region [ AlbumViewOffset ]

			/// <summary>
			/// Gets or sets the album view offset.
			/// </summary>
			/// <value>The album view offset.</value>
			public double AlbumViewOffset
			{
				get { return (double)GetValue(AlbumViewOffsetProperty); }
				set { SetValue(AlbumViewOffsetProperty, value); }
			}

			/// <summary> 
			/// Identifies the AlbumViewOffset dependency property.
			/// </summary>
			public static readonly DependencyProperty AlbumViewOffsetProperty =
				DependencyProperty.Register(
					"AlbumViewOffset",
					typeof(double),
					typeof(NavigationTray),
					null);

			#endregion

			#region [ BackgroundOpacity ]

			/// <summary>
			/// Gets or sets the background opacity.
			/// </summary>
			/// <value>The album view offset.</value>
			public double BackgroundOpacity
			{
				get { return (double)GetValue(BackgroundOpacityProperty); }
				set { SetValue(BackgroundOpacityProperty, value); }
			}

			/// <summary> 
			/// Identifies the BackgroundOpacity dependency property.
			/// </summary>
			public static readonly DependencyProperty BackgroundOpacityProperty =
				DependencyProperty.Register(
					"BackgroundOpacity",
					typeof(double),
					typeof(NavigationTray),
					null);

			#endregion

			#region [ ThumbnailViewerWidth ]
			public double ThumbnailViewerWidth
			{
				get { return (double)GetValue(ThumbnailViewerWidthProperty); }
				set { SetValue(ThumbnailViewerWidthProperty, value); }
			}

			/// <summary>
			/// Identifies the ThumbnailViewerWidth dependency property.
			/// </summary>
			public static readonly DependencyProperty ThumbnailViewerWidthProperty =
				DependencyProperty.Register(
					"ThumbnailViewerWidth",
					typeof(double),
					typeof(NavigationTray),
					null);

			#endregion

			#region [ TrayHeight ]
			public double TrayHeight
			{
				get { return (double)GetValue(TrayHeightProperty); }
				set { SetValue(TrayHeightProperty, value); }
			}

			public static readonly DependencyProperty TrayHeightProperty =
				DependencyProperty.Register(
					"TrayHeight",
					typeof(double),
					typeof(NavigationTray),
					null);

			#endregion

			#region [ AlbumButton ]

				#region [ AlbumButtonWidth ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double AlbumButtonWidth
				{
					get { return (double)GetValue(AlbumButtonWidthProperty); }
					set { SetValue(AlbumButtonWidthProperty, value); }
				}

				/// <summary>
				/// Identifies the Height dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonWidthProperty =
					DependencyProperty.Register(
						"AlbumButtonWidth",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ AlbumButtonHeight ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double AlbumButtonHeight
				{
					get { return (double)GetValue(AlbumButtonHeightProperty); }
					set { SetValue(AlbumButtonHeightProperty, value); }
				}

				/// <summary>
				/// Identifies the Height dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonHeightProperty =
					DependencyProperty.Register(
						"AlbumButtonHeight",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ AlbumButtonPathWidth ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double AlbumButtonPathWidth
				{
					get { return (double)GetValue(AlbumButtonPathWidthProperty); }
					set { SetValue(AlbumButtonPathWidthProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonPathWidthProperty =
					DependencyProperty.Register(
						"AlbumButtonPathWidth",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ AlbumButtonPathHeight ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double AlbumButtonPathHeight
				{
					get { return (double)GetValue(AlbumButtonPathHeightProperty); }
					set { SetValue(AlbumButtonPathHeightProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonPathHeightProperty =
					DependencyProperty.Register(
						"AlbumButtonPathHeight",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ AlbumButtonBackground1Brush ]
				/// <summary>
				/// Gets or sets the album button brush.
				/// </summary>
				/// <value>The album button brush.</value>
				public Brush AlbumButtonBackground1Brush
				{
					get { return GetValue(AlbumButtonBackground1BrushProperty) as Brush; }
					set { SetValue(AlbumButtonBackground1BrushProperty, value); }
				}

				/// <summary>
				/// Identifies the AlbumButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonBackground1BrushProperty =
					DependencyProperty.Register(
						"AlbumButtonBackground1Brush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ AlbumButtonBackground2Brush ]
				/// <summary>
				/// Gets or sets the album button brush.
				/// </summary>
				/// <value>The album button brush.</value>
				public Brush AlbumButtonBackground2Brush
				{
					get { return GetValue(AlbumButtonBackground2BrushProperty) as Brush; }
					set { SetValue(AlbumButtonBackground2BrushProperty, value); }
				}

				/// <summary>
				/// Identifies the AlbumButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonBackground2BrushProperty =
					DependencyProperty.Register(
						"AlbumButtonBackground2Brush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ AlbumButtonForegroundBrush ]
				/// <summary>
				/// Gets or sets the album button brush.
				/// </summary>
				/// <value>The album button brush.</value>
				public Brush AlbumButtonForegroundBrush
				{
					get { return GetValue(AlbumButtonForegroundBrushProperty) as Brush; }
					set { SetValue(AlbumButtonForegroundBrushProperty, value); }
				}

				/// <summary>
				/// Identifies the AlbumButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonForegroundBrushProperty =
					DependencyProperty.Register(
						"AlbumButtonForegroundBrush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ AlbumButtonForegroundHoverBrush ]
				/// <summary>
				/// Gets or sets the album button hover brush.
				/// </summary>
				/// <value>The album button hover brush.</value>
				public Brush AlbumButtonForegroundHoverBrush
				{
					get { return GetValue(AlbumButtonForegroundHoverBrushProperty) as Brush; }
					set { SetValue(AlbumButtonForegroundHoverBrushProperty, value); }
				}

				/// <summary>
				/// Identifies the AlbumButtonHoverBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonForegroundHoverBrushProperty =
					DependencyProperty.Register(
						"AlbumButtonForegroundHoverBrush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ AlbumButtonRadiusX ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double AlbumButtonRadiusX
				{
					get { return (double)GetValue(AlbumButtonRadiusXProperty); }
					set { SetValue(AlbumButtonRadiusXProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonRadiusXProperty =
					DependencyProperty.Register(
						"AlbumButtonRadiusX",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ AlbumButtonRadiusY ]
				/// <summary>
				/// Gets or sets the Y radius of the background rectangle.
				/// </summary>
				/// <value>the Y radius of the background rectangle.</value>
				public double AlbumButtonRadiusY
				{
					get { return (double)GetValue(AlbumButtonRadiusYProperty); }
					set { SetValue(AlbumButtonRadiusYProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonRadiusYProperty =
					DependencyProperty.Register(
						"AlbumButtonRadiusY",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ AlbumViewButtonVisibility ]
				public Visibility AlbumViewButtonVisibility
				{
					get { return (Visibility)GetValue(AlbumViewButtonVisibilityProperty); }
					set { SetValue(AlbumViewButtonVisibilityProperty, value); }
				}

				/// <summary>
				/// Identifies the AlbumViewButtonVisibility dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumViewButtonVisibilityProperty =
					DependencyProperty.Register(
						"AlbumViewButtonVisibility",
						typeof(Visibility),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ AlbumButtonPathData ]
			/// <summary>
			/// Gets or sets the album button path data.
			/// </summary>
			/// <value>The album button path data.</value>
			public string AlbumButtonPathData
			{
				get { return (string)GetValue(AlbumButtonPathDataProperty); }
				set { SetValue(AlbumButtonPathDataProperty,  value); }
			}

			/// <summary>
			/// Identifies the AlbumButtonPathData dependency property.
			/// </summary>
			public static readonly DependencyProperty AlbumButtonPathDataProperty =
				DependencyProperty.Register(
					"AlbumButtonPathData",
					typeof(string),
					typeof(NavigationTray),
					null);
				#endregion

				#region [ AlbumButtonMargin ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public Thickness AlbumButtonMargin
				{
					get { return (Thickness)GetValue(AlbumButtonMarginProperty); }
					set { SetValue(AlbumButtonMarginProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty AlbumButtonMarginProperty =
					DependencyProperty.Register(
						"AlbumButtonMargin",
						typeof(Thickness),
						typeof(NavigationTray),
						null);
				#endregion

			#endregion

            #region [ SaveButton ]

                #region [ SaveButtonWidth ]
                /// <summary>
                /// Gets or sets the width of the button rectangle.
                /// </summary>
                /// <value>the width of the button rectangle.</value>
                public double SaveButtonWidth
                {
                    get { return (double)GetValue(SaveButtonWidthProperty); }
                    set { SetValue(SaveButtonWidthProperty, value); }
                }

                /// <summary>
                /// Identifies the Width dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonWidthProperty =
                    DependencyProperty.Register(
                        "SaveButtonWidth",
                        typeof(double),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonHeight ]
                /// <summary>
                /// Gets or sets the height of the button rectangle.
                /// </summary>
                /// <value>the height of the button rectangle.</value>
                public double SaveButtonHeight
                {
                    get { return (double)GetValue(SaveButtonHeightProperty); }
                    set { SetValue(SaveButtonHeightProperty, value); }
                }

                /// <summary>
                /// Identifies the Height dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonHeightProperty =
                    DependencyProperty.Register(
                        "SaveButtonHeight",
                        typeof(double),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonPathWidth ]
                /// <summary>
                /// Gets or sets the path width.
                /// </summary>
                /// <value>the path width.</value>
                public double SaveButtonPathWidth
                {
                    get { return (double)GetValue(SaveButtonPathWidthProperty); }
                    set { SetValue(SaveButtonPathWidthProperty, value); }
                }

                /// <summary>
                /// Identifies the PathWidth dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonPathWidthProperty =
                    DependencyProperty.Register(
                        "SaveButtonPathWidth",
                        typeof(double),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonPathHeight ]
                /// <summary>
                /// Gets or sets the path height.
                /// </summary>
                /// <value>the path height.</value>
                public double SaveButtonPathHeight
                {
                    get { return (double)GetValue(SaveButtonPathHeightProperty); }
                    set { SetValue(SaveButtonPathHeightProperty, value); }
                }

                /// <summary>
                /// Identifies the PathHeight dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonPathHeightProperty =
                    DependencyProperty.Register(
                        "SaveButtonPathHeight",
                        typeof(double),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonBackground1Brush ]
                /// <summary>
                /// Gets or sets the start gradient info button brush.
                /// </summary>
                /// <value>The start gradient info button brush.</value>
                public Brush SaveButtonBackground1Brush
                {
                    get { return GetValue(SaveButtonBackground1BrushProperty) as Brush; }
                    set { SetValue(SaveButtonBackground1BrushProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonBackground1Brush dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonBackground1BrushProperty =
                    DependencyProperty.Register(
                        "SaveButtonBackground1Brush",
                        typeof(Brush),
                        typeof(NavigationTray),
                        null);

                #endregion

                #region [ SaveButtonBackground2Brush ]
                /// <summary>
                /// Gets or sets the end gradient info button brush.
                /// </summary>
                /// <value>The end gradient info button brush.</value>
                public Brush SaveButtonBackground2Brush
                {
                    get { return GetValue(SaveButtonBackground2BrushProperty) as Brush; }
                    set { SetValue(SaveButtonBackground2BrushProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonBackground2Brush dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonBackground2BrushProperty =
                    DependencyProperty.Register(
                        "SaveButtonBackground2Brush",
                        typeof(Brush),
                        typeof(NavigationTray),
                        null);

                #endregion

                #region [ SaveButtonForegroundBrush ]
                /// <summary>
                /// Gets or sets the foreground info button brush.
                /// </summary>
                /// <value>The foreground info button brush.</value>
                public Brush SaveButtonForegroundBrush
                {
                    get { return GetValue(SaveButtonForegroundBrushProperty) as Brush; }
                    set { SetValue(SaveButtonForegroundBrushProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonForegroundBrush dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonForegroundBrushProperty =
                    DependencyProperty.Register(
                        "SaveButtonForegroundBrush",
                        typeof(Brush),
                        typeof(NavigationTray),
                        null);

                #endregion

                #region [ SaveButtonForegroundHoverBrush ]
                /// <summary>
                /// Gets or sets the info button hover brush.
                /// </summary>
                /// <value>The info button hover brush.</value>
                public Brush SaveButtonForegroundHoverBrush
                {
                    get { return GetValue(SaveButtonForegroundHoverBrushProperty) as Brush; }
                    set { SetValue(SaveButtonForegroundHoverBrushProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonForegroundHoverBrush dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonForegroundHoverBrushProperty =
                    DependencyProperty.Register(
                        "SaveButtonForegroundHoverBrush",
                        typeof(Brush),
                        typeof(NavigationTray),
                        null);

                #endregion

                #region [ SaveButtonRadiusX ]
                /// <summary>
                /// Gets or sets the X radius of the background rectangle.
                /// </summary>
                /// <value>the X radius of the background rectangle.</value>
                public double SaveButtonRadiusX
                {
                    get { return (double)GetValue(SaveButtonRadiusXProperty); }
                    set { SetValue(SaveButtonRadiusXProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonRadiusX dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonRadiusXProperty =
                    DependencyProperty.Register(
                        "SaveButtonRadiusX",
                        typeof(double),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonRadiusY ]
                /// <summary>
                /// Gets or sets the Y radius of the background rectangle.
                /// </summary>
                /// <value>the Y radius of the background rectangle.</value>
                public double SaveButtonRadiusY
                {
                    get { return (double)GetValue(SaveButtonRadiusYProperty); }
                    set { SetValue(SaveButtonRadiusYProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonRadiusY dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonRadiusYProperty =
                    DependencyProperty.Register(
                        "SaveButtonRadiusY",
                        typeof(double),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonVisibility ]
                public Visibility SaveButtonVisibility
                {
                    get { return (Visibility)GetValue(SaveButtonVisibilityProperty); }
                    set { SetValue(SaveButtonVisibilityProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonVisibility dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonVisibilityProperty =
                    DependencyProperty.Register(
                        "SaveButtonVisibility",
                        typeof(Visibility),
                        typeof(NavigationTray),
                        null);

                #endregion

                #region [ SaveButtonPathData ]
                /// <summary>
                /// Gets or sets the info button path data.
                /// </summary>
                /// <value>The info button path data.</value>
                public string SaveButtonPathData
                {
                    get { return (string)GetValue(SaveButtonPathDataProperty); }
                    set { SetValue(SaveButtonPathDataProperty, value); }
                }

                /// <summary>
                /// Identifies the SaveButtonPathData dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonPathDataProperty =
                    DependencyProperty.Register(
                        "SaveButtonPathData",
                        typeof(string),
                        typeof(NavigationTray),
                        null);
                #endregion

                #region [ SaveButtonMargin ]
                /// <summary>
                /// Gets or sets the X radius of the background rectangle.
                /// </summary>
                /// <value>the X radius of the background rectangle.</value>
                public Thickness SaveButtonMargin
                {
                    get { return (Thickness)GetValue(SaveButtonMarginProperty); }
                    set { SetValue(SaveButtonMarginProperty, value); }
                }

                /// <summary>
                /// Identifies the PathHeight dependency property.
                /// </summary>
                public static readonly DependencyProperty SaveButtonMarginProperty =
                    DependencyProperty.Register(
                        "SaveButtonMargin",
                        typeof(Thickness),
                        typeof(NavigationTray),
                        null);
                #endregion

            #endregion

			#region [ FullScreenButton ]

				#region [ FullScreenButtonWidth ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double FullScreenButtonWidth
				{
					get { return (double)GetValue(FullScreenButtonWidthProperty); }
					set { SetValue(FullScreenButtonWidthProperty, value); }
				}

				/// <summary>
				/// Identifies the Height dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonWidthProperty =
					DependencyProperty.Register(
						"FullScreenButtonWidth",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ FullScreenButtonHeight ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double FullScreenButtonHeight
				{
					get { return (double)GetValue(FullScreenButtonHeightProperty); }
					set { SetValue(FullScreenButtonHeightProperty, value); }
				}

				/// <summary>
				/// Identifies the Height dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonHeightProperty =
					DependencyProperty.Register(
						"FullScreenButtonHeight",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ FullScreenButtonPathWidth ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double FullScreenButtonPathWidth
				{
					get { return (double)GetValue(FullScreenButtonPathWidthProperty); }
					set { SetValue(FullScreenButtonPathWidthProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonPathWidthProperty =
					DependencyProperty.Register(
						"FullScreenButtonPathWidth",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ FullScreenButtonPathHeight ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double FullScreenButtonPathHeight
				{
					get { return (double)GetValue(FullScreenButtonPathHeightProperty); }
					set { SetValue(FullScreenButtonPathHeightProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonPathHeightProperty =
					DependencyProperty.Register(
						"FullScreenButtonPathHeight",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ FullScreenButtonBackground1Brush ]
				/// <summary>
				/// Gets or sets the full screen button brush.
				/// </summary>
				/// <value>The full screen button brush.</value>
				public Brush FullScreenButtonBackground1Brush
				{
					get { return GetValue(FullScreenButtonBackground1BrushProperty) as Brush; }
					set { SetValue(FullScreenButtonBackground1BrushProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonBackground1BrushProperty =
					DependencyProperty.Register(
						"FullScreenButtonBackground1Brush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ FullScreenButtonBackground2Brush ]
				/// <summary>
				/// Gets or sets the full screen button brush.
				/// </summary>
				/// <value>The full screen button brush.</value>
				public Brush FullScreenButtonBackground2Brush
				{
					get { return GetValue(FullScreenButtonBackground2BrushProperty) as Brush; }
					set { SetValue(FullScreenButtonBackground2BrushProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonBackground2BrushProperty =
					DependencyProperty.Register(
						"FullScreenButtonBackground2Brush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ FullScreenButtonForegroundBrush ]
				/// <summary>
				/// Gets or sets the full screen button brush.
				/// </summary>
				/// <value>The full screen button brush.</value>
				public Brush FullScreenButtonForegroundBrush
				{
					get { return GetValue(FullScreenButtonForegroundBrushProperty) as Brush; }
					set { SetValue(FullScreenButtonForegroundBrushProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonForegroundBrushProperty =
					DependencyProperty.Register(
						"FullScreenButtonForegroundBrush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ FullScreenButtonForegroundHoverBrush ]
				/// <summary>
				/// Gets or sets the full screen button hover brush.
				/// </summary>
				/// <value>The full screen button hover brush.</value>
				public Brush FullScreenButtonForegroundHoverBrush
				{
					get { return GetValue(FullScreenButtonForegroundHoverBrushProperty) as Brush; }
					set { SetValue(FullScreenButtonForegroundHoverBrushProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonHoverBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonForegroundHoverBrushProperty =
					DependencyProperty.Register(
						"FullScreenButtonForegroundHoverBrush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ FullScreenButtonRadiusX ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double FullScreenButtonRadiusX
				{
					get { return (double)GetValue(FullScreenButtonRadiusXProperty); }
					set { SetValue(FullScreenButtonRadiusXProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonRadiusXProperty =
					DependencyProperty.Register(
						"FullScreenButtonRadiusX",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ FullScreenButtonRadiusY ]
				/// <summary>
				/// Gets or sets the Y radius of the background rectangle.
				/// </summary>
				/// <value>the Y radius of the background rectangle.</value>
				public double FullScreenButtonRadiusY
				{
					get { return (double)GetValue(FullScreenButtonRadiusYProperty); }
					set { SetValue(FullScreenButtonRadiusYProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonRadiusYProperty =
					DependencyProperty.Register(
						"FullScreenButtonRadiusY",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ GoToFullScreenButtonVisibility ]
				public Visibility GoToFullScreenButtonVisibility
				{
					get { return (Visibility)GetValue(GoToFullScreenButtonVisibilityProperty); }
					set { SetValue(GoToFullScreenButtonVisibilityProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonHoverBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty GoToFullScreenButtonVisibilityProperty =
					DependencyProperty.Register(
						"GoToFullScreenButtonVisibility",
						typeof(Visibility),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ EscapeFullScreenButtonVisibility ]
				public Visibility EscapeFullScreenButtonVisibility
				{
					get { return (Visibility)GetValue(EscapeFullScreenButtonVisibilityProperty); }
					set { SetValue(EscapeFullScreenButtonVisibilityProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonHoverBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty EscapeFullScreenButtonVisibilityProperty =
					DependencyProperty.Register(
						"EscapeFullScreenButtonVisibility",
						typeof(Visibility),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ GoToFullScreenButtonPathData ]
				/// <summary>
				/// Gets or sets the album button path data.
				/// </summary>
				/// <value>The album button path data.</value>
				public string GoToFullScreenButtonPathData
				{
					get { return (string)GetValue(GoToFullScreenButtonPathDataProperty); }
					set { SetValue(GoToFullScreenButtonPathDataProperty, value); }
				}

				/// <summary>
				/// Identifies the FullScreenButtonPathData dependency property.
				/// </summary>
				public static readonly DependencyProperty GoToFullScreenButtonPathDataProperty =
					DependencyProperty.Register(
						"GoToFullScreenButtonPathData",
						typeof(string),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EscapeFullScreenButtonPathData ]
			/// <summary>
			/// Gets or sets the album button path data.
			/// </summary>
			/// <value>The album button path data.</value>
			public string EscapeFullScreenButtonPathData
			{
				get { return (string)GetValue(EscapeFullScreenButtonPathDataProperty); }
				set { SetValue(EscapeFullScreenButtonPathDataProperty, value); }
			}

			/// <summary>
			/// Identifies the FullScreenButtonPathData dependency property.
			/// </summary>
			public static readonly DependencyProperty EscapeFullScreenButtonPathDataProperty =
				DependencyProperty.Register(
					"EscapeFullScreenButtonPathData",
					typeof(string),
					typeof(NavigationTray),
					null);
				#endregion

				#region [ FullScreenButtonMargin ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public Thickness FullScreenButtonMargin
				{
					get { return (Thickness)GetValue(FullScreenButtonMarginProperty); }
					set { SetValue(FullScreenButtonMarginProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty FullScreenButtonMarginProperty =
					DependencyProperty.Register(
						"FullScreenButtonMargin",
						typeof(Thickness),
						typeof(NavigationTray),
						null);
				#endregion

			#endregion

			#region [ EmbedButton ]

				#region [ EmbedButtonWidth ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double EmbedButtonWidth
				{
					get { return (double)GetValue(EmbedButtonWidthProperty); }
					set { SetValue(EmbedButtonWidthProperty, value); }
				}

				/// <summary>
				/// Identifies the Height dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonWidthProperty =
					DependencyProperty.Register(
						"EmbedButtonWidth",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedButtonHeight ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double EmbedButtonHeight
				{
					get { return (double)GetValue(EmbedButtonHeightProperty); }
					set { SetValue(EmbedButtonHeightProperty, value); }
				}

				/// <summary>
				/// Identifies the Height dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonHeightProperty =
					DependencyProperty.Register(
						"EmbedButtonHeight",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedButtonPathWidth ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double EmbedButtonPathWidth
				{
					get { return (double)GetValue(EmbedButtonPathWidthProperty); }
					set { SetValue(EmbedButtonPathWidthProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonPathWidthProperty =
					DependencyProperty.Register(
						"EmbedButtonPathWidth",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedButtonPathHeight ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double EmbedButtonPathHeight
				{
					get { return (double)GetValue(EmbedButtonPathHeightProperty); }
					set { SetValue(EmbedButtonPathHeightProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonPathHeightProperty =
					DependencyProperty.Register(
						"EmbedButtonPathHeight",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedButtonBackground1Brush ]
				/// <summary>
				/// Gets or sets the album button brush.
				/// </summary>
				/// <value>The album button brush.</value>
				public Brush EmbedButtonBackground1Brush
				{
					get { return GetValue(EmbedButtonBackground1BrushProperty) as Brush; }
					set { SetValue(EmbedButtonBackground1BrushProperty, value); }
				}

				/// <summary>
				/// Identifies the EmbedButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonBackground1BrushProperty =
					DependencyProperty.Register(
						"EmbedButtonBackground1Brush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ EmbedButtonBackground2Brush ]
				/// <summary>
				/// Gets or sets the album button brush.
				/// </summary>
				/// <value>The album button brush.</value>
				public Brush EmbedButtonBackground2Brush
				{
					get { return GetValue(EmbedButtonBackground2BrushProperty) as Brush; }
					set { SetValue(EmbedButtonBackground2BrushProperty, value); }
				}

				/// <summary>
				/// Identifies the EmbedButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonBackground2BrushProperty =
					DependencyProperty.Register(
						"EmbedButtonBackground2Brush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ EmbedButtonForegroundBrush ]
				/// <summary>
				/// Gets or sets the album button brush.
				/// </summary>
				/// <value>The album button brush.</value>
				public Brush EmbedButtonForegroundBrush
				{
					get { return GetValue(EmbedButtonForegroundBrushProperty) as Brush; }
					set { SetValue(EmbedButtonForegroundBrushProperty, value); }
				}

				/// <summary>
				/// Identifies the EmbedButtonBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonForegroundBrushProperty =
					DependencyProperty.Register(
						"EmbedButtonForegroundBrush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ EmbedButtonForegroundHoverBrush ]
				/// <summary>
				/// Gets or sets the album button hover brush.
				/// </summary>
				/// <value>The album button hover brush.</value>
				public Brush EmbedButtonForegroundHoverBrush
				{
					get { return GetValue(EmbedButtonForegroundHoverBrushProperty) as Brush; }
					set { SetValue(EmbedButtonForegroundHoverBrushProperty, value); }
				}

				/// <summary>
				/// Identifies the EmbedButtonHoverBrush dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonForegroundHoverBrushProperty =
					DependencyProperty.Register(
						"EmbedButtonForegroundHoverBrush",
						typeof(Brush),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ EmbedButtonRadiusX ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double EmbedButtonRadiusX
				{
					get { return (double)GetValue(EmbedButtonRadiusXProperty); }
					set { SetValue(EmbedButtonRadiusXProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonRadiusXProperty =
					DependencyProperty.Register(
						"EmbedButtonRadiusX",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedButtonRadiusY ]
				/// <summary>
				/// Gets or sets the Y radius of the background rectangle.
				/// </summary>
				/// <value>the Y radius of the background rectangle.</value>
				public double EmbedButtonRadiusY
				{
					get { return (double)GetValue(EmbedButtonRadiusYProperty); }
					set { SetValue(EmbedButtonRadiusYProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonRadiusYProperty =
					DependencyProperty.Register(
						"EmbedButtonRadiusY",
						typeof(double),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedViewButtonVisibility ]
				public Visibility EmbedViewButtonVisibility
				{
					get { return (Visibility)GetValue(EmbedViewButtonVisibilityProperty); }
					set { SetValue(EmbedViewButtonVisibilityProperty, value); }
				}

				/// <summary>
				/// Identifies the EmbedViewButtonVisibility dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedViewButtonVisibilityProperty =
					DependencyProperty.Register(
						"EmbedViewButtonVisibility",
						typeof(Visibility),
						typeof(NavigationTray),
						null);

				#endregion

				#region [ EmbedButtonPathData ]
				/// <summary>
				/// Gets or sets the album button path data.
				/// </summary>
				/// <value>The album button path data.</value>
				public string EmbedButtonPathData
				{
					get { return (string)GetValue(EmbedButtonPathDataProperty); }
					set { SetValue(EmbedButtonPathDataProperty, value); }
				}

				/// <summary>
				/// Identifies the EmbedButtonPathData dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonPathDataProperty =
					DependencyProperty.Register(
						"EmbedButtonPathData",
						typeof(string),
						typeof(NavigationTray),
						null);
				#endregion

				#region [ EmbedButtonMargin ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public Thickness EmbedButtonMargin
				{
					get { return (Thickness)GetValue(EmbedButtonMarginProperty); }
					set { SetValue(EmbedButtonMarginProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty EmbedButtonMarginProperty =
					DependencyProperty.Register(
						"EmbedButtonMargin",
						typeof(Thickness),
						typeof(NavigationTray),
						null);
				#endregion

			#endregion

		#endregion

		#region [ Events ]

		/// <summary>
		/// Occurs when [finished loading].
		/// </summary>
		public event EventHandler FinishedLoading;

		#endregion

        /// <summary>
        /// Displays the thumb nail viewer.
        /// </summary>
        /// <param name="thumbnailViewer">The thumbnail viewer.</param>
		public void DisplayThumbnailViewer(ThumbnailViewer thumbnailViewer)
		{
			Thickness margin = Configuration.Options.ThumbnailViewer.Margin;
			thumbnailViewer.Margin = new Thickness(0, margin.Top, 0, margin.Bottom);
			if (ThumbnailViewerRootElement != null)
			{
				ThumbnailViewerRootElement.Children.Clear();
				ThumbnailViewerRootElement.Children.Add(thumbnailViewer);
			}
			
			thumbnailViewer.Populate();
		}

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(Panel).IsInstanceOfType(root) || (root == null),
				"The template part RootElement is not an instance of Panel!");
			
			RootElement = root as Panel;

			if (RootElement != null)
			{
				object albumViewTranslationStoryboard = GetTemplateChild(ElementAlbumViewTranslationStoryboardName);
				Debug.Assert(typeof(Storyboard).IsInstanceOfType(albumViewTranslationStoryboard) || (albumViewTranslationStoryboard == null),
					"The template part AlbumViewTranslationStoryboardElement is not an instance of Storyboard!");
				AlbumViewTranslationStoryboardElement = albumViewTranslationStoryboard as Storyboard;

				object albumViewTranslationKeyFrame = GetTemplateChild(ElementAlbumViewTranslationKeyFrameName);
				Debug.Assert(typeof(SplineDoubleKeyFrame).IsInstanceOfType(albumViewTranslationKeyFrame) || (albumViewTranslationKeyFrame == null),
					"The template part AlbumViewTranslationKeyFrameElement is not an instance of SplineDoubleKeyFrame!");
				
				AlbumViewTranslationKeyFrameElement = albumViewTranslationKeyFrame as SplineDoubleKeyFrame;

				object albumViewOutTranslationStoryboard = GetTemplateChild(ElementAlbumViewOutTranslationStoryboardName);
				Debug.Assert(typeof(Storyboard).IsInstanceOfType(albumViewOutTranslationStoryboard) || (albumViewOutTranslationStoryboard == null),
					"The template part AlbumViewOutTranslationStoryboardElement is not an instance of Storyboard!");
				
				AlbumViewOutTranslationStoryboardElement = albumViewOutTranslationStoryboard as Storyboard;

				object albumViewOutTranslationKeyFrame = GetTemplateChild(ElementAlbumViewOutTranslationKeyFrameName);
				Debug.Assert(typeof(SplineDoubleKeyFrame).IsInstanceOfType(albumViewOutTranslationKeyFrame) || (albumViewOutTranslationKeyFrame == null),
					"The template part AlbumViewOutTranslationKeyFrameElement is not an instance of SplineDoubleKeyFrame!");
				
				AlbumViewOutTranslationKeyFrameElement = albumViewOutTranslationKeyFrame as SplineDoubleKeyFrame;

				object background = GetTemplateChild(ElementBackgroundName);
				Debug.Assert(typeof(Rectangle).IsInstanceOfType(background) || (background == null),
					"The template part BackgroundElement is not an instance of Rectangle!");
				BackgroundElement = background as Rectangle;

				object nav = GetTemplateChild(ElementSlideNavigationName);
				Debug.Assert(typeof(SlideNavigation).IsInstanceOfType(nav) || (nav == null),
					"The template part SlideNavigationElement is not an instance of SlideNavigation!");
				
				SlideNavigationElement = nav as SlideNavigation;

				object thumbnailViewerRoot = GetTemplateChild(ElementThumbnailViewerRootName);
				Debug.Assert(typeof(Canvas).IsInstanceOfType(thumbnailViewerRoot) || (thumbnailViewerRoot == null),
					"The template part ThumbnailViewerRootElement is not an instance of ThumbnailViewerRoot!");
				
				ThumbnailViewerRootElement = thumbnailViewerRoot as Canvas;

				object albumViewer = GetTemplateChild(ElementAlbumViewerName);
				Debug.Assert(typeof(AlbumViewer).IsInstanceOfType(albumViewer) || (albumViewer == null),
					"The template part AlbumViewerElement is not an instance of AlbumViewer!");
				AlbumViewerElement = albumViewer as AlbumViewer;

				object albumButton = GetTemplateChild(ElementAlbumButtonName);
				Debug.Assert(typeof(PathButton).IsInstanceOfType(albumButton) || (albumButton == null),
					"The template part AlbumButtonElement is not an instance of PathButton!");
				
				AlbumButtonElement = albumButton as PathButton;

				object saveHyperlinkButton = GetTemplateChild(ElementSaveHyperlinkButtonName);
				Debug.Assert(typeof(HyperlinkButton).IsInstanceOfType(saveHyperlinkButton) || (saveHyperlinkButton == null),
					"The template part SaveHyperlinkButtonElement is not an instance of HyperlinkButton!");

				SaveHyperlinkButtonElement = saveHyperlinkButton as HyperlinkButton;

                object saveButton = GetTemplateChild(ElementSaveButtonName);
                Debug.Assert(typeof(PathButton).IsInstanceOfType(saveButton) || (saveButton == null),
                    "The template part SaveButtonElement is not an instance of PathButton!");

                SaveButtonElement = saveButton as PathButton;

				object goToFullScreenButton = GetTemplateChild(ElementGoToFullScreenButtonName);
				Debug.Assert(typeof(PathButton).IsInstanceOfType(goToFullScreenButton) || (goToFullScreenButton == null),
					"The template part GoToFullScreenButtonElement is not an instance of PathButton!");
				
				GoToFullScreenButtonElement = goToFullScreenButton as PathButton;

				object escapeFullScreenButton = GetTemplateChild(ElementEscapeFullScreenButtonName);
				Debug.Assert(typeof(PathButton).IsInstanceOfType(escapeFullScreenButton) || (escapeFullScreenButton == null),
					"The template part EscapeFullScreenButtonElement is not an instance of PathButton!");
				EscapeFullScreenButtonElement = escapeFullScreenButton as PathButton;

				object embedButton = GetTemplateChild(ElementEmbedButtonName);
				Debug.Assert(typeof(PathButton).IsInstanceOfType(embedButton) || (embedButton == null),
					"The template part EmbedButtonElement is not an instance of PathButton!");
				
				EmbedButtonElement = embedButton as PathButton;

				if (!Configuration.Options.NavigationTray.Enabled)
				{
					Visibility = Visibility.Collapsed;
				}

				Content_Resized(this, EventArgs.Empty);
				Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
			}

			if (FinishedLoading != null)
			{
				FinishedLoading(this, EventArgs.Empty);
			}
		}

        /// <summary>
        /// Handles the Resized event of the Content control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			double appHeight = Application.Current.Host.Content.ActualHeight;
			double appWidth = Application.Current.Host.Content.ActualWidth;

			if (RootElement != null)
			{
				Width = appWidth;
				Height = appHeight;

				if (!IsAlbumView)
				{
					Canvas.SetTop(RootElement, appHeight - TrayHeight);
				}
			}

			if (Application.Current.Host.Content.IsFullScreen)
			{
				GoToFullScreenButtonVisibility = Visibility.Collapsed;
				EscapeFullScreenButtonVisibility = 
					Configuration.Options.ToggleFullScreenButton.Enabled  ?
					Visibility.Visible : Visibility.Collapsed;
			}
			else
			{
				GoToFullScreenButtonVisibility =
					Configuration.Options.ToggleFullScreenButton.Enabled  ?
					Visibility.Visible : Visibility.Collapsed;
				EscapeFullScreenButtonVisibility = Visibility.Collapsed;
			}
		}

        /// <summary>
        /// Shows this instance.
        /// </summary>
		public void Show()
		{
			if (!IsAlbumView)
			{
				VisualStateManager.GoToState(this, "Show", true);
			}
		}

        /// <summary>
        /// Hides this instance.
        /// </summary>
		public void Hide()
		{
			if (!IsAlbumView)
			{
				VisualStateManager.GoToState(this, "Hide", true);
			}
		}

        /// <summary>
        /// Toggles the album view.
        /// </summary>
		public void ToggleAlbumView()
		{
			if (!Configuration.Options.AlbumViewer.Enabled && !IsAlbumView)
			{
				return;
			}

			double appHeight = Application.Current.Host.Content.ActualHeight;

			if (IsAlbumView)
			{
				VisualStateManager.GoToState(this, "Show", true);
				AlbumViewOutTranslationKeyFrameElement.Value = appHeight - TrayHeight;				
				AlbumViewOutTranslationStoryboardElement.Begin();
			}
			else
			{
				VisualStateManager.GoToState(this, "Show", true);
				AlbumViewTranslationKeyFrameElement.Value = appHeight - TrayHeight;
				AlbumViewTranslationStoryboardElement.Begin();
			}

			IsAlbumView = !IsAlbumView;
		}
	}
}