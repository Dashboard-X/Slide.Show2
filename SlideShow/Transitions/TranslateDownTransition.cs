namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' slide downward. 
	/// </summary>
	public class TranslateDownTransition : TranslateTransition
	{
		/// <summary>
		/// Gets the in translation animation from.
		/// </summary>
		/// <returns>a double</returns>
		protected override double inTranslationAnimationFrom()
		{
			return -App.Current.Host.Content.ActualHeight;
		}

		/// <summary>
		/// Gets the in translation animation to.
		/// </summary>
		/// <returns>a double</returns>
		protected override double inTranslationAnimationTo()
		{
			return 0;
		}

		/// <summary>
		/// Gets the in translation animation property path.
		/// </summary>
		/// <returns>a string</returns>
		protected override string inTranslationAnimationPropertyPath()
		{
			return "(Canvas.Top)";
		}
	}
}