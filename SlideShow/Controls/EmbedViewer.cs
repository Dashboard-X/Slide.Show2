using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents an EmbedViewer
	/// </summary>
	public class EmbedViewer : ModalDialog
	{
        /// <summary>
		/// Initializes a new instance of the <see cref="EmbedViewer"/> class.
        /// </summary>
		public EmbedViewer()
		{
			DefaultStyleKey = typeof(EmbedViewer); 
			
			Options.EmbedViewerOptions options = Configuration.Options.EmbedViewer;

			Background = options.Background;
			BackgroundOpacity = options.BackgroundOpacity;
            CornerRadius = options.CornerRadius;

			#region [ Dialog ]
			DialogBackground = options.DialogBackground;
			DialogBackgroundOpacity = options.DialogBackgroundOpacity;
			DialogBorderBrush = options.DialogBorderBrush;
			DialogBorderThickness = options.DialogBorderThickness;
			DialogWidth = options.DialogWidth;
			DialogHeight = options.DialogHeight;
			#endregion

			#region [ CloseButton ]
			CloseButtonBackground1Brush = options.CloseButtonBackground1Brush;
			CloseButtonBackground2Brush = options.CloseButtonBackground2Brush;
			CloseButtonForegroundBrush = options.CloseButtonForegroundBrush;
			CloseButtonForegroundHoverBrush = options.CloseButtonForegroundHoverBrush;
			CloseButtonRadiusX = options.CloseButtonRadiusX;
			CloseButtonRadiusY = options.CloseButtonRadiusY;
			CloseButtonMargin = options.CloseButtonMargin;
			CloseButtonWidth = options.CloseButtonWidth;
			CloseButtonHeight = options.CloseButtonHeight;
			CloseButtonPathWidth = options.CloseButtonPathWidth;
			CloseButtonPathHeight = options.CloseButtonPathHeight;
			CloseButtonPathData = options.CloseButtonPathData;
			#endregion

			#region [ HyperlinkButton ]
			HyperlinkButtonText = options.HyperlinkButtonText;
			HyperlinkButtonMargin = options.HyperlinkButtonMargin;
			HyperlinkButtonForeground = options.HyperlinkButtonForeground;
			HyperlinkButtonBackground = options.HyperlinkButtonBackground;
			HyperlinkButtonWidth = options.HyperlinkButtonWidth;
			HyperlinkButtonHeight = options.HyperlinkButtonHeight;
			HyperlinkButtonFontFamily = options.HyperlinkButtonFontFamily;
			HyperlinkButtonFontSize = options.HyperlinkButtonFontSize;

			#endregion

			#region [ TextBlock ]
			TextBlockFontFamily = options.TextBlockFontFamily;
			TextBlockFontSize = options.TextBlockFontSize;
			TitleText = options.TitleText;
			TextBlockMargin = options.TextBlockMargin;
			TextBlockForeground = options.TextBlockForeground;
			#endregion

			#region [ TextBox ]
			TextBoxWidth = options.TextBoxWidth;
			TextBoxHeight = options.TextBoxHeight;
			TextBoxBackground = options.TextBoxBackground;
			TextBoxForeground = options.TextBoxForeground;
			TextBoxFontFamily = options.TextBoxFontFamily;
			TextBoxFontSize = options.TextBoxFontSize;
			#endregion
		}

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}

		/// <summary>
		/// Formats the configuration provider settings into a string
		/// </summary>
		/// <returns>A string of init parameters</returns>
		private string FormatInitParams()
		{
			StringBuilder initParams = new StringBuilder();
			bool addComa = false;
			bool addSemi = false;

			foreach (KeyValuePair<string, Dictionary<string, string>> d in ConfigurationProvider.initParams)
			{
				addSemi = false;
				if (addComa)
				{
					initParams.Append(",");
				}

				foreach (KeyValuePair<string, string> item in d.Value)
				{
					if (addSemi)
					{
						initParams.Append(";");
					}

					initParams.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}", item.Key, item.Value);
					addSemi = true;
				}

				addComa = true;
			}

			return initParams.ToString();
		}

		/// <summary>
		/// Generates the output text displayed by this embed control
		/// </summary>
		protected override void SetOutputText()
		{
			const string ObjectData = "data:application/x-silverlight-2,";
			const string ObjectType = "application/x-silverlight-2";
			const string LinkPath = "http://go.microsoft.com/fwlink/?LinkID=124807";
			const string ImageLinkPath = "http://go.microsoft.com/fwlink/?LinkId=108181";

			StringBuilder snip = new StringBuilder();

			snip.AppendFormat(CultureInfo.InvariantCulture,
				"<object type=\"{0}\" data=\"{1}\" width=\"{2}\" height=\"{3}\">\n",
					ObjectType,
					ObjectData,
					Application.Current.Host.Content.ActualWidth,
					Application.Current.Host.Content.ActualHeight);

			snip.AppendFormat(CultureInfo.InvariantCulture,
				"\t<param name=\"background\" value=\"{0}\" />\n",
					Application.Current.Host.Background);

			snip.AppendFormat(CultureInfo.InvariantCulture,
				"\t<param name=\"source\" value=\"{0}\" />\n",
					Application.Current.Host.Source);

			snip.AppendFormat(CultureInfo.InvariantCulture,
				"\t<param name=\"initParams\" value=\"{0}\" />\n",
					FormatInitParams());

			snip.AppendFormat(CultureInfo.InvariantCulture,
				"\t<a href=\"{0}\" style=\"text-decoration: none;\"><img src=\"{1}\" alt=\"Get Microsoft Silverlight\" style=\"border-style: none\" /></a>\n",
					LinkPath, ImageLinkPath);

			snip.Append("</object>");

			OutputText = snip.ToString();
		}

		/// <summary>
		/// Determines whether or not this control is enabled
		/// </summary>
		/// <returns>a boolean</returns>
		protected override bool IsEnabled()
		{
			return Configuration.Options.EmbedViewer.Enabled;
		}
	}
}
