using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Vertigo.SlideShow.Controls
{
	/// <summary>
	/// Represents the base class for all Viewer controls.
	/// </summary>
	[TemplatePart(Name = ImageViewer.ElementRootName, Type = typeof(FrameworkElement))]
	public abstract class SlideViewer : ButtonBase, IDisposable
	{
		#region [ Constructors ]
		protected SlideViewer(Slide slide)
		{
			this.Slide = slide;

			if (slide != null)
			{
				Source = slide.Source;

				if (slide.Link != null)
				{
					Cursor = Cursors.Hand;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SlideViewer"/> class.
		/// </summary>
		public SlideViewer()
			: this(null)
		{
			IsTabStop = false;
			IsEnabled = true;

			Background = Configuration.Options.General.Background;
		}

		#endregion
		
		#region Template Parts
		/// <summary>
		/// Root template part of the ImageViewer.
		/// </summary>
		/// <remarks>This field is marked internal for unit testing.</remarks>
		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		#endregion

		#region Private Properties
		/// <summary>
		/// Gets or sets the root element.
		/// </summary>
		/// <value>The root element.</value>
		protected FrameworkElement RootElement
		{
			get { return _elementRoot; }
			set { _elementRoot = value; }
		}
		#endregion

		#region [ Source ]
		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public Uri Source
		{
			get { return GetValue(SourceProperty) as Uri; }
			set
			{
				SetValue(SourceProperty, value);
			}
		}

		/// <summary>
		/// Identifies the Source dependency property.
		/// </summary>
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register(
				"Source",
				typeof(Uri),
				typeof(SlideViewer),
				new PropertyMetadata(OnSourcePropertyChanged));

		/// <summary>
		/// SourceProperty property changed handler.
		/// </summary>
		/// <param name="d">SlideViewer that changed its Source.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SlideViewer source = d as SlideViewer;
			Debug.Assert(source != null,
				"The source is not an instance of SlideViewer!");

            if (e.NewValue != e.OldValue)
            {
                source.OnSourceChanged();
            }
		}
		#endregion

		#region [ Slide ]
		/// <summary>
		/// Gets the slide.
		/// </summary>
		/// <value>The slide.</value>
		public Slide Slide { get; protected set; }
		#endregion

		#region [ events ]
		/// <summary>
		/// Occurs when [source changed].
		/// </summary>
		public event RoutedEventHandler SourceChanged;

		/// <summary>
		/// Occurs when [media completed].
		/// </summary>
		public event RoutedEventHandler MediaCompleted;

		#endregion

		/// <summary>
		/// Raises the <see cref="E:SourceChanged"/> event.
		/// </summary>
		protected virtual void OnSourceChanged()
		{
			RoutedEventHandler handler = SourceChanged;
			if (handler != null)
			{
				handler(this, new RoutedEventArgs());
			}
		}

		/// <summary>
		/// Raises the <see cref="E:MediaCompleted"/> event.
		/// </summary>
		protected virtual void OnMediaCompleted()
		{
			RoutedEventHandler handler = MediaCompleted;
			if (handler != null)
			{
				handler(this, new RoutedEventArgs());
			}
		}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Application.Current.Host.Content.Resized += ContentResized;
            Application.Current.Host.Content.FullScreenChanged += ContentResized;
        }

        private void ContentResized(object sender, EventArgs e)
        {
            OnContentResized();
        }

        /// <summary>
        /// Called when [content resized].
        /// </summary>
        protected virtual void OnContentResized()
        {
            OnContentResized(
                Application.Current.Host.Content.ActualWidth,
                Application.Current.Host.Content.ActualHeight);
        }

        /// <summary>
        /// Called when [content resized].
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected virtual void OnContentResized(double width, double height)
        {
            Width = width;
            Height = height;
        }

		/// <summary>
		/// Creates the desired viewer based on the source type.
		/// </summary>
		/// <param name="slide">The slide.</param>
		/// <returns>A slide viewer</returns>
		public static SlideViewer Create(Slide slide)
		{
			if (slide == null)
			{
				throw new ArgumentNullException("slide");
			}

			switch (Data.GetSourceType(slide.Source))
			{
				case DataSourceType.Image:
					return new ImageViewer(slide);

				case DataSourceType.Video:
					return new VideoViewer(slide);

				default:
                    return new ImageViewer(slide);
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
            Application.Current.Host.Content.Resized -= ContentResized;
            Application.Current.Host.Content.FullScreenChanged -= ContentResized;
		}
	}
}