namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' and 'from' slides horizontally. 
	/// </summary>
	public abstract class HorizontalSlideTransition : SlideTransition
	{
		protected abstract bool GoRight();
		private bool goRight { get; set; }

		/// <summary>
		/// Gets the in translation animation from.
		/// </summary>
		/// <returns>A double indicating where the in translation is coming from</returns>
		protected override double inTranslationAnimationFrom() 
		{
			goRight = GoRight();

			return
				goRight ?
				App.Current.Host.Content.ActualWidth :
				-App.Current.Host.Content.ActualWidth;
		}

		/// <summary>
		/// Gets the in translation animation to.
		/// </summary>
		/// <returns>A double indicating where the in translation is going to</returns>
		protected override double inTranslationAnimationTo()
		{
			return 0;
		}

		/// <summary>
		/// Gets the out translation animation from.
		/// </summary>
		/// <returns>A double indicating where the out translation is coming from</returns>
		protected override double outTranslationAnimationFrom()
		{
			return 0;
		}

		/// <summary>
		/// Gets the out translation animation to.
		/// </summary>
		/// <returns>A double indicating where the out translation is going to </returns>
		protected override double outTranslationAnimationTo()
		{
			return
				goRight ?
				-App.Current.Host.Content.ActualWidth :
				App.Current.Host.Content.ActualWidth;
		}

		/// <summary>
		/// Gets the in translation animation property path.
		/// </summary>
		/// <returns>The left coordinate of the in translation</returns>
		protected override string inTranslationAnimationPropertyPath()
		{
			return "(Canvas.Left)";
		}

		/// <summary>
		/// Gets the out translation animation property path.
		/// </summary>
		/// <returns>The left coordinate of the out translation</returns>
		protected override string outTranslationAnimationPropertyPath()
		{
			return "(Canvas.Left)";
		}
	}
}