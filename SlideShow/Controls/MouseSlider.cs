using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Vertigo.SlideShow.Controls
{
	/// <summary>
	/// Represents a MouseSlider
	/// </summary>
	public class MouseSlider : Slider
    {
		/// <summary>
		/// Occurs when [thumb drag started].
		/// </summary>
        public event EventHandler<EventArgs> ThumbDragStarted;

		/// <summary>
		/// Occurs when [thumb drag completed].
		/// </summary>
        public event EventHandler<EventArgs> ThumbDragCompleted;

		/// <summary>
		/// Initializes a new instance of the <see cref="MouseSlider"/> class.
		/// </summary>
        public MouseSlider()
        {
            DefaultStyleKey = typeof(Slider);
        }

		/// <summary>
		/// Builds the visual tree for the <see cref="T:System.Windows.Controls.Slider"/> control when a new template is applied.
		/// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Thumb thumb = GetTemplateChild("HorizontalThumb") as Thumb;
            if (thumb != null)
            {
                thumb.DragStarted += OnDragStarted;
                thumb.DragCompleted += OnDragCompleted;
            }
        }

		/// <summary>
		/// Called when [drag completed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragCompletedEventArgs"/> instance containing the event data.</param>
        private void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnThumbDragCompleted(this, new EventArgs());
        }

		/// <summary>
		/// Called when [drag started].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Controls.Primitives.DragStartedEventArgs"/> instance containing the event data.</param>
        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            OnThumbDragStarted(this, new EventArgs());
        }

		/// <summary>
		/// Called when [thumb drag started].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnThumbDragStarted(object sender, EventArgs e)
        {
            if (ThumbDragStarted != null)
            {
                ThumbDragStarted(sender, e);
            }
        }

		/// <summary>
		/// Called when [thumb drag completed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnThumbDragCompleted(object sender, EventArgs e)
        {
            if (ThumbDragCompleted != null)
            {
                ThumbDragCompleted(sender, e);
            }
        }
    }
}