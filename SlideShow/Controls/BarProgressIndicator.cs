using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the BarProgressIndicator.
    /// </summary>
	[TemplatePart(Name = BarProgressIndicator.ElementBackgroundBarName, Type = typeof(Rectangle))]
	[TemplatePart(Name = BarProgressIndicator.ElementLoadingBarName, Type = typeof(Rectangle))]
	public class BarProgressIndicator : DownloadProgressIndicator
	{
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="BarProgressIndicator"/> class.
		/// </summary>
		public BarProgressIndicator()
		{
			Options.LoadingProgressIndicatorOptions options = Configuration.Options.LoadingProgressIndicator;
			LoadingBarBrush = options.Foreground;
			BackgroundBarBrush = options.Background;
			Height = options.Height;
			Width = options.Width;
			DefaultStyleKey = typeof(BarProgressIndicator);
		}
		#endregion
		
		#region [Template Parts]

		internal Rectangle _elementBackgroundBar;
		internal const string ElementBackgroundBarName = "BackgroundBarElement";

		internal Rectangle _elementLoadingBar;
		internal const string ElementLoadingBarName = "LoadingBarElement";

		#endregion

		#region [ Private Properties ]

        /// <summary>
        /// Gets or sets the background bar element.
        /// </summary>
        /// <value>The background bar element.</value>
		private Rectangle BackgroundBarElement
		{
			get { return _elementBackgroundBar; }
			set { _elementBackgroundBar = value; }
		}

        /// <summary>
        /// Gets or sets the loading bar element.
        /// </summary>
        /// <value>The loading bar element.</value>
		private Rectangle LoadingBarElement
		{
			get { return _elementLoadingBar; }
			set { _elementLoadingBar = value; }
		}

		#endregion

		#region [ Dependency Properties ]

        /// <summary>
        /// Identifies the LoadingBarBrush dependency property.
        /// </summary>
		public static readonly DependencyProperty LoadingBarBrushProperty =
			DependencyProperty.Register("LoadingBarBrush", typeof(Brush), typeof(BarProgressIndicator), null);

        /// <summary>
        /// Gets or sets the loading bar brush.
        /// </summary>
        /// <value>The loading bar brush.</value>
		public Brush LoadingBarBrush
		{
			get { return (Brush)GetValue(LoadingBarBrushProperty); }
			set { SetValue(LoadingBarBrushProperty, value); }
		}

        /// <summary>
        /// Identifies the BackgroundBarBrush dependency property.
        /// </summary>
		public static readonly DependencyProperty BackgroundBarBrushProperty =
			DependencyProperty.Register("BackgroundBarBrush", typeof(Brush), typeof(BarProgressIndicator), null);

        /// <summary>
        /// Gets or sets the background bar brush.
        /// </summary>
        /// <value>The background bar brush.</value>
		public Brush BackgroundBarBrush
		{
			get { return (Brush)GetValue(BackgroundBarBrushProperty); }
			set { SetValue(BackgroundBarBrushProperty, value); }
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
			if (root != null)
			{
				Content_Resized(this, EventArgs.Empty);
				Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
				object backgroundBar = GetTemplateChild(ElementBackgroundBarName);
				Debug.Assert(typeof(Rectangle).IsInstanceOfType(backgroundBar) || (backgroundBar == null),
					"The template part BackgroundBarElement is not an instance of Rectangle!");
				
				BackgroundBarElement = backgroundBar as Rectangle;
				object loadingBar = GetTemplateChild(ElementLoadingBarName);
				Debug.Assert(typeof(Rectangle).IsInstanceOfType(loadingBar) || (root == null),
					"The template part LoadingBarElement is not an instance of Rectangle!");
				
				LoadingBarElement = loadingBar as Rectangle;
			}
		}

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="Percent">The percent.</param>
		public override void UpdateProgress(int Percent)
		{
			Progress = Percent / 100;
			if (LoadingBarElement != null)
			{
				LoadingBarElement.Width = Width * Progress;
			}
		}

		/// <summary>
		/// Handles the Resized event of the Content control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			if (RootElement != null)
			{
				RootElement.Width = Application.Current.Host.Content.ActualWidth;
				RootElement.Height = Application.Current.Host.Content.ActualHeight;
			}
		}
	}
}