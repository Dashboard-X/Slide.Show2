namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' and 'from' slides rightward. 
	/// </summary>
	public class SlideRightTransition : HorizontalSlideTransition
	{
		/// <summary>
		/// Goes to the right.
		/// </summary>
		/// <returns>A boolean indicating the go right value</returns>
		protected override bool GoRight()
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