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
    /// Represents the SlideDescription class
    /// </summary>
    [TemplatePart(Name = SlideDescription.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = SlideDescription.ElementBackgroundName, Type = typeof(Rectangle))]
    [TemplatePart(Name = SlideDescription.ElementCloseButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = SlideDescription.ElementTitleName, Type = typeof(TextBlock))]
	[TemplatePart(Name = SlideDescription.ElementTitleBackgroundName, Type = typeof(Rectangle))]
	[TemplatePart(Name = SlideDescription.ElementDescriptionName, Type = typeof(TextBlock))]
	[TemplatePart(Name = SlideDescription.ElementDescriptionBackgroundName, Type = typeof(Rectangle))]
	[TemplateVisualState(Name = "Show", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Hide", GroupName = "CommonStates")]
    public partial class SlideDescription : Control
    {
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="SlideDescription"/> class.
		/// </summary>
		public SlideDescription()
		{
			IsTabStop = false;
			DefaultStyleKey = typeof(SlideDescription);
			Options.SlideDescriptionOptions options = Configuration.Options.SlideDescription;

			#region [ Title ]
			TitleHeight = options.TitleHeight;
			TitleForegroundBrush = options.TitleForeground;
			TitleBackgroundBrush = options.TitleBackground;
			TitleBackgroundOpacity = options.TitleBackgroundOpacity;
			TitleFontFamily = options.TitleFontFamily;
			TitleFontSize = options.TitleFontSize;
			TitleMargin = options.TitleMargin;
			#endregion

			#region [ Description ]
			DescriptionHeight = options.DescriptionHeight;
			DescriptionBackgroundOpacity = options.DescriptionBackgroundOpacity;
			DescriptionForegroundBrush = options.DescriptionForeground;
			DescriptionBackgroundBrush = options.DescriptionBackground;
			DescriptionFontFamily = options.DescriptionFontFamily;
			DescriptionFontSize = options.DescriptionFontSize;
			DescriptionMargin = options.DescriptionMargin;
			#endregion

			#region [ Background ]
			Background = options.Background;
			BackgroundRadiusX = options.BackgroundRadiusX;
			BackgroundRadiusY = options.BackgroundRadiusY;
			BackgroundOpacity = options.BackgroundOpacity;
			Height = options.TitleHeight + options.DescriptionHeight;
			BackgroundMargin = options.BackgroundMargin;
			#endregion

			#region [ CloseButton ]
			CloseButtonBackground1Brush = options.CloseButtonBackground1Brush;
			CloseButtonBackground2Brush = options.CloseButtonBackground2Brush;
			CloseButtonForegroundBrush = options.CloseButtonForegroundBrush;
			CloseButtonForegroundHoverBrush = options.CloseButtonForegroundHoverBrush;
			CloseButtonRadiusX = options.CloseButtonRadiusX;
			CloseButtonRadiusY = options.CloseButtonRadiusY;
			CloseButtonMargin = options.CloseButtonMargin;
			CloseButtonWidth = options.CloseButtonWidth;
			CloseButtonHeight = options.CloseButtonHeight;
			CloseButtonPathWidth = options.CloseButtonPathWidth;
			CloseButtonPathHeight = options.CloseButtonPathHeight;
			CloseButtonPathData = options.CloseButtonPathData;
			#endregion

			if (!options.Enabled)
			{
				Visibility = Visibility.Collapsed;
			}
		}
		#endregion

        #region [ Template Parts ]
        internal FrameworkElement _elementRoot;
        internal const string ElementRootName = "RootElement";

		internal TextBlock _elementTitle;
		internal const string ElementTitleName = "TitleElement";

		internal TextBlock _elementDescription;
		internal const string ElementDescriptionName = "DescriptionElement";

        internal PathButton _elementCloseButton;
        internal const string ElementCloseButtonName = "CloseButtonElement";

		internal Rectangle _elementBackground;
		internal const string ElementBackgroundName = "BackgroundElement";

		internal Rectangle _elementTitleBackground;
		internal const string ElementTitleBackgroundName = "TitleBackgroundElement";

		internal Rectangle _elementDescriptionBackground;
		internal const string ElementDescriptionBackgroundName = "DescriptionBackgroundElement";
        #endregion

		#region [ Private Properties ]
        /// <summary>
        /// Gets or sets the root element.
        /// </summary>
        /// <value>The root element.</value>
		private FrameworkElement RootElement
		{
			get { return _elementRoot; }
			set { _elementRoot = value; }
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

        /// <summary>
        /// Gets or sets the title background element.
        /// </summary>
        /// <value>The title background element.</value>
		private Rectangle TitleBackgroundElement
		{
			get { return _elementTitleBackground; }
			set { _elementTitleBackground = value; }
		}

        /// <summary>
        /// Gets or sets the description background element.
        /// </summary>
        /// <value>The description background element.</value>
		private Rectangle DescriptionBackgroundElement
		{
			get { return _elementDescriptionBackground; }
			set { _elementDescriptionBackground = value; }
		}

        private PathButton CloseButtonElement
        {
            get { return _elementCloseButton; }
            set { _elementCloseButton = value; }
        }

        /// <summary>
        /// Gets or sets the title element.
        /// </summary>
        /// <value>The title element.</value>
		private TextBlock TitleElement
		{
			get { return _elementTitle; }
			set
			{
				_elementTitle = value;

                // if specified in XAML, the property gets set before ApplyConfiguration
                // so apply the property
				if (_elementTitle != null)
				{
					_elementTitle.Text = Title;
                    TrimText(Title, TitleElement, TitleHeight);
				}
			}
		}

        /// <summary>
        /// Gets or sets the description element.
        /// </summary>
        /// <value>The description element.</value>
		private TextBlock DescriptionElement
		{
			get { return _elementDescription; }
			set
			{
				_elementDescription = value;

                // if specified in XAML, the property gets set before ApplyConfiguration
                // so apply the property
				if (_elementDescription != null)
				{
					_elementDescription.Text = Description;
                    TrimText(Description, DescriptionElement, DescriptionHeight);
				}
			}
		}
		#endregion

		#region [ Dependency Properties ]

			#region [ Title ] 

				#region [ TitleHeight ]
				public double TitleHeight
				{
					get { return (double)GetValue(TitleHeightProperty); }
					set { SetValue(TitleHeightProperty, value); }
				}

				public static readonly DependencyProperty TitleHeightProperty =
							DependencyProperty.Register(
								"TitleHeight",
								typeof(double),
								typeof(SlideDescription),
								null);

				#endregion

				#region [ Title ]
				/// <summary>
				/// Gets or sets the title.
				/// </summary>
				/// <value>The title.</value>
				public string Title
				{
					get { return (string)GetValue(TitleProperty); }
					set
					{
						SetValue(TitleProperty, value);

						if (TitleElement != null)
						{
							TitleElement.Text = Title;
							TrimText(Title, TitleElement, TitleHeight);
						}
					}
				}

				/// <summary>
				/// Identifies the Title dependency property.
				/// </summary>
				public static readonly DependencyProperty TitleProperty =
					DependencyProperty.Register(
						"Title",
						typeof(string),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ TitleBackgroundBrush ]
				public Brush TitleBackgroundBrush
				{
					get { return GetValue(TitleBackgroundBrushProperty) as Brush; }
					set { SetValue(TitleBackgroundBrushProperty, value); }
				}

				public static readonly DependencyProperty TitleBackgroundBrushProperty =
					DependencyProperty.Register(
						"TitleBackgroundBrush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ TitleForegroundBrush ]

				public Brush TitleForegroundBrush
				{
					get { return GetValue(TitleForegroundBrushProperty) as Brush; }
					set { SetValue(TitleForegroundBrushProperty, value); }
				}

				public static readonly DependencyProperty TitleForegroundBrushProperty =
					DependencyProperty.Register(
						"TitleForegroundBrush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ TitleBackgroundOpacity ]
				public double TitleBackgroundOpacity
				{
					get { return (double)GetValue(TitleBackgroundOpacityProperty); }
					set { SetValue(TitleBackgroundOpacityProperty, value); }
				}

				public static readonly DependencyProperty TitleBackgroundOpacityProperty =
					DependencyProperty.Register(
						"TitleBackgroundOpacity",
						typeof(double),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ TitleMargin ]

				public Thickness TitleMargin
				{
					get { return (Thickness)GetValue(TitleMarginProperty); }
					set { SetValue(TitleMarginProperty, value); }
				}

				public static readonly DependencyProperty TitleMarginProperty =
					DependencyProperty.Register(
						"TitleMargin",
						typeof(Thickness),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ TitleFontFamily ]

				public FontFamily TitleFontFamily
				{
					get { return (FontFamily)GetValue(TitleFontFamilyProperty); }
					set { SetValue(TitleFontFamilyProperty, value); }
				}

				public static readonly DependencyProperty TitleFontFamilyProperty =
					DependencyProperty.Register(
						"TitleFontFamily",
						typeof(FontFamily),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ TitleFontSize ]
				/// <summary>
				/// Gets or sets the Y radius of the background rectangle.
				/// </summary>
				/// <value>the Y radius of the background rectangle.</value>
				public double TitleFontSize
				{
					get { return (double)GetValue(TitleFontSizeProperty); }
					set { SetValue(TitleFontSizeProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty TitleFontSizeProperty =
					DependencyProperty.Register(
						"TitleFontSize",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

			#endregion

			#region [ Description ]

				#region [ DescriptionHeight ]
				public double DescriptionHeight
				{
					get { return (double)GetValue(DescriptionHeightProperty); }
					set { SetValue(DescriptionHeightProperty, value); }
				}

				public static readonly DependencyProperty DescriptionHeightProperty =
							DependencyProperty.Register(
								"DescriptionHeight",
								typeof(double),
								typeof(SlideDescription),
								null);

				#endregion

				#region [ Description ]
					/// <summary>
				/// Gets or sets the description.
				/// </summary>
				/// <value>The description.</value>
				public string Description
				{
					get { return (string)GetValue(DescriptionProperty); }
					set
					{
						SetValue(DescriptionProperty, value);

						if (DescriptionElement != null)
						{
							DescriptionElement.Text = Description;
							TrimText(Description, DescriptionElement, DescriptionHeight);
						}
					}
				}

				/// <summary>
				/// Identifies the Description dependency property.
				/// </summary>
				public static readonly DependencyProperty DescriptionProperty =
					DependencyProperty.Register(
						"Description",
						typeof(string),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ DescriptionBackgroundBrush ]
				public Brush DescriptionBackgroundBrush
				{
					get { return GetValue(DescriptionBackgroundBrushProperty) as Brush; }
					set { SetValue(DescriptionBackgroundBrushProperty, value); }
				}

				public static readonly DependencyProperty DescriptionBackgroundBrushProperty =
					DependencyProperty.Register(
						"DescriptionBackgroundBrush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ DescriptionForegroundBrush ]

				public Brush DescriptionForegroundBrush
				{
					get { return GetValue(DescriptionForegroundBrushProperty) as Brush; }
					set { SetValue(DescriptionForegroundBrushProperty, value); }
				}

				public static readonly DependencyProperty DescriptionForegroundBrushProperty =
					DependencyProperty.Register(
						"DescriptionForegroundBrush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ DescriptionBackgroundOpacity ]
				public double DescriptionBackgroundOpacity
				{
					get { return (double)GetValue(DescriptionBackgroundOpacityProperty); }
					set { SetValue(DescriptionBackgroundOpacityProperty, value); }
				}

				public static readonly DependencyProperty DescriptionBackgroundOpacityProperty =
					DependencyProperty.Register(
						"DescriptionBackgroundOpacity",
						typeof(double),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ DescriptionMargin ]

				public Thickness DescriptionMargin
				{
					get { return (Thickness)GetValue(DescriptionMarginProperty); }
					set { SetValue(DescriptionMarginProperty, value); }
				}

				public static readonly DependencyProperty DescriptionMarginProperty =
					DependencyProperty.Register(
						"DescriptionMargin",
						typeof(Thickness),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ DescriptionFontFamily ]

				public FontFamily DescriptionFontFamily
				{
					get { return (FontFamily)GetValue(DescriptionFontFamilyProperty); }
					set { SetValue(DescriptionFontFamilyProperty, value); }
				}

				public static readonly DependencyProperty DescriptionFontFamilyProperty =
					DependencyProperty.Register(
						"DescriptionFontFamily",
						typeof(FontFamily),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ DescriptionFontSize ]
			/// <summary>
			/// Gets or sets the Y radius of the background rectangle.
			/// </summary>
			/// <value>the Y radius of the background rectangle.</value>
			public double DescriptionFontSize
			{
				get { return (double)GetValue(DescriptionFontSizeProperty); }
				set { SetValue(DescriptionFontSizeProperty, value); }
			}

			/// <summary>
			/// Identifies the PathHeight dependency property.
			/// </summary>
			public static readonly DependencyProperty DescriptionFontSizeProperty =
				DependencyProperty.Register(
					"DescriptionFontSize",
					typeof(double),
					typeof(SlideDescription),
					null);
			#endregion
			
			#endregion

			#region [ Background ] 

				#region [ BackgroundOpacity ]
				public double BackgroundOpacity
				{
					get { return (double)GetValue(BackgroundOpacityProperty); }
					set { SetValue(BackgroundOpacityProperty, value); }
				}

				public static readonly DependencyProperty BackgroundOpacityProperty =
					DependencyProperty.Register(
						"BackgroundOpacity",
						typeof(double),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ BackgroundMargin ]

				public Thickness BackgroundMargin
				{
					get { return (Thickness)GetValue(BackgroundMarginProperty); }
					set { SetValue(BackgroundMarginProperty, value); }
				}

				public static readonly DependencyProperty BackgroundMarginProperty =
					DependencyProperty.Register(
						"BackgroundMargin",
						typeof(Thickness),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ BackgroundRadiusX ]
				/// <summary>
				/// Gets or sets the X radius of the background rectangle.
				/// </summary>
				/// <value>the X radius of the background rectangle.</value>
				public double BackgroundRadiusX
				{
					get { return (double)GetValue(BackgroundRadiusXProperty); }
					set { SetValue(BackgroundRadiusXProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty BackgroundRadiusXProperty =
					DependencyProperty.Register(
						"BackgroundRadiusX",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ BackgroundRadiusY ]
				/// <summary>
				/// Gets or sets the Y radius of the background rectangle.
				/// </summary>
				/// <value>the Y radius of the background rectangle.</value>
				public double BackgroundRadiusY
				{
					get { return (double)GetValue(BackgroundRadiusYProperty); }
					set { SetValue(BackgroundRadiusYProperty, value); }
				}

				/// <summary>
				/// Identifies the PathHeight dependency property.
				/// </summary>
				public static readonly DependencyProperty BackgroundRadiusYProperty =
					DependencyProperty.Register(
						"BackgroundRadiusY",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

			#endregion

			#region [ Close Button ]

			#region [ CloseButtonBackground1Brush ]
			public Brush CloseButtonBackground1Brush
				{
					get { return GetValue(CloseButtonBackground1BrushProperty) as Brush; }
					set { SetValue(CloseButtonBackground1BrushProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonBackground1BrushProperty =
					DependencyProperty.Register(
						"CloseButtonBackground1Brush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ CloseButtonBackground2Brush ]
				public Brush CloseButtonBackground2Brush
				{
					get { return GetValue(CloseButtonBackground2BrushProperty) as Brush; }
					set { SetValue(CloseButtonBackground2BrushProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonBackground2BrushProperty =
					DependencyProperty.Register(
						"CloseButtonBackground2Brush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ CloseButtonForegroundBrush ]
				public Brush CloseButtonForegroundBrush
				{
					get { return GetValue(CloseButtonForegroundBrushProperty) as Brush; }
					set { SetValue(CloseButtonForegroundBrushProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonForegroundBrushProperty =
					DependencyProperty.Register(
						"CloseButtonForegroundBrush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ CloseButtonForegroundHoverBrush ]
				public Brush CloseButtonForegroundHoverBrush
				{
					get { return GetValue(CloseButtonForegroundHoverBrushProperty) as Brush; }
					set { SetValue(CloseButtonForegroundHoverBrushProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonForegroundHoverBrushProperty =
					DependencyProperty.Register(
						"CloseButtonForegroundHoverBrush",
						typeof(Brush),
						typeof(SlideDescription),
						null);

				#endregion

				#region [ CloseButtonRadiusX ]
				public double CloseButtonRadiusX
				{
					get { return (double)GetValue(CloseButtonRadiusXProperty); }
					set { SetValue(CloseButtonRadiusXProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonRadiusXProperty =
					DependencyProperty.Register(
						"CloseButtonRadiusX",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonRadiusY ]
				public double CloseButtonRadiusY
				{
					get { return (double)GetValue(CloseButtonRadiusYProperty); }
					set { SetValue(CloseButtonRadiusYProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonRadiusYProperty =
					DependencyProperty.Register(
						"CloseButtonRadiusY",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonMargin ]

				public Thickness CloseButtonMargin
				{
					get { return (Thickness)GetValue(CloseButtonMarginProperty); }
					set { SetValue(CloseButtonMarginProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonMarginProperty =
					DependencyProperty.Register(
						"CloseButtonMargin",
						typeof(Thickness),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonWidth ]

				public double CloseButtonWidth
				{
					get { return (double)GetValue(CloseButtonWidthProperty); }
					set { SetValue(CloseButtonWidthProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonWidthProperty =
					DependencyProperty.Register(
						"CloseButtonWidth",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonHeight ]

				public double CloseButtonHeight
				{
					get { return (double)GetValue(CloseButtonHeightProperty); }
					set { SetValue(CloseButtonHeightProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonHeightProperty =
					DependencyProperty.Register(
						"CloseButtonHeight",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonPathWidth ]

				public double CloseButtonPathWidth
				{
					get { return (double)GetValue(CloseButtonPathWidthProperty); }
					set { SetValue(CloseButtonPathWidthProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonPathWidthProperty =
					DependencyProperty.Register(
						"CloseButtonPathWidth",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonPathHeight ]

				public double CloseButtonPathHeight
				{
					get { return (double)GetValue(CloseButtonPathHeightProperty); }
					set { SetValue(CloseButtonPathHeightProperty, value); }
				}

				public static readonly DependencyProperty CloseButtonPathHeightProperty =
					DependencyProperty.Register(
						"CloseButtonPathHeight",
						typeof(double),
						typeof(SlideDescription),
						null);
				#endregion

				#region [ CloseButtonPathData ]
			public string CloseButtonPathData
			{
				get { return (string)GetValue(CloseButtonPathDataProperty); }
				set { SetValue(CloseButtonPathDataProperty, value); }
			}

			public static readonly DependencyProperty CloseButtonPathDataProperty =
				DependencyProperty.Register(
					"CloseButtonPathData",
					typeof(string),
					typeof(SlideDescription),
					null);
			#endregion

			#endregion

		#endregion

		/// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Get the elements
			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null),
				"The template part RootElement is not an instance of FrameworkElement!");
			
			RootElement = root as FrameworkElement;

			object background = GetTemplateChild(ElementBackgroundName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(background) || (background == null),
				"The template part BackgroundElement is not an instance of Rectangle!");
			
			BackgroundElement = background as Rectangle;

			object title = GetTemplateChild(ElementTitleName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(title) || (title == null),
				"The template part TitleElement is not an instance of TextBlock!");
			
			TitleElement = title as TextBlock;

			object titleBackground = GetTemplateChild(ElementTitleBackgroundName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(titleBackground) || (titleBackground == null),
				"The template part TitleBackgroundElement is not an instance of Rectangle!");
			
			TitleBackgroundElement = titleBackground as Rectangle;

			object desc = GetTemplateChild(ElementDescriptionName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(desc) || (desc == null),
				"The template part DescriptionElement is not an instance of TextBlock!");
			
			DescriptionElement = desc as TextBlock;

			object descBackground = GetTemplateChild(ElementDescriptionBackgroundName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(descBackground) || (descBackground == null),
				"The template part DescriptionBackgroundElement is not an instance of Rectangle!");
			
			DescriptionBackgroundElement = descBackground as Rectangle;

            object closeButton = GetTemplateChild(ElementCloseButtonName);
            Debug.Assert(typeof(PathButton).IsInstanceOfType(closeButton) || (closeButton == null),
                "The template part CloseButtonElement is not an instance of PathButton!");
            
			CloseButtonElement = closeButton as PathButton;

			if (CloseButtonElement != null)
			{
				CloseButtonElement.Click += new RoutedEventHandler(CloseButtonElement_Click);
			}

			if (RootElement != null)
			{
				Content_Resized(this, EventArgs.Empty);
				Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
			}

			object showVisualState = GetTemplateChild("Show");
			Debug.Assert(typeof(VisualState).IsInstanceOfType(showVisualState) || (showVisualState == null),
				"The template part Show is not an instance of VisualState!");

			if (showVisualState != null)
			{
				(((DoubleAnimationUsingKeyFrames)((VisualState)showVisualState).Storyboard.Children[0])).KeyFrames[0].Value = Height * -1;
			}

			object hideVisualState = GetTemplateChild("Hide");
			Debug.Assert(typeof(VisualState).IsInstanceOfType(hideVisualState) || (hideVisualState == null),
				"The template part Hide is not an instance of VisualState!");

			if (hideVisualState != null)
			{
				(((DoubleAnimationUsingKeyFrames)((VisualState)hideVisualState).Storyboard.Children[0])).KeyFrames[1].Value = Height * -1;
			}
		}

        /// <summary>
        /// Handles the Resized event of the Content control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			if (RootElement != null)
			{
				Width = Application.Current.Host.Content.ActualWidth;
                TrimText(Title, TitleElement, TitleHeight);
                TrimText(Description, DescriptionElement, DescriptionHeight);
			}
		}

        /// <summary>
        /// Trims the overflowing text of a TextBlock and adds ellipsis if needed
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="element">The element.</param>
        private void TrimText(string text, TextBlock element, double elementHeight)
        {
			if (string.IsNullOrEmpty(text) || element == null || RootElement == null)
			{
				return;
			}

            double width = RootElement.Width -
                (element.Margin.Left + element.Margin.Right) -  // subtract margins
                (element.Padding.Left + element.Padding.Right); // subtract padding

			//account for multiple line descriptions
			//each line in the descriptions should be 30 high
			if (elementHeight > 30)
				element.TextWrapping = TextWrapping.Wrap;

            // create a copy TextBlock
            TextBlock copyBlock = new TextBlock
            {
                Visibility = Visibility.Collapsed,
                FontFamily = element.FontFamily,
                FontSize = element.FontSize,
                FontStyle = element.FontStyle,
                FontWeight = element.FontWeight,
                Text = text
            };

            // don't mess with trimming if it's short enough to fit
            if (copyBlock.ActualWidth <= width)
            {
                element.Text = text;    // reset the text in case of sizing outward
                return;
            }

			string copyText = text;
			int numRows = ((int)elementHeight / 30);

			for (int i = 1; i <= numRows; i++)
			{
				do
				{
					// if contains a space, try to break after words
					if (copyText.LastIndexOf(' ') >= 0)
					{
						copyText = copyText.Substring(0, copyText.LastIndexOf(' '));
					}
					else
					{
						copyText = copyText.Substring(0, copyText.Length - 1);
					}

					copyBlock.Text = copyText + ( i<numRows ? "" : "...");
				}
				while (copyBlock.ActualWidth > width);

				if (i < numRows)
					copyText = text.Substring(copyText.Length);
			}

			element.Text = text.Substring(0, text.IndexOf(copyText) + copyText.Length) + "...";
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
		public void Show()
		{
			VisualStateManager.GoToState(this, "Show", true);
		}

        /// <summary>
        /// Hides this instance.
        /// </summary>
		public void Hide()
		{
			VisualStateManager.GoToState(this, "Hide", true);
		}

        private void CloseButtonElement_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}