using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Vertigo.SlideShow.Controls
{
    /// <summary>
    /// Represents the AlbumPage
    /// </summary>
	[TemplatePart(Name = AlbumPage.ElementRootName, Type = typeof(FrameworkElement))]
	[TemplatePart(Name = AlbumPage.ElementAlbumButtonGridName, Type = typeof(Grid))]
	public class AlbumPage : Control
	{
		#region [ Template Parts ]

		internal FrameworkElement _elementRoot;
		internal const string ElementRootName = "RootElement";

		internal Grid _elementAlbumButtonGrid;
		internal const string ElementAlbumButtonGridName = "AlbumButtonGridElement";

		#endregion

		#region [ Private Properties ]

        /// <summary>
        /// Gets or sets the root element.
        /// </summary>
        /// <value>The root element.</value>
		private FrameworkElement RootElement
		{
			get { return _elementRoot; }
			set { _elementRoot = value; }
		}

        /// <summary>
        /// Gets or sets the album button grid element.
        /// </summary>
        /// <value>The album button grid element.</value>
		private Grid AlbumButtonGridElement
		{
			get { return _elementAlbumButtonGrid; }
			set { _elementAlbumButtonGrid = value; }
		}

		#endregion

		#region [ Public Properties ]

        /// <summary>
        /// Gets the column count.
        /// </summary>
        /// <value>The column count.</value>
		public int ColumnCount { get; private set; }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
		public int RowCount { get; private set; }

        /// <summary>
        /// Gets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
		public int PageIndex { get; private set; }

        /// <summary>
        /// Gets the page count.
        /// </summary>
        /// <value>The page count.</value>
		public int PageCount { get; private set; }

		#endregion

		#region [ Events ]

		/// <summary>
		/// AlbumClicked event handler
		/// </summary>
		/// <param name="sender">the sending object</param>
		/// <param name="e">the event args</param>
		public delegate void AlbumButtonClickedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when [album button clicked].
        /// </summary>
		public event AlbumButtonClickedEventHandler AlbumButtonClicked;

        /// <summary>
        /// Defines the PageNumberChangedEventHandler
        /// </summary>
		public delegate void PageNumberChangedEventHandler();

        /// <summary>
        /// Occurs when [page number changed].
        /// </summary>
		public event PageNumberChangedEventHandler PageNumberChanged;

		#endregion

		#region [ Constructor ]

		/// <summary>
        /// Initializes a new instance of the <see cref="AlbumPage"/> class.
        /// </summary>
		public AlbumPage()
		{
			Options.AlbumPageOptions options = Configuration.Options.AlbumPage;
			Background = options.Background;
			IsTabStop = true;
			DefaultStyleKey = typeof(AlbumPage);
		}

		#endregion

		/// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
        /// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			object root = GetTemplateChild(ElementRootName);
			Debug.Assert(typeof(FrameworkElement).IsInstanceOfType(root) || (root == null),
				"The template part RootElement is not an instance of FrameworkElement!");
			
			RootElement = root as FrameworkElement;
			if (RootElement != null)
			{
				object albumButtonGrid = GetTemplateChild(ElementAlbumButtonGridName);
				Debug.Assert(typeof(Grid).IsInstanceOfType(albumButtonGrid) || (albumButtonGrid == null),
					"The template part AlbumButtonGridElement is not an instance of Grid!");
				
				AlbumButtonGridElement = albumButtonGrid as Grid;

				Configuration.DataProvider.DataFinishedLoading += new EventHandler(Content_Resized);
				Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_Resized);
			}
		}

        /// <summary>
        /// Sets the page number.
        /// </summary>
        /// <param name="pageNum">The page number.</param>
		public void SetPage(int pageNum)
		{
			if (pageNum > PageCount - 1 && PageCount > 0)
			{
				PageIndex = PageCount - 1;
			}
			else if (pageNum < 0)
			{
				PageIndex = 0;
			}
			else
			{
				PageIndex = pageNum;
			}
			
			UpdateArrangement();
		}

		/// <summary>
		/// Handles the Resized event of the Content control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Content_Resized(object sender, EventArgs e)
		{
			if (AlbumButtonGridElement != null)
			{
				Width = Application.Current.Host.Content.ActualWidth;
				double navHeight = Configuration.Options.SlideThumbnail.Height + 8; // 8 = padding around thumbnail

				// Make sure not to set a negative height:
				if (navHeight <= Application.Current.Host.Content.ActualHeight)
				{
					Height = Application.Current.Host.Content.ActualHeight - navHeight;
				}
			}

			SetPage(PageIndex);
		}

		/// <summary>
		/// Updates the arrangement of elements.
		/// </summary>
		private void UpdateArrangement()
		{
			if (AlbumButtonGridElement == null || Data.Albums == null)
			{
				return;
			}
			else if (Data.Albums.Length < 1)
			{
				return;
			}

			Options.AlbumButtonOptions options = Configuration.Options.AlbumButton;

			// All buttons must be the same size:
			double albumButtonWidth = options.Width;
			double albumButtonHeight = options.Height;
			double albumButtonPadding = options.Padding;

			double availWidth = Width;
			double availHeight = Height - Configuration.Options.AlbumViewer.PageNumberFontSize;

			if (albumButtonWidth == 0 || albumButtonHeight == 0)
			{
				return;
			}

			// The remainder after adding the maximum number of buttons on the page
			double widthResidue = availWidth % (albumButtonWidth + 2 * albumButtonPadding);
			double heightResidue = availHeight % (albumButtonHeight + 2 * albumButtonPadding);

			ColumnCount = (int)((availWidth - widthResidue) / (albumButtonWidth + 2 * albumButtonPadding));
			RowCount = (int)((availHeight - heightResidue) / (albumButtonHeight + 2 * albumButtonPadding));

			// Place atleast one row and column of buttons on page
			// (even if they don't fit)
			ColumnCount = ColumnCount != 0 ? ColumnCount : 1;
			RowCount = RowCount != 0 ? RowCount : 1;

			// Number of buttons on last page
			int pageResidue = Data.Albums.Length % (ColumnCount * RowCount);

			// Number of pages needed
			PageCount = pageResidue == 0 ?
				Data.Albums.Length / (ColumnCount * RowCount) :
				(Data.Albums.Length - pageResidue) / (ColumnCount * RowCount) + 1;

			AlbumButtonGridElement.Children.Clear();
			AlbumButtonGridElement.ColumnDefinitions.Clear();
			AlbumButtonGridElement.RowDefinitions.Clear();

			// Add a left padding
			AlbumButtonGridElement.ColumnDefinitions.Add(
			new ColumnDefinition()
			{
				Width = new GridLength(
					availWidth > albumButtonWidth + 2 * albumButtonPadding ?
					widthResidue / 2 :
					0)
			});

			// Add the columns
			for (int i = 0; i < ColumnCount; i++)
			{
				AlbumButtonGridElement.ColumnDefinitions.Add(
					new ColumnDefinition()
					{
						Width = new GridLength(albumButtonWidth + 2 * albumButtonPadding)
					});
			}

			// Add a right padding
			AlbumButtonGridElement.ColumnDefinitions.Add(
			new ColumnDefinition()
			{
				Width = new GridLength(widthResidue / 2)
			});

			// Add the rows
			for (int i = 0; i < RowCount; i++)
			{
				AlbumButtonGridElement.RowDefinitions.Add(
					new RowDefinition()
					{
						Height = new GridLength(albumButtonHeight + albumButtonPadding)
					});
			}

			#region [ Created By OGITREV ]

			// Add the "Created By OGITREV" logo
			string createdByOGITREV = string.Format(CultureInfo.InvariantCulture,
						@"<Canvas 
							xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
							xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
							Width='133'
							Grid.Row='{0}'
							Grid.Column='{1}'
							HorizontalAlignment='Right'
							VerticalAlignment='Top'>
							<Canvas>
								<Canvas Width='66.2699' Height='17.297' Canvas.Left='53.5548' Canvas.Top='17.6598'>
									<Path Width='9.93008' Height='16.8304' Canvas.Left='56.3398' Canvas.Top='0.232819' Stretch='Fill' Fill='#FF223344' Data='F1 M 61.2816,11.9115L 61.3272,11.9115L 63.426,0.232819L 66.2699,0.232819L 62.7499,17.0633L 59.8594,17.0633L 56.3398,0.232819L 59.1832,0.232819L 61.2816,11.9115 Z '/>
									<Path Width='8.0655' Height='16.8304' Canvas.Left='46.8832' Canvas.Top='0.232819' Stretch='Fill' Fill='#FF223344' Data='F1 M 46.8832,17.0633L 46.8832,14.5459L 52.0582,14.5459L 52.0582,9.6041L 48.3286,9.6041L 48.3286,7.08618L 52.0582,7.08618L 52.0582,2.75027L 47.2096,2.75027L 47.2096,0.232819L 54.9487,0.232819L 54.9487,17.0633L 46.8832,17.0633 Z '/>
									<Path Width='9.37126' Height='16.8304' Canvas.Left='35.7254' Canvas.Top='0.232819' Stretch='Fill' Fill='#FF223344' Data='F1 M 42.2062,10.1638C 41.6935,10.1638 41.0868,10.1401 40.7138,10.0935L 38.6629,17.0633L 35.7254,17.0633L 38.1726,9.44153C 37.1946,8.99826 36.1455,7.90207 36.1455,5.19788C 36.1455,1.60843 37.8005,0.232819 41.0407,0.232819L 45.0967,0.232819L 45.0967,17.0633L 42.2062,17.0633L 42.2062,10.1638 Z M 42.2062,2.6106L 41.1799,2.6106C 39.6185,2.6106 39.0355,3.19315 39.0355,5.19788C 39.0355,7.20261 39.6185,7.7861 41.1799,7.7861L 42.2062,7.7861L 42.2062,2.6106 Z '/>
									<Path Width='8.48554' Height='16.8304' Canvas.Left='26.7126' Canvas.Top='0.232819' Stretch='Fill' Fill='#FF223344' Data='F1 M 32.4003,17.0633L 29.5094,17.0633L 29.5094,2.75027L 26.7126,2.75027L 26.7126,0.232819L 35.1981,0.232819L 35.1981,2.75027L 32.4003,2.75027L 32.4003,17.0633 Z '/>
									<Rectangle Width='2.89044' Height='16.8304' Canvas.Left='22.2224' Canvas.Top='0.232819' Stretch='Fill' Fill='#FF223344'/>
									<Path Width='8.71838' Height='17.297' Canvas.Left='10.9482' Canvas.Top='0' Stretch='Fill' Fill='#FF223344' Data='F1 M 15.1905,10.8158L 13.8386,10.8158L 13.8386,12.4475C 13.8386,14.1263 14.2582,14.7787 15.4243,14.7787C 16.4259,14.7787 16.7757,14.0793 16.7757,13.0072L 16.7757,4.35873C 16.7757,2.93747 16.1922,2.51743 15.4937,2.51743C 14.3518,2.51743 13.8386,3.17032 13.8386,5.68774L 11.0874,5.68774L 11.0874,4.61575C 11.0874,2.09741 12.5334,0 15.4243,0C 18.2686,0 19.6666,1.93484 19.6666,4.31262L 19.6666,12.9835C 19.6666,15.6644 18.0814,17.297 15.7973,17.297C 14.4445,17.297 13.7451,16.8295 12.8831,15.8041L 12.3466,17.0633L 10.9482,17.0633L 10.9482,8.53116L 15.1905,8.53116L 15.1905,10.8158 Z '/>
									<Path Width='8.85808' Height='17.297' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' Fill='#FF223344' Data='F1 M 8.85808,12.9835C 8.85808,15.3612 7.46012,17.297 4.42949,17.297C 1.39889,17.297 0,15.3612 0,12.9835L 0,4.31262C 0,1.93484 1.39889,0 4.42949,0C 7.46012,0 8.85808,1.93484 8.85808,4.31262L 8.85808,12.9835 Z M 5.96811,4.38199C 5.96811,3.12283 5.59507,2.51743 4.42949,2.51743C 3.26437,2.51743 2.89136,3.12283 2.89136,4.38199L 2.89136,12.9141C 2.89136,14.1724 3.26437,14.7787 4.42949,14.7787C 5.59507,14.7787 5.96811,14.1724 5.96811,12.9141L 5.96811,4.38199 Z '/>
								</Canvas>
								<Canvas Width='2.62975' Height='1.57785' Canvas.Left='121.636' Canvas.Top='17.8926'>
									<Path Width='1.00533' Height='1.57785' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' Fill='#FF223344' Data='F1 M 1.00533,0L 1.00533,0.266663L 0.653328,0.266663L 0.653328,1.57785L 0.352005,1.57785L 0.352005,0.266663L 0,0.266663L 0,0L 1.00533,0 Z '/>
									<Path Width='1.3943' Height='1.57785' Canvas.Left='1.23544' Canvas.Top='0' Stretch='Fill' Fill='#FF223344' Data='F1 M 1.23544,1.57785L 1.23544,0L 1.65727,0L 1.93031,1.03595L 1.93486,1.03595L 2.20789,0L 2.62975,0L 2.62975,1.57785L 2.3718,1.57785L 2.3718,0.336487L 2.36768,0.336487L 2.04627,1.57785L 1.81892,1.57785L 1.49751,0.336487L 1.49293,0.336487L 1.49293,1.57785L 1.23544,1.57785 Z '/>
								</Canvas>
								<Canvas Width='45.3642' Height='16.8596' Canvas.Left='1.82809' Canvas.Top='18.0618'>
									<Path Width='5.35449' Height='7.56787' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' Fill='#FF223344' Data='F1 M 5.35449,6.4324C 5.35449,6.49356 5.35255,6.547 5.34879,6.59277C 5.34491,6.63867 5.33827,6.67871 5.3288,6.7131C 5.31922,6.74744 5.3069,6.77792 5.2917,6.8046C 5.27653,6.8313 5.24993,6.86374 5.21192,6.90164C 5.1738,6.93967 5.09402,6.99753 4.97235,7.07535C 4.85068,7.1532 4.69967,7.2291 4.51912,7.30307C 4.33854,7.37717 4.13127,7.43982 3.89751,7.49106C 3.66375,7.54221 3.40807,7.56787 3.1306,7.56787C 2.65157,7.56787 2.21909,7.48795 1.8333,7.32816C 1.44739,7.16837 1.11857,6.93259 0.846805,6.62051C 0.574923,6.30847 0.365828,5.92325 0.219497,5.46475C 0.0731657,5.00635 0,4.47836 0,3.88098C 0,3.26846 0.078869,2.72244 0.236608,2.24304C 0.394347,1.76367 0.615666,1.35745 0.90078,1.02451C 1.18592,0.691559 1.52706,0.437622 1.92428,0.262543C 2.32148,0.0875549 2.76136,0 3.24419,0C 3.45704,0 3.6642,0.019989 3.86565,0.0598145C 4.06712,0.0996399 4.25338,0.149872 4.42437,0.21048C 4.59534,0.27121 4.74738,0.341278 4.88048,0.420929C 5.01356,0.50061 5.10566,0.56604 5.15704,0.617157C 5.20839,0.668427 5.24161,0.707428 5.25678,0.734161C 5.27197,0.760864 5.28429,0.792358 5.29388,0.828644C 5.30335,0.86496 5.31099,0.907867 5.31671,0.95752C 5.32242,1.00729 5.32527,1.06641 5.32527,1.13513C 5.32527,1.21158 5.32139,1.27652 5.31375,1.32996C 5.3061,1.38351 5.29467,1.42834 5.27939,1.46463C 5.2641,1.50095 5.24593,1.52768 5.22494,1.54477C 5.20393,1.56201 5.17814,1.57056 5.14755,1.57056C 5.09402,1.57056 5.01949,1.53336 4.92396,1.45895C 4.82842,1.38452 4.70516,1.30234 4.55425,1.21262C 4.40324,1.12289 4.21984,1.04086 4.00389,0.966431C 3.78793,0.891998 3.52906,0.854706 3.22728,0.854706C 2.89844,0.854706 2.5993,0.920288 2.32992,1.05161C 2.06056,1.18283 1.83022,1.37598 1.63915,1.63083C 1.4481,1.88583 1.29994,2.19687 1.19491,2.56393C 1.08979,2.93112 1.03729,3.35056 1.03729,3.8223C 1.03729,4.29025 1.08785,4.70401 1.18921,5.06351C 1.29045,5.4231 1.43564,5.72375 1.62489,5.96524C 1.81414,6.20688 2.04628,6.3895 2.32148,6.51309C 2.59666,6.63684 2.90816,6.69861 3.25604,6.69861C 3.55029,6.69861 3.80733,6.66251 4.02718,6.59024C 4.247,6.51816 4.43431,6.43735 4.58908,6.34793C 4.74385,6.2587 4.87099,6.17789 4.97041,6.10562C 5.06971,6.03354 5.14813,5.99731 5.20542,5.99731C 5.23212,5.99731 5.25507,6.00314 5.27424,6.01456C 5.29332,6.02597 5.3086,6.048 5.32001,6.08041C 5.33142,6.11282 5.34011,6.15768 5.34582,6.21497C 5.35152,6.27225 5.35449,6.34476 5.35449,6.4324 Z '/>
									<Path Width='4.92373' Height='7.42178' Canvas.Left='7.24006' Canvas.Top='0.0876465' Stretch='Fill' Fill='#FF223344' Data='F1 M 12.1638,7.32693C 12.1638,7.35739 12.1581,7.384 12.1467,7.40683C 12.1352,7.42966 12.1105,7.44861 12.0725,7.46378C 12.0344,7.47897 11.9812,7.49036 11.9127,7.49802C 11.8442,7.50552 11.7529,7.50943 11.6387,7.50943C 11.5398,7.50943 11.459,7.50552 11.3962,7.49802C 11.3334,7.49036 11.283,7.47803 11.245,7.46103C 11.2068,7.44394 11.1774,7.42017 11.1565,7.38968C 11.1355,7.35934 11.1175,7.32132 11.1023,7.27567L 10.4232,5.53687C 10.3433,5.3392 10.2604,5.15771 10.1748,4.99243C 10.0892,4.82715 9.98738,4.6846 9.86935,4.56485C 9.75135,4.44516 9.61231,4.35202 9.4524,4.28543C 9.29251,4.21902 9.10029,4.1857 8.87568,4.1857L 8.21892,4.1857L 8.21892,7.32706C 8.21892,7.35739 8.21036,7.384 8.19323,7.40683C 8.17612,7.42966 8.14963,7.44778 8.11356,7.46103C 8.07749,7.47415 8.02819,7.4856 7.96564,7.49515C 7.90309,7.50464 7.82434,7.50943 7.72948,7.50943C 7.63453,7.50943 7.55577,7.50464 7.49322,7.49515C 7.43067,7.4856 7.38034,7.47415 7.34245,7.46094C 7.30455,7.44766 7.27795,7.42953 7.26278,7.40671C 7.24759,7.38388 7.24006,7.35727 7.24006,7.32669L 7.24006,0.487274C 7.24006,0.338867 7.27898,0.23526 7.35706,0.176239C 7.43501,0.117218 7.51776,0.0876465 7.60531,0.0876465L 9.17449,0.0876465C 9.36086,0.0876465 9.51587,0.0924377 9.63959,0.101929C 9.7632,0.111542 9.8745,0.12204 9.97344,0.133423C 10.2587,0.182983 10.5107,0.261017 10.7295,0.367676C 10.9482,0.474365 11.1317,0.609619 11.2801,0.773407C 11.4285,0.937347 11.5398,1.12497 11.614,1.33633C 11.6882,1.54773 11.7253,1.78116 11.7253,2.03638C 11.7253,2.28403 11.6919,2.50595 11.6253,2.70215C 11.5586,2.89835 11.4625,3.07175 11.337,3.22217C 11.2113,3.37259 11.0609,3.50305 10.8858,3.61353C 10.7107,3.72403 10.5146,3.81738 10.2976,3.89352C 10.4193,3.94681 10.5296,4.01428 10.6286,4.09598C 10.7274,4.17773 10.8198,4.27563 10.9054,4.38965C 10.991,4.50381 11.0718,4.63498 11.148,4.7832C 11.224,4.93161 11.3001,5.09879 11.3762,5.28506L 12.0382,6.91055C 12.0914,7.04752 12.1257,7.14352 12.1409,7.19861C 12.1561,7.25378 12.1638,7.29654 12.1638,7.32693 Z M 10.688,2.13831C 10.688,1.84885 10.6231,1.60413 10.4936,1.40414C 10.364,1.20419 10.1468,1.06046 9.84207,0.972809C 9.74677,0.946136 9.63915,0.927032 9.51919,0.915619C 9.39911,0.904205 9.24195,0.898529 9.04768,0.898529L 8.21892,0.898529L 8.21892,3.3895L 9.17905,3.3895C 9.43814,3.3895 9.66196,3.35806 9.85065,3.2952C 10.0392,3.23242 10.1964,3.14478 10.3221,3.03247C 10.4479,2.92014 10.5403,2.78772 10.5994,2.63538C 10.6584,2.48312 10.688,2.31738 10.688,2.13831 Z '/>
									<Path Width='4.11266' Height='7.38525' Canvas.Left='14.1706' Canvas.Top='0.0876465' Stretch='Fill' Fill='#FF223344' Data='F1 M 18.2832,7.06668C 18.2832,7.13538 18.2793,7.19543 18.2718,7.24692C 18.2641,7.29849 18.2508,7.34143 18.2317,7.37564C 18.2127,7.41 18.1908,7.43478 18.1661,7.45007C 18.1413,7.46536 18.1137,7.4729 18.0832,7.4729L 14.5362,7.4729C 14.4485,7.4729 14.3657,7.44345 14.2877,7.38443C 14.2096,7.32556 14.1706,7.2218 14.1706,7.07339L 14.1706,0.487152C 14.1706,0.338776 14.2096,0.235138 14.2877,0.176117C 14.3657,0.117218 14.4486,0.0876465 14.5363,0.0876465L 18.0451,0.0876465C 18.0755,0.0876465 18.1031,0.0953064 18.1279,0.110626C 18.1526,0.125885 18.1726,0.150665 18.1879,0.184906C 18.2032,0.219269 18.2155,0.262207 18.2251,0.313629C 18.2346,0.365234 18.2394,0.429077 18.2394,0.50528C 18.2394,0.574005 18.2346,0.634033 18.225,0.685516C 18.2154,0.737091 18.2031,0.779022 18.1878,0.811401C 18.1725,0.843842 18.1525,0.867676 18.1278,0.882965C 18.103,0.898285 18.0754,0.905792 18.0448,0.905792L 15.1494,0.905792L 15.1494,3.22147L 17.6288,3.22147C 17.6592,3.22147 17.6868,3.23013 17.7116,3.24722C 17.7363,3.2645 17.7573,3.28833 17.7744,3.31894C 17.7915,3.34952 17.804,3.39151 17.8116,3.44495C 17.8191,3.49832 17.823,3.56137 17.823,3.63382C 17.823,3.70267 17.8191,3.76193 17.8116,3.81155C 17.804,3.86121 17.7915,3.90128 17.7744,3.93185C 17.7573,3.96243 17.7363,3.98434 17.7116,3.99771C 17.6868,4.01108 17.6592,4.0177 17.6288,4.0177L 15.1494,4.0177L 15.1494,6.65479L 18.083,6.65479C 18.1135,6.65479 18.1411,6.66238 18.166,6.6777C 18.1908,6.69299 18.2127,6.71677 18.2317,6.74915C 18.2508,6.78159 18.2641,6.82358 18.2718,6.87503C 18.2793,6.92664 18.2832,6.99045 18.2832,7.06668 Z '/>
									<Path Width='6.35786' Height='7.45828' Canvas.Left='19.5599' Canvas.Top='0.0511475' Stretch='Fill' Fill='#FF223344' Data='F1 M 25.8691,7.12625C 25.8995,7.21005 25.9157,7.27771 25.9176,7.32919C 25.9195,7.38077 25.9052,7.41983 25.8748,7.44653C 25.8443,7.47314 25.7939,7.49026 25.7236,7.49802C 25.6532,7.50552 25.559,7.50943 25.4411,7.50943C 25.3231,7.50943 25.2289,7.50656 25.1586,7.50085C 25.0882,7.49515 25.0349,7.4856 24.9988,7.47232C 24.9626,7.45898 24.936,7.44083 24.9189,7.41788C 24.9018,7.39505 24.8856,7.36642 24.8704,7.33203L 24.2369,5.52982L 21.1666,5.52982L 20.5616,7.30923C 20.5502,7.3436 20.535,7.37314 20.516,7.39792C 20.4969,7.42282 20.4693,7.44382 20.4332,7.46094C 20.3971,7.47794 20.3457,7.49036 20.2792,7.49802C 20.2125,7.50552 20.126,7.50943 20.0195,7.50943C 19.9091,7.50943 19.8187,7.50464 19.7484,7.49515C 19.678,7.48547 19.6286,7.46753 19.6,7.44083C 19.5715,7.41415 19.5582,7.37509 19.5601,7.32349C 19.5619,7.272 19.5781,7.20435 19.6086,7.12057L 22.0854,0.25647C 22.1006,0.214722 22.1206,0.18045 22.1453,0.153839C 22.17,0.127289 22.2062,0.106415 22.2538,0.091095C 22.3012,0.0758972 22.3622,0.0654297 22.4364,0.0596924C 22.5106,0.0539856 22.6047,0.0511475 22.7189,0.0511475C 22.8406,0.0511475 22.9414,0.0539856 23.0213,0.0596924C 23.1012,0.0654297 23.1659,0.0758972 23.2154,0.091095C 23.2648,0.106415 23.3028,0.128204 23.3295,0.156708C 23.3561,0.185272 23.377,0.220428 23.3923,0.262207L 25.8691,7.12625 Z M 22.6903,1.06653L 22.6846,1.06653L 21.412,4.74817L 23.9801,4.74817L 22.6903,1.06653 Z '/>
									<Path Width='5.52249' Height='7.42178' Canvas.Left='25.8792' Canvas.Top='0.0876465' Stretch='Fill' Fill='#FF223344' Data='F1 M 31.4016,0.509766C 31.4016,0.582031 31.3978,0.644775 31.3902,0.697968C 31.3826,0.751251 31.3703,0.794067 31.3531,0.826385C 31.336,0.858673 31.315,0.882416 31.2904,0.897614C 31.2656,0.912872 31.238,0.92041 31.2076,0.92041L 29.1298,0.92041L 29.1298,7.32669C 29.1298,7.35718 29.1222,7.38388 29.107,7.40671C 29.0918,7.42953 29.0652,7.44766 29.0273,7.46094C 28.9894,7.47415 28.9391,7.4856 28.8765,7.49515C 28.814,7.50464 28.7353,7.50943 28.6404,7.50943C 28.5493,7.50943 28.4715,7.50464 28.407,7.49515C 28.3425,7.4856 28.2912,7.47415 28.2533,7.46094C 28.2155,7.44766 28.1889,7.42953 28.1737,7.40671C 28.1585,7.38388 28.151,7.35718 28.151,7.32669L 28.151,0.92041L 26.0732,0.92041C 26.0427,0.92041 26.0151,0.912872 25.9904,0.897614C 25.9657,0.882416 25.9457,0.858673 25.9305,0.826385C 25.9152,0.794067 25.9029,0.751251 25.8934,0.697968C 25.8838,0.644775 25.8792,0.582031 25.8792,0.509766C 25.8792,0.4375 25.8838,0.37381 25.8934,0.318695C 25.9029,0.26355 25.9152,0.218903 25.9305,0.184692C 25.9457,0.150482 25.9657,0.125763 25.9904,0.110504C 26.0151,0.0953064 26.0427,0.0876465 26.0731,0.0876465L 31.2077,0.0876465C 31.2381,0.0876465 31.2656,0.0953064 31.2904,0.110504C 31.315,0.125763 31.336,0.150482 31.3531,0.184692C 31.3703,0.218903 31.3826,0.26355 31.3902,0.318695C 31.3978,0.37381 31.4016,0.4375 31.4016,0.509766 Z '/>
									<Path Width='4.11266' Height='7.38525' Canvas.Left='33.0503' Canvas.Top='0.0876465' Stretch='Fill' Fill='#FF223344' Data='F1 M 37.1629,7.06668C 37.1629,7.13538 37.159,7.19543 37.1515,7.24692C 37.1438,7.29849 37.1305,7.34143 37.1114,7.37564C 37.0924,7.41 37.0705,7.43478 37.0458,7.45007C 37.021,7.46536 36.9934,7.4729 36.9629,7.4729L 33.4159,7.4729C 33.3282,7.4729 33.2454,7.44345 33.1674,7.38443C 33.0893,7.32556 33.0503,7.2218 33.0503,7.07339L 33.0503,0.487152C 33.0503,0.338776 33.0893,0.235138 33.1674,0.176117C 33.2454,0.117218 33.3283,0.0876465 33.416,0.0876465L 36.9248,0.0876465C 36.9552,0.0876465 36.9828,0.0953064 37.0076,0.110626C 37.0323,0.125885 37.0523,0.150665 37.0676,0.184906C 37.0829,0.219269 37.0952,0.262207 37.1048,0.313629C 37.1143,0.365234 37.1191,0.429077 37.1191,0.50528C 37.1191,0.574005 37.1143,0.634033 37.1047,0.685516C 37.0951,0.737091 37.0828,0.779022 37.0675,0.811401C 37.0522,0.843842 37.0322,0.867676 37.0074,0.882965C 36.9827,0.898285 36.9551,0.905792 36.9245,0.905792L 34.0291,0.905792L 34.0291,3.22147L 36.5084,3.22147C 36.5389,3.22147 36.5665,3.23013 36.5913,3.24722C 36.616,3.2645 36.637,3.28833 36.6541,3.31894C 36.6712,3.34952 36.6836,3.39151 36.6913,3.44495C 36.6988,3.49832 36.7027,3.56137 36.7027,3.63382C 36.7027,3.70267 36.6988,3.76193 36.6913,3.81155C 36.6836,3.86121 36.6712,3.90128 36.6541,3.93185C 36.637,3.96243 36.616,3.98434 36.5913,3.99771C 36.5665,4.01108 36.5389,4.0177 36.5084,4.0177L 34.0291,4.0177L 34.0291,6.65479L 36.9627,6.65479C 36.9932,6.65479 37.0208,6.66238 37.0457,6.6777C 37.0705,6.69299 37.0924,6.71677 37.1114,6.74915C 37.1305,6.78159 37.1438,6.82358 37.1515,6.87503C 37.159,6.92664 37.1629,6.99045 37.1629,7.06668 Z '/>
									<Path Width='5.67588' Height='7.38525' Canvas.Left='39.3416' Canvas.Top='0.0876465' Stretch='Fill' Fill='#FF223344' Data='F1 M 45.0175,3.67184C 45.0175,4.31104 44.9356,4.86942 44.7722,5.34692C 44.6086,5.82449 44.37,6.22021 44.0563,6.53415C 43.7425,6.84799 43.3575,7.08301 42.9011,7.23892C 42.4448,7.39493 41.9009,7.4729 41.2697,7.4729L 39.7067,7.4729C 39.6192,7.4729 39.5364,7.44345 39.4585,7.38443C 39.3805,7.32556 39.3416,7.2218 39.3416,7.07339L 39.3416,0.487152C 39.3416,0.338776 39.3805,0.235138 39.4585,0.176117C 39.5364,0.117218 39.6192,0.0876465 39.7067,0.0876465L 41.3781,0.0876465C 42.0169,0.0876465 42.5569,0.17041 42.998,0.335907C 43.4392,0.501434 43.809,0.739288 44.1076,1.04941C 44.4062,1.3595 44.6324,1.73526 44.7865,2.17651C 44.9405,2.61789 45.0175,3.11633 45.0175,3.67184 Z M 43.9875,3.71109C 43.9875,3.31198 43.938,2.94043 43.839,2.59644C 43.7399,2.25244 43.5848,1.95499 43.3734,1.7041C 43.162,1.45322 42.8946,1.25748 42.5709,1.11685C 42.2472,0.976227 41.8303,0.905792 41.3201,0.905792L 40.3205,0.905792L 40.3205,6.64746L 41.3315,6.64746C 41.8036,6.64746 42.2016,6.58859 42.5253,6.47079C 42.849,6.353 43.1192,6.17239 43.3363,5.92908C 43.5534,5.68582 43.7162,5.38074 43.8247,5.01389C 43.9332,4.64706 43.9875,4.21289 43.9875,3.71109 Z '/>
									<Path Width='4.86505' Height='7.38525' Canvas.Left='34.2202' Canvas.Top='9.4379' Stretch='Fill' Fill='#FF223344' Data='F1 M 39.0852,14.7068C 39.0852,14.9387 39.0557,15.1526 38.9968,15.3485C 38.9379,15.5443 38.8551,15.7203 38.7488,15.876C 38.6423,16.0322 38.514,16.1692 38.3638,16.2869C 38.2136,16.4047 38.0443,16.5038 37.8561,16.5837C 37.6679,16.6636 37.4664,16.7234 37.2516,16.7631C 37.0368,16.8033 36.7906,16.8232 36.513,16.8232L 34.5852,16.8232C 34.4978,16.8232 34.415,16.7937 34.3371,16.7348C 34.2591,16.6759 34.2202,16.5721 34.2202,16.4237L 34.2202,9.8374C 34.2202,9.689 34.2591,9.58536 34.3371,9.52628C 34.415,9.46738 34.4978,9.4379 34.5852,9.4379L 36.2621,9.4379C 36.7032,9.4379 37.0643,9.47971 37.3458,9.56372C 37.6271,9.64725 37.8619,9.76913 38.0501,9.92896C 38.2383,10.089 38.38,10.285 38.4751,10.5172C 38.5701,10.7494 38.6177,11.0119 38.6177,11.305C 38.6177,11.4803 38.5967,11.6479 38.555,11.8076C 38.5132,11.9674 38.4513,12.115 38.3696,12.2501C 38.2879,12.3852 38.1851,12.5062 38.0616,12.6128C 37.938,12.7194 37.7964,12.8071 37.6367,12.8756C 37.8381,12.9135 38.0264,12.9829 38.2012,13.0837C 38.3761,13.1846 38.5292,13.313 38.6603,13.4686C 38.7915,13.6248 38.8951,13.8074 38.9711,14.0165C 39.0471,14.2256 39.0852,14.4557 39.0852,14.7068 Z M 37.6096,11.4006C 37.6096,11.2214 37.585,11.0596 37.5357,10.9146C 37.4864,10.7699 37.4088,10.6469 37.3026,10.5458C 37.1965,10.4449 37.058,10.3677 36.8875,10.3142C 36.717,10.2609 36.4914,10.2342 36.211,10.2342L 35.199,10.2342L 35.199,12.6302L 36.3134,12.6302C 36.5672,12.6302 36.7719,12.5968 36.9273,12.5299C 37.0828,12.4633 37.2117,12.3738 37.314,12.2615C 37.4163,12.149 37.4912,12.0175 37.5385,11.8668C 37.5859,11.7163 37.6096,11.5609 37.6096,11.4006 Z M 38.0553,14.7634C 38.0553,14.5429 38.0201,14.3491 37.95,14.1818C 37.8798,14.0144 37.7774,13.8738 37.6427,13.7597C 37.5081,13.6458 37.3393,13.559 37.1363,13.5001C 36.9334,13.4413 36.6784,13.4118 36.3711,13.4118L 35.199,13.4118L 35.199,16.0123L 36.6214,16.0123C 36.8451,16.0123 37.0405,15.9856 37.2074,15.9326C 37.3743,15.8792 37.5213,15.8003 37.6485,15.6957C 37.7755,15.5912 37.875,15.461 37.9472,15.3053C 38.0192,15.1492 38.0553,14.9689 38.0553,14.7634 Z '/>
									<Path Width='5.31739' Height='7.45825' Canvas.Left='40.0407' Canvas.Top='9.4014' Stretch='Fill' Fill='#FF223344' Data='F1 M 43.189,13.953L 43.189,16.6768C 43.189,16.7074 43.1813,16.7341 43.1663,16.757C 43.1512,16.7798 43.1247,16.7978 43.087,16.811C 43.0494,16.8243 42.9985,16.8357 42.9343,16.8453C 42.8702,16.8549 42.7929,16.8596 42.7024,16.8596C 42.608,16.8596 42.5297,16.8549 42.4675,16.8453C 42.4053,16.8357 42.3544,16.8243 42.3148,16.811C 42.2752,16.7978 42.2478,16.7798 42.2327,16.757C 42.2177,16.7341 42.2101,16.7074 42.2101,16.6768L 42.2101,13.953L 40.1214,9.78397C 40.0795,9.69656 40.0538,9.62808 40.0443,9.57834C 40.0348,9.52902 40.0443,9.49112 40.0729,9.46414C 40.1014,9.43765 40.1528,9.42032 40.227,9.41281C 40.3012,9.40527 40.401,9.4014 40.5266,9.4014C 40.6407,9.4014 40.7329,9.40527 40.8034,9.41281C 40.8737,9.42032 40.9298,9.43088 40.9717,9.44409C 41.0135,9.45755 41.0449,9.47653 41.0659,9.50137C 41.0868,9.52628 41.1068,9.55753 41.1258,9.59567L 42.1474,11.72C 42.242,11.9218 42.3365,12.1332 42.4308,12.3537C 42.525,12.5747 42.6213,12.7975 42.7194,13.0221L 42.7307,13.0221C 42.8175,12.805 42.9069,12.5888 42.9994,12.3738C 43.0918,12.1588 43.1854,11.9464 43.2803,11.7372L 44.3075,9.60117C 44.319,9.56323 44.3351,9.53085 44.3561,9.50436C 44.3769,9.47742 44.4055,9.45755 44.4417,9.44409C 44.4777,9.43088 44.5282,9.42032 44.5929,9.41281C 44.6575,9.40527 44.7393,9.4014 44.8383,9.4014C 44.9753,9.4014 45.0826,9.40622 45.1607,9.41577C 45.2387,9.42538 45.2929,9.44315 45.3234,9.47009C 45.3537,9.49658 45.3642,9.5347 45.3548,9.58423C 45.3452,9.63379 45.3195,9.7002 45.2777,9.78397L 43.189,13.953 Z '/>
								</Canvas>
								<Path Width='2' Height='18.9519' Canvas.Left='48.7562' Canvas.Top='16.7988' Stretch='Fill' StrokeThickness='2' StrokeMiterLimit='2.75' Stroke='#FF000000' Data='F1 M 49.7562,17.7988L 49.7562,34.7507'/>
							</Canvas>
						</Canvas>", 
						(Data.Albums.Count() - (Data.Albums.Count() % ColumnCount)) / ColumnCount, ColumnCount);

			try
			{
				AlbumButtonGridElement.Children.Add((Canvas)System.Windows.Markup.XamlReader.Load(createdByOGITREV));
			}
			catch
			{
			}

			#endregion

			// Add the album buttons
			for (int albumIndex = PageIndex * ColumnCount * RowCount; albumIndex < Data.Albums.Length && albumIndex < (PageIndex + 1) * ColumnCount * RowCount; albumIndex++)
			{
				AlbumButton albumButton = new AlbumButton()
				{
					Title = Data.Albums[albumIndex].Title,
					Description = Data.Albums[albumIndex].Description,
					Index = albumIndex,
					ImageSource = Data.Albums[albumIndex].Thumbnail ??
										Data.Albums[albumIndex].Slides[0].Preview ??
										Data.Albums[albumIndex].Slides[0].Source
				};

				Grid.SetColumn(albumButton, (albumIndex % ColumnCount) + 1);
				Grid.SetRow(albumButton, ((albumIndex - (albumIndex % ColumnCount)) / ColumnCount) % RowCount);

				albumButton.Click += delegate(object sender, RoutedEventArgs e)
				{
					if (AlbumButtonClicked != null)
					{
						AlbumButtonClicked(sender, null);
					}
				};

				AlbumButtonGridElement.Children.Add(albumButton);
			}

			if (PageNumberChanged != null)
			{
				PageNumberChanged();
			}
		}
	}
}