using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the CircleProgressIndicator.
	/// </summary>
	[TemplatePart(Name = CircleProgressIndicator.ElementEllipseName, Type = typeof(Ellipse))]
	[TemplatePart(Name = CircleProgressIndicator.ElementBackgroundEllipseName, Type = typeof(Ellipse))]
	public class CircleProgressIndicator : DownloadProgressIndicator
	{
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="CircleProgressIndicator"/> class.
		/// </summary>
		public CircleProgressIndicator()
		{
			Options.LoadingProgressIndicatorOptions options = Configuration.Options.LoadingProgressIndicator;
			EllipseBrush = options.Background;
			BackgroundEllipseBrush = options.Foreground;
			Height = options.Height;
			Width = options.Width;
			DefaultStyleKey = typeof(CircleProgressIndicator);
		}

		#endregion

		#region [Template Parts]

		internal Ellipse _elementBackgroundEllipse;
		internal const string ElementBackgroundEllipseName = "BackgroundEllipseElement";

		internal Ellipse _elementEllipse;
		internal const string ElementEllipseName = "EllipseElement";

		#endregion

		#region [ Private Properties ]

        /// <summary>
		/// Gets or sets the background Ellipse element.
        /// </summary>
		/// <value>The background Ellipse element.</value>
		private Ellipse BackgroundEllipseElement
		{
			get { return _elementBackgroundEllipse; }
			set { _elementBackgroundEllipse = value; }
		}

        /// <summary>
		/// Gets or sets the Ellipse element.
        /// </summary>
		/// <value>The Ellipse element.</value>
		private Ellipse EllipseElement
		{
			get { return _elementEllipse; }
			set { _elementEllipse = value; }
		}

		#endregion

		#region [ Dependency Properties ]

        /// <summary>
		/// Identifies the EllipseBrushProperty dependency property.
        /// </summary>
		public static readonly DependencyProperty EllipseBrushProperty =
			DependencyProperty.Register("EllipseBrush", typeof(Brush), typeof(CircleProgressIndicator), null);

        /// <summary>
		/// Gets or sets the Ellipse brush.
        /// </summary>
		/// <value>The Ellipse brush.</value>
		public Brush EllipseBrush
		{
			get { return (Brush)GetValue(EllipseBrushProperty); }
			set { SetValue(EllipseBrushProperty, value); }
		}

        /// <summary>
		/// Identifies the EllipseBrushProperty dependency property.
        /// </summary>
		public static readonly DependencyProperty BackgroundEllipseBrushProperty =
			DependencyProperty.Register("BackgroundEllipseBrush", typeof(Brush), typeof(CircleProgressIndicator), null);

        /// <summary>
		/// Gets or sets the background Ellipse brush.
        /// </summary>
		/// <value>The background Ellipse brush.</value>
		public Brush BackgroundEllipseBrush
		{
			get { return (Brush)GetValue(BackgroundEllipseBrushProperty); }
			set { SetValue(BackgroundEllipseBrushProperty, value); }
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
				object backgroundEllipse = GetTemplateChild(ElementBackgroundEllipseName);
				Debug.Assert(typeof(Ellipse).IsInstanceOfType(backgroundEllipse) || (backgroundEllipse == null),
					"The template part BackgroundEllipseElement is not an instance of Ellipse!");
				
				BackgroundEllipseElement = backgroundEllipse as Ellipse;
				object ellipse = GetTemplateChild(ElementEllipseName);
				Debug.Assert(typeof(Ellipse).IsInstanceOfType(ellipse) || (root == null),
					"The template part EllipseElement is not an instance of Ellipse!");
				
				EllipseElement = ellipse as Ellipse;
			}
		}

        /// <summary>
        /// Updates the progress.
        /// </summary>
        /// <param name="Percent">The percent.</param>
		public override void UpdateProgress(int Percent)
		{
			try
			{
				Progress = Percent / 100.0;
				if (EllipseElement == null)
				{
					return;
				}

				double centerX = Canvas.GetLeft(EllipseElement);
				double centerY = Canvas.GetTop(EllipseElement);
				double radius = EllipseElement.ActualWidth / 2;
				string size = string.Format(CultureInfo.InvariantCulture, "{0},{1}", radius, radius);
				double angle = 2 * Math.PI * Progress;
				double direction = (Progress > 0.5) ? 1 : 0;
				string startPoint = string.Format(CultureInfo.InvariantCulture, "{0},{1}", centerX, centerY - radius);
				string endPoint = (Progress < 1) ? string.Format(CultureInfo.InvariantCulture, "{0},{1}", centerX + Math.Sin(angle) * radius, centerY + Math.Cos(angle) * -radius) : string.Format(CultureInfo.InvariantCulture, "{0},{1}", centerX, centerY - radius);
				string pathString = string.Format(CultureInfo.InvariantCulture, "M {0} A {1} {2} {3} 1 {4} L {5},{6}", startPoint, size, angle, direction, endPoint, centerX, centerY);
				PathGeometry path = PathGeometryParser.Parse(pathString);
				((Canvas)GetTemplateChild("CanvasElement")).Children.Add(new Path() { Data = path, Fill = new SolidColorBrush(Colors.Red), Stroke = new SolidColorBrush(Colors.Blue) });
			}
			catch (Exception ex)
			{
				System.Windows.Browser.HtmlPage.Window.Alert(ex.Message);
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