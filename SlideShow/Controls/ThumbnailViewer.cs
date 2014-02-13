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
    /// Represents the ThumbnailViewer
    /// </summary>
	[TemplatePart(Name = ThumbnailViewer.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = ThumbnailViewer.ElementBackgroundRectangleName, Type = typeof(Rectangle))]
	[TemplatePart(Name = ThumbnailViewer.ElementGridScrollStoryboardName, Type = typeof(Storyboard))]
	[TemplatePart(Name = ThumbnailViewer.ElementGridScrollAnimationName, Type = typeof(DoubleAnimation))]
	[TemplatePart(Name = ThumbnailViewer.ElementScrollLeftRepeatPathButtonName, Type = typeof(RepeatPathButton))]
	[TemplatePart(Name = ThumbnailViewer.ElementThumbContainerName, Type = typeof(StackPanel))]
	[TemplatePart(Name = ThumbnailViewer.ElementThumbViewportName, Type = typeof(Canvas))]
	[TemplatePart(Name = ThumbnailViewer.ElementScrollRightRepeatPathButtonName, Type = typeof(RepeatPathButton))]
	public partial class ThumbnailViewer : Control
	{
		#region [ Constructors ]
		/// <summary>
		/// Initializes a new instance of the <see cref="ThumbnailViewer"/> class.
		/// </summary>
		/// <param name="album">The album.</param>
		public ThumbnailViewer(Album album)
		{
			IsTabStop = false;
			DefaultStyleKey = typeof(ThumbnailViewer);
			this.Album = album;

			Options.ThumbnailViewerOptions thumbnailViewerOptions = Configuration.Options.ThumbnailViewer;
			Options.SlideThumbnailOptions slideThumbnailOptions = Configuration.Options.SlideThumbnail;
			Options.SlidePreviewOptions slidePreviewOptions = Configuration.Options.SlidePreview;

			ThumbnailAreaWidth = thumbnailViewerOptions.Width;
			Width = ThumbnailAreaWidth
				+ thumbnailViewerOptions.ScrollButtonWidth * 2
				+ thumbnailViewerOptions.ScrollButtonMargin.Left * 2
				+ thumbnailViewerOptions.ScrollButtonMargin.Right * 2
				+ 4 * 2; // 4 = padding on each side
			ThumbnailHeight = slideThumbnailOptions.Height +
				slideThumbnailOptions.BorderThickness.Bottom +
				slideThumbnailOptions.BorderThickness.Top;
			Height = ThumbnailHeight
				+ thumbnailViewerOptions.Margin.Top
				+ thumbnailViewerOptions.Margin.Bottom
				+ slideThumbnailOptions.BorderThickness.Top
				+ slideThumbnailOptions.BorderThickness.Bottom; // 6 = Padding
			ScrollIncrement = thumbnailViewerOptions.ScrollIncrement;
			Background = thumbnailViewerOptions.Background;
			BackgroundOpacity = thumbnailViewerOptions.BackgroundOpacity;
			BackgroundRadiusX = thumbnailViewerOptions.BackgroundRadiusX;
			BackgroundRadiusY = thumbnailViewerOptions.BackgroundRadiusY;
			ScrollButtonBrush = thumbnailViewerOptions.ScrollButtonBrush;
			ScrollButtonHoverBrush = thumbnailViewerOptions.ScrollButtonHoverBrush;
			LeftScrollPathButtonData = thumbnailViewerOptions.LeftScrollButtonData;
			RightScrollPathButtonData = thumbnailViewerOptions.RightScrollButtonData;
			ScrollPathButtonWidth = thumbnailViewerOptions.ScrollButtonWidth;
			ScrollPathButtonHeight = thumbnailViewerOptions.ScrollButtonHeight;
			ScrollButtonMargin = thumbnailViewerOptions.ScrollButtonMargin;
			ScrollRepeatButtonInterval = thumbnailViewerOptions.ScrollRepeatButtonInterval;

			FinishedLoading += new EventHandler(ThumbnailViewer_FinishedLoading);

			if (!thumbnailViewerOptions.Enabled)
			{
				Visibility = Visibility.Collapsed;
			}
		}

		#endregion

		#region [Template Parts]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal Rectangle _elementBackgroundRectangle;
		internal const string ElementBackgroundRectangleName = "BackgroundRectangleElement";

		internal Storyboard _elementGridScrollStoryboard;
		internal const string ElementGridScrollStoryboardName = "GridScrollStoryboardElement";

		internal DoubleAnimation _elementGridScrollAnimation;
		internal const string ElementGridScrollAnimationName = "GridScrollAnimationElement";

		internal RepeatPathButton _elementScrollLeftRepeatPathButton;
		internal const string ElementScrollLeftRepeatPathButtonName = "ScrollLeftRepeatPathButtonElement";

		internal StackPanel _elementThumbContainer;
		internal const string ElementThumbContainerName = "ThumbContainerElement";

		internal Canvas _elementThumbViewport;
		internal const string ElementThumbViewportName = "ThumbViewportElement";

		internal RepeatPathButton _elementScrollRightRepeatPathButton;
		internal const string ElementScrollRightRepeatPathButtonName = "ScrollRightRepeatPathButtonElement";

		#endregion

		#region [Private Properties]

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
        /// Gets or sets the background rectangle element.
        /// </summary>
        /// <value>The background rectangle element.</value>
		private Rectangle BackgroundRectangleElement
		{
			get { return _elementBackgroundRectangle; }
			set { _elementBackgroundRectangle = value; }
		}

        /// <summary>
        /// Gets or sets the grid scroll storyboard element.
        /// </summary>
        /// <value>The grid scroll storyboard element.</value>
		private Storyboard GridScrollStoryboardElement
		{
			get { return _elementGridScrollStoryboard; }
			set { _elementGridScrollStoryboard = value; }
		}

        /// <summary>
        /// Gets or sets the grid scroll animation element.
        /// </summary>
        /// <value>The grid scroll animation element.</value>
		private DoubleAnimation GridScrollAnimationElement
		{
			get { return _elementGridScrollAnimation; }
			set { _elementGridScrollAnimation = value; }
		}

        /// <summary>
        /// Gets or sets the scroll left repeat path button element.
        /// </summary>
        /// <value>The scroll left repeat path button element.</value>
		private RepeatPathButton ScrollLeftRepeatPathButtonElement
		{
			get { return _elementScrollLeftRepeatPathButton; }
			set { _elementScrollLeftRepeatPathButton = value; }
		}

        /// <summary>
        /// Gets or sets the thumb container element.
        /// </summary>
        /// <value>The thumb container element.</value>
		private StackPanel ThumbContainerElement
		{
			get { return _elementThumbContainer; }
			set { _elementThumbContainer = value; }
		}

        /// <summary>
        /// Gets or sets the thumb viewport element.
        /// </summary>
        /// <value>The thumb viewport element.</value>
		private Canvas ThumbViewportElement
		{
			get { return _elementThumbViewport; }
			set { _elementThumbViewport = value; }
		}

        /// <summary>
        /// Gets or sets the scroll right repeat path button element.
        /// </summary>
        /// <value>The scroll right repeat path button element.</value>
		private RepeatPathButton ScrollRightRepeatPathButtonElement
		{
			get { return _elementScrollRightRepeatPathButton; }
			set { _elementScrollRightRepeatPathButton = value; }
		}

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
		private double Offset { get; set; }

		private int lastThumbnailIndex = 0;

		#endregion

		#region [ Public Properties ]

        /// <summary>
        /// Gets the album.
        /// </summary>
        /// <value>The album.</value>
		public Album Album { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is finished loading.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is finished loading; otherwise, <c>false</c>.
        /// </value>
		public bool IsFinishedLoading { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is populated.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is populated; otherwise, <c>false</c>.
        /// </value>
		public bool IsPopulated { get; private set; }

        /// <summary>
        /// Gets the scroll increment.
        /// </summary>
        /// <value>The scroll increment.</value>
		public static double ScrollIncrement { get; private set; }

		#endregion

		#region [ Events]

        /// <summary>
        /// Occurs when [finished loading].
        /// </summary>
		public event EventHandler FinishedLoading;

		/// <summary>
		/// Defines the ThumbnailClickedEventHandler delegate.
		/// </summary>
		/// <param name="sender">The sending object</param>
		/// <param name="index">The index as an int</param>
		public delegate void ThumbnailClickedEventHandler(object sender, int index);

		/// <summary>
		/// Occurs when [thumbnail clicked].
		/// </summary>
		public event ThumbnailClickedEventHandler ThumbnailClicked;

		/// <summary>
		/// Defines the HighlightThumbnailChanged delegate.
		/// </summary>
		/// <param name="index">The index as an int</param>
		public delegate void HighlightThumbnailChangedEventHandler(int index);

		/// <summary>
		/// Occurs when the thumbnail to be highlighted changes.
		/// </summary>
		public event HighlightThumbnailChangedEventHandler HighlightThumbnailChanged;

		#endregion

		#region [ ScrollButtonBrush ]
		public Brush ScrollButtonBrush
		{
			get { return GetValue(ScrollButtonBrushProperty) as Brush; }
			set { SetValue(ScrollButtonBrushProperty, value); }
		}

		public static readonly DependencyProperty ScrollButtonBrushProperty =
			DependencyProperty.Register(
				"ScrollButtonBrush",
				typeof(Brush),
				typeof(ThumbnailViewer),
				null);

		#endregion

		#region [ ScrollButtonHoverBrush ]
		public Brush ScrollButtonHoverBrush
		{
			get { return GetValue(ScrollButtonHoverBrushProperty) as Brush; }
			set { SetValue(ScrollButtonHoverBrushProperty, value); }
		}

		public static readonly DependencyProperty ScrollButtonHoverBrushProperty =
			DependencyProperty.Register(
				"ScrollButtonHoverBrush",
				typeof(Brush),
				typeof(ThumbnailViewer),
				null);

		#endregion

		#region [ ThumbnailHeight ]
		public double ThumbnailHeight
		{
			get { return (double)GetValue(ThumbnailHeightProperty); }
			set { SetValue(ThumbnailHeightProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailHeightProperty =
			DependencyProperty.Register(
				"ThumbnailHeight",
				typeof(double),
				typeof(ThumbnailViewer),
				null);

		#endregion

		#region [ ThumbnailAreaWidth ]
		public double ThumbnailAreaWidth
		{
			get { return (double)GetValue(ThumbnailAreaWidthProperty); }
			set { SetValue(ThumbnailAreaWidthProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailAreaWidthProperty =
			DependencyProperty.Register(
				"ThumbnailAreaWidth",
				typeof(double),
				typeof(ThumbnailViewer),
				null);

		#endregion

		#region [ ScrollPathButtonWidth ]

		public double ScrollPathButtonWidth
		{
			get { return (double)GetValue(ScrollPathButtonWidthProperty); }
			set { SetValue(ScrollPathButtonWidthProperty, value); }
		}

		public static readonly DependencyProperty ScrollPathButtonWidthProperty =
			DependencyProperty.Register(
				"ScrollPathButtonWidth",
				typeof(double),
				typeof(ThumbnailViewer),
				null);
		#endregion

		#region [ ScrollPathButtonHeight ]

		public double ScrollPathButtonHeight
		{
			get { return (double)GetValue(ScrollPathButtonHeightProperty); }
			set { SetValue(ScrollPathButtonHeightProperty, value); }
		}

		public static readonly DependencyProperty ScrollPathButtonHeightProperty =
			DependencyProperty.Register(
				"ScrollPathButtonHeight",
				typeof(double),
				typeof(ThumbnailViewer),
				null);
		#endregion

		#region [ ScrollButtonMargin ]

		public Thickness ScrollButtonMargin
		{
			get { return (Thickness)GetValue(ScrollButtonMarginProperty); }
			set { SetValue(ScrollButtonMarginProperty, value); }
		}

		public static readonly DependencyProperty ScrollButtonMarginProperty =
			DependencyProperty.Register(
				"ScrollButtonMargin",
				typeof(Thickness),
				typeof(ThumbnailViewer),
				null);
		#endregion

		#region [ LeftScrollPathButtonData ]

		public string LeftScrollPathButtonData
		{
			get { return (string)GetValue(LeftScrollPathButtonDataProperty); }
			set { SetValue(LeftScrollPathButtonDataProperty, value); }
		}

		public static readonly DependencyProperty LeftScrollPathButtonDataProperty =
			DependencyProperty.Register(
				"LeftScrollPathButtonData",
				typeof(string),
				typeof(ThumbnailViewer),
				null);
		#endregion

		#region [ RightScrollPathButtonData ]

		public string RightScrollPathButtonData
		{
			get { return (string)GetValue(RightScrollPathButtonDataProperty); }
			set { SetValue(RightScrollPathButtonDataProperty, value); }
		}

		public static readonly DependencyProperty RightScrollPathButtonDataProperty =
			DependencyProperty.Register(
				"RightScrollPathButtonData",
				typeof(string),
				typeof(ThumbnailViewer),
				null);
		#endregion

		#region [ ScrollRepeatButtonInterval ]

		public int ScrollRepeatButtonInterval
		{
			get { return (int)GetValue(ScrollRepeatButtonIntervalProperty); }
			set { SetValue(ScrollRepeatButtonIntervalProperty, value); }
		}

		public static readonly DependencyProperty ScrollRepeatButtonIntervalProperty =
			DependencyProperty.Register(
				"ScrollRepeatButtonInterval",
				typeof(int),
				typeof(ThumbnailViewer),
				null);
		#endregion

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
				typeof(ThumbnailViewer),
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
				typeof(ThumbnailViewer),
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
				typeof(ThumbnailViewer),
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

			if (RootElement != null)
			{
				object gridScrollStoryboard = GetTemplateChild(ElementGridScrollStoryboardName);
				Debug.Assert(typeof(Storyboard).IsInstanceOfType(gridScrollStoryboard) || (gridScrollStoryboard == null),
					"The template part GridScrollStoryboardElement is not an instance of Storyboard!");
				GridScrollStoryboardElement = gridScrollStoryboard as Storyboard;

				GridScrollStoryboardElement.Completed += new EventHandler(GridScrollStoryboardElement_Completed);

				object backgroundRectangle = GetTemplateChild(ElementBackgroundRectangleName);
				Debug.Assert(typeof(Rectangle).IsInstanceOfType(backgroundRectangle) || (backgroundRectangle == null),
					"The template part BackgroundRectangleElement is not an instance of Rectangle!");
				
				BackgroundRectangleElement = backgroundRectangle as Rectangle;

				object gridScrollAnimation = GetTemplateChild(ElementGridScrollAnimationName);
				Debug.Assert(typeof(DoubleAnimation).IsInstanceOfType(gridScrollAnimation) || (gridScrollAnimation == null),
					"The template part GridScrollAnimationElement is not an instance of Animation!");
				
				GridScrollAnimationElement = gridScrollAnimation as DoubleAnimation;

				object scrollLeftRepeatPathButton = GetTemplateChild(ElementScrollLeftRepeatPathButtonName);
				Debug.Assert(typeof(RepeatPathButton).IsInstanceOfType(scrollLeftRepeatPathButton) || (scrollLeftRepeatPathButton == null),
					"The template part ScrollLeftRepeatPathButtonElement is not an instance of RepeatPathButton!");
				
				ScrollLeftRepeatPathButtonElement = scrollLeftRepeatPathButton as RepeatPathButton;

				if (ScrollLeftRepeatPathButtonElement != null)
				{
					ScrollLeftRepeatPathButtonElement.Click += new RoutedEventHandler(ScrollLeftRepeatPathButtonElement_Click);
				}

				object scrollRightRepeatPathButton = GetTemplateChild(ElementScrollRightRepeatPathButtonName);
				Debug.Assert(typeof(RepeatPathButton).IsInstanceOfType(scrollRightRepeatPathButton) || (scrollRightRepeatPathButton == null),
					"The template part ScrollRightRepeatPathButtonElement is not an instance of RepeatPathButton!");
				ScrollRightRepeatPathButtonElement = scrollRightRepeatPathButton as RepeatPathButton;

				if (ScrollRightRepeatPathButtonElement != null)
				{
					ScrollRightRepeatPathButtonElement.Click += new RoutedEventHandler(ScrollRightRepeatPathButtonElement_Click);
				}

				object thumbContainer = GetTemplateChild(ElementThumbContainerName);
				Debug.Assert(typeof(StackPanel).IsInstanceOfType(thumbContainer) || (thumbContainer == null),
					"The template part ThumbContainerElement is not an instance of StackPanel!");
				
				ThumbContainerElement = thumbContainer as StackPanel;

				object thumbViewport = GetTemplateChild(ElementThumbViewportName);
				Debug.Assert(typeof(Canvas).IsInstanceOfType(thumbViewport) || (thumbViewport == null),
					"The template part ThumbViewportElement is not an instance of Canvas!");
				
				ThumbViewportElement = thumbViewport as Canvas;
				
				if (ThumbViewportElement != null)
				{
					ThumbViewportElement.SizeChanged += new SizeChangedEventHandler(ThumbViewportElement_SizeChanged);
				}

				Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
			}

			if (FinishedLoading != null)
			{
				FinishedLoading(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Handles the SizeChanged event of the ThumbViewportElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
		void ThumbViewportElement_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetViewportClippingPath();
			ThumbViewportElement.SizeChanged -= ThumbViewportElement_SizeChanged;
		}

		/// <summary>
		/// Handles the Completed event of the GridScrollStoryboardElement control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void GridScrollStoryboardElement_Completed(object sender, EventArgs e)
		{
			SlideThumbnail_FinishedLoading();
			GridScrollStoryboardElement.Completed -= GridScrollStoryboardElement_Completed;
		}

		/// <summary>
		/// Handles the Resized event of the Content control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			SetViewportClippingPath();
		}

		/// <summary>
		/// Sets the viewport clipping path.
		/// </summary>
		private void SetViewportClippingPath()
		{
			if (ThumbViewportElement == null)
			{
				return;
			}
			
			Size appSize = Application.Current.RootVisual.RenderSize;
			RectangleGeometry thumbRectangle = new RectangleGeometry()
			{
				Rect = new Rect(0, 0, ThumbViewportElement.ActualWidth, ThumbViewportElement.ActualHeight)
			};

			double previewRectangleHeight = appSize.Height - ThumbViewportElement.ActualHeight;
			previewRectangleHeight = previewRectangleHeight < 0 ? 0 : previewRectangleHeight; // Make sure not to set a negative height

			RectangleGeometry previewRectangle = new RectangleGeometry()
			{
				Rect = new Rect(-(appSize.Width - ThumbViewportElement.ActualWidth) / 2, ThumbViewportElement.ActualHeight - appSize.Height, appSize.Width, previewRectangleHeight)
			};

			GeometryGroup group = new GeometryGroup();
			group.Children.Add(thumbRectangle);
			group.Children.Add(previewRectangle);
			ThumbViewportElement.Clip = group;
		}

        /// <summary>
        /// Populates this instance.
        /// </summary>
		public void Populate()
		{
			if (IsFinishedLoading)
			{
				PopulateThumbContainer();
			}
		}

        /// <summary>
        /// Populates the thumb container.
        /// </summary>
		private void PopulateThumbContainer()
		{
			if (IsPopulated || ThumbContainerElement == null)
			{
				return;
			}

			if (Album.Slides.Length != 0)
			{
				SlideThumbnail_FinishedLoading();
			}

			IsPopulated = true;
		}

		/// <summary>
		/// Handles the FinishedLoading event of the SlideThumbnail control.
		/// </summary>
		private void SlideThumbnail_FinishedLoading()
		{
			if (lastThumbnailIndex < Album.Slides.Length)
			{
				AddNextSlideThumbnail();
			}
		}

        /// <summary>
		/// Adds the next slide thumbnail.
        /// </summary>
		public void AddNextSlideThumbnail()
		{
			if (lastThumbnailIndex >= Album.Slides.Length)
			{
				return;
			}

			SlideThumbnail slideThumbnail = new SlideThumbnail(Album.Slides[lastThumbnailIndex]) 
			{
				Height = ThumbnailHeight 
					- Configuration.Options.SlideThumbnail.BorderThickness.Bottom 
					- Configuration.Options.SlideThumbnail.BorderThickness.Top,
				Width = ThumbnailHeight 
					- Configuration.Options.SlideThumbnail.BorderThickness.Bottom 
					- Configuration.Options.SlideThumbnail.BorderThickness.Top,
				Index = lastThumbnailIndex,
				Margin = new Thickness(0, 0, Configuration.Options.ThumbnailViewer.ThumbSpacing, 0)
			};

			ThumbContainerElement.Children.Add(slideThumbnail);

			slideThumbnail.Click += new RoutedEventHandler(slideThumbnail_Click);

			HighlightThumbnailChanged += delegate(int index)
			{
				if (index == slideThumbnail.Index)
				{
					slideThumbnail.BorderBrush = Configuration.Options.SlideThumbnail.BorderHighlightBrush;
				}
				else
				{
					slideThumbnail.BorderBrush = Configuration.Options.SlideThumbnail.BorderBrush;
				}
			};

			lastThumbnailIndex++;
			SlideThumbnail_FinishedLoading();
		}

        /// <summary>
        /// Handles the Click event of the slideThumbnail control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void slideThumbnail_Click(object sender, RoutedEventArgs e)
		{
			if (!(sender is SlideThumbnail) || ThumbnailClicked == null)
			{
				return;
			}

			ThumbnailClicked(sender, ((SlideThumbnail)sender).Index);
		}

		/// <summary>
		/// Changes the border color of the thumbnail specified and returns the previously highlighted thumbnail border color to the default.
		/// </summary>
		/// <param name="index">The index of the of the thumbnail to highlight.</param>
		public void HighlightThumbnail(int index)
		{
			if (HighlightThumbnailChanged != null)
			{
				HighlightThumbnailChanged(index);
			}
		}

        /// <summary>
        /// Handles the Click event of the ScrollLeftRepeatPathButtonElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void ScrollLeftRepeatPathButtonElement_Click(object sender, RoutedEventArgs e)
		{
			if (ThumbContainerElement.ActualWidth < ThumbnailAreaWidth
				+ Configuration.Options.SlideThumbnail.BorderThickness.Right
				+ Configuration.Options.ThumbnailViewer.ThumbSpacing)
			{
				// All thumbnails are visible, no need to scroll
				return;
			}

			if (Offset > -ScrollIncrement)
			{
				Offset = 0;
			}
			else
			{
				Offset += ScrollIncrement;
			}

			GridScrollAnimationElement.To = Offset;
			GridScrollStoryboardElement.Begin();
		}

        /// <summary>
        /// Handles the Click event of the ScrollRightRepeatPathButtonElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void ScrollRightRepeatPathButtonElement_Click(object sender, RoutedEventArgs e)
		{
			if (ThumbContainerElement.ActualWidth < ThumbnailAreaWidth 
				+ Configuration.Options.SlideThumbnail.BorderThickness.Right 
				+ Configuration.Options.ThumbnailViewer.ThumbSpacing)
			{
				// All thumbnails are visible, no need to scroll
				return;
			}

			if (Offset > ThumbnailAreaWidth - ThumbContainerElement.ActualWidth)
			{
				Offset -= ScrollIncrement + 5; // TODO: Why is the scrolling off by 5 pixels?
			}

			GridScrollAnimationElement.To = Offset;
			GridScrollStoryboardElement.Begin();
		}

		/// <summary>
		/// Handles the FinishedLoading event of the ThumbnailViewer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void ThumbnailViewer_FinishedLoading(object sender, EventArgs e)
		{
			IsFinishedLoading = true;
			PopulateThumbContainer();
			FinishedLoading -= ThumbnailViewer_FinishedLoading;
		}
	}
}