using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Vertigo.SlideShow
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			Startup += App_Startup;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			InstantiateConfigurationProvider(e);
		}

		/// <summary>
		/// Breaks up init params into dictionaries of key-value pairs of configuration settings.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.StartupEventArgs"/> instance containing the event data.</param>
		/// <param name="initParams">The init params.</param>
		private void SplitStartupEventArgs(StartupEventArgs e, ref Dictionary<string, Dictionary<string, string>> initParams)
		{
			// TODO: # 385 - Make sure the user doesn't identify the same key twice or we'll get an exception when adding to dictionary.
			Dictionary<string, string> subParams;

			foreach (KeyValuePair<string, string> subParamsCollection in e.InitParams)
			{
				subParams = new Dictionary<string, string>();
				foreach (string subParamPair in subParamsCollection.Value.Split(';'))
				{
					// only want to split at the first '='
					int indexOfEqualSign = subParamPair.IndexOf('=');

					// '=' not found
					if (indexOfEqualSign < 0)
					{
						subParams.Add(subParamsCollection.Key, subParamPair);
					}
					else // '=' found 
					{
						string key = subParamPair.Substring(0, indexOfEqualSign);
						string value = string.Empty;

						// unlikely, but safe 
						if (indexOfEqualSign + 1 < subParamPair.Length)
							value = subParamPair.Substring(indexOfEqualSign + 1);

						subParams.Add(key, value);
					}
				}

				initParams.Add(subParamsCollection.Key, subParams);
			}
		}

		/// <summary>
		/// Instantiates the configuration provider.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.StartupEventArgs"/> instance containing the event data, specified in the html object tag.</param>
		private void InstantiateConfigurationProvider(StartupEventArgs e)
		{
			Dictionary<string, Dictionary<string, string>> initParams = new Dictionary<string, Dictionary<string, string>>();
			SplitStartupEventArgs(e, ref initParams);

			Type ConfigurationProviderType = null;

			if (e.InitParams.ContainsKey("ConfigurationProvider"))
			{
				switch (initParams["ConfigurationProvider"]["ConfigurationProvider"].ToLower())
				{
					case "xmlconfigurationprovider":
						ConfigurationProviderType = typeof(XmlConfigurationProvider);
						break;
					case "lighttheme":
						ConfigurationProviderType = typeof(LightTheme);
						break;
					case "darktheme":
						ConfigurationProviderType = typeof(DarkTheme);
						break;
					case "simpletheme":
						ConfigurationProviderType = typeof(SimpleTheme);
						break;
					default:
						ConfigurationProviderType = typeof(LightTheme);
						break;
				}
			}
			else
			{
				ConfigurationProviderType = typeof(LightTheme);
			}

			ConfigurationProvider.ConfigurationFinishedLoading += new EventHandler(ConfigurationProvider_ConfigurationFinishedLoading);
			ConfigurationProvider.LoadConfigurationProvider(ConfigurationProviderType, initParams);
		}

		private void ConfigurationProvider_ConfigurationFinishedLoading(object sender, EventArgs e)
		{
			if (Configuration.DataProvider != null)
			{
				Configuration.DataProvider.DataFinishedLoading += new EventHandler(DataProvider_DataFinishedLoading);
			}

			// Load the main control
			RootVisual = new Page();
		}

		/// <summary>
		/// Validates and sanitizes the data.
		/// </summary>
		/// <returns>a stribg</returns>
		private string ValidateData()
		{
			int numberOfEmptyAlbums = 0;
			int numberOfEmptySlides = 0;

			foreach (Album a in Data.Albums)
			{
				a.Description = a.Description ?? string.Empty;
				a.Title = a.Title ?? string.Empty;
				if (a.Slides.Length == 0)
				{
					numberOfEmptyAlbums++;
				}

				foreach (Slide s in a.Slides)
				{
					s.Description = s.Description ?? string.Empty;
					s.Title = s.Title ?? string.Empty;
					s.Transition = s.Transition ?? Data.FALLBACK_TRANSITION;
					if (s.Source == null)
					{
						numberOfEmptySlides++;
					}
				}
			}

			if (numberOfEmptyAlbums == 0 && numberOfEmptySlides == 0)
			{
				return null;
			}
			else
			{
				string dataValidationError = string.Empty;
				if (numberOfEmptyAlbums > 0)
				{
					dataValidationError += "\nFound " + numberOfEmptyAlbums + " album(s) without any slides.";
				}

				if (numberOfEmptySlides > 0)
				{
					dataValidationError += "\nFound " + numberOfEmptySlides + " slide(s) without a source set.";
				}

				return dataValidationError;
			}
		}

		/// <summary>
		/// Handles the DataFinishedLoading event of the DataProvider control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void DataProvider_DataFinishedLoading(object sender, EventArgs e)
		{
			string dataValidationError = ValidateData();

			if (dataValidationError != null)
			{
				// TODO: #386 - Throw exception
				Page.Alert(dataValidationError);
				return;
			}

			new DataHandler();

			((Page)RootVisual).ApplyConfiguration();

			Navigation.StartSlideShow();
		}
	}
}