using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the RepeatPathButton which extends the PathButton but follows the
    /// functionality of a RepeatButton
    /// </summary>
	public class RepeatPathButton : PathButton
	{
		#region Delay
        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>The delay.</value>
		public int Delay
		{
			get { return (int)GetValue(DelayProperty); }
			set { SetValue(DelayProperty, value); }
		}

        /// <summary>
        /// Identifies the Delay dependency property.
        /// </summary>
		public static readonly DependencyProperty DelayProperty =
			DependencyProperty.Register(
				"Delay",
				typeof(int),
				typeof(RepeatPathButton),
				new PropertyMetadata(OnDelayPropertyChanged));

        /// <summary>
        /// Called when [delay property changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnDelayPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RepeatPathButton button = d as RepeatPathButton;
			Debug.Assert(button != null);

			int delay = (int)e.NewValue;
			if (delay < 0)
			{
				throw new ArgumentException("Delay cannot be a negative value.", DelayProperty.ToString());
			}
		}
		#endregion

		#region Interval
        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
		public int Interval
		{
			get { return (int)GetValue(IntervalProperty); }
			set { SetValue(IntervalProperty, value); }
		}

        /// <summary>
        /// Identifies the Interval dependency property.
        /// </summary>
		public static readonly DependencyProperty IntervalProperty =
			DependencyProperty.Register(
				"Interval",
				typeof(int),
				typeof(RepeatPathButton),
				new PropertyMetadata(OnIntervalPropertyChanged));

        /// <summary>
        /// Called when [interval property changed].
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RepeatPathButton button = d as RepeatPathButton;
			Debug.Assert(button != null);

			int interval = (int)e.NewValue;
			if (interval < 0)
			{
				throw new ArgumentException("Interval cannot be a negative value.", IntervalProperty.ToString());
			}
		}
		#endregion

		#region Timer
		private DispatcherTimer _timer;

        /// <summary>
        /// Starts the timer.
        /// </summary>
		private void StartTimer()
		{
			if (this._timer == null)
			{
				this._timer = new DispatcherTimer();
				this._timer.Tick += new EventHandler(OnTimeout);
			}
			else if (this._timer.IsEnabled)
			{
				return;
			}

			this._timer.Interval = TimeSpan.FromMilliseconds(Delay);
			this._timer.Start();
		}

        /// <summary>
        /// Stops the timer.
        /// </summary>
		private void StopTimer()
		{
			if (this._timer != null)
			{
				this._timer.Stop();
			}
		}

        /// <summary>
        /// Called when [timeout].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void OnTimeout(object sender, EventArgs e)
		{
			int interval = Interval;

			if (this._timer.Interval.Milliseconds != interval)
			{
				this._timer.Interval = TimeSpan.FromMilliseconds(interval);
			}

			if (IsPressed)
			{
				OnClick();
			}
		}
		#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPathButton"/> class.
        /// </summary>
		public RepeatPathButton()
		{
			ClickMode = ClickMode.Press;
			Delay = 250;
			Interval = 250;
			DefaultStyleKey = typeof(PathButton);
		}

		#region Mouse Handlers
        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> event that occurs when the left mouse button is pressed while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The event data.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="e"/> is null.</exception>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);

			if (ClickMode != ClickMode.Hover)
			{
				StartTimer();
			}
		}

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> event that occurs when the left mouse button is released while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The event data.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="e"/> is null.</exception>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);

			if (ClickMode != ClickMode.Hover)
			{
				StopTimer();
			}
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

			if (IsPressed)
			{
				OnClick();
				StartTimer();
			}
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

			StopTimer();
		}
		#endregion
	}
}
