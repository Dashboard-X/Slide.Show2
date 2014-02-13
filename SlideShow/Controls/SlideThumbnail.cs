using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Vertigo.SlideShow.Controls
{
	/// <summary>
	/// Represents the SlideThumbnail
	/// </summary>
	[TemplatePart(Name = SlideThumbnail.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = SlideThumbnail.ElementSlidePreviewName, Type = typeof(SlidePreview))]
	[TemplatePart(Name = SlideThumbnail.ElementThumbnailImageName, Type = typeof(Image))]
	[TemplatePart(Name = SlideThumbnail.ElementThumbnailImageBorderName, Type = typeof(Border))]
	public class SlideThumbnail : ButtonBase
	{
		#region [ Template Parts ]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal SlidePreview _elementSlidePreview;
		internal const string ElementSlidePreviewName = "SlidePreviewElement";

		internal Image _elementThumbnailImage;
		internal const string ElementThumbnailImageName = "ThumbnailImageElement";

		internal Border _elementThumbnailImageBorder;
		internal const string ElementThumbnailImageBorderName = "ThumbnailImageBorderElement";

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
		/// Gets or sets the slide preview element.
		/// </summary>
		/// <value>The slide preview element.</value>
		private SlidePreview SlidePreviewElement
		{
			get { return _elementSlidePreview; }
			set { _elementSlidePreview = value; }
		}

		/// <summary>
		/// Gets or sets the thumbnail image element.
		/// </summary>
		/// <value>The thumbnail image element.</value>
		private Image ThumbnailImageElement
		{
			get { return _elementThumbnailImage; }
			set
			{
				_elementThumbnailImage = value;

				if (_elementThumbnailImage != null)
				{
					_elementThumbnailImage.Stretch = Stretch.Fill;
					_elementThumbnailImage.Width = Height;
					_elementThumbnailImage.Height = Height;

					if (Source != null && Data.GetSourceType(Source) != DataSourceType.Video)
					{
						_elementThumbnailImage.Source = new BitmapImage(new Uri(Application.Current.Host.Source, Source));
					}
					else
					{
						BitmapImage blankBmp = new BitmapImage();
						blankBmp.SetSource(Application.GetResourceStream(new Uri("Vertigo.SlideShow;component/Resources/Blank.jpg", UriKind.Relative)).Stream);
						_elementThumbnailImage.Source = blankBmp;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the thumbnail image border element.
		/// </summary>
		/// <value>The thumbnail image border element.</value>
		private Border ThumbnailImageBorderElement
		{
			get { return _elementThumbnailImageBorder; }
			set { _elementThumbnailImageBorder = value; }
		}

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		private Uri Source { get; set; }

		#endregion

		#region [ Public Properties ]

		/// <summary>
		/// Gets or sets the slide.
		/// </summary>
		/// <value>The slide.</value>
		public Slide Slide { get; set; }

		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="SlideThumbnail"/> class.
		/// </summary>
		/// <param name="slide">The slide.</param>
		public SlideThumbnail(Slide slide)
		{
			BorderThickness = Configuration.Options.SlideThumbnail.BorderThickness;
			this.Slide = slide;
			IsTabStop = false;

			Source = slide.Thumbnail ?? slide.Preview ?? slide.Source;

			DefaultStyleKey = typeof(SlideThumbnail);
			Options.SlideThumbnailOptions options = Configuration.Options.SlideThumbnail;

			LayoutUpdated += new EventHandler(SlideThumbnail_LayoutUpdated);

			BorderBrush = options.BorderBrush;
		}

		/// <summary>
		/// Handles the LayoutUpdated event of the SlideThumbnail control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void SlideThumbnail_LayoutUpdated(object sender, EventArgs e)
			{
			Options.SlideThumbnailOptions options = Configuration.Options.SlideThumbnail;

				if (ThumbnailImageElement != null)
				{
					Width = ThumbnailImageElement.ActualHeight + options.BorderThickness.Left + options.BorderThickness.Right;
					Height = ThumbnailImageElement.ActualHeight + options.BorderThickness.Top + options.BorderThickness.Bottom;
					
					if (SlidePreviewElement != null)
					{
						SlidePreviewElement.SetSize(new Size(ThumbnailImageElement.ActualWidth, ThumbnailImageElement.ActualHeight));
					}
				}

			LayoutUpdated -= SlideThumbnail_LayoutUpdated;
		}

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Get the elements
			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null), "The template part RootElement is not an instance of FrameworkElement!");
			RootElement = root as FrameworkElement;

			if (RootElement != null)
			{
				object slidePreview = GetTemplateChild(ElementSlidePreviewName);
				Debug.Assert(typeof(SlidePreview).IsInstanceOfType(slidePreview) || (slidePreview == null), "The template part SlidePreviewElement is not an instance of SlidePreview!");
				SlidePreviewElement = slidePreview as SlidePreview;

				if (SlidePreviewElement != null)
				{
					SlidePreviewElement.SetSlide(Slide);
				}

				object thumbnailImage = GetTemplateChild(ElementThumbnailImageName);
				Debug.Assert(typeof(Image).IsInstanceOfType(thumbnailImage) || (thumbnailImage == null), "The template part ThumbnailImageElement is not an instance of Image!");
				ThumbnailImageElement = thumbnailImage as Image;

				object thumbnailImageBorder = GetTemplateChild(ElementThumbnailImageBorderName);
				Debug.Assert(typeof(Border).IsInstanceOfType(thumbnailImageBorder) || (thumbnailImageBorder == null), "The template part ThumbnailImageBorderElement is not an instance of Border!");
				ThumbnailImageBorderElement = thumbnailImageBorder as Border;

				if (ThumbnailImageElement != null)
				{
					ThumbnailImageElement.ImageFailed += delegate
					{
						BitmapImage blankBmp = new BitmapImage();
						blankBmp.SetSource(Application.GetResourceStream(new Uri("Vertigo.SlideShow;component/Resources/Blank.jpg", UriKind.Relative)).Stream);
						ThumbnailImageElement.Source = blankBmp;
					};

					ThumbnailImageElement.MouseEnter += delegate
					{
						if (SlidePreviewElement != null)
						{
							SlidePreviewElement.Show();
						}
					};

					ThumbnailImageElement.MouseLeave += delegate
					{
						if (SlidePreviewElement != null)
						{
							SlidePreviewElement.Hide();
						}
					};
				}
			}
		}
	}
}