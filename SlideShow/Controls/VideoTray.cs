using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vertigo.SlideShow.Controls
{
	/// <summary>
	/// Represents a VideoTray
	/// </summary>
	[TemplatePart(Name = VideoTray.RootElementName, Type = typeof(MediaElement))]
	[TemplatePart(Name = VideoTray.PositionElementName, Type = typeof(MouseSlider))]
	[TemplatePart(Name = VideoTray.VolumeElementName, Type = typeof(MouseSlider))]
	[TemplatePart(Name = VideoTray.PlayElementName, Type = typeof(PathButton))]
	[TemplatePart(Name = VideoTray.PauseElementName, Type = typeof(PathButton))]
	[TemplatePart(Name = VideoTray.PositionTextElementName, Type = typeof(TextBlock))]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Hidden", GroupName = "CommonStates")]
	public class VideoTray : Control
	{
		#region [ Events ]

		/// <summary>
		/// Occurs when [played].
		/// </summary>
		public event EventHandler Played;

		/// <summary>
		/// Occurs when [paused].
		/// </summary>
		public event EventHandler Paused;

		/// <summary>
		/// Occurs when [position changed].
		/// </summary>
		public event EventHandler PositionChanged;

		/// <summary>
		/// Occurs when [volume changed].
		/// </summary>
		public event EventHandler VolumeChanged;

		private bool positionChanging = false;
		private double navTrayHeight = 0;

		#endregion

		#region [ Template Parts ]
		internal const string RootElementName = "RootElement";
		internal FrameworkElement RootElement { get; set; }

		internal const string PositionElementName = "PositionElement";
		internal MouseSlider PositionElement { get; set; }

		internal const string VolumeElementName = "VolumeElement";
		internal MouseSlider VolumeElement { get; set; }

		internal const string PlayElementName = "PlayElement";
		internal PathButton PlayElement { get; set; }

		internal const string PauseElementName = "PauseElement";
		internal PathButton PauseElement { get; set; }

		internal const string PositionTextElementName = "PositionTextElement";
		internal TextBlock PositionTextElement { get; set; }
		#endregion

		#region [ PlayPathData ]

		/// <summary>
		/// Gets or sets the play path data.
		/// </summary>
		/// <value>The play path data.</value>
		public string PlayPathData
		{
			get { return (string)GetValue(PlayPathDataProperty); }
			set { SetValue(PlayPathDataProperty, value); }
		}

		/// <summary>
		/// Identifies the PlayPathDataProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPathDataProperty =
			DependencyProperty.Register(
				"PlayPathData",
				typeof(string),
				typeof(VideoTray),
				null);
		#endregion

		#region [ PausePathData ]

		/// <summary>
		/// Gets or sets the pause path data.
		/// </summary>
		/// <value>The pause path data.</value>
		public string PausePathData
		{
			get { return (string)GetValue(PausePathDataProperty); }
			set { SetValue(PausePathDataProperty, value); }
		}

		/// <summary>
		/// Identifies the PausePathDataProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty PausePathDataProperty =
			DependencyProperty.Register(
				"PausePathData",
				typeof(string),
				typeof(VideoTray),
				null);
		#endregion

		#region [ ForegroundBrush ]

		/// <summary>
		/// Gets or sets the foreground brush.
		/// </summary>
		/// <value>The foreground brush.</value>
		public Brush ForegroundBrush
		{
			get { return GetValue(ForegroundBrushProperty) as Brush; }
			set { SetValue(ForegroundBrushProperty, value); }
		}

		/// <summary>
		/// Identifies the ForegroundBrushProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty ForegroundBrushProperty =
			DependencyProperty.Register(
				"ForegroundBrush",
				typeof(Brush),
				typeof(VideoTray),
				null);
		#endregion

		#region [ ForegroundHoverBrush ]

		/// <summary>
		/// Gets or sets the foreground hover brush.
		/// </summary>
		/// <value>The foreground hover brush.</value>
		public Brush ForegroundHoverBrush
		{
			get { return GetValue(ForegroundHoverBrushProperty) as Brush; }
			set { SetValue(ForegroundHoverBrushProperty, value); }
		}

		/// <summary>
		/// Identifies the ForegroundHoverBrushProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty ForegroundHoverBrushProperty =
			DependencyProperty.Register(
				"ForegroundHoverBrush",
				typeof(Brush),
				typeof(VideoTray),
				null);
		#endregion

		#region [ PlayPauseButtonWidth ]
		/// <summary>
		/// Gets or sets the width of the play pause button.
		/// </summary>
		/// <value>The width of the play pause button.</value>
		public double PlayPauseButtonWidth
		{
			get { return (double)GetValue(PlayPauseButtonWidthProperty); }
			set { SetValue(PlayPauseButtonWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PlayPauseButtonWidthProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonWidthProperty =
			DependencyProperty.Register(
				"PlayPauseButtonWidth",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ PlayPauseButtonHeight ]
		/// <summary>
		/// Gets or sets the height of the play pause button.
		/// </summary>
		/// <value>The height of the play pause button.</value>
		public double PlayPauseButtonHeight
		{
			get { return (double)GetValue(PlayPauseButtonHeightProperty); }
			set { SetValue(PlayPauseButtonHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PlayPauseButtonHeightProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonHeightProperty =
			DependencyProperty.Register(
				"PlayPauseButtonHeight",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ PlayPauseButtonPathWidth ]
		/// <summary>
		/// Gets or sets the width of the play pause button path.
		/// </summary>
		/// <value>The width of the play pause button path.</value>
		public double PlayPauseButtonPathWidth
		{
			get { return (double)GetValue(PlayPauseButtonPathWidthProperty); }
			set { SetValue(PlayPauseButtonPathWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the PlayPauseButtonPathWidthProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonPathWidthProperty =
			DependencyProperty.Register(
				"PlayPauseButtonPathWidth",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ PlayPauseButtonPathHeight ]
		/// <summary>
		/// Gets or sets the height of the play pause button path.
		/// </summary>
		/// <value>The height of the play pause button path.</value>
		public double PlayPauseButtonPathHeight
		{
			get { return (double)GetValue(PlayPauseButtonPathHeightProperty); }
			set { SetValue(PlayPauseButtonPathHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PlayPauseButtonPathHeightProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty PlayPauseButtonPathHeightProperty =
			DependencyProperty.Register(
				"PlayPauseButtonPathHeight",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ BackgroundOpacity ]

		/// <summary>
		/// Gets or sets the background opacity.
		/// </summary>
		/// <value>The background opacity.</value>
		public double BackgroundOpacity
		{
			get { return (double)GetValue(BackgroundOpacityProperty); }
			set { SetValue(BackgroundOpacityProperty, value); }
		}

		/// <summary>
		/// Identifies the BackgroundOpacityProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty BackgroundOpacityProperty =
			DependencyProperty.Register(
				"BackgroundOpacity",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ RadiusX ]
		/// <summary>
		/// Gets or sets the radius X.
		/// </summary>
		/// <value>The radius X.</value>
		public double RadiusX
		{
			get { return (double)GetValue(RadiusXProperty); }
			set { SetValue(RadiusXProperty, value); }
		}

		/// <summary>
		/// Identifies the RadiusXProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty RadiusXProperty =
			DependencyProperty.Register(
				"RadiusX",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ RadiusY ]
		/// <summary>
		/// Gets or sets the radius Y.
		/// </summary>
		/// <value>The radius Y.</value>
		public double RadiusY
		{
			get { return (double)GetValue(RadiusYProperty); }
			set { SetValue(RadiusYProperty, value); }
		}

		/// <summary>
		/// Identifies the RadiusYProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty RadiusYProperty =
			DependencyProperty.Register(
				"RadiusY",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ TextFontFamily ]

		public FontFamily TextFontFamily
		{
			get { return (FontFamily)GetValue(TextFontFamilyProperty); }
			set { SetValue(TextFontFamilyProperty, value); }
		}

		public static readonly DependencyProperty TextFontFamilyProperty =
			DependencyProperty.Register(
				"TextFontFamily",
				typeof(FontFamily),
				typeof(VideoTray),
				null);
		#endregion

		#region [ TextFontSize ]
		/// <summary>
		/// Gets or sets the size of the text font.
		/// </summary>
		/// <value>The size of the text font.</value>
		public double TextFontSize
		{
			get { return (double)GetValue(TextFontSizeProperty); }
			set { SetValue(TextFontSizeProperty, value); }
		}

		/// <summary>
		/// Identifies the TextFontSizeProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty TextFontSizeProperty =
			DependencyProperty.Register(
				"TextFontSize",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		#region [ TextForegroundBrush ]

		/// <summary>
		/// Gets or sets the text foreground brush.
		/// </summary>
		/// <value>The text foreground brush.</value>
		public Brush TextForegroundBrush
		{
			get { return GetValue(TextForegroundBrushProperty) as Brush; }
			set { SetValue(TextForegroundBrushProperty, value); }
		}

		/// <summary>
		/// Identifies the TextForegroundBrushProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty TextForegroundBrushProperty =
			DependencyProperty.Register(
				"TextForegroundBrush",
				typeof(Brush),
				typeof(VideoTray),
				null);
		#endregion

		#region [ VolumeWidth ]
		/// <summary>
		/// Gets or sets the width of the volume.
		/// </summary>
		/// <value>The width of the volume.</value>
		public double VolumeWidth
		{
			get { return (double)GetValue(VolumeWidthProperty); }
			set { SetValue(VolumeWidthProperty, value); }
		}

		/// <summary>
		/// Identifies the VolumeWidthProperty dependency property.
		/// </summary>
		public static readonly DependencyProperty VolumeWidthProperty =
			DependencyProperty.Register(
				"VolumeWidth",
				typeof(double),
				typeof(VideoTray),
				null);
		#endregion

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public TimeSpan Position
		{
			get { return TimeSpan.FromMilliseconds(PositionElement.Value); }
			set
			{
				if (!positionChanging)
				{
					PositionElement.Value = value.TotalMilliseconds;
				}

				OnMediaUpdated();
			}
		}

		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>The volume.</value>
		public double Volume
		{
			get { return VolumeElement.Value; }
			set { VolumeElement.Value = value; }
		}

		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public TimeSpan Duration
		{
			get { return TimeSpan.FromMilliseconds(PositionElement.Maximum); }
			set
			{
				PositionElement.Maximum = value.TotalMilliseconds;
				OnMediaUpdated();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VideoTray"/> class.
		/// </summary>
		public VideoTray()
		{
			DefaultStyleKey = typeof(VideoTray);
			Options.VideoTrayOptions options = Configuration.Options.VideoTray;

			Width = options.Width;
			Height = options.Height;
            Margin = options.Margin;

			PlayPathData = options.PlayPathData;
			PausePathData = options.PausePathData;
			PlayPauseButtonWidth = options.PlayPauseButtonWidth;
			PlayPauseButtonHeight = options.PlayPauseButtonHeight;
			PlayPauseButtonPathWidth = options.PlayPauseButtonPathWidth;
			PlayPauseButtonPathHeight = options.PlayPauseButtonPathHeight;

			ForegroundBrush = options.Foreground;
			ForegroundHoverBrush = options.ForegroundHover;
			Background = options.Background;
			BackgroundOpacity = options.BackgroundOpacity;
			RadiusX = options.RadiusX;
			RadiusY = options.RadiusY;

			TextFontFamily = options.TextFontFamily;
			TextFontSize = options.TextFontSize;
			TextForegroundBrush = options.TextForegroundBrush;

			VolumeWidth = options.VolumeWidth;

			Options.SlideThumbnailOptions slideThumb = Configuration.Options.SlideThumbnail;
			Options.ThumbnailViewerOptions thumbViewer = Configuration.Options.ThumbnailViewer;

			navTrayHeight =
				slideThumb.Height +
				thumbViewer.Margin.Top +
				thumbViewer.Margin.Bottom +
				slideThumb.BorderThickness.Top +
				slideThumb.BorderThickness.Bottom +
				thumbViewer.BackgroundRadiusY;

			if (!options.Enabled)
			{
				Visibility = Visibility.Collapsed;
			}
		}

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			object root = GetTemplateChild(RootElementName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null),
				"The template part RootElement is not an instance of FrameworkElement!");

			RootElement = root as FrameworkElement;

			object pos = GetTemplateChild(PositionElementName);
			Debug.Assert(typeof(MouseSlider).IsInstanceOfType(pos) || (pos == null),
				"The template part PositionElement is not an instance of MouseSlider!");

			PositionElement = pos as MouseSlider;

			object volume = GetTemplateChild(VolumeElementName);
			Debug.Assert(typeof(MouseSlider).IsInstanceOfType(volume) || (volume == null),
				"The template part VolumeElement is not an instance of MouseSlider!");

			VolumeElement = volume as MouseSlider;

			object play = GetTemplateChild(PlayElementName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(play) || (play == null),
				"The template part PlayElement is not an instance of PathButton!");

			PlayElement = play as PathButton;

			object pause = GetTemplateChild(PauseElementName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(pause) || (pause == null),
				"The template part PauseElement is not an instance of PathButton!");

			PauseElement = pause as PathButton;

			object text = GetTemplateChild(PositionTextElementName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(text) || (text == null),
				"The template part PositionTextElement is not an instance of TextBlock!");

			PositionTextElement = text as TextBlock;

			if (PlayElement != null)
			{
				PlayElement.Click += new RoutedEventHandler(PlayElement_Click);
			}

			if (PauseElement != null)
			{
				PauseElement.Click += new RoutedEventHandler(PauseElement_Click);
			}

			if (PositionElement != null)
			{
				PositionElement.ThumbDragStarted += new EventHandler<EventArgs>(PositionElement_ThumbDragStarted);
				PositionElement.ThumbDragCompleted += new EventHandler<EventArgs>(PositionElement_ThumbDragCompleted);
			}

			if (VolumeElement != null)
			{
				VolumeElement.ThumbDragCompleted += new EventHandler<EventArgs>(VolumeElement_ThumbDragCompleted);
			}

			Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
			Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);

			Show();
		}

		/// <summary>
		/// Plays this instance.
		/// </summary>
		public void Play()
		{
			OnPlayed();
		}

		/// <summary>
		/// Pauses this instance.
		/// </summary>
		public void Pause()
		{
			OnPaused();
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			PlayElement.Visibility = Visibility.Visible;
			PauseElement.Visibility = Visibility.Collapsed;
		}

		public void Show()
		{
			double top = Application.Current.Host.Content.ActualHeight - navTrayHeight - Height;
			double left = (Application.Current.Host.Content.ActualWidth / 2) - (Width / 2);

			Canvas.SetTop(RootElement, top);
			Canvas.SetLeft(RootElement, left);

			VisualStateManager.GoToState(this, "Normal", true);
		}

		/// <summary>
		/// Hides this instance.
		/// </summary>
		public void Hide()
		{
			VisualStateManager.GoToState(this, "Hidden", true);
		}

		/// <summary>
		/// Called when [played].
		/// </summary>
		private void OnPlayed()
		{
			PlayElement.Visibility = Visibility.Collapsed;
			PauseElement.Visibility = Visibility.Visible;

			if (Played != null)
			{
				Played(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [paused].
		/// </summary>
		private void OnPaused()
		{
			PlayElement.Visibility = Visibility.Visible;
			PauseElement.Visibility = Visibility.Collapsed;

			if (Paused != null)
			{
				Paused(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [position changed].
		/// </summary>
		private void OnPositionChanged()
		{
			if (PositionChanged != null)
			{
				PositionChanged(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [volume changed].
		/// </summary>
		private void OnVolumeChanged()
		{
			if (VolumeChanged != null)
			{
				VolumeChanged(this, new EventArgs());
			}
		}

		/// <summary>
		/// Called when [media updated].
		/// </summary>
		private void OnMediaUpdated()
		{
			TimeSpan position = this.Position;
			TimeSpan duration = this.Duration;

			PositionTextElement.Text = string.Format(CultureInfo.CurrentCulture, "{0:00}:{1:00}/{2:00}:{3:00}",
				position.Minutes, position.Seconds,
				duration.Minutes, duration.Seconds);
		}

		/// <summary>
		/// Handles the Resized event of the Content control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			Show();
		}

		/// <summary>
		/// Handles the ThumbDragCompleted event of the VolumeElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void VolumeElement_ThumbDragCompleted(object sender, EventArgs e)
		{
			OnVolumeChanged();
		}

		/// <summary>
		/// Handles the ThumbDragCompleted event of the PositionElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void PositionElement_ThumbDragCompleted(object sender, EventArgs e)
		{
			positionChanging = false;
			OnPositionChanged();
		}

		/// <summary>
		/// Handles the ThumbDragStarted event of the PositionElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void PositionElement_ThumbDragStarted(object sender, EventArgs e)
		{
			positionChanging = true;
		}

		/// <summary>
		/// Handles the Click event of the PauseElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void PauseElement_Click(object sender, RoutedEventArgs e)
		{
			OnPaused();
		}

		/// <summary>
		/// Handles the Click event of the PlayElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void PlayElement_Click(object sender, RoutedEventArgs e)
		{
			OnPlayed();
		}
	}
}