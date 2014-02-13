using System;
using System.Globalization;
using System.IO;

namespace Vertigo.SlideShow
{
    /// <summary>
    /// Represents the IDataProvider interface
    /// </summary>
	public interface IDataProvider
	{
        /// <summary>
        /// Occurs when [data finished loading].
        /// </summary>
		event EventHandler DataFinishedLoading;
	}

    /// <summary>
    /// Represents the Data class
    /// </summary>
	public class Data
	{
		/// <summary>
		/// The transition used in case no global or individual transition is specified.
		/// </summary>
		public const string FALLBACK_TRANSITION = "CrossFadeTransition";
		
        /// <summary>
        /// Gets or sets the start index of the album.
        /// </summary>
        /// <value>The start index of the album.</value>
		public static int StartAlbumIndex { get; set; }

        /// <summary>
        /// Gets or sets the albums.
        /// </summary>
        /// <value>The albums.</value>
		public static Album[] Albums { get; set; }
		
		/// <summary>
		/// Gets the source type from the specified URI.
		/// </summary>
		/// <param name="source">The source URI.</param>
		/// <returns>The source type (e.g. image, video).</returns>
		public static DataSourceType GetSourceType(Uri source)
		{
			string ext = Path.GetExtension(source.OriginalString).ToLower(CultureInfo.InvariantCulture);

			switch (ext)
			{
				case ".jpg":
				case ".jpeg":
				case ".png":
				case ".wmf":
				case ".tiff":
				case ".gif":
				case ".exif":
				case ".emf":
				case ".bmp":
					return DataSourceType.Image;

				case ".wmv":
				case ".wma":
				case ".mp3":
					return DataSourceType.Video;

				default:
					return DataSourceType.Unknown;
			}
		}
	}

    /// <summary>
    /// Represents the Album class
    /// </summary>
	public class Album
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
        /// Gets or sets the slides.
        /// </summary>
        /// <value>The slides.</value>
		public Slide[] Slides { get; set; }
	}

    /// <summary>
    /// Represents the Slide class
    /// </summary>
	public class Slide
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
	}

	public enum DataSourceType
	{
		Unknown,
		Image,
		Video
	}
}