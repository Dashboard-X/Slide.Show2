using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the SlideNavigation which shows the standard previous/next/play/pause
    /// </summary>
	[TemplatePart(Name = SlideNavigation.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = SlideNavigation.ElementPreviousName, Type = typeof(PathButton))]
	[TemplatePart(Name = SlideNavigation.ElementPlayName, Type = typeof(PathButton))]
	[TemplatePart(Name = SlideNavigation.ElementPauseName, Type = typeof(PathButton))]
	[TemplatePart(Name = SlideNavigation.ElementNextName, Type = typeof(PathButton))]
	public class SlideNavigation : Control
	{
		#region [ constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="SlideNavigation"/> class.
		/// </summary>
		public SlideNavigation()
		{
			this.DefaultStyleKey = typeof(SlideNavigation);
			Options.SlideNavigationOptions options = Configuration.Options.SlideNavigation;

			Height = options.Height;
			Width = options.Width;

			PlayPauseButtonWidth = options.PlayPauseButtonWidth;
			PlayPauseButtonHeight = options.PlayPauseButtonHeight;
			PreviousButtonWidth = options.PreviousButtonWidth;
			PreviousButtonHeight = options.PreviousButtonHeight;
			NextButtonWidth = options.NextButtonWidth;
			NextButtonHeight = options.NextButtonHeight;

			PlayPauseButtonPathWidth = options.PlayPauseButtonPathWidth;
			PlayPauseButtonPathHeight = options.PlayPauseButtonPathHeight;
			PreviousButtonPathWidth = options.PreviousButtonPathWidth;
			PreviousButtonPathHeight = options.PreviousButtonPathHeight;
			NextButtonPathWidth = options.NextButtonPathWidth;
			NextButtonPathHeight = options.NextButtonPathHeight;

			PlayPathData = options.PlayPathData;
			PausePathData = options.PausePathData;
			PreviousPathData = options.PreviousPathData;
			NextPathData = options.NextPathData;
			ForegroundBrush = options.Foreground;
			ForegroundHoverBrush = options.ForegroundHover;
			Background1 = options.Background1Brush;
			Background2 = options.Background2Brush;
			RadiusX = options.RadiusX;
			RadiusY = options.RadiusY;

			if (!options.Enabled)
			{
				Visibility = Visibility.Collapsed;
			}
		}
		#endregion

		#region [ Template Parts ]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal PathButton _elementPrevious;
		internal const string ElementPreviousName = "PreviousElement";

		internal PathButton _elementPlay;
		internal const string ElementPlayName = "PlayElement";

		internal PathButton _elementPause;
		internal const string ElementPauseName = "PauseElement";

		internal PathButton _elementNext;
		internal const string ElementNextName = "NextElement";

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
        /// Gets or sets the previous element.
        /// </summary>
        /// <value>The previous element.</value>
		private PathButton PreviousElement
		{
			get { return _elementPrevious; }
			set { _elementPrevious = value; }
		}

		/// <summary>
		/// Gets or sets the play element.
		/// </summary>
		/// <value>The play element.</value>
		private PathButton PlayElement
		{
			get { return _elementPlay; }
			set { _elementPlay = value; }
		}

		/// <summary>
		/// Gets or sets the pause element.
		/// </summary>
		/// <value>The pause element.</value>
		private PathButton PauseElement
		{
			get { return _elementPause; }
			set { _elementPause = value; }
		}

        /// <summary>
        /// Gets or sets the next element.
        /// </summary>
        /// <value>The next element.</value>
		private PathButton NextElement
		{
			get { return _elementNext; }
			set { _elementNext = value; }
		}

		#endregion

		#region [ Events ]

		/// <summary>
        /// Occurs when [previous click].
        /// </summary>
		public event RoutedEventHandler PreviousClick;

        /// <summary>
        /// Occurs when [play click].
        /// </summary>
		public event RoutedEventHandler PlayClick;

        /// <summary>
        /// Occurs when [pause click].
        /// </summary>
		public event RoutedEventHandler PauseClick;

        /// <summary>
        /// Occurs when [next click].
        /// </summary>
		public event RoutedEventHandler NextClick;

		#endregion

		#region [ PlayPathData ]

		public string PlayPathData
		{
			get { return (string)GetValue(PlayPathDataProperty); }
			set { SetValue(PlayPathDataProperty, value); }
		}

		public static readonly DependencyProperty PlayPathDataProperty =
			DependencyProperty.Register(
				"PlayPathData",
				typeof(string),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ PausePathData ]

		public string PausePathData
		{
			get { return (string)GetValue(PausePathDataProperty); }
			set { SetValue(PausePathDataProperty, value); }
		}

		public static readonly DependencyProperty PausePathDataProperty =
			DependencyProperty.Register(
				"PausePathData",
				typeof(string),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ PreviousPathData ]

		public string PreviousPathData
		{
			get { return (string)GetValue(PreviousPathDataProperty); }
			set { SetValue(PreviousPathDataProperty, value); }
		}

		public static readonly DependencyProperty PreviousPathDataProperty =
			DependencyProperty.Register(
				"PreviousPathData",
				typeof(string),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ NextPathData ]

		public string NextPathData
		{
			get { return (string)GetValue(NextPathDataProperty); }
			set { SetValue(NextPathDataProperty, value); }
		}

		public static readonly DependencyProperty NextPathDataProperty =
			DependencyProperty.Register(
				"NextPathData",
				typeof(string),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ RadiusX ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double RadiusX
		{
			get { return (double)GetValue(RadiusXProperty); }
			set { SetValue(RadiusXProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty RadiusXProperty =
			DependencyProperty.Register(
				"RadiusX",
				typeof(double),
				typeof(SlideNavigation),
				null);
		
		#endregion

		#region [ RadiusY ]
		/// <summary>
		/// Gets or sets the Y radius of the background rectangle.
		/// </summary>
		/// <value>the Y radius of the background rectangle.</value>
		public double RadiusY
		{
			get { return (double)GetValue(RadiusYProperty); }
			set { SetValue(RadiusYProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty RadiusYProperty =
			DependencyProperty.Register(
				"RadiusY",
				typeof(double),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ ForegroundBrush ]

		public Brush ForegroundBrush
		{
			get { return GetValue(ForegroundBrushProperty) as Brush; }
			set { SetValue(ForegroundBrushProperty, value); }
		}

		public static readonly DependencyProperty ForegroundBrushProperty =
			DependencyProperty.Register(
				"ForegroundBrush",
				typeof(Brush),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ ForegroundHoverBrush ]

		public Brush ForegroundHoverBrush
		{
			get { return GetValue(ForegroundHoverBrushProperty) as Brush; }
			set { SetValue(ForegroundHoverBrushProperty, value); }
		}

		public static readonly DependencyProperty ForegroundHoverBrushProperty =
			DependencyProperty.Register(
				"ForegroundHoverBrush",
				typeof(Brush),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ Background1 ]
		public Brush Background1
		{
			get { return GetValue(Background1Property) as Brush; }
			set { SetValue(Background1Property, value); }
		}

		public static readonly DependencyProperty Background1Property =
			DependencyProperty.Register(
				"Background1",
				typeof(Brush),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ Background2 ]
		public Brush Background2
		{
			get { return GetValue(Background2Property) as Brush; }
			set { SetValue(Background2Property, value); }
		}

		public static readonly DependencyProperty Background2Property =
			DependencyProperty.Register(
				"Background2",
				typeof(Brush),
				typeof(SlideNavigation),
				null);

		#endregion

		#region [ PlayPauseButtonWidth ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PlayPauseButtonWidth
		{
			get { return (double)GetValue(PlayPauseButtonWidthProperty); }
			set { SetValue(PlayPauseButtonWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PathWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonWidthProperty =
			DependencyProperty.Register(
				"PlayPauseButtonWidth",
				typeof(double),
				typeof(SlideNavigation),
				null);
		
		#endregion

		#region [ PlayPauseButtonHeight ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PlayPauseButtonHeight
		{
			get { return (double)GetValue(PlayPauseButtonHeightProperty); }
			set { SetValue(PlayPauseButtonHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonHeightProperty =
			DependencyProperty.Register(
				"PlayPauseButtonHeight",
				typeof(double),
				typeof(SlideNavigation),
				null);
		
		#endregion

		#region [ PreviousButtonWidth ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PreviousButtonWidth
		{
			get { return (double)GetValue(PreviousButtonWidthProperty); }
			set { SetValue(PreviousButtonWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PathWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty PreviousButtonWidthProperty =
			DependencyProperty.Register(
				"PreviousButtonWidth",
				typeof(double),
				typeof(SlideNavigation),
				null);
		
		#endregion

		#region [ PreviousButtonHeight ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PreviousButtonHeight
		{
			get { return (double)GetValue(PreviousButtonHeightProperty); }
			set { SetValue(PreviousButtonHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PreviousButtonHeightProperty =
			DependencyProperty.Register(
				"PreviousButtonHeight",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ NextButtonWidth ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double NextButtonWidth
		{
			get { return (double)GetValue(NextButtonWidthProperty); }
			set { SetValue(NextButtonWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PathWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty NextButtonWidthProperty =
			DependencyProperty.Register(
				"NextButtonWidth",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ NextButtonHeight ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double NextButtonHeight
		{
			get { return (double)GetValue(NextButtonHeightProperty); }
			set { SetValue(NextButtonHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty NextButtonHeightProperty =
			DependencyProperty.Register(
				"NextButtonHeight",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ PlayPauseButtonPathWidth ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PlayPauseButtonPathWidth
		{
			get { return (double)GetValue(PlayPauseButtonPathWidthProperty); }
			set { SetValue(PlayPauseButtonPathWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PathPathWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonPathWidthProperty =
			DependencyProperty.Register(
				"PlayPauseButtonPathWidth",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ PlayPauseButtonPathHeight ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PlayPauseButtonPathHeight
		{
			get { return (double)GetValue(PlayPauseButtonPathHeightProperty); }
			set { SetValue(PlayPauseButtonPathHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathPathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonPathHeightProperty =
			DependencyProperty.Register(
				"PlayPauseButtonPathHeight",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ PreviousButtonPathWidth ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PreviousButtonPathWidth
		{
			get { return (double)GetValue(PreviousButtonPathWidthProperty); }
			set { SetValue(PreviousButtonPathWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PathPathWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty PreviousButtonPathWidthProperty =
			DependencyProperty.Register(
				"PreviousButtonPathWidth",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ PreviousButtonPathHeight ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double PreviousButtonPathHeight
		{
			get { return (double)GetValue(PreviousButtonPathHeightProperty); }
			set { SetValue(PreviousButtonPathHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathPathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PreviousButtonPathHeightProperty =
			DependencyProperty.Register(
				"PreviousButtonPathHeight",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ NextButtonPathWidth ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double NextButtonPathWidth
		{
			get { return (double)GetValue(NextButtonPathWidthProperty); }
			set { SetValue(NextButtonPathWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PathPathWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty NextButtonPathWidthProperty =
			DependencyProperty.Register(
				"NextButtonPathWidth",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ NextButtonPathHeight ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double NextButtonPathHeight
		{
			get { return (double)GetValue(NextButtonPathHeightProperty); }
			set { SetValue(NextButtonPathHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathPathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty NextButtonPathHeightProperty =
			DependencyProperty.Register(
				"NextButtonPathHeight",
				typeof(double),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ PlayButtonVisibility ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Visibility PlayButtonVisibility
		{
			get { return (Visibility)GetValue(PlayButtonVisibilityProperty); }
			set { SetValue(PlayButtonVisibilityProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayButtonVisibilityProperty =
			DependencyProperty.Register(
				"PlayButtonVisibility",
				typeof(Visibility),
				typeof(SlideNavigation),
				null);
		#endregion

		#region [ PauseButtonVisibility ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Visibility PauseButtonVisibility
		{
			get { return (Visibility)GetValue(PauseButtonVisibilityProperty); }
			set { SetValue(PauseButtonVisibilityProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PauseButtonVisibilityProperty =
			DependencyProperty.Register(
				"PauseButtonVisibility",
				typeof(Visibility),
				typeof(SlideNavigation),
				null);
		#endregion

		/// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null),
				"The template part RootElement is not an instance of FrameworkElement!");
			
			RootElement = root as FrameworkElement;

			object previous = GetTemplateChild(ElementPreviousName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(previous) || (previous == null),
				"The template part PreviousElement is not an instance of PathButton!");
			
			PreviousElement = previous as PathButton;

			object play = GetTemplateChild(ElementPlayName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(play) || (play == null),
				"The template part PlayElement is not an instance of PathButton!");
			
			PlayElement = play as PathButton;

			object pause = GetTemplateChild(ElementPauseName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(pause) || (pause == null),
				"The template part PauseElement is not an instance of PathButton!");
			
			PauseElement = pause as PathButton;

			object next = GetTemplateChild(ElementNextName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(next) || (next == null),
				"The template part NextElement is not an instance of PathButton!");
			
			NextElement = next as PathButton;

			if (PreviousElement != null)
			{
				PreviousElement.Click += new RoutedEventHandler(PreviousElement_Click);
			}

			if (PlayElement != null)
			{
				PlayElement.Click += new RoutedEventHandler(PlayElement_Click);
			}

			if (PauseElement != null)
			{
				PauseElement.Click += new RoutedEventHandler(PauseElement_Click);
			}

			if (NextElement != null)
			{
				NextElement.Click += new RoutedEventHandler(NextElement_Click);
			}
		}

        /// <summary>
        /// Handles the Click event of the NextElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void NextElement_Click(object sender, RoutedEventArgs e)
		{
			OnNextClick();
		}

		/// <summary>
		/// Handles the Click event of the PlayElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void PlayElement_Click(object sender, RoutedEventArgs e)
		{
			OnPlayClick();
		}

		/// <summary>
		/// Handles the Click event of the PauseElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void PauseElement_Click(object sender, RoutedEventArgs e)
		{
			OnPauseClick();
		}

        /// <summary>
        /// Handles the Click event of the PreviousElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void PreviousElement_Click(object sender, RoutedEventArgs e)
		{
			OnPreviousClick();
		}

        /// <summary>
        /// Called when [previous click].
        /// </summary>
		private void OnPreviousClick()
		{
			if (PreviousClick != null)
			{
				PreviousClick(this, new RoutedEventArgs());
			}
		}

        /// <summary>
        /// Called when [play click].
        /// </summary>
		private void OnPlayClick()
		{
			DisplayPlayState();

			if (PlayClick != null)
			{
				PlayClick(this, new RoutedEventArgs());
			}
		}

        /// <summary>
        /// Displays the state of the play.
        /// </summary>
		public void DisplayPlayState()
		{
			PlayButtonVisibility = Visibility.Collapsed;
			PauseButtonVisibility = Visibility.Visible;
		}

        /// <summary>
        /// Called when [pause click].
        /// </summary>
		private void OnPauseClick()
		{
			DisplayPauseState();

			if (PauseClick != null)
			{
				PauseClick(this, new RoutedEventArgs());
			}
		}

        /// <summary>
        /// Displays the state of the pause.
        /// </summary>
		public void DisplayPauseState()
		{
			PlayButtonVisibility = Visibility.Visible;
			PauseButtonVisibility = Visibility.Collapsed;		
		}

        /// <summary>
        /// Called when [next click].
        /// </summary>
		private void OnNextClick()
		{
			if (NextClick != null)
			{
				NextClick(this, new RoutedEventArgs());
			}
		}

        /// <summary>
        /// Plays this instance.
        /// </summary>
		public void Play()
		{
			OnPlayClick();
		}

        /// <summary>
        /// Pauses this instance.
        /// </summary>
		public void Pause()
		{
			OnPauseClick();
		}
	}
}