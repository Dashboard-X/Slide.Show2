using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the SlidePreview
    /// </summary>
    [TemplatePart(Name = SlidePreview.ElementRootName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = SlidePreview.ElementPreviewGridName, Type = typeof(Grid))]
    [TemplatePart(Name = SlidePreview.ElementPreviewRectangleName, Type = typeof(Rectangle))]
    [TemplatePart(Name = SlidePreview.ElementPreviewImageBrushName, Type = typeof(ImageBrush))]
    public class SlidePreview : Control
    {
        #region [ Template Parts ]

        internal FrameworkElement _elementRoot;
        internal const string ElementRootName = "RootElement";

        internal Grid _elementPreviewGrid;
        internal const string ElementPreviewGridName = "PreviewGridElement";

        internal Rectangle _elementPreviewRectangle;
        internal const string ElementPreviewRectangleName = "PreviewRectangleElement";

        internal ImageBrush _elementPreviewImageBrush;
        internal const string ElementPreviewImageBrushName = "PreviewImageBrushElement";

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
        /// Gets or sets the preview grid element.
        /// </summary>
        /// <value>The preview grid element.</value>
        private Grid PreviewGridElement
        {
            get { return _elementPreviewGrid; }
            set { _elementPreviewGrid = value; }
        }

        /// <summary>
        /// Gets or sets the preview rectangle element.
        /// </summary>
        /// <value>The preview rectangle element.</value>
        private Rectangle PreviewRectangleElement
        {
            get { return _elementPreviewRectangle; }
            set { _elementPreviewRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the preview image brush element.
        /// </summary>
        /// <value>The preview image brush element.</value>
        private ImageBrush PreviewImageBrushElement
        {
            get { return _elementPreviewImageBrush; }
            set
            {
                _elementPreviewImageBrush = value;

                if (_elementPreviewImageBrush != null)
                {
					if (Source != null && Data.GetSourceType(Source) != DataSourceType.Video)
					{
						_elementPreviewImageBrush.ImageSource = new BitmapImage(new Uri(Application.Current.Host.Source, Source));
					}
					else
					{
						BitmapImage blankBmp = new BitmapImage();
						blankBmp.SetSource(Application.GetResourceStream(new Uri("Vertigo.SlideShow;component/Resources/Blank.jpg", UriKind.Relative)).Stream);
						_elementPreviewImageBrush.ImageSource = blankBmp;
					}
                }
            }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        private Uri Source { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        private Size Size { get; set; }

        #endregion

        #region [ Public Properties ]

        /// <summary>
        /// Gets the slide.
        /// </summary>
        /// <value>The slide.</value>
        public Slide Slide { get; private set; }

        #endregion

        #region [ PreviewWidth ]

        /// <summary>
        /// Gets or sets the width of the preview.
        /// </summary>
        /// <value>The width of the preview.</value>
        public double PreviewWidth
        {
            get { return (double)GetValue(PreviewWidthProperty); }
            set { SetValue(PreviewWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the PreviewWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty PreviewWidthProperty =
            DependencyProperty.Register(
                "PreviewWidth",
                typeof(double),
                typeof(SlidePreview),
                null);
        #endregion

        #region [ PreviewHeight ]

        /// <summary>
        /// Gets or sets the height of the preview.
        /// </summary>
        /// <value>The height of the preview.</value>
        public double PreviewHeight
        {
            get { return (double)GetValue(PreviewHeightProperty); }
            set { SetValue(PreviewHeightProperty, value); }
        }

        /// <summary>
        /// Identifies the PreviewHeight dependency property.
        /// </summary>
        public readonly DependencyProperty PreviewHeightProperty =
            DependencyProperty.Register(
                "PreviewHeight",
                typeof(double),
                typeof(SlidePreview),
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
                typeof(SlidePreview),
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
                typeof(SlidePreview),
                null);
        #endregion

        #region [ BorderWidth ]
        /// <summary>
        /// Gets or sets the Y radius of the background rectangle.
        /// </summary>
        /// <value>the Y radius of the background rectangle.</value>
        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the PathHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty BorderWidthProperty =
            DependencyProperty.Register(
                "BorderWidth",
                typeof(double),
                typeof(SlidePreview),
                null);
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidePreview"/> class.
        /// </summary>
        public SlidePreview()
        {
            Options.SlidePreviewOptions options = Configuration.Options.SlidePreview;

            Height = options.Height;
            RadiusX = options.RadiusX;
            RadiusY = options.RadiusY;
            BorderBrush = options.BorderBrush;
            BorderWidth = options.BorderWidth;

            IsTabStop = false;

            DefaultStyleKey = typeof(SlidePreview);
        }

        /// <summary>
        /// Sets the slide.
        /// </summary>
        /// <param name="slide">The slide.</param>
        public void SetSlide(Slide slide)
        {
            this.Slide = slide;
			Source = slide.Preview ?? slide.Thumbnail ?? slide.Source;
        }

        /// <summary>
        /// Sets the size.
        /// </summary>
        /// <param name="size">The size.</param>
        public void SetSize(Size size)
        {
            this.Size = size;
            PreviewHeight = Height;
            PreviewWidth = Height * size.Width / size.Height;
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public void Show()
        {
            if (!Configuration.Options.SlidePreview.Enabled || NavigationTray.IsAlbumView)
            {
                return;
            }

            if (PreviewGridElement != null)
            {
                PreviewGridElement.Visibility = Visibility.Visible;
                VisualStateManager.GoToState(this, "MouseOver", true);
                Canvas.SetLeft(PreviewGridElement, (Size.Width - PreviewWidth) / 2);
                Canvas.SetTop(PreviewGridElement, -Height * Size.Width / Size.Height);
            }
        }

        /// <summary>
        /// Hides this instance.
        /// </summary>
        public void Hide()
        {
            PreviewGridElement.Visibility = Visibility.Collapsed;
            VisualStateManager.GoToState(this, "Normal", true);
        }

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
                object previewGrid = GetTemplateChild(ElementPreviewGridName);
                Debug.Assert(typeof(Grid).IsInstanceOfType(previewGrid) || (previewGrid == null), "The template part PreviewGridElement is not an instance of Canvas!");
                PreviewGridElement = previewGrid as Grid;

                object previewRectangle = GetTemplateChild(ElementPreviewRectangleName);
                Debug.Assert(typeof(Rectangle).IsInstanceOfType(previewRectangle) || (previewRectangle == null), "The template part PreviewRectangleElement is not an instance of Rectangle!");
                PreviewRectangleElement = previewRectangle as Rectangle;

                object previewImageBrush = GetTemplateChild(ElementPreviewImageBrushName);
                Debug.Assert(typeof(ImageBrush).IsInstanceOfType(previewImageBrush) || (previewImageBrush == null), "The template part PreviewImageBrushElement is not an instance of ImageBrush!");
                PreviewImageBrushElement = previewImageBrush as ImageBrush;
               
				if (PreviewImageBrushElement != null)
                {
                    PreviewImageBrushElement.ImageFailed += delegate
					{
						BitmapImage blankBmp = new BitmapImage();
						blankBmp.SetSource(Application.GetResourceStream(new Uri("Vertigo.SlideShow;component/Resources/Blank.jpg", UriKind.Relative)).Stream);
						PreviewImageBrushElement.ImageSource = blankBmp;
					};
                }
            }
        }
    }
}