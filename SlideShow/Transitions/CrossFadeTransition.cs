using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vertigo.SlideShow.Controls;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Animates the opacity of the 'to' slide in and the 'from' slide out.
	/// </summary>
	public class CrossFadeTransition : ITransition
	{
		private Storyboard inOpacityStoryboard;
		private Storyboard outOpacityStoryboard;
		private Canvas MediaRoot;
		public event TransitionCompletedEventHandler ExecutionComplete;
		private TransitionEventArgs transitionEventArgs;
		private SlideViewer toSlide;
		private SlideViewer fromSlide;

		/// <summary>
		/// Initializes a new instance of the <see cref="CrossFadeTransition"/> class.
		/// </summary>
		public CrossFadeTransition()
		{
			MediaRoot = DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.MediaRoot;
		}

		/// <summary>
		/// Executes the specified from slide index.
		/// </summary>
		/// <param name="fromSlideIndex">Index of from slide.</param>
		/// <param name="toSlideIndex">Index of to slide.</param>
		/// <param name="duration">The duration.</param>
		public void Execute(int fromSlideIndex, int toSlideIndex, int duration)
		{
			MediaRoot.Children.Clear();
			MediaRoot.Resources.Clear();
			if (toSlide != null)
				toSlide.Dispose();
			if (fromSlide != null)
				fromSlide.Dispose();

			toSlide = SlideViewer.Create(Data.Albums[DataHandler.CurrentAlbumIndex].Slides[toSlideIndex]);
			MediaRoot.Children.Add(toSlide);

			fromSlide = SlideViewer.Create(Data.Albums[DataHandler.CurrentAlbumIndex].Slides[fromSlideIndex]);
			MediaRoot.Children.Add(fromSlide);

			transitionEventArgs = new TransitionEventArgs(fromSlideIndex, toSlideIndex, fromSlide, toSlide, this.GetType(), duration);

			inOpacityStoryboard = new Storyboard();
			DoubleAnimation inOpacityAnimation = new DoubleAnimation();
			inOpacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
			inOpacityAnimation.From = 0;
			inOpacityAnimation.To = 1;
			Storyboard.SetTarget(inOpacityAnimation, toSlide);
			Storyboard.SetTargetProperty(inOpacityAnimation, new PropertyPath("Opacity"));
			inOpacityStoryboard.Children.Add(inOpacityAnimation);
			MediaRoot.Resources.Add("inOpacityStoryboard", inOpacityStoryboard);
			inOpacityStoryboard.Completed += new EventHandler(inOpacityStoryboard_Completed);

			outOpacityStoryboard = new Storyboard();
			DoubleAnimation outOpacityAnimation = new DoubleAnimation();
			outOpacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
			outOpacityAnimation.From = 1;
			outOpacityAnimation.To = 0;
			Storyboard.SetTarget(outOpacityAnimation, fromSlide);
			Storyboard.SetTargetProperty(outOpacityAnimation, new PropertyPath("Opacity"));
			outOpacityStoryboard.Children.Add(outOpacityAnimation);
			MediaRoot.Resources.Add("outOpacityStoryboard", outOpacityStoryboard);

			inOpacityStoryboard.Begin();
			outOpacityStoryboard.Begin();
		}

		/// <summary>
		/// Handles the Completed event of the inOpacityStoryboard control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void inOpacityStoryboard_Completed(object sender, EventArgs e)
		{
			SkipToFill();
			ExecutionComplete(this, transitionEventArgs);
		}

		/// <summary>
		/// Skips to fill.
		/// </summary>
		public void SkipToFill()
		{
			inOpacityStoryboard.SkipToFill();
			outOpacityStoryboard.SkipToFill();
			MediaRoot.Children.Remove(fromSlide);
		}
	}
}