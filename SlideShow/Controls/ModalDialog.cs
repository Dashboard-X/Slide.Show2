using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents an ModalDialog
	/// </summary>
	[TemplatePart(Name = ModalDialog.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = ModalDialog.ElementTextBlockName, Type = typeof(TextBlock))]
	[TemplatePart(Name = ModalDialog.ElementTextBoxName, Type = typeof(TextBox))]
	[TemplatePart(Name = ModalDialog.ElementCloseButtonName, Type = typeof(PathButton))]
	[TemplatePart(Name = ModalDialog.ElementHyperlinkCopyToClipboardName, Type = typeof(HyperlinkButton))]

	public abstract class ModalDialog : Control
	{
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="ModalDialog"/> class.
		/// </summary>
		public ModalDialog()
		{
			DefaultStyleKey = typeof(ModalDialog);
		}
		#endregion
		
		#region [ Template Parts ]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal TextBlock _elementTextBlock;
		internal const string ElementTextBlockName = "TextBlockElement";

		internal TextBox _elementTextBox;
		internal const string ElementTextBoxName = "TextBoxElement";

		internal PathButton _elementCloseButton;
		internal const string ElementCloseButtonName = "CloseButtonElement";

		internal HyperlinkButton _elementHyperlinkCopyToClipboard;
		internal const string ElementHyperlinkCopyToClipboardName = "HyperlinkCopyToClipboard";

		#endregion

		#region [ Private Properties ]

		private FrameworkElement RootElement
		{
			get { return _elementRoot; }
			set { _elementRoot = value; }
		}

		private TextBlock TextBlockElement
		{
			get { return _elementTextBlock; }
			set { _elementTextBlock = value; }
		}

		private TextBox TextBoxElement
		{
			get { return _elementTextBox; }
			set { _elementTextBox = value; }
		}

		private PathButton CloseButtonElement
		{
			get { return _elementCloseButton; }
			set { _elementCloseButton = value; }
		}

		private HyperlinkButton HyperlinkCopyToClipboardElement
		{
			get { return _elementHyperlinkCopyToClipboard; }
			set { _elementHyperlinkCopyToClipboard = value; }
		}

		#endregion

		#region [ Events ]

		public event EventHandler CloseButtonClicked;

		#endregion

		#region [ BackgroundOpacity ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double BackgroundOpacity
		{
			get { return (double)GetValue(BackgroundOpacityProperty); }
			set { SetValue(BackgroundOpacityProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty BackgroundOpacityProperty =
			DependencyProperty.Register(
				"BackgroundOpacity",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

        #region [ CornerRadius ]

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary> 
        /// Identifies the CornerRadius dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                "CornerRadius",
                typeof(CornerRadius),
                typeof(ModalDialog),
                null);

        #endregion

		#region [ Dialog ]

		#region [ DialogBackground ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Brush DialogBackground
		{
			get { return (Brush)GetValue(DialogBackgroundProperty); }
			set { SetValue(DialogBackgroundProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty DialogBackgroundProperty =
			DependencyProperty.Register(
				"DialogBackground",
				typeof(Brush),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ DialogBackgroundOpacity ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double DialogBackgroundOpacity
		{
			get { return (double)GetValue(DialogBackgroundOpacityProperty); }
			set { SetValue(DialogBackgroundOpacityProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty DialogBackgroundOpacityProperty =
			DependencyProperty.Register(
				"DialogBackgroundOpacity",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ DialogBorderBrush ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Brush DialogBorderBrush
		{
			get { return (Brush)GetValue(DialogBorderBrushProperty); }
			set { SetValue(DialogBorderBrushProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty DialogBorderBrushProperty =
			DependencyProperty.Register(
				"DialogBorderBrush",
				typeof(Brush),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ DialogBorderThickness ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Thickness DialogBorderThickness
		{
			get { return (Thickness)GetValue(DialogBorderThicknessProperty); }
			set { SetValue(DialogBorderThicknessProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty DialogBorderThicknessProperty =
			DependencyProperty.Register(
				"DialogBorderThickness",
				typeof(Thickness),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ DialogWidth ]

		public double DialogWidth
		{
			get { return (double)GetValue(DialogWidthProperty); }
			set { SetValue(DialogWidthProperty, value); }
		}

		/// <summary> 
		/// Identifies the DialogWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty DialogWidthProperty =
			DependencyProperty.Register(
				"DialogWidth",
				typeof(double),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ DialogHeight ]

		public double DialogHeight
		{
			get { return (double)GetValue(DialogHeightProperty); }
			set { SetValue(DialogHeightProperty, value); }
		}

		/// <summary> 
		/// Identifies the DialogHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty DialogHeightProperty =
			DependencyProperty.Register(
				"DialogHeight",
				typeof(double),
				typeof(ModalDialog),
				null);

		#endregion

		#endregion

		#region [ CloseButton ]

		#region [ CloseButtonBackground1Brush ]
		/// <summary>
		/// Gets or sets the full screen button brush.
		/// </summary>
		/// <value>The full screen button brush.</value>
		public Brush CloseButtonBackground1Brush
		{
			get { return GetValue(CloseButtonBackground1BrushProperty) as Brush; }
			set { SetValue(CloseButtonBackground1BrushProperty, value); }
		}

		/// <summary>
		/// Identifies the CloseButtonBrush dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonBackground1BrushProperty =
			DependencyProperty.Register(
				"CloseButtonBackground1Brush",
				typeof(Brush),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ CloseButtonBackground2Brush ]
		/// <summary>
		/// Gets or sets the full screen button brush.
		/// </summary>
		/// <value>The full screen button brush.</value>
		public Brush CloseButtonBackground2Brush
		{
			get { return GetValue(CloseButtonBackground2BrushProperty) as Brush; }
			set { SetValue(CloseButtonBackground2BrushProperty, value); }
		}

		/// <summary>
		/// Identifies the CloseButtonBrush dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonBackground2BrushProperty =
			DependencyProperty.Register(
				"CloseButtonBackground2Brush",
				typeof(Brush),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ CloseButtonForegroundBrush ]
		/// <summary>
		/// Gets or sets the full screen button brush.
		/// </summary>
		/// <value>The full screen button brush.</value>
		public Brush CloseButtonForegroundBrush
		{
			get { return GetValue(CloseButtonForegroundBrushProperty) as Brush; }
			set { SetValue(CloseButtonForegroundBrushProperty, value); }
		}

		/// <summary>
		/// Identifies the CloseButtonBrush dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonForegroundBrushProperty =
			DependencyProperty.Register(
				"CloseButtonForegroundBrush",
				typeof(Brush),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ CloseButtonForegroundHoverBrush ]
		/// <summary>
		/// Gets or sets the full screen button hover brush.
		/// </summary>
		/// <value>The full screen button hover brush.</value>
		public Brush CloseButtonForegroundHoverBrush
		{
			get { return GetValue(CloseButtonForegroundHoverBrushProperty) as Brush; }
			set { SetValue(CloseButtonForegroundHoverBrushProperty, value); }
		}

		/// <summary>
		/// Identifies the CloseButtonHoverBrush dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonForegroundHoverBrushProperty =
			DependencyProperty.Register(
				"CloseButtonForegroundHoverBrush",
				typeof(Brush),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ CloseButtonRadiusX ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public double CloseButtonRadiusX
		{
			get { return (double)GetValue(CloseButtonRadiusXProperty); }
			set { SetValue(CloseButtonRadiusXProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonRadiusXProperty =
			DependencyProperty.Register(
				"CloseButtonRadiusX",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonRadiusY ]
		/// <summary>
		/// Gets or sets the Y radius of the background rectangle.
		/// </summary>
		/// <value>the Y radius of the background rectangle.</value>
		public double CloseButtonRadiusY
		{
			get { return (double)GetValue(CloseButtonRadiusYProperty); }
			set { SetValue(CloseButtonRadiusYProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonRadiusYProperty =
			DependencyProperty.Register(
				"CloseButtonRadiusY",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonMargin ]

		public Thickness CloseButtonMargin
		{
			get { return (Thickness)GetValue(CloseButtonMarginProperty); }
			set { SetValue(CloseButtonMarginProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonMarginProperty =
			DependencyProperty.Register(
				"CloseButtonMargin",
				typeof(Thickness),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonWidth ]

		public double CloseButtonWidth
		{
			get { return (double)GetValue(CloseButtonWidthProperty); }
			set { SetValue(CloseButtonWidthProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonWidthProperty =
			DependencyProperty.Register(
				"CloseButtonWidth",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonHeight ]

		public double CloseButtonHeight
		{
			get { return (double)GetValue(CloseButtonHeightProperty); }
			set { SetValue(CloseButtonHeightProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonHeightProperty =
			DependencyProperty.Register(
				"CloseButtonHeight",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonPathWidth ]

		public double CloseButtonPathWidth
		{
			get { return (double)GetValue(CloseButtonPathWidthProperty); }
			set { SetValue(CloseButtonPathWidthProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonPathWidthProperty =
			DependencyProperty.Register(
				"CloseButtonPathWidth",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonPathHeight ]

		public double CloseButtonPathHeight
		{
			get { return (double)GetValue(CloseButtonPathHeightProperty); }
			set { SetValue(CloseButtonPathHeightProperty, value); }
		}

		public static readonly DependencyProperty CloseButtonPathHeightProperty =
			DependencyProperty.Register(
				"CloseButtonPathHeight",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ CloseButtonPathData ]
		/// <summary>
		/// Gets or sets the album button path data.
		/// </summary>
		/// <value>The album button path data.</value>
		public string CloseButtonPathData
		{
			get { return (string)GetValue(CloseButtonPathDataProperty); }
			set { SetValue(CloseButtonPathDataProperty, value); }
		}

		/// <summary>
		/// Identifies the CloseButtonPathData dependency property.
		/// </summary>
		public static readonly DependencyProperty CloseButtonPathDataProperty =
			DependencyProperty.Register(
				"CloseButtonPathData",
				typeof(string),
				typeof(ModalDialog),
				null);
		#endregion

		#endregion

		#region [ HyperlinkButton ]

		#region HyperlinkButtonText

		public string HyperlinkButtonText
		{
			get { return (string)GetValue(HyperlinkButtonTextProperty); }
			set { SetValue(HyperlinkButtonTextProperty, value); }
		}

		/// <summary> 
		/// Identifies the HyperlinkButtonText dependency property.
		/// </summary>
		public static readonly DependencyProperty HyperlinkButtonTextProperty =
			DependencyProperty.Register(
				"HyperlinkButtonText",
				typeof(string),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ HyperlinkButtonMargin ]

		public Thickness HyperlinkButtonMargin
		{
			get { return (Thickness)GetValue(HyperlinkButtonMarginProperty); }
			set { SetValue(HyperlinkButtonMarginProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonMarginProperty =
			DependencyProperty.Register(
				"HyperlinkButtonMargin",
				typeof(Thickness),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ HyperlinkButtonForeground ]

		public Brush HyperlinkButtonForeground
		{
			get { return (Brush)GetValue(HyperlinkButtonForegroundProperty); }
			set { SetValue(HyperlinkButtonForegroundProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonForegroundProperty =
			DependencyProperty.Register(
				"HyperlinkButtonForeground",
				typeof(Brush),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ HyperlinkButtonBackground ]

		public Brush HyperlinkButtonBackground
		{
			get { return (Brush)GetValue(HyperlinkButtonBackgroundProperty); }
			set { SetValue(HyperlinkButtonBackgroundProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonBackgroundProperty =
			DependencyProperty.Register(
				"HyperlinkButtonBackground",
				typeof(Brush),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ HyperlinkButtonWidth ]

		public double HyperlinkButtonWidth
		{
			get { return (double)GetValue(HyperlinkButtonWidthProperty); }
			set { SetValue(HyperlinkButtonWidthProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonWidthProperty =
			DependencyProperty.Register(
				"HyperlinkButtonWidth",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ HyperlinkButtonHeight ]

		public double HyperlinkButtonHeight
		{
			get { return (double)GetValue(HyperlinkButtonHeightProperty); }
			set { SetValue(HyperlinkButtonHeightProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonHeightProperty =
			DependencyProperty.Register(
				"HyperlinkButtonHeight",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ HyperlinkButtonFontFamily ]

		public FontFamily HyperlinkButtonFontFamily
		{
			get { return (FontFamily)GetValue(HyperlinkButtonFontFamilyProperty); }
			set { SetValue(HyperlinkButtonFontFamilyProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonFontFamilyProperty =
			DependencyProperty.Register(
				"HyperlinkButtonFontFamily",
				typeof(FontFamily),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ HyperlinkButtonFontSize ]

		public double HyperlinkButtonFontSize
		{
			get { return (double)GetValue(HyperlinkButtonFontSizeProperty); }
			set { SetValue(HyperlinkButtonFontSizeProperty, value); }
		}

		public static readonly DependencyProperty HyperlinkButtonFontSizeProperty =
			DependencyProperty.Register(
				"HyperlinkButtonFontSize",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#endregion

		#region [ TextBlock ]

		#region [ TextBlockFontFamily ]

		public FontFamily TextBlockFontFamily
		{
			get { return (FontFamily)GetValue(TextBlockFontFamilyProperty); }
			set { SetValue(TextBlockFontFamilyProperty, value); }
		}

		public static readonly DependencyProperty TextBlockFontFamilyProperty =
			DependencyProperty.Register(
				"TextBlockFontFamily",
				typeof(FontFamily),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ TextBlockFontSize ]

		public double TextBlockFontSize
		{
			get { return (double)GetValue(TextBlockFontSizeProperty); }
			set { SetValue(TextBlockFontSizeProperty, value); }
		}

		public static readonly DependencyProperty TextBlockFontSizeProperty =
			DependencyProperty.Register(
				"TextBlockFontSize",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ TitleText ]

		public string TitleText
		{
			get { return (string)GetValue(TitleTextProperty); }
			set { SetValue(TitleTextProperty, value); }
		}

		/// <summary> 
		/// Identifies the TitleText dependency property.
		/// </summary>
		public static readonly DependencyProperty TitleTextProperty =
			DependencyProperty.Register(
				"TitleText",
				typeof(string),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ TextBlockMargin ]

		public Thickness TextBlockMargin
		{
			get { return (Thickness)GetValue(TextBlockMarginProperty); }
			set { SetValue(TextBlockMarginProperty, value); }
		}

		public static readonly DependencyProperty TextBlockMarginProperty =
			DependencyProperty.Register(
				"TextBlockMargin",
				typeof(Thickness),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ TextBlockForeground ]
		/// <summary>
		/// Gets or sets the full screen button brush.
		/// </summary>
		/// <value>The full screen button brush.</value>
		public Brush TextBlockForeground
		{
			get { return GetValue(TextBlockForegroundProperty) as Brush; }
			set { SetValue(TextBlockForegroundProperty, value); }
		}

		/// <summary>
		/// Identifies the CloseButtonBrush dependency property.
		/// </summary>
		public static readonly DependencyProperty TextBlockForegroundProperty =
			DependencyProperty.Register(
				"TextBlockForeground",
				typeof(Brush),
				typeof(ModalDialog),
				null);

		#endregion

		#endregion

		#region [ TextBox ]

		#region [ OutputText ]

		public string OutputText
		{
			get { return (string)GetValue(OutputTextProperty); }
			set { SetValue(OutputTextProperty, value); }
		}

		/// <summary> 
		/// Identifies the OutputText dependency property.
		/// </summary>
		public static readonly DependencyProperty OutputTextProperty =
			DependencyProperty.Register(
				"OutputText",
				typeof(string),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ TextBoxWidth ]

		public double TextBoxWidth
		{
			get { return (double)GetValue(TextBoxWidthProperty); }
			set { SetValue(TextBoxWidthProperty, value); }
		}

		/// <summary> 
		/// Identifies the TextBoxWidth dependency property.
		/// </summary>
		public static readonly DependencyProperty TextBoxWidthProperty =
			DependencyProperty.Register(
				"TextBoxWidth",
				typeof(double),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ TextBoxHeight ]

		public double TextBoxHeight
		{
			get { return (double)GetValue(TextBoxHeightProperty); }
			set { SetValue(TextBoxHeightProperty, value); }
		}

		/// <summary> 
		/// Identifies the TextBoxHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty TextBoxHeightProperty =
			DependencyProperty.Register(
				"TextBoxHeight",
				typeof(double),
				typeof(ModalDialog),
				null);

		#endregion

		#region [ TextBoxBackground ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Brush TextBoxBackground
		{
			get { return (Brush)GetValue(TextBoxBackgroundProperty); }
			set { SetValue(TextBoxBackgroundProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty TextBoxBackgroundProperty =
			DependencyProperty.Register(
				"TextBoxBackground",
				typeof(Brush),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ TextBoxForeground ]
		/// <summary>
		/// Gets or sets the X radius of the background rectangle.
		/// </summary>
		/// <value>the X radius of the background rectangle.</value>
		public Brush TextBoxForeground
		{
			get { return (Brush)GetValue(TextBoxForegroundProperty); }
			set { SetValue(TextBoxForegroundProperty, value); }
		}

		/// <summary>
		/// Identifies the PathHeight dependency property.
		/// </summary>
		public static readonly DependencyProperty TextBoxForegroundProperty =
			DependencyProperty.Register(
				"TextBoxForeground",
				typeof(Brush),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ TextBoxFontFamily ]

		public FontFamily TextBoxFontFamily
		{
			get { return (FontFamily)GetValue(TextBoxFontFamilyProperty); }
			set { SetValue(TextBoxFontFamilyProperty, value); }
		}

		public static readonly DependencyProperty TextBoxFontFamilyProperty =
			DependencyProperty.Register(
				"TextBoxFontFamily",
				typeof(FontFamily),
				typeof(ModalDialog),
				null);
		#endregion

		#region [ TextBoxFontSize ]

		public double TextBoxFontSize
		{
			get { return (double)GetValue(TextBoxFontSizeProperty); }
			set { SetValue(TextBoxFontSizeProperty, value); }
		}

		public static readonly DependencyProperty TextBoxFontSizeProperty =
			DependencyProperty.Register(
				"TextBoxFontSize",
				typeof(double),
				typeof(ModalDialog),
				null);
		#endregion

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
			if (RootElement != null)
			{
				RootElement.Visibility = Visibility.Collapsed;
			}

			object textBlock = GetTemplateChild(ElementTextBlockName);
			Debug.Assert(typeof(TextBlock).IsInstanceOfType(textBlock) || (textBlock == null),
				"The template part TextBlockElement is not an instance of TextBlock!");
			
			TextBlockElement = textBlock as TextBlock;
			object textBox = GetTemplateChild(ElementTextBoxName);
			Debug.Assert(typeof(TextBox).IsInstanceOfType(textBox) || (textBox == null),
				"The template part TextBoxElement is not an instance of TextBox!");
			
			TextBoxElement = textBox as TextBox;
			object closeButton = GetTemplateChild(ElementCloseButtonName);
			Debug.Assert(typeof(PathButton).IsInstanceOfType(closeButton) || (closeButton == null),
				"The template part CloseButtonElement is not an instance of PathButton!");
			
			CloseButtonElement = closeButton as PathButton;
			if (CloseButtonElement != null)
			{
				CloseButtonElement.Click += delegate
				{
					if (CloseButtonClicked != null)
					{
						CloseButtonClicked(this, EventArgs.Empty);
					}
				};
			}

			object hyperLinkButton = GetTemplateChild(ElementHyperlinkCopyToClipboardName);
			Debug.Assert(typeof(HyperlinkButton).IsInstanceOfType(hyperLinkButton) || (hyperLinkButton == null),
				"The template part ElementHyperlinkCopyToClipboard is not an instance of HyperlinkButton!");

			HyperlinkCopyToClipboardElement = hyperLinkButton as HyperlinkButton;

			// hide the clipboard link if clipboard isn't supported
			if (HyperlinkCopyToClipboardElement != null && GetClipboard() == null)
			{
				HyperlinkCopyToClipboardElement.Visibility = Visibility.Collapsed;
			}

			// verify clipboard access; Links are collapsed by default
			if (HyperlinkCopyToClipboardElement != null && GetClipboard() != null && textBlock != null)
			{
				HyperlinkCopyToClipboardElement.Click += delegate
				{
					CopyToClipboard(OutputText);
				};
			}

			Content_Resized(this, EventArgs.Empty);
			Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
			Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
		}

		/// <summary>
		/// Handles the Resized event of the Content control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			Width = Application.Current.Host.Content.ActualWidth;
			Height = Application.Current.Host.Content.ActualHeight;
		}

		protected abstract void SetOutputText();
		protected abstract new bool IsEnabled();

		public void Display()
		{
			if (!IsEnabled())
			{
				return;
			}

			SetOutputText();

			if (TextBoxElement != null)
			{
				TextBoxElement.Focus();
				TextBoxElement.SelectAll();
			}

			if (RootElement != null)
			{
				RootElement.Visibility = Visibility.Visible;
				VisualStateManager.GoToState(this, "Normal", true);
			}
		}

		public void Hide()
		{
			if (RootElement != null)
			{
				VisualStateManager.GoToState(this, "Hidden", true);
				RootElement.Visibility = Visibility.Collapsed;
			}
		}

		/// <summary>
		/// Attempts to get the user's clipboard.
		/// </summary>
		/// <returns>The clipboard.</returns>
		private static ScriptObject GetClipboard()
		{
			if (HtmlPage.IsEnabled)
			{
				return HtmlPage.Window.GetProperty("clipboardData") as ScriptObject;
			}

			return null;
		}

		/// <summary>
		/// Copies text to the user's clipboard.
		/// </summary>
		/// <param name="text">The text to copy.</param>
		private static void CopyToClipboard(string text)
		{
			ScriptObject clipboard = GetClipboard();

			if (clipboard == null)
			{
				HtmlPage.Window.Alert("Clipboard is unavailable.");
			}
			else if (!(bool)clipboard.Invoke("setData", "text", text))
			{
				HtmlPage.Window.Alert("Unable to copy to clipboard.");
			}
		}
	}
}