namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' and 'from' slides downward.
	/// </summary>
	public class SlideDownTransition : VerticalSlideTransition
	{
		/// <summary>
		/// Goes up.
		/// </summary>
		/// <returns>A boolean indicating the go up value</returns>
		protected override bool GoUp()
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