using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Vertigo.SlideShow.Controls
{
	/// <summary>
	/// Represents a VideoViewer control, which reacts to the SourceChanged event.
	/// </summary>
	[TemplatePart(Name = VideoViewer.ElementVideoName, Type = typeof(MediaElement))]
	[TemplatePart(Name = VideoViewer.ElementVideoTrayName, Type = typeof(VideoTray))]
	public class VideoViewer : SlideViewer
	{
        #region [ Constructors ]
		/// <summary>
		/// Initializes a new instance of the <see cref="VideoViewer"/> class.
		/// </summary>
		public VideoViewer()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="VideoViewer"/> class.
		/// </summary>
		/// <param name="slide">The slide</param>
		public VideoViewer(Slide slide)
			: base(slide)
		{
			IsTabStop = false;
			DefaultStyleKey = typeof(VideoViewer);
			timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
			timer.Tick += new EventHandler(timer_Tick);
		}
		#endregion
		
		private DispatcherTimer timer = new DispatcherTimer();

		#region Template Parts

		/// <summary>
		/// Video template part of the VideoViewer.
		/// </summary>
		/// <remarks>This field is marked internal for unit testing.</remarks>
		internal MediaElement _elementVideo;
		internal const string ElementVideoName = "VideoElement";

		internal const string ElementVideoTrayName = "VideoTrayElement";
		internal VideoTray VideoTrayElement { get; set; }
		#endregion

		#region Private Properties
        /// <summary>
        /// Gets or sets the video element.
        /// </summary>
        /// <value>The video element.</value>
		private MediaElement VideoElement
		{
			get { return _elementVideo; }
			set
			{
				_elementVideo = value;

                // if specified in XAML, the property gets set before ApplyConfiguration
                // so apply the property
				if (_elementVideo != null && Source != null)
				{
					// Subscribe to the MediaEnded event to know when to trigger the MediaCompleted event.
                    OnSourceChanged();
				}
			}
		}
		#endregion

		/// <summary>
		/// Apply a template to the VideoViewer
		/// </summary>
		public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get the elements
            object root = GetTemplateChild(ElementRootName);
            Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null),
                "The template part RootElement is not an instance of FrameworkElement!");

            RootElement = root as FrameworkElement;

            object video = GetTemplateChild(ElementVideoName);
            Debug.Assert(typeof(MediaElement).IsInstanceOfType(video) || (video == null),
                "The template part VideoElement is not an instance of MediaElement!");

            VideoElement = video as MediaElement;

            if (VideoElement != null)
            {
				VideoElement.Stretch = Configuration.Options.SlideViewer.Stretch;
                VideoElement.AutoPlay = Configuration.Options.VideoViewer.AutoPlay;
            }

            object tray = GetTemplateChild(ElementVideoTrayName);
            Debug.Assert(typeof(VideoTray).IsInstanceOfType(tray) || (tray == null),
                "The template part VideoTrayElement is not an instance of VideoTray!");

            VideoTrayElement = tray as VideoTray;

            if (VideoElement != null)
            {
                VideoElement.MediaFailed += OnMediaFailed;
                VideoElement.MediaOpened += new RoutedEventHandler(VideoElement_MediaOpened);
                VideoElement.MediaEnded += new RoutedEventHandler(VideoElement_MediaEnded);
                OnContentResized();
            }

            if (VideoTrayElement != null)
            {
                VideoTrayElement.Played += new EventHandler(VideoTrayElement_Played);
                VideoTrayElement.Paused += new EventHandler(VideoTrayElement_Paused);
                VideoTrayElement.PositionChanged += new EventHandler(VideoTrayElement_PositionChanged);
                VideoTrayElement.VolumeChanged += new EventHandler(VideoTrayElement_VolumeChanged);
            }

            Application.Current.RootVisual.MouseEnter += new MouseEventHandler(Element_MouseMove);
            Application.Current.RootVisual.MouseLeave += new MouseEventHandler(Element_MouseLeave);
        }

		private void Element_MouseLeave(object sender, MouseEventArgs e)
		{
			VideoTrayElement.Hide();
		}

		private void Element_MouseMove(object sender, MouseEventArgs e)
		{
			VideoTrayElement.Show();
		}

        private void OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // "trap" the exception so it's not presented to the user
            Debug.WriteLine(e.ErrorException.ToString());
        }

		private void VideoElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			VideoElement.Stop();
			VideoTrayElement.Stop();
			OnMediaCompleted();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (VideoElement != null)
				VideoTrayElement.Position = VideoElement.Position;
		}

		private void VideoElement_MediaOpened(object sender, RoutedEventArgs e)
		{
			VideoTrayElement.Duration = VideoElement.NaturalDuration.TimeSpan;
			VideoTrayElement.Volume = VideoElement.Volume;
			timer.Start();
			VideoTrayElement.Show();
		}

		private void VideoTrayElement_VolumeChanged(object sender, EventArgs e)
		{
			VideoElement.Volume = VideoTrayElement.Volume;
		}

		private void VideoTrayElement_PositionChanged(object sender, EventArgs e)
		{
			VideoElement.Position = VideoTrayElement.Position;
		}

		private void VideoTrayElement_Paused(object sender, EventArgs e)
		{
			VideoElement.Pause();
		}

		private void VideoTrayElement_Played(object sender, EventArgs e)
		{
			VideoElement.Play();
		}

        /// <summary>
        /// Raises the <see cref="E:SourceChanged"/> event.
        /// </summary>
		protected override void OnSourceChanged()
		{
			if (VideoElement != null)
			{
				VideoElement.Source = new Uri(Application.Current.Host.Source, Source);
			}

			base.OnSourceChanged();
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (VideoElement != null)
			{
				VideoElement.Stop();
				VideoElement = null;
			}

			base.Dispose(disposing);
		}
	}
}