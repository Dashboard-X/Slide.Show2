namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Defines a circle transition, as a child of ShapeTransition.
	/// </summary>
	public class CircleInTransition : CircleTransition
	{
		/// <summary>
		/// Goes forward.
		/// </summary>
		/// <returns>a boolean value representing go forward</returns>
		protected override bool GoForward()
		{
			if (fromSlideIndex == TransitionManager.Mod(toSlideIndex - 1, currentAlbumLength))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}