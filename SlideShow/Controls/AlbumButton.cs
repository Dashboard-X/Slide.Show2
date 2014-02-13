using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents an AlbumButton
    /// </summary>
	[TemplatePart(Name = AlbumButton.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = AlbumButton.ElementBackgroundName, Type = typeof(Rectangle))]
	[TemplatePart(Name = AlbumButton.ElementBackgroundHoverName, Type = typeof(Rectangle))]
	[TemplatePart(Name = AlbumButton.ElementImageBackgroundName, Type = typeof(Rectangle))]
	[TemplatePart(Name = AlbumButton.ElementImageName, Type = typeof(Rectangle))]
	[TemplatePart(Name = AlbumButton.ElementImageBrushName, Type = typeof(ImageBrush))]
	[TemplatePart(Name = AlbumButton.ElementTitleName, Type = typeof(TextBlock))]
	[TemplatePart(Name = AlbumButton.ElementDescriptionName, Type = typeof(TextBlock))]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
	public class AlbumButton : ButtonBase
	{
		#region [ Template Parts ]
		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal Rectangle _elementBackground;
		internal const string ElementBackgroundName = "BackgroundElement";

		internal Rectangle _elementBackgroundHover;
		internal const string ElementBackgroundHoverName = "BackgroundHoverElement";

		internal Rectangle _elementImageBackground;
		internal const string ElementImageBackgroundName = "ImageBackgroundElement";

		internal Rectangle _elementImage;
		internal const string ElementImageName = "ImageElement";

		internal ImageBrush _elementImageBrush;
		internal const string ElementImageBrushName = "ImageBrushElement";

		internal TextBlock _elementTitle;
		internal const string ElementTitleName = "TitleElement";

		internal TextBlock _elementDescription;
		internal const string ElementDescriptionName = "DescriptionElement";
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

		private Rectangle BackgroundHoverElement
		{
			get { return _elementBackgroundHover; }
			set { _elementBackgroundHover = value; }
		}

        /// <summary>
        /// Gets or sets the image background element.
        /// </summary>
        /// <value>The image background element.</value>
		private Rectangle ImageBackgroundElement
		{
			get { return _elementImageBackground; }
			set { _elementImageBackground = value; }
		}

        /// <summary>
        /// Gets or sets the image element.
        /// </summary>
        /// <value>The image element.</value>
		private Rectangle ImageElement
		{
			get { return _elementImage; }
			set { _elementImage = value; }
		}

        /// <summary>
        /// Gets or sets the image brush element.
        /// </summary>
        /// <value>The image brush element.</value>
		private ImageBrush ImageBrushElement
		{
			get { return _elementImageBrush; }
			set
			{
				_elementImageBrush = value;

				// if specified in XAML, the property gets set before ApplyConfiguration, so apply the property
				if (_elementImageBrush != null && ImageSource != null)
				{
					Uri source = new Uri(Application.Current.Host.Source, ImageSource);
					_elementImageBrush.ImageSource = new BitmapImage(source);
				}
			}
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
				if (_elementTitle != null && !string.IsNullOrEmpty(Title))
				{
					_elementTitle.Text = Title;
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
				if (_elementDescription != null && !string.IsNullOrEmpty(Description))
				{
					_elementDescription.Text = Description;
				}
			}
		}
		#endregion

		#region [ Public Properties ]

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
		public int Index { get; set; }

		#endregion

		#region [ Title ]
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
		public string Title
		{
			get { return GetValue(TitleProperty) as string; }
			set
			{
				SetValue(TitleProperty, value);

                // if specified in XAML, the property gets set before ApplyConfiguration
                // so apply the property
				if (TitleElement != null)
				{
					TitleElement.Text = Title;
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
				null);
		#endregion

		#region [ Description ]
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
		public string Description
		{
			get { return GetValue(DescriptionProperty) as string; }
			set
			{
				SetValue(DescriptionProperty, value);

				if (DescriptionElement != null)
				{
					DescriptionElement.Text = Description;
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
				null);
		#endregion

		#region [ BackgroundBrush ]
		public Brush BackgroundBrush
		{
			get { return GetValue(BackgroundBrushProperty) as Brush; }
			set { SetValue(BackgroundBrushProperty, value); }
		}

		public static readonly DependencyProperty BackgroundBrushProperty =
			DependencyProperty.Register(
				"BackgroundBrush",
				typeof(Brush),
				typeof(AlbumButton),
				null);

		#endregion

		#region [ BackgroundHoverBrush ]
		public Brush BackgroundHoverBrush
		{
			get { return GetValue(BackgroundHoverBrushProperty) as Brush; }
			set { SetValue(BackgroundHoverBrushProperty, value); }
		}

		public static readonly DependencyProperty BackgroundHoverBrushProperty =
			DependencyProperty.Register(
				"BackgroundHoverBrush",
				typeof(Brush),
				typeof(AlbumButton),
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
				typeof(AlbumButton),
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
				typeof(AlbumButton),
				null);
		#endregion

		#region [ ImageSource ]
        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
		public Uri ImageSource
		{
			get { return GetValue(SourceProperty) as Uri; }
			set
			{
				SetValue(SourceProperty, value);

				if (ImageBrushElement != null)
				{
					ImageBrushElement.ImageSource = (value != null ? new BitmapImage(value) : null);
				}
			}
		}

        /// <summary>
        /// Identifies the ImageSource dependency property.
        /// </summary>
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register(
				"ImageSource",
				typeof(Uri),
				typeof(AlbumButton),
				null);
		#endregion

		#region [ ThumbnailWidth ]
		public double ThumbnailWidth
		{
			get { return (double)GetValue(ThumbnailWidthProperty); }
			set { SetValue(ThumbnailWidthProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailWidthProperty =
			DependencyProperty.Register(
				"ThumbnailWidth",
				typeof(double),
				typeof(AlbumButton),
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
				typeof(AlbumButton),
				null);

		#endregion

		#region [ ThumbnailRadiusX ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double ThumbnailRadiusX
		{
			get { return (double)GetValue(ThumbnailRadiusXProperty); }
			set { SetValue(ThumbnailRadiusXProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty ThumbnailRadiusXProperty =
			DependencyProperty.Register(
				"ThumbnailRadiusX",
				typeof(double),
				typeof(AlbumButton),
				null);
		#endregion

		#region [ ThumbnailRadiusY ]
		/// <summary>
		/// Gets or sets the Y radius of the background rectangle.
		/// </summary>
		/// <value>the Y radius of the background rectangle.</value>
		public double ThumbnailRadiusY
		{
			get { return (double)GetValue(ThumbnailRadiusYProperty); }
			set { SetValue(ThumbnailRadiusYProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty ThumbnailRadiusYProperty =
			DependencyProperty.Register(
				"ThumbnailRadiusY",
				typeof(double),
				typeof(AlbumButton),
				null);
		#endregion

		#region [ ThumbnailMargin ]

		public Thickness ThumbnailMargin
		{
			get { return (Thickness)GetValue(ThumbnailMarginProperty); }
			set { SetValue(ThumbnailMarginProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailMarginProperty =
			DependencyProperty.Register(
				"ThumbnailMargin",
				typeof(Thickness),
				typeof(AlbumButton),
				null);
		#endregion

		#region [ ThumbnailBorderStroke ]
		public Brush ThumbnailBorderStroke
		{
			get { return GetValue(ThumbnailBorderStrokeProperty) as Brush; }
			set { SetValue(ThumbnailBorderStrokeProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailBorderStrokeProperty =
			DependencyProperty.Register(
				"ThumbnailBorderStroke",
				typeof(Brush),
				typeof(AlbumButton),
				null);

		#endregion

		#region [ ThumbnailBorderThickness ]

		public Thickness ThumbnailBorderThickness
		{
			get { return (Thickness)GetValue(ThumbnailBorderThicknessProperty); }
			set { SetValue(ThumbnailBorderThicknessProperty, value); }
		}

		public static readonly DependencyProperty ThumbnailBorderThicknessProperty =
			DependencyProperty.Register(
				"ThumbnailBorderThickness",
				typeof(Thickness),
				typeof(AlbumButton),
				null);
		#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumButton"/> class.
        /// </summary>
		public AlbumButton()
		{
			Options.AlbumButtonOptions options = Configuration.Options.AlbumButton;

			Width = options.Width;
			Height = options.Height;

			TitleFontFamily = options.TitleFontFamily;
			TitleFontSize = options.TitleFontSize;
			TitleForegroundBrush = options.TitleForeground;
			TitleMargin = options.TitleMargin;

			DescriptionFontFamily = options.DescriptionFontFamily;
			DescriptionFontSize = options.DescriptionFontSize;
			DescriptionForegroundBrush = options.DescriptionForeground;
			DescriptionMargin = options.DescriptionMargin;

			BackgroundBrush = options.Background;
			BackgroundHoverBrush = options.BackgroundHover;
			BackgroundRadiusX = options.BackgroundRadiusX;
			BackgroundRadiusY = options.BackgroundRadiusY;

			ThumbnailWidth = options.ThumbnailWidth;
			ThumbnailHeight = options.ThumbnailHeight;
			ThumbnailRadiusX = options.ThumbnailRadiusX;
			ThumbnailRadiusY = options.ThumbnailRadiusY;
			ThumbnailMargin = options.ThumbnailMargin;
			ThumbnailBorderStroke = options.ThumbnailBorderStroke;
			ThumbnailBorderThickness = options.ThumbnailBorderThickness;

			DefaultStyleKey = typeof(AlbumButton);
			IsEnabled = true;
		}

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null), "The template part RootElement is not an instance of FrameworkElement!");
			RootElement = root as FrameworkElement;

			object background = GetTemplateChild(ElementBackgroundName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(background) || (background == null), "The template part BackgroundElement is not an instance of Rectangle!");
			BackgroundElement = background as Rectangle;

			object backgroundHover = GetTemplateChild(ElementBackgroundHoverName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(backgroundHover) || (backgroundHover == null), "The template part BackgroundHoverElement is not an instance of Rectangle!");
			BackgroundHoverElement = backgroundHover as Rectangle;

			object imageBackground = GetTemplateChild(ElementImageBackgroundName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(imageBackground) || (imageBackground == null), "The template part ImageBackgroundElement is not an instance of Rectangle!");
			ImageBackgroundElement = imageBackground as Rectangle;

			object image = GetTemplateChild(ElementImageName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(image) || (image == null), "The template part ImageElement is not an instance of Rectangle!");
			ImageElement = image as Rectangle;

			object imageBrush = GetTemplateChild(ElementImageBrushName);
			Debug.Assert(typeof(ImageBrush).IsInstanceOfType(imageBrush) || (imageBrush == null), "The template part ImageBrushElement is not an instance of ImageBrush!");
			ImageBrushElement = imageBrush as ImageBrush;

			object title = GetTemplateChild(ElementTitleName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(title) || (title == null), "The template part TitleElement is not an instance of TextBlock!");
			TitleElement = title as TextBlock;

			object desc = GetTemplateChild(ElementDescriptionName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(desc) || (desc == null), "The template part DescriptionElement is not an instance of TextBlock!");
			DescriptionElement = desc as TextBlock;

            if (ImageBrushElement != null)
            {
                ImageBrushElement.ImageFailed += delegate
				{
					BitmapImage blankBmp = new BitmapImage();
					blankBmp.SetSource(Application.GetResourceStream(new Uri("Vertigo.SlideShow;component/Resources/Blank.jpg", UriKind.Relative)).Stream);
					ImageBrushElement.ImageSource = blankBmp;
				};
            }
		}

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.MouseEnter"/> event that occurs when the mouse enters this control.
        /// </summary>
        /// <param name="e">The event data.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="e"/> is null.</exception>
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);

			VisualStateManager.GoToState(this, "MouseOver", true);
		}

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeave"/> routed event that occurs when the mouse leaves an element.
        /// </summary>
        /// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseLeave"/> event.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="e"/> is null.</exception>
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);

			VisualStateManager.GoToState(this, "Normal", true);
		}
	}
}