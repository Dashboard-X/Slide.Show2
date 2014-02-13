using System.Windows.Controls;
using Vertigo.SlideShow.Controls;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Does not animate any properties. Swaps the new image for the old one.
	/// </summary>
	public class NoTransition : ITransition
	{
		private Canvas MediaRoot;
		public event TransitionCompletedEventHandler ExecutionComplete;
		private TransitionEventArgs transitionEventArgs;

		/// <summary>
		/// Initializes a new instance of the <see cref="NoTransition"/> class.
		/// </summary>
		public NoTransition()
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
			SlideViewer toSlide = SlideViewer.Create(Data.Albums[DataHandler.CurrentAlbumIndex].Slides[toSlideIndex]);
			toSlide.Height = App.Current.Host.Content.ActualHeight;
			transitionEventArgs = new TransitionEventArgs(fromSlideIndex, toSlideIndex, null, toSlide, this.GetType(), 0);
			MediaRoot.Children.Add(toSlide);
			ExecutionComplete(this, transitionEventArgs);
		}

		/// <summary>
		/// Skips to fill.
		/// </summary>
		public void SkipToFill()
		{
		}
	}
}