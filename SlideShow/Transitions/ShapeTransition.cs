using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vertigo.SlideShow.Controls;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Defines a shape transition. Places the toSlide on top of the fromSlide and animates the clipping path of the toSlide.
	/// </summary>
	public abstract class ShapeTransition : ITransition
	{
		protected Storyboard radiusXStoryboard;
		protected DoubleAnimation radiusXAnimation;
		protected DoubleAnimation radiusYAnimation;
		protected Storyboard radiusYStoryboard;
		protected Canvas MediaRoot;
		public event TransitionCompletedEventHandler ExecutionComplete;
		protected TransitionEventArgs transitionEventArgs;
		protected SlideViewer fromSlide;
		protected SlideViewer toSlide;
		protected int fromSlideIndex { get; set; }
		protected int toSlideIndex { get; set; }
		protected int duration { get; set; }
		protected int currentAlbumLength { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ShapeTransition"/> class.
		/// </summary>
		public ShapeTransition()
		{
			MediaRoot = DataHandler.Albums[DataHandler.CurrentAlbumIndex].TransitionManager.MediaRoot;
			currentAlbumLength = DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides.Length;
		}

		protected abstract void OnResize();
		protected abstract void AddSlidesToMediaRoot();
		protected abstract void SetClippingGeometry();

		protected abstract double RadiusXFrom();
		protected abstract double RadiusXTo();
		protected abstract string XPropertyPath();
		protected abstract DependencyObject xTarget { get; set; }

		protected abstract double RadiusYFrom();
		protected abstract double RadiusYTo();
		protected abstract string YPropertyPath();
		protected abstract DependencyObject yTarget { get; set; }

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
			toSlide = SlideViewer.Create(Data.Albums[DataHandler.CurrentAlbumIndex].Slides[toSlideIndex]);

			AddSlidesToMediaRoot();

			App.Current.Host.Content.FullScreenChanged += new EventHandler(Content_SizeChanged);
			App.Current.Host.Content.Resized += new EventHandler(Content_SizeChanged);

			SetClippingGeometry();

			transitionEventArgs = new TransitionEventArgs(fromSlideIndex, toSlideIndex, fromSlide, toSlide, this.GetType(), duration);

			radiusXStoryboard = new Storyboard();
			radiusXAnimation = new DoubleAnimation();
			radiusXAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
			radiusXAnimation.From = RadiusXFrom();
			radiusXAnimation.To = RadiusXTo();
			Storyboard.SetTarget(radiusXAnimation, xTarget);
			Storyboard.SetTargetProperty(radiusXAnimation, new PropertyPath(XPropertyPath()));
			radiusXStoryboard.Children.Add(radiusXAnimation);
			MediaRoot.Resources.Add("radiusXStoryboard", radiusXStoryboard);
			radiusXStoryboard.Completed += new EventHandler(radiusXStoryboard_Completed);

			radiusYStoryboard = new Storyboard();
			radiusYAnimation = new DoubleAnimation();
			radiusYAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(duration));
			radiusYAnimation.From = RadiusYFrom();
			radiusYAnimation.To = RadiusYTo();
			Storyboard.SetTarget(radiusYAnimation, yTarget);
			Storyboard.SetTargetProperty(radiusYAnimation, new PropertyPath(YPropertyPath()));
			radiusYStoryboard.Children.Add(radiusYAnimation);
			MediaRoot.Resources.Add("radiusYStoryboard", radiusYStoryboard);

			radiusXStoryboard.Begin();
			radiusYStoryboard.Begin();
		}

		/// <summary>
		/// Handles the SizeChanged event of the Content control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_SizeChanged(object sender, EventArgs e)
		{
			OnResize();
			radiusXAnimation.To = RadiusXTo();
			radiusYAnimation.To = RadiusYTo();
		}

		/// <summary>
		/// Handles the Completed event of the radiusXStoryboard control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void radiusXStoryboard_Completed(object sender, EventArgs e)
		{
			SkipToFill();
			ExecutionComplete(this, transitionEventArgs);
		}

		/// <summary>
		/// Skips to fill.
		/// </summary>
		public void SkipToFill()
		{
			radiusXStoryboard.SkipToFill();
			radiusYStoryboard.SkipToFill();
			toSlide.Clip = null;
			fromSlide.Clip = null;
			MediaRoot.Children.Remove(fromSlide);
		}
	}
}