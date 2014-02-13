using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the PeopleTag class
    /// </summary>
    [TemplatePart(Name = PeopleTag.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = PeopleTag.ElementBorderName, Type = typeof(Border))]
	[TemplatePart(Name = PeopleTag.ElementPersonRectangleName, Type = typeof(Rectangle))]
	[TemplatePart(Name = PeopleTag.ElementPersonNameName, Type = typeof(TextBlock))]
	[TemplateVisualState(Name = "Hide", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Show", GroupName = "CommonStates")]
	public partial class PeopleTag : Control
    {
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="PeopleTag"/> class.
		/// </summary>
		public PeopleTag()
		{
			IsTabStop = false;
			DefaultStyleKey = typeof(PeopleTag);
			Options.PeopleTagOptions options = Configuration.Options.PeopleTag;

			#region [ PersonName ]
			PersonNameForeground = options.PersonNameForeground;
			PersonNameFontFamily = options.PersonNameFontFamily;
			PersonNameFontSize = options.PersonNameFontSize;
			PersonNameMargin = options.PersonNameMargin;
			#endregion

			#region [ Background ]
			Background = options.Background;
			BackgroundBorderBrush = options.BackgroundBorderBrush;
			BackgroundCornerRadius = options.BackgroundCornerRadius;
			BackgroundBorderThickness = options.BackgroundBorderThickness;
			BackgroundOpacity = options.BackgroundOpacity;
			Height = 30;
			#endregion

			if (!options.Enabled)
			{
				Visibility = Visibility.Collapsed;
			}
		}
		#endregion

        #region [ Template Parts ]
        internal FrameworkElement _elementRoot;
        internal const string ElementRootName = "RootElement";

		internal Border _elementBorder;
		internal const string ElementBorderName = "BorderElement";

		internal Rectangle _elementPersonRectangle;
		internal const string ElementPersonRectangleName = "PersonRectangleElement";

		internal TextBlock _elementPersonName;
		internal const string ElementPersonNameName = "PersonNameElement";
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
		/// Gets or sets the background border element.
		/// </summary>
		/// <value>The background border element.</value>
		private Border BorderElement
		{
			get { return _elementBorder; }
			set { _elementBorder = value; }
		}

		/// <summary>
		/// Gets or sets the name element.
		/// </summary>
		/// <value>The name element.</value>
		private TextBlock PersonNameElement
		{
			get { return _elementPersonName; }
			set	{ _elementPersonName = value; }
		}

		/// <summary>
		/// Gets the height of the image, scaled to fit in the viewer.
		/// </summary>
		/// <value>the height of the image, scaled to fit in the viewer.</value>
		private double ImageHeight
		{
			get
			{
				if (ImageSize.Height / Application.Current.Host.Content.ActualHeight >= ImageSize.Width / Application.Current.Host.Content.ActualWidth)
					return Application.Current.Host.Content.ActualHeight;
				else
					return ImageSize.Height / (ImageSize.Width / Application.Current.Host.Content.ActualWidth);
			}
		}

		/// <summary>
		/// Gets the width of the image, scaled to fit in the viewer.
		/// </summary>
		/// <value>the width of the image, scaled to fit in the viewer.</value>
		private double ImageWidth
		{
			get
			{
				if (ImageSize.Height / Application.Current.Host.Content.ActualHeight <= ImageSize.Width / Application.Current.Host.Content.ActualWidth)
					return Application.Current.Host.Content.ActualWidth;
				else
					return ImageSize.Width / (ImageSize.Height / Application.Current.Host.Content.ActualHeight);
			}
		}

		#endregion

		#region [ Public Properties ]

		/// <summary>
		/// Gets or sets the person rectangle element.
		/// </summary>
		/// <value>The person rectangle element.</value>
		public Rectangle PersonRectangleElement
		{
			get { return _elementPersonRectangle; }
			set { _elementPersonRectangle = value; }
		}


		/// <summary>
		/// Gets or sets the peopletag definition.
		/// </summary>
		/// <value>The peopletag definition.</value>
		public PeopleTagDefinition TagDefinition { get; set; }


		/// <summary>
		/// Gets or sets the related image's size.
		/// </summary>
		/// <value>The related image's size.</value>
		public Size ImageSize { get; set; }

		#endregion

		#region [ Dependency Properties ]

		#region [ PersonName ]

		#region [ PersonNameForeground ]

		public Brush PersonNameForeground
		{
			get { return GetValue(PersonNameForegroundProperty) as Brush; }
			set { SetValue(PersonNameForegroundProperty, value); }
		}

		public static readonly DependencyProperty PersonNameForegroundProperty =
			DependencyProperty.Register(
				"PersonNameForeground",
				typeof(Brush),
				typeof(PeopleTag),
				null);

		#endregion

		#region [ PersonNameMargin ]

		public Thickness PersonNameMargin
		{
			get { return (Thickness)GetValue(PersonNameMarginProperty); }
			set { SetValue(PersonNameMarginProperty, value); }
		}

		public static readonly DependencyProperty PersonNameMarginProperty =
			DependencyProperty.Register(
				"PersonNameMargin",
				typeof(Thickness),
				typeof(PeopleTag),
				null);
		#endregion

		#region [ PersonNameFontFamily ]

		public FontFamily PersonNameFontFamily
		{
			get { return (FontFamily)GetValue(PersonNameFontFamilyProperty); }
			set { SetValue(PersonNameFontFamilyProperty, value); }
		}

		public static readonly DependencyProperty PersonNameFontFamilyProperty =
			DependencyProperty.Register(
				"PersonNameFontFamily",
				typeof(FontFamily),
				typeof(PeopleTag),
				null);
		#endregion

		#region [ PersonNameFontSize ]

		public double PersonNameFontSize
		{
			get { return (double)GetValue(PersonNameFontSizeProperty); }
			set { SetValue(PersonNameFontSizeProperty, value); }
		}

		public static readonly DependencyProperty PersonNameFontSizeProperty =
			DependencyProperty.Register(
				"PersonNameFontSize",
				typeof(double),
				typeof(PeopleTag),
				null);
		#endregion

		#endregion

		#region [ Background ]

		#region [ BackgroundBorderBrush ]

		public Brush BackgroundBorderBrush
		{
			get { return GetValue(BackgroundBorderBrushProperty) as Brush; }
			set { SetValue(BackgroundBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty BackgroundBorderBrushProperty =
			DependencyProperty.Register(
				"BackgroundBorderBrush",
				typeof(Brush),
				typeof(PeopleTag),
				null);

		#endregion

		#region [ BackgroundOpacity ]
		public double BackgroundOpacity
		{
			get { return (double)GetValue(BackgroundOpacityProperty); }
			set { SetValue(BackgroundOpacityProperty, value); }
		}

		public static readonly DependencyProperty BackgroundOpacityProperty =
			DependencyProperty.Register(
				"BackgroundOpacity",
				typeof(double),
				typeof(PeopleTag),
				null);

		#endregion

		#region [ BackgroundCornerRadius ]

		public CornerRadius BackgroundCornerRadius
		{
			get { return (CornerRadius)GetValue(BackgroundCornerRadiusProperty); }
			set { SetValue(BackgroundCornerRadiusProperty, value); }
		}

		public static readonly DependencyProperty BackgroundCornerRadiusProperty =
			DependencyProperty.Register(
				"BackgroundCornerRadius",
				typeof(CornerRadius),
				typeof(PeopleTag),
				null);
		#endregion

		#region [ BackgroundBorderThickness ]

		public Thickness BackgroundBorderThickness
		{
			get { return (Thickness)GetValue(BackgroundBorderThicknessProperty); }
			set { SetValue(BackgroundBorderThicknessProperty, value); }
		}

		public static readonly DependencyProperty BackgroundBorderThicknessProperty =
			DependencyProperty.Register(
				"BackgroundBorderThickness",
				typeof(Thickness),
				typeof(PeopleTag),
				null);
		#endregion

		#endregion

		#endregion

		/// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			// Get the elements
			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null),
				"The template part RootElement is not an instance of FrameworkElement!");
			
			RootElement = root as FrameworkElement;
			RootElement.Opacity = 0;

			object personRectangle = GetTemplateChild(ElementPersonRectangleName);
			Debug.Assert(typeof(Rectangle).IsInstanceOfType(personRectangle) || (personRectangle == null),
				"The template part PersonRectangleElement is not an instance of Rectangle!");

			Page page = Application.Current.RootVisual as Page;

			PersonRectangleElement = personRectangle as Rectangle;
			PersonRectangleElement.Height = TagDefinition.HeightPercentage * ImageHeight;
			PersonRectangleElement.Width = TagDefinition.WidthPercentage * ImageWidth;
			PersonRectangleElement.SetValue(Canvas.TopProperty, TagDefinition.YPositionPercentage * ImageHeight + ((Application.Current.Host.Content.ActualHeight - ImageHeight) / 2));
			PersonRectangleElement.SetValue(Canvas.LeftProperty, TagDefinition.XPositionPercentage * ImageWidth + ((Application.Current.Host.Content.ActualWidth - ImageWidth) / 2));
			PersonRectangleElement.MouseEnter += new MouseEventHandler(PersonRectangleElement_MouseEnter);
			PersonRectangleElement.MouseLeave += new MouseEventHandler(PersonRectangleElement_MouseLeave);

			object border = GetTemplateChild(ElementBorderName);
			Debug.Assert(typeof(Border).IsInstanceOfType(border) || (border == null),
				"The template part BackgroundElement is not an instance of Border!");
			
			BorderElement = border as Border;

			object personName = GetTemplateChild(ElementPersonNameName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(personName) || (personName == null),
			    "The template part PersonNameElement is not an instance of TextBlock!");

			PersonNameElement = personName as TextBlock;
			PersonNameElement.Text = TagDefinition.DisplayName;
		}

		void PersonRectangleElement_MouseLeave(object sender, MouseEventArgs e)
		{
			this.Hide();
		}

		void PersonRectangleElement_MouseEnter(object sender, MouseEventArgs e)
		{
			this.Show();
		}

        /// <summary>
        /// Shows this instance.
        /// </summary>
		public void Show()
		{
			BorderElement.SetValue(Canvas.TopProperty, (double)PersonRectangleElement.GetValue(Canvas.TopProperty) + PersonRectangleElement.ActualHeight);
			BorderElement.SetValue(Canvas.LeftProperty, (double)PersonRectangleElement.GetValue(Canvas.LeftProperty) + (PersonRectangleElement.ActualWidth / 2) - (BorderElement.ActualWidth / 2));

			VisualStateManager.GoToState(this, "Show", true);
		}

        /// <summary>
        /// Hides this instance.
        /// </summary>
		public void Hide()
		{
			VisualStateManager.GoToState(this, "Hide", true);
		}
    }
}