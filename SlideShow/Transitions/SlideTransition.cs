using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vertigo.SlideShow.Controls;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Base class for the slide transitions. Animates a translation of the 'to' and 'from' slides.
	/// </summary>
	public abstract class SlideTransition : ITransition
	{
		protected Storyboard inTranslationStoryboard;
		protected Storyboard outTranslationStoryboard;
		protected Canvas MediaRoot;
		public event TransitionCompletedEventHandler ExecutionComplete;
		protected TransitionEventArgs transitionEventArgs;
		private SlideViewer fromSlide;
		private SlideViewer toSlide;
		protected int fromSlideIndex { get; set; }
		protected int toSlideIndex { get; set; }
		protected int currentAlbumLength { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SlideTransition"/> class.
		/// </summary>
		public SlideTransition()
		{
			MediaRoot = DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.MediaRoot;
			currentAlbumLength = DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides.Length;
		}

		protected abstract double inTranslationAnimationFrom();
		protected abstract double inTranslationAnimationTo();
		protected abstract double outTranslationAnimationFrom();
		protected abstract double outTranslationAnimationTo();

		protected abstract string inTranslationAnimationPropertyPath();
		protected abstract string outTranslationAnimationPropertyPath();

		/// <summary>
		/// Executes the specified from slide index.
		/// </summary>
		/// <param name="fromSlideIndex">Index of from slide.</param>
		/// <param name="toSlideIndex">Index of to slide.</param>
		/// <param name="duration">The duration.</param>
		public void Execute(int fromSlideIndex, int toSlideIndex, int duration)
		{
			this.fromSlideIndex = fromSlideIndex;
			this.toSlideIndex = toSlideIndex;

			MediaRoot.Children.Clear();
			MediaRoot.Resources.Clear();
			if (toSlide != null)
				toSlide.Dispose();
			if (fromSlide != null)
				fromSlide.Dispose();

			fromSlide = SlideViewer.Create(Data.Albums[DataHandler.CurrentAlbumIndex].Slides[fromSlideIndex]);
			MediaRoot.Children.Add(fromSlide);

			toSlide = SlideViewer.Create(Data.Albums[DataHandler.CurrentAlbumIndex].Slides[toSlideIndex]);
			MediaRoot.Children.Add(toSlide);

			transitionEventArgs = new TransitionEventArgs(fromSlideIndex, toSlideIndex, fromSlide, toSlide, GetType(), duration);

			inTranslationStoryboard = new Storyboard();
			DoubleAnimation inTranslationAnimation = new DoubleAnimation();
			inTranslationAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
			inTranslationAnimation.From = inTranslationAnimationFrom();
			inTranslationAnimation.To = inTranslationAnimationTo();
			Storyboard.SetTarget(inTranslationAnimation, toSlide);
			Storyboard.SetTargetProperty(inTranslationAnimation, new PropertyPath(inTranslationAnimationPropertyPath()));
			inTranslationStoryboard.Children.Add(inTranslationAnimation);
			MediaRoot.Resources.Add("inTranslationStoryboard", inTranslationStoryboard);
			inTranslationStoryboard.Completed += new EventHandler(inTranslationStoryboard_Completed);

			outTranslationStoryboard = new Storyboard();
			DoubleAnimation outTranslationAnimation = new DoubleAnimation();
			outTranslationAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
			outTranslationAnimation.From = outTranslationAnimationFrom();
			outTranslationAnimation.To = outTranslationAnimationTo();
			Storyboard.SetTarget(outTranslationAnimation, fromSlide);
			Storyboard.SetTargetProperty(outTranslationAnimation, new PropertyPath(outTranslationAnimationPropertyPath()));
			outTranslationStoryboard.Children.Add(outTranslationAnimation);
			MediaRoot.Resources.Add("outTranslationStoryboard", outTranslationStoryboard);

			inTranslationStoryboard.Begin();
			outTranslationStoryboard.Begin();
		}

		/// <summary>
		/// Handles the Completed event of the inTranslationStoryboard control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void inTranslationStoryboard_Completed(object sender, EventArgs e)
		{
			SkipToFill();
			ExecutionComplete(this, transitionEventArgs);
		}

		/// <summary>
		/// Skips to fill.
		/// </summary>
		public void SkipToFill()
		{
			inTranslationStoryboard.SkipToFill();
			outTranslationStoryboard.SkipToFill();
			MediaRoot.Children.Remove(fromSlide);
		}
	}
}