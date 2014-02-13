using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Vertigo.SlideShow.Controls
{
	/// <summary>
	/// Represents an ImageViewer control, which reacts to the SourceChanged event.
	/// </summary>
	[TemplatePart(Name = ImageViewer.ElementImageName, Type = typeof(Image))]
	public class ImageViewer : SlideViewer
	{
		#region [ Constructors ]

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageViewer"/> class.
		/// </summary>
		public ImageViewer() : this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageViewer"/> class.
		/// </summary>
		/// <param name="slide">The slide</param>
		public ImageViewer(Slide slide) : base(slide)
		{
			IsTabStop = false;
			DefaultStyleKey = typeof(ImageViewer);
		}

		#endregion

		#region [ Template Parts ]

		/// <summary>
		/// Image template part of the ImageViewer.
		/// </summary>
		/// <remarks>This field is marked internal for unit testing.</remarks>
		internal Image _elementImage;
		internal const string ElementImageName = "ImageElement";

		#endregion

		#region [ Private Properties ]

		/// <summary>
		/// Gets or sets the image element.
		/// </summary>
		/// <value>The image element.</value>
		private Image ImageElement
		{
			get { return _elementImage; }
			set
			{
				_elementImage = value;

				// if specified in XAML, the property gets set before ApplyConfiguration, so apply the property
				if (_elementImage != null)
				{
					OnSourceChanged();
				}
			}
		}

		#endregion

		/// <summary>
		/// Apply a template to the ImageViewer.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Get the elements
			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null), "The template part RootElement is not an instance of FrameworkElement!");

			RootElement = root as FrameworkElement;

			object image = GetTemplateChild(ElementImageName);
			Debug.Assert(typeof(Image).IsInstanceOfType(image) || (image == null), "The template part ImageElement is not an instance of Image!");

			ImageElement = image as Image;

			// Set the dimensions of the image using the global style
			if (ImageElement != null)
			{
				ImageElement.ImageFailed += delegate
				{
					BitmapImage blankBmp = new BitmapImage();
					blankBmp.SetSource(Application.GetResourceStream(new Uri("Vertigo.SlideShow;component/Resources/Blank.jpg", UriKind.Relative)).Stream);
					ImageElement.Source = blankBmp;
				};

				ImageElement.Stretch = Configuration.Options.SlideViewer.Stretch;
				OnContentResized();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:SourceChanged"/> event.
		/// </summary>
		protected override void OnSourceChanged()
		{
			if (ImageElement != null)
			{
				if (Source != null)
				{
					ImageElement.Source = new BitmapImage(new Uri(Application.Current.Host.Source, Source));
				}
				else
				{
					ImageElement.Source = null;
				}

				OnMediaCompleted();
			}

			Page page = App.Current.RootVisual as Page;
			page.navigationTray.SaveHyperlinkButtonElement.NavigateUri = new Uri(Configuration.Options.ToggleSaveButton.DownloadImageHandler + DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides[DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.ToSlideIndex].Source.OriginalString, UriKind.Absolute);

			base.OnSourceChanged();
		}
	}
}