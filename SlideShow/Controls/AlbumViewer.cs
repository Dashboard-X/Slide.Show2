using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vertigo.SlideShow.Controls
{
	[TemplatePart(Name = AlbumViewer.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = AlbumViewer.ElementPageContainerCanvasName, Type = typeof(Canvas))]
	[TemplatePart(Name = AlbumViewer.ElementPageNumberTextBlockName, Type = typeof(TextBlock))]
	[TemplatePart(Name = AlbumViewer.ElementScrollLeftPathButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = AlbumViewer.ElementScrollRightPathButtonName, Type = typeof(PathButton))]
	public class AlbumViewer : Control
	{
		#region [ Template Parts ]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal Canvas _elementPageContainerCanvas;
		internal const string ElementPageContainerCanvasName = "PageContainerCanvasElement";

		internal TextBlock _elementPageNumberTextBlock;
		internal const string ElementPageNumberTextBlockName = "PageNumberTextBlockElement";

		internal PathButton _elementScrollLeftPathButton;
		internal const string ElementScrollLeftPathButtonName = "ScrollLeftPathButtonElement";

		internal PathButton _elementScrollRightPathButton;
		internal const string ElementScrollRightPathButtonName = "ScrollRightPathButtonElement";

		#endregion

		#region [ Private Properties ]

		private FrameworkElement RootElement
		{
			get { return _elementRoot; }
			set { _elementRoot = value; }
		}

		private Canvas PageContainerCanvasElement
		{
			get { return _elementPageContainerCanvas; }
			set { _elementPageContainerCanvas = value; }
		}

		private TextBlock PageNumberTextBlockElement
		{
			get { return _elementPageNumberTextBlock; }
			set { _elementPageNumberTextBlock = value; }
		}

		private PathButton ScrollLeftPathButtonElement
		{
			get { return _elementScrollLeftPathButton; }
			set { _elementScrollLeftPathButton = value; }
		}

		private PathButton ScrollRightPathButtonElement
		{
			get { return _elementScrollRightPathButton; }
			set { _elementScrollRightPathButton = value; }
		}

		#endregion

		#region [ Public Properties ]

        /// <summary>
        /// Gets the current album page.
        /// </summary>
        /// <value>The current album page.</value>
		public AlbumPage CurrentAlbumPage { get; private set; }

		#endregion

		#region [ ScrollPathButtonForegroundBrush ]

		public Brush ScrollPathButtonForegroundBrush
		{
			get { return GetValue(ScrollPathButtonForegroundBrushProperty) as Brush; }
			set { SetValue(ScrollPathButtonForegroundBrushProperty, value); }
		}

		public static readonly DependencyProperty ScrollPathButtonForegroundBrushProperty =
			DependencyProperty.Register(
				"ScrollPathButtonForegroundBrush",
				typeof(Brush),
				typeof(AlbumViewer),
				null);

		#endregion

		#region [ ScrollPathButtonForegroundHoverBrush ]

		public Brush ScrollPathButtonForegroundHoverBrush
		{
			get { return GetValue(ScrollPathButtonForegroundHoverBrushProperty) as Brush; }
			set { SetValue(ScrollPathButtonForegroundHoverBrushProperty, value); }
		}

		public static readonly DependencyProperty ScrollPathButtonForegroundHoverBrushProperty =
			DependencyProperty.Register(
				"ScrollPathButtonForegroundHoverBrush",
				typeof(Brush),
				typeof(AlbumViewer),
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
				typeof(AlbumViewer),
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
				typeof(AlbumViewer),
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
				typeof(AlbumViewer),
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
				typeof(AlbumViewer),
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
				typeof(AlbumViewer),
				null);
		#endregion

		#region [ PageNumberFontFamily ]

		public FontFamily PageNumberFontFamily
		{
			get { return (FontFamily)GetValue(PageNumberFontFamilyProperty); }
			set { SetValue(PageNumberFontFamilyProperty, value); }
		}

		public static readonly DependencyProperty PageNumberFontFamilyProperty =
			DependencyProperty.Register(
				"PageNumberFontFamily",
				typeof(FontFamily),
				typeof(AlbumViewer),
				null);
		#endregion

		#region [ PageNumberFontSize ]

		public double PageNumberFontSize
		{
			get { return (double)GetValue(PageNumberFontSizeProperty); }
			set { SetValue(PageNumberFontSizeProperty, value); }
		}

		public static readonly DependencyProperty PageNumberFontSizeProperty =
			DependencyProperty.Register(
				"PageNumberFontSize",
				typeof(double),
				typeof(AlbumViewer),
				null);
		#endregion

		#region [ PageNumberForeground ]

		public Brush PageNumberForeground
		{
			get { return GetValue(PageNumberForegroundProperty) as Brush; }
			set { SetValue(PageNumberForegroundProperty, value); }
		}

		public static readonly DependencyProperty PageNumberForegroundProperty =
			DependencyProperty.Register(
				"PageNumberForeground",
				typeof(Brush),
				typeof(AlbumViewer),
				null);

		#endregion

		#region [ Constructor ]

		/// <summary>
        /// Initializes a new instance of the <see cref="AlbumViewer"/> class.
        /// </summary>
		public AlbumViewer()
		{
			IsTabStop = true;
			DefaultStyleKey = typeof(AlbumViewer);
			Options.AlbumViewerOptions options = Configuration.Options.AlbumViewer;

			ScrollPathButtonForegroundBrush = options.ScrollButtonForeground;
			ScrollPathButtonForegroundHoverBrush = options.ScrollButtonForegroundHover;
			ScrollPathButtonWidth = options.ScrollButtonWidth;
			ScrollPathButtonHeight = options.ScrollButtonHeight;
			ScrollButtonMargin = options.ScrollButtonMargin;
			LeftScrollPathButtonData = options.LeftScrollButtonData;
			RightScrollPathButtonData = options.RightScrollButtonData;
			PageNumberFontFamily = options.PageNumberFontFamily;
			PageNumberFontSize = options.PageNumberFontSize;
			PageNumberForeground = options.PageNumberForeground;
		}

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
				object pageContainerCanvas = GetTemplateChild(ElementPageContainerCanvasName);
				Debug.Assert(typeof(Canvas).IsInstanceOfType(pageContainerCanvas) || (pageContainerCanvas == null),
					"The template part PageContainerCanvasElement is not an instance of Canvas!");
				
				PageContainerCanvasElement = pageContainerCanvas as Canvas;
				CurrentAlbumPage = new AlbumPage();

				if (PageContainerCanvasElement != null)
				{
					PageContainerCanvasElement.Children.Add(CurrentAlbumPage);
				}

				object pageNumberTextBlock = GetTemplateChild(ElementPageNumberTextBlockName);
				Debug.Assert(typeof(TextBlock).IsInstanceOfType(pageNumberTextBlock) || (pageNumberTextBlock == null),
					"The template part PageContainerCanvasElement is not an instance of TextBlock!");
				
				PageNumberTextBlockElement = pageNumberTextBlock as TextBlock;
				PageNumberTextBlockElement.Visibility = Visibility.Collapsed;
				object scrollLeftPathButton = GetTemplateChild(ElementScrollLeftPathButtonName);
				Debug.Assert(typeof(PathButton).IsInstanceOfType(scrollLeftPathButton) || (scrollLeftPathButton == null),
					"The template part ScrollLeftPathButtonElement is not an instance of PathButton!");
				
				ScrollLeftPathButtonElement = scrollLeftPathButton as PathButton;
				if (ScrollLeftPathButtonElement != null)
				{
					ScrollLeftPathButtonElement.Visibility = Visibility.Collapsed;
					ScrollLeftPathButtonElement.Click += new RoutedEventHandler(ScrollLeftPathButtonElement_Click);
				}

				object scrollRightPathButton = GetTemplateChild(ElementScrollRightPathButtonName);
				Debug.Assert(typeof(PathButton).IsInstanceOfType(scrollRightPathButton) || (scrollRightPathButton == null),
					"The template part ScrollRightPathButtonElement is not an instance of PathButton!");
				
				ScrollRightPathButtonElement = scrollRightPathButton as PathButton;
				if (ScrollRightPathButtonElement != null)
				{
					ScrollRightPathButtonElement.Visibility = Visibility.Collapsed;
					ScrollRightPathButtonElement.Click += new RoutedEventHandler(ScrollRightPathButtonElement_Click);
				}

				Content_Resized(this, EventArgs.Empty);
				Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
				CurrentAlbumPage.PageNumberChanged += new AlbumPage.PageNumberChangedEventHandler(AlbumPage_PageNumberChanged);
			}
		}

        /// <summary>
        /// Handles the Click event of the ScrollLeftPathButtonElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void ScrollLeftPathButtonElement_Click(object sender, RoutedEventArgs e)
		{
			CurrentAlbumPage.SetPage(CurrentAlbumPage.PageIndex - 1);
		}

        /// <summary>
        /// Handles the Click event of the ScrollRightPathButtonElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void ScrollRightPathButtonElement_Click(object sender, RoutedEventArgs e)
		{
			CurrentAlbumPage.SetPage(CurrentAlbumPage.PageIndex + 1);
		}

        /// <summary>
        /// Handles the PageNumberChanged event of the AlbumPage control.
        /// </summary>
		private void AlbumPage_PageNumberChanged()
		{
			if (CurrentAlbumPage != null && PageNumberTextBlockElement != null)
			{
				PageNumberTextBlockElement.Text = "Page " + (CurrentAlbumPage.PageIndex + 1) + " of " + CurrentAlbumPage.PageCount;
			}

			if (CurrentAlbumPage.PageCount < 2)
			{
				PageNumberTextBlockElement.Visibility = Visibility.Collapsed;
				ScrollRightPathButtonElement.Visibility = Visibility.Collapsed;
				ScrollLeftPathButtonElement.Visibility = Visibility.Collapsed;
			}
			else
			{
				PageNumberTextBlockElement.Visibility = Visibility.Visible;
				ScrollRightPathButtonElement.Visibility = Visibility.Visible;
				ScrollLeftPathButtonElement.Visibility = Visibility.Visible;
			}
		}

        /// <summary>
        /// Handles the Resized event of the Content control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
		}
	}
}