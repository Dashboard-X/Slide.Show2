namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Defines a circle transition, as a child of ShapeTransition.
	/// </summary>
	public class CircleOutTransition : CircleTransition
	{
		/// <summary>
		/// Goes forward.
		/// </summary>
		/// <returns>a boolean value indicating go farward</returns>
		protected override bool GoForward()
		{
			if (fromSlideIndex == TransitionManager.Mod(toSlideIndex - 1, currentAlbumLength))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}