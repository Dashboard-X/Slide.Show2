using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vertigo.SlideShow.Controls;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Base class for the translate transitions. Animates a translation of the 'to' slide.
	/// </summary>
	public abstract class TranslateTransition : ITransition
	{
		protected Storyboard inTranslationStoryboard;
		protected Canvas MediaRoot;
		public event TransitionCompletedEventHandler ExecutionComplete;
		protected TransitionEventArgs transitionEventArgs;
		private SlideViewer fromSlide;
		private SlideViewer toSlide;

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslateTransition"/> class.
		/// </summary>
		public TranslateTransition()
		{
			MediaRoot = DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.MediaRoot;
		}

		protected abstract double inTranslationAnimationFrom();
		protected abstract double inTranslationAnimationTo();
		protected abstract string inTranslationAnimationPropertyPath();

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

			inTranslationStoryboard.Begin();
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
			MediaRoot.Children.Remove(fromSlide);
		}
	}
}