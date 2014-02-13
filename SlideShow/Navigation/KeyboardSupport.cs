using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Vertigo.SlideShow
{
	/// <summary>
	/// Handles keydown events and maps methods to certains keys.
	/// </summary>
	public class KeyboardSupport
	{
		/// <summary>
		/// Gets or sets a value indicating whether [listen to input].
		/// </summary>
		/// <value><c>true</c> if [listen to input]; otherwise, <c>false</c>.</value>
		public static bool ListenToInput { get; set; }

		private delegate void KeyboardInputHandler();
		private KeyboardInputHandler Left;
		private KeyboardInputHandler Right;
		private KeyboardInputHandler Down;
		private KeyboardInputHandler Up;
		private KeyboardInputHandler E;
		private KeyboardInputHandler F;
		private KeyboardInputHandler G;
		private KeyboardInputHandler P;
		private KeyboardInputHandler Esc;
		private KeyboardInputHandler Space;
		private KeyboardInputHandler W;
		private KeyboardInputHandler A;
		private KeyboardInputHandler S;
		private KeyboardInputHandler D;
		private KeyboardInputHandler V;

		private Dictionary<Key, KeyboardInputHandler> keyMap;

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyboardSupport"/> class.
		/// </summary>
		public KeyboardSupport()
		{
			ListenToInput = true;

			App.Current.RootVisual.KeyDown += new KeyEventHandler(RootVisual_KeyDown);

			#region [ Key Assignment ]

			Left = new KeyboardInputHandler(KeyboardFunctions.GoToPreviousSlide);
			Right = new KeyboardInputHandler(KeyboardFunctions.GoToNextSlide);
			Down = new KeyboardInputHandler(KeyboardFunctions.GoToPreviousAlbum);
			Up = new KeyboardInputHandler(KeyboardFunctions.GoToNextAlbum);
			E = new KeyboardInputHandler(KeyboardFunctions.ToggleEmbedView);
			F = new KeyboardInputHandler(KeyboardFunctions.ToggleFullScreen);
			G = new KeyboardInputHandler(KeyboardFunctions.ToggleAlbumView);
			P = new KeyboardInputHandler(KeyboardFunctions.TogglePause);
			Space = new KeyboardInputHandler(KeyboardFunctions.GoToNextSlide);
			Esc = new KeyboardInputHandler(KeyboardFunctions.ToggleFullScreen);
			W = new KeyboardInputHandler(KeyboardFunctions.GoToNextSlideW);
			A = new KeyboardInputHandler(KeyboardFunctions.GoToNextSlideA);
			S = new KeyboardInputHandler(KeyboardFunctions.GoToNextSlideS);
			D = new KeyboardInputHandler(KeyboardFunctions.GoToNextSlideD);
			V = new KeyboardInputHandler(KeyboardFunctions.Vertigo);

			#endregion

			#region [ Key Mapping ]

			keyMap = new Dictionary<Key, KeyboardInputHandler>()
			{
				{ Key.Left, Left },
				{ Key.Right, Right },
				{ Key.Down, Down },
				{ Key.Up, Up },
				{ Key.F, F },
				{ Key.E, E },
				{ Key.G, G },
				{ Key.P, P },
				{ Key.Escape, Esc },
				{ Key.Space, Space },
				{ Key.W, W },
				{ Key.A, A },
				{ Key.S, S },
				{ Key.D, D },
				{ Key.V, V }
			};

			#endregion
		}

		/// <summary>
		/// Handles the KeyDown event of the RootVisual control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
		private void RootVisual_KeyDown(object sender, KeyEventArgs e)
		{
			if (ListenToInput)
			{
				if (keyMap.ContainsKey(e.Key))
				{
					keyMap[e.Key].DynamicInvoke();
				}
			}
		}

		/// <summary>
		/// Represents a KeyboardFunctions
		/// </summary>
		public static class KeyboardFunctions
		{
			/// <summary>
			/// Goes to previous slide.
			/// </summary>
			public static void GoToPreviousSlide()
			{
				Navigation.SkipToPreviousSlide(null);
			}

			/// <summary>
			/// Goes to next slide.
			/// </summary>
			public static void GoToNextSlide()
			{
				Navigation.SkipToNextSlide(null);
			}

			/// <summary>
			/// Goes to previous album.
			/// </summary>
			public static void GoToPreviousAlbum()
			{
				Navigation.SelectPreviousAlbum();
			}

			/// <summary>
			/// Goes to next album.
			/// </summary>
			public static void GoToNextAlbum()
			{
				Navigation.SelectNextAlbum();
			}

			/// <summary>
			/// Toggles full screen.
			/// </summary>
			public static void ToggleFullScreen()
			{
				Navigation.ToggleFullScreen();
			}

			/// <summary>
			/// Toggles the embed view.
			/// </summary>
			public static void ToggleEmbedView()
			{
				Navigation.ToggleEmbedView();
			}

			/// <summary>
			/// Toggles the album view.
			/// </summary>
			public static void ToggleAlbumView()
			{
				Navigation.ToggleAlbumView();
			}

			/// <summary>
			/// Toggles pause.
			/// </summary>
			public static void TogglePause()
			{
				Navigation.TogglePause();
			}

			/// <summary>
			/// Goes to next slide W.
			/// </summary>
			public static void GoToNextSlideW()
			{
				Navigation.SkipToNextSlide("SlideDownTransition");
			}

			/// <summary>
			/// Goes to next slide A.
			/// </summary>
			public static void GoToNextSlideA()
			{
				Navigation.SkipToPreviousSlide("SlideLeftTransition");
			}

			/// <summary>
			/// Goes to next slide S.
			/// </summary>
			public static void GoToNextSlideS()
			{
				Navigation.SkipToPreviousSlide("SlideDownTransition");
			}

			/// <summary>
			/// Goes to next slide D.
			/// </summary>
			public static void GoToNextSlideD()
			{
				Navigation.SkipToNextSlide("SlideLeftTransition");
			}

			/// <summary>
			/// Gets or sets the angle.
			/// </summary>
			/// <value>The angle.</value>
			public static int angle { get; set; }

			/// <summary>
			/// Gets or sets the rotate transform.
			/// </summary>
			/// <value>The rotate transform.</value>
			public static RotateTransform rotateTransform { get; set; }

			/// <summary>
			/// Vertigoes this instance. (flips it)
			/// </summary>
			public static void Vertigo()
			{
				angle = (angle + 180) % 360;

				RotateRootVisual(null, null);

				// In case you already attached these event handlers
				Application.Current.Host.Content.Resized -= new EventHandler(RotateRootVisual);
				Application.Current.Host.Content.FullScreenChanged -= new EventHandler(RotateRootVisual);

				// Now add them (back) on
				Application.Current.Host.Content.Resized += new EventHandler(RotateRootVisual);
				Application.Current.Host.Content.FullScreenChanged += new EventHandler(RotateRootVisual);
			}

			/// <summary>
			/// Rotates the root visual.
			/// </summary>
			/// <param name="sender">The sender.</param>
			/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
			public static void RotateRootVisual(object sender, EventArgs e)
			{
				rotateTransform = new RotateTransform()
				{
					Angle = angle,
					CenterX = App.Current.Host.Content.ActualWidth / 2,
					CenterY = App.Current.Host.Content.ActualHeight / 2
				};

				if (rotateTransform != null)
				{
					App.Current.RootVisual.RenderTransform = rotateTransform;
				}
			}
		}
	}
}