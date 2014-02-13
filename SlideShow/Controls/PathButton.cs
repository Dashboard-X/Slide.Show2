using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents a PathButton which uses PathGeometry data for the content.
    /// </summary>
	[TemplatePart(Name = PathButton.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = PathButton.ElementBackgroundName, Type = typeof(Rectangle))]
	[TemplatePart(Name = PathButton.ElementPathName, Type = typeof(Path))]
	[TemplatePart(Name = PathButton.ElementPathHoverName, Type = typeof(Path))]
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
	public class PathButton : ButtonBase
	{
		#region [ Constructor ]

		/// <summary>
		/// Initializes a new instance of the <see cref="PathButton"/> class.
		/// </summary>
		public PathButton()
		{
			DefaultStyleKey = typeof(PathButton);
			IsEnabled = true;
		}

		#endregion
		
		#region [ Template Parts ]
		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal Rectangle _elementBackground;
		internal const string ElementBackgroundName = "BackgroundElement";

		internal Path _elementPath;
		internal const string ElementPathName = "PathElement";

		internal Path _elementPathHover;
		internal const string ElementPathHoverName = "PathHoverElement";
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

        /// <summary>
        /// Gets or sets the path element.
        /// </summary>
        /// <value>The path element.</value>
		private Path PathElement
		{
			get { return _elementPath; }
			set
			{
				_elementPath = value;
 
                // if specified in XAML, the properties gets set before ApplyConfiguration
                // so apply the properties
				if (_elementPath != null)
				{
					_elementPath.Width = PathWidth;
					_elementPath.Height = PathHeight;

					if (!string.IsNullOrEmpty(Data))
					{
						_elementPath.Data = PathGeometryParser.Parse(Data);
					}
				}
			}
		}

		private Path PathHoverElement
		{
			get { return _elementPathHover; }
			set
			{
				_elementPathHover = value;

				if (_elementPathHover != null)
				{
					_elementPathHover.Width = PathWidth;
					_elementPathHover.Height = PathHeight;

					if (!string.IsNullOrEmpty(Data))
					{
						_elementPathHover.Data = PathGeometryParser.Parse(Data);
					}
				}
			}
		}
		#endregion

		#region [ Data ]
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
		public string Data
		{
			get { return (string)GetValue(DataProperty); }
			set
			{
				SetValue(DataProperty, value);

				if (PathElement != null && !string.IsNullOrEmpty(value))
				{
					PathElement.Data = PathGeometryParser.Parse(value);
				}
			}
		}

        /// <summary>
        /// Identifies the Data dependency property.
        /// </summary>
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register(
				"Data",
				typeof(string),
				typeof(PathButton),
				null);
		#endregion

		#region [ PathWidth ]
        /// <summary>
        /// Gets or sets the width of the path.
        /// </summary>
        /// <value>The width of the path.</value>
		public double PathWidth
		{
			get { return (double)GetValue(PathWidthProperty); }
			set { SetValue(PathWidthProperty, value); }
		}

        /// <summary>
        /// Identifies the PathWidth dependency property.
        /// </summary>
		public static readonly DependencyProperty PathWidthProperty =
			DependencyProperty.Register(
				"PathWidth",
				typeof(double),
				typeof(PathButton),
				null);
		#endregion

		#region [ PathHeight ]
		/// <summary>
		/// Gets or sets the height of the path.
		/// </summary>
		/// <value>The height of the path.</value>
		public double PathHeight
		{
			get { return (double)GetValue(PathHeightProperty); }
			set { SetValue(PathHeightProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty PathHeightProperty =
			DependencyProperty.Register(
				"PathHeight",
				typeof(double),
				typeof(PathButton),
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
				typeof(PathButton),
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
				typeof(PathButton),
				null);
		#endregion

        #region [ Background1 ]
        public Brush Background1
        {
            get { return GetValue(Background1Property) as Brush; }
            set { SetValue(Background1Property, value); }
        }

        public static readonly DependencyProperty Background1Property =
            DependencyProperty.Register(
                "Background1",
                typeof(Brush),
                typeof(PathButton),
                null);
        #endregion

        #region [ Background2 ]
        public Brush Background2
        {
            get { return GetValue(Background2Property) as Brush; }
            set { SetValue(Background2Property, value); }
        }

        public static readonly DependencyProperty Background2Property =
            DependencyProperty.Register(
                "Background2",
                typeof(Brush),
                typeof(PathButton),
                null);
        #endregion

		#region [ ForegroundHover ]
        /// <summary>
        /// Gets or sets the foreground hover.
        /// </summary>
        /// <value>The foreground hover.</value>
		public Brush ForegroundHover
		{
			get { return GetValue(ForegroundHoverProperty) as Brush; }
			set { SetValue(ForegroundHoverProperty, value); }
		}

        /// <summary>
        /// Identifies the ForegroundHover dependency property.
        /// </summary>
		public static readonly DependencyProperty ForegroundHoverProperty =
			DependencyProperty.Register(
				"ForegroundHover",
				typeof(Brush),
				typeof(PathButton),
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
			object background = GetTemplateChild(ElementBackgroundName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(background) || (background == null),
				"The template part BackgroundElement is not an instance of Rectangle!");
			
			BackgroundElement = background as Rectangle;

			object path = GetTemplateChild(ElementPathName);
			Debug.Assert(typeof(Path).IsInstanceOfType(path) || (path == null),
				"The template part PathElement is not an instance of Path!");
			
			PathElement = path as Path;
			object pathHover = GetTemplateChild(ElementPathHoverName);
			Debug.Assert(typeof(Path).IsInstanceOfType(pathHover) || (pathHover == null),
				"The template part PathHoverElement is not an instance of Path!");
			
			PathHoverElement = pathHover as Path;
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