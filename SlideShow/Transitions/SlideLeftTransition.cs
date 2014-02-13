namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Translates the 'to' and 'from' slides leftward. 
	/// </summary>
	public class SlideLeftTransition : HorizontalSlideTransition
	{
		/// <summary>
		/// Goes to the right.
		/// </summary>
		/// <returns>A boolean indicating the go right value</returns>
		protected override bool GoRight()
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