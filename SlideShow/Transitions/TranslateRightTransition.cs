namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' slide rightward. 
	/// </summary>
	public class TranslateRightTransition : TranslateTransition
	{
		/// <summary>
		/// Gets the in translation animation from.
		/// </summary>
		/// <returns>a double</returns>
		protected override double inTranslationAnimationFrom()
		{
			return -App.Current.Host.Content.ActualWidth;
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
			return "(Canvas.Left)";
		}
	}
}