namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' and 'from' slides vertically.
	/// </summary>
	public abstract class VerticalSlideTransition : SlideTransition
	{
		protected abstract bool GoUp();
		private bool goUp { get; set; }

		/// <summary>
		/// Gets the in translation animation from.
		/// </summary>
		/// <returns>a double representing the in translation from value</returns>
		protected override double inTranslationAnimationFrom()
		{
			goUp = GoUp();

			return
				goUp ?
				App.Current.Host.Content.ActualHeight :
				-App.Current.Host.Content.ActualHeight;
		}

		/// <summary>
		/// Gets the in translation animation to.
		/// </summary>
		/// <returns>a double representing the in translation to value</returns>
		protected override double inTranslationAnimationTo()
		{
			return 0;
		}

		/// <summary>
		/// Gets the out translation animation from.
		/// </summary>
		/// <returns>a double representing the out translation from value</returns>
		protected override double outTranslationAnimationFrom()
		{
			return 0;
		}

		/// <summary>
		/// Gets the out translation animation to.
		/// </summary>
		/// <returns>a double representing the out translation to value</returns>
		protected override double outTranslationAnimationTo()
		{
			return
				goUp ?
				-App.Current.Host.Content.ActualHeight :
				App.Current.Host.Content.ActualHeight;
		}

		/// <summary>
		/// Gets the in translation animation property path.
		/// </summary>
		/// <returns>a double representing the canvas left of the in translation</returns>
		protected override string inTranslationAnimationPropertyPath()
		{
			return "(Canvas.Top)";
		}

		/// <summary>
		/// Gets the out translation animation property path.
		/// </summary>
		/// <returns>a double representing the canvas left of the out translation</returns>
		protected override string outTranslationAnimationPropertyPath()
		{
			return "(Canvas.Top)";
		}
	}
}