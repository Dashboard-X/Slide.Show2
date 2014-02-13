using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Represents a FlickrDataProvider
	/// </summary>
	public class FlickrDataProvider : IDataProvider
	{
		private const string MethodFindUserId = "flickr.people.findByUsername";
		private const string MethodGetPhotosets = "flickr.photosets.getList";
		private const string MethodGetPhotos = "flickr.photosets.getPhotos";
		private const string FlickrBaseUrl = "http://api.flickr.com/services/rest/?method=";
		private const string ThumbnailFormat = "http://farm{0}.static.flickr.com/{1}/{2}_{3}_t.jpg";
		private const string PreviewFormat = "http://farm{0}.static.flickr.com/{1}/{2}_{3}_m.jpg";
		private const string SourceFormat = "http://farm{0}.static.flickr.com/{1}/{2}_{3}.jpg";
		private const string LinkFormat = "http://flickr.com/photos/{0}/{1}/";
		private const string FlickrDisclaimer = "This product uses the Flickr API but is not endorsed or certified by Flickr.";
		private const string ErrorRequest = "An error occurred during the Flickr request.";

		/// <summary>
		/// Occurs when [data finished loading].
		/// </summary>
		public event EventHandler DataFinishedLoading;

		private string apiKey;
		private string userName;
		private string userId;
		private Photoset[] photosets;

		/// <summary>
		/// Initializes a new instance of the <see cref="FlickrDataProvider"/> class.
		/// </summary>
		/// <param name="initParams">The init params.</param>
		public FlickrDataProvider(Dictionary<string, Dictionary<string, string>> initParams)
		{
			// Priority: 1st check initParams, 2nd check Options["dataProvider"]
			if (initParams.ContainsKey("DataProvider") && initParams["DataProvider"].ContainsKey("ApiKey"))
			{
				// Use init params
				apiKey = initParams["DataProvider"]["ApiKey"];
			}

			if (string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(Configuration.Options.DataProvider.ApiKey))
			{
				// Use the data provider
				apiKey = Configuration.Options.DataProvider.ApiKey;
			}

			// check that we have an api key
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new FlickrRequestException("Flickr API key not specified");
			}

			// Priority: 1st check initParams, 2nd check Options["dataProvider"]
			if (initParams.ContainsKey("DataProvider") && initParams["DataProvider"].ContainsKey("UserName"))
			{
				// use init params
				userName = initParams["DataProvider"]["UserName"];
			}

			if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(Configuration.Options.DataProvider.UserName))
			{
				// use configuration
				userName = Configuration.Options.DataProvider.UserName;
			}

			// check that we have a username
			if (string.IsNullOrEmpty(userName))
			{
				throw new FlickrRequestException("Flickr username not specified");
			}

			BeginRequest();
		}

		/// <summary>
		/// Begins the request.
		/// </summary>
		private void BeginRequest()
		{
			CallFlickrMethod(MethodFindUserId, OnUserIdRequestCompleted,
				new KeyValuePair<string, string>("username", userName));
		}

		/// <summary>
		/// Called when [user id request completed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		private void OnUserIdRequestCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			using (XmlReader reader = XmlReader.Create(new StringReader(e.Result)))
			{
				if (reader.ReadToFollowing("user"))
				{
					userId = reader.GetAttribute("id");
				}
			}

			CallFlickrMethod(MethodGetPhotosets, OnPhotosetsRequestCompleted,
				new KeyValuePair<string, string>("user_id", userId));
		}

		/// <summary>
		/// Called when [photosets request completed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		private void OnPhotosetsRequestCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			List<Photoset> sets = new List<Photoset>();

			using (XmlReader reader = XmlReader.Create(new StringReader(e.Result)))
			{
				while (reader.ReadToFollowing("photoset"))
				{
					Photoset set = new Photoset();
					set.Id = reader.GetAttribute("id");

					using (XmlReader subReader = reader.ReadSubtree())
					{
						while (subReader.Read())
						{
							if (subReader.IsStartElement())
							{
								switch (subReader.Name)
								{
									case "title":
										set.Title = subReader.ReadElementContentAsString();
										break;
									case "description":
										set.Description = subReader.ReadElementContentAsString();
										break;
								}
							}
						}
					}

					sets.Add(set);
				}
			}

			photosets = sets.ToArray();

			foreach (Photoset set in sets)
			{
				CallFlickrMethod(MethodGetPhotos, OnPhotosRequestCompleted,
					new KeyValuePair<string, string>("photoset_id", set.Id),
					new KeyValuePair<string, string>("extras", "icon_server,date_taken"));
			}
		}

		/// <summary>
		/// Called when [photos request completed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		private void OnPhotosRequestCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			string setId = string.Empty;
			List<Photo> photos = new List<Photo>();

			using (XmlReader reader = XmlReader.Create(new StringReader(e.Result)))
			{
				if (reader.ReadToFollowing("photoset"))
				{
					setId = reader.GetAttribute("id");

					while (reader.Read())
					{
						if (reader.IsStartElement() && reader.Name == "photo")
						{
							photos.Add(new Photo()
							{
								Id = reader.GetAttribute("id"),
								Secret = reader.GetAttribute("secret"),
								Server = reader.GetAttribute("server"),
								Farm = reader.GetAttribute("farm"),
								Title = reader.GetAttribute("title"),
								DateTaken = reader.GetAttribute("datetaken")
							});
						}
					}
				}
			}

			Photoset set = (from p in photosets
							where p.Id == setId
							select p).ElementAt(0);

			if (set != null)
			{
				set.Photos = photos.ToArray();
			}

			if (photosets.All(p => p.Photos != null))
			{
				LoadData();
			}
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		private void LoadData()
		{
			List<Album> albums = new List<Album>();

			foreach (Photoset set in photosets)
			{
				List<Slide> slides = new List<Slide>();

				foreach (Photo photo in set.Photos)
				{
					slides.Add(new Slide()
					{
						Title = photo.Title,
						Description =
							string.Format(CultureInfo.InvariantCulture, "Taken {0} ({1})",
								photo.DateTaken,
								FlickrDisclaimer),
						Thumbnail = new Uri(
							string.Format(CultureInfo.InvariantCulture, ThumbnailFormat,
								photo.Farm,
								photo.Server,
								photo.Id,
								photo.Secret),
							UriKind.Absolute),
						Preview = new Uri(
							string.Format(CultureInfo.InvariantCulture, PreviewFormat,
								photo.Farm,
								photo.Server,
								photo.Id,
								photo.Secret),
							UriKind.Absolute),
						Source = new Uri(
							string.Format(CultureInfo.InvariantCulture, SourceFormat,
								photo.Farm,
								photo.Server,
								photo.Id,
								photo.Secret),
							UriKind.Absolute),
						Link = new Uri(
							string.Format(CultureInfo.InvariantCulture, LinkFormat,
								userId,
								photo.Id),
							UriKind.Absolute)
					});
				}

				albums.Add(new Album()
				{
					Title = set.Title,
					Description = set.Description,
					Thumbnail = null,
					Slides = slides.ToArray()
				});
			}

			Data.Albums = albums.ToArray();

			OnDataFinishedLoading();
		}

		/// <summary>
		/// Calls the flickr method.
		/// </summary>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="asyncResult">The async result.</param>
		/// <param name="parameters">The parameters.</param>
		private void CallFlickrMethod(string methodName, DownloadStringCompletedEventHandler asyncResult, params KeyValuePair<string, string>[] parameters)
		{
			StringBuilder url = new StringBuilder();

			url.Append(FlickrBaseUrl + methodName);
			url.AppendFormat(CultureInfo.InvariantCulture, "&api_key={0}", apiKey);

			foreach (KeyValuePair<string, string> param in parameters)
			{
				url.AppendFormat(CultureInfo.InvariantCulture, "&{0}={1}", param.Key, param.Value);
			}

			WebClient client = new WebClient();
			client.DownloadStringCompleted += asyncResult;
			client.DownloadStringAsync(new Uri(url.ToString(), UriKind.Absolute));
		}

		/// <summary>
		/// Scans the response.
		/// </summary>
		/// <param name="response">The <see cref="System.Net.DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		private void ScanResponse(DownloadStringCompletedEventArgs response)
		{
			if (response.Error != null)
			{
				throw new Exception(ErrorRequest, response.Error);
			}
		}

		/// <summary>
		/// Called when [data finished loading].
		/// </summary>
		private void OnDataFinishedLoading()
		{
			if (DataFinishedLoading != null)
			{
				DataFinishedLoading(this, new EventArgs());
			}
		}

		/// <summary>
		/// Represents a Photoset
		/// </summary>
		public class Photoset
		{
			/// <summary>
			/// Gets or sets the id.
			/// </summary>
			/// <value>The id.</value>
			public string Id { get; set; }

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
			/// Gets or sets the photos.
			/// </summary>
			/// <value>The photos.</value>
			public Photo[] Photos { get; set; }
		}

		/// <summary>
		/// Represents a Photo
		/// </summary>
		public class Photo
		{
			/// <summary>
			/// Gets or sets the id.
			/// </summary>
			/// <value>The id.</value>
			public string Id { get; set; }

			/// <summary>
			/// Gets or sets the secret.
			/// </summary>
			/// <value>The secret.</value>
			public string Secret { get; set; }

			/// <summary>
			/// Gets or sets the server.
			/// </summary>
			/// <value>The server.</value>
			public string Server { get; set; }

			/// <summary>
			/// Gets or sets the farm.
			/// </summary>
			/// <value>The farm.</value>
			public string Farm { get; set; }

			/// <summary>
			/// Gets or sets the title.
			/// </summary>
			/// <value>The title.</value>
			public string Title { get; set; }

			/// <summary>
			/// Gets or sets the date taken.
			/// </summary>
			/// <value>The date taken.</value>
			public string DateTaken { get; set; }
		}
	}

	/// <summary>
	/// Represents a FlickrRequestException
	/// </summary>
	public class FlickrRequestException : Exception
	{
		/// <summary>
		/// Gets the code.
		/// </summary>
		/// <value>The code.</value>
		public string Code { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FlickrRequestException"/> class.
		/// </summary>
		public FlickrRequestException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FlickrRequestException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public FlickrRequestException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FlickrRequestException"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="message">The message.</param>
		public FlickrRequestException(string code, string message) : base(message)
		{
			Code = code;
		}
	}
}