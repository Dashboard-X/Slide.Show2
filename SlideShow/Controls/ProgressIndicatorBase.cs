using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the DownloadProgressIndicator abstract class.
    /// </summary>
	[TemplatePart(Name = DownloadProgressIndicator.ElementRootName, Type = typeof(FrameworkElement))]
	public abstract class DownloadProgressIndicator : Control
	{
		#region [ Constructor ]
		/// <summary>
		/// Initializes a new instance of the <see cref="DownloadProgressIndicator"/> class.
		/// </summary>
		public DownloadProgressIndicator()
		{
			IsTabStop = false;

			if (!Configuration.Options.LoadingProgressIndicator.Enabled)
			{
				Visibility = Visibility.Collapsed;
			}
		}
		#endregion

		#region [Template Parts]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		#endregion

		#region [Private Properties]

		/// <summary>
		/// Gets the progress.
		/// </summary>
		/// <value>The progress.</value>
		public double Progress { get; protected set; }

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
		}

		/// <summary>
		/// Creates the specified indicator.
		/// </summary>
		/// <param name="indicator">The indicator.</param>
		/// <returns>a DownloadProgressIndicator</returns>
		public static DownloadProgressIndicator Create(string indicator)
		{
			Type type;
			switch (indicator.ToLower(CultureInfo.InvariantCulture))
			{
				case "bar":
					type = typeof(BarProgressIndicator);
					break;
				case "circle":
					type = typeof(CircleProgressIndicator);
					break;
				default:
					type = typeof(BarProgressIndicator);
					break;
			}

			return (DownloadProgressIndicator)Activator.CreateInstance(type);
		}

		/// <summary>
		/// Updates the progress.
		/// </summary>
		/// <param name="percent">The percent.</param>
		public abstract void UpdateProgress(int percent);
	}
}