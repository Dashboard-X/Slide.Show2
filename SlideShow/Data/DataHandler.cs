using System;
using System.Windows;
using System.Collections.Generic;
using Vertigo.SlideShow.Controls;
using Vertigo.SlideShow.Transitions;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Wrapper for Data.
	/// </summary>
	public class DataHandler
	{
		/// <summary>
		/// Gets or sets the index of the current album.
		/// </summary>
		/// <value>The index of the current album.</value>
		public static int CurrentAlbumIndex { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is preloaded.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is preloaded; otherwise, <c>false</c>.
		/// </value>
		public static bool IsPreloaded { get; set; }

		/// <summary>
		/// Gets or sets the preloader.
		/// </summary>
		/// <value>The preloader.</value>
		public static DataPreloader Preloader { get; set; }

		/// <summary>
		/// Gets or sets the albums.
		/// </summary>
		/// <value>The albums.</value>
		public static AlbumHandler[] Albums { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataHandler"/> class.
		/// </summary>
		public DataHandler()
		{
			CurrentAlbumIndex = Data.StartAlbumIndex;
			IsPreloaded = false;
			Preloader = new DataPreloader();

			List<AlbumHandler> albumList = new List<AlbumHandler>();

			foreach (Album a in Data.Albums)
			{
				albumList.Add(new AlbumHandler(a));
			}

			Albums = albumList.ToArray();
		}
	}

	/// <summary>
	/// Wrapper for Album.
	/// </summary>
	public class AlbumHandler
	{
		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the thumbnail.
		/// </summary>
		/// <value>The thumbnail.</value>
		public Uri Thumbnail { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is preloaded.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is preloaded; otherwise, <c>false</c>.
		/// </value>
		public bool IsPreloaded { get; set; }

		/// <summary>
		/// Gets or sets the transition manager.
		/// </summary>
		/// <value>The transition manager.</value>
		public TransitionManager TransitionManager { get; set; }

		/// <summary>
		/// Gets or sets the thumbnail viewer.
		/// </summary>
		/// <value>The thumbnail viewer.</value>
		public ThumbnailViewer ThumbnailViewer { get; set; }

		/// <summary>
		/// Gets or sets the slides.
		/// </summary>
		/// <value>The slides.</value>
		public SlideHandler[] Slides { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AlbumHandler"/> class.
		/// </summary>
		/// <param name="album">The album.</param>
		public AlbumHandler(Album album)
		{
			Title = album.Title;
			Description = album.Description;
			Thumbnail = album.Thumbnail;
			IsPreloaded = false;
			this.ThumbnailViewer = new ThumbnailViewer(album);

			List<SlideHandler> slideList = new List<SlideHandler>();

			foreach (Slide a in album.Slides)
			{
				slideList.Add(new SlideHandler(a));
			}

			Slides = slideList.ToArray();
		}
	}

	/// <summary>
	/// Wrapper for Slide.
	/// </summary>
	public class SlideHandler
	{
		/// <summary>
		/// Occurs when [to slide finished loading].
		/// </summary>
		public event EventHandler ToSlideFinishedLoading;

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the source.
		/// </summary>
		/// <value>The source.</value>
		public Uri Source { get; set; }

		/// <summary>
		/// Gets or sets the preview.
		/// </summary>
		/// <value>The preview.</value>
		public Uri Preview { get; set; }

		/// <summary>
		/// Gets or sets the thumbnail.
		/// </summary>
		/// <value>The thumbnail.</value>
		public Uri Thumbnail { get; set; }

		/// <summary>
		/// Gets or sets the transition.
		/// </summary>
		/// <value>The transition.</value>
		public string Transition { get; set; }

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		/// <value>The link.</value>
		public Uri Link { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is preloaded.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is preloaded; otherwise, <c>false</c>.
		/// </value>
		public bool IsPreloaded { get; set; }

		/// <summary>
		/// Gets or sets a list of PeopleTagDefinitions for the slide.
		/// </summary>
		/// <value>A list of PeopleTagDefinitions for the slide.</value>
		public List<PeopleTagDefinition> PeopleTagDefinitions { get; set; }

		/// <summary>
		/// Gets or sets the size of the slide's image.
		/// </summary>
		/// <value>The size of the slide's image.</value>
		public Size ImageSize { get; set; } 

		/// <summary>
		/// Initializes a new instance of the <see cref="SlideHandler"/> class.
		/// </summary>
		/// <param name="slide">The slide.</param>
		public SlideHandler(Slide slide)
		{
			Title = slide.Title;
			Description = slide.Description;
			Source = slide.Source;
			Preview = slide.Preview;
			Thumbnail = slide.Thumbnail;
			Transition = slide.Transition;
			Link = slide.Link;
			IsPreloaded = false;
			PeopleTagDefinitions = new List<PeopleTagDefinition>();
		}

		/// <summary>
		/// Triggers the slide finished loading event.
		/// </summary>
		public void TriggerToSlideFinishedLoadingEvent()
		{
            if (ToSlideFinishedLoading != null)
            {
                ToSlideFinishedLoading(this, EventArgs.Empty);
            }
		}
	}

	public class PeopleTagDefinition
	{
		/// <summary>
		/// Gets or sets the x position (given as a percentage of the image width) for a PeopleTag in the slide.
		/// </summary>
		/// <value>
		/// 	The x position (given as a percentage of the image width) for a PeopleTag in the slide.
		/// </value>
		public double XPositionPercentage { get; set; }

		/// <summary>
		/// Gets or sets the y position (given as a percentage of the image height) for a PeopleTag in the slide.
		/// </summary>
		/// <value>
		/// 	The y position (given as a percentage of the image height) for a PeopleTag in the slide.
		/// </value>
		public double YPositionPercentage { get; set; }

		/// <summary>
		/// Gets or sets the width (given as a percentage of the image width) for a PeopleTag in the slide.
		/// </summary>
		/// <value>
		/// 	The width (given as a percentage of the image width) for a PeopleTag in the slide.
		/// </value>
		public double WidthPercentage { get; set; }

		/// <summary>
		/// Gets or sets the height (given as a percentage of the image height) for a PeopleTag in the slide.
		/// </summary>
		/// <value>
		/// 	The height (given as a percentage of the image height) for a PeopleTag in the slide.
		/// </value>
		public double HeightPercentage { get; set; }

		/// <summary>
		/// Gets or sets the display name for the PeopleTag in the slide.
		/// </summary>
		/// <value>
		/// 	The display name for the PeopleTag in the slide.
		/// </value>
		public string DisplayName { get; set; }
	}
}