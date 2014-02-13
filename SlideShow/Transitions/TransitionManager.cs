using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Vertigo.SlideShow.Controls;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// The interface that all transitions must implement.
	/// </summary>
	public interface ITransition
	{
		void Execute(int currentSlideIndex, int nextSlideIndex, int durration); // Should execute transition
		void SkipToFill(); // Should call SkipToFill method on all used storyboards.
		event TransitionCompletedEventHandler ExecutionComplete; // Should be raised when transition finishes
	}

	/// <summary>
	/// Event that must be implemented in a transition and called when it completes.
	/// </summary>
	/// <param name="sender">The sender</param>
	/// <param name="e">The event transition arguments</param>
	public delegate void TransitionCompletedEventHandler(object sender, TransitionEventArgs e);

	/// <summary>
	/// Event arguments used in the completion of a transition.
	/// </summary>
	public class TransitionEventArgs : EventArgs
	{
		public int FromSlideIndex { get; private set; }
		public int ToSlideIndex { get; private set; }
		public SlideViewer FromSlide { get; private set; }
		public SlideViewer ToSlide { get; private set; }
		public Type TransitionUsed { get; private set; }
		public int TransitionDuration { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TransitionEventArgs"/> class.
		/// </summary>
		/// <param name="fromSlideIndex">Index of from slide.</param>
		/// <param name="toSlideIndex">Index of to slide.</param>
		/// <param name="fromSlide">From slide.</param>
		/// <param name="toSlide">To slide.</param>
		/// <param name="transitionUsed">The transition used.</param>
		/// <param name="transitionDuration">Duration of the transition.</param>
		public TransitionEventArgs(int fromSlideIndex, int toSlideIndex, SlideViewer fromSlide, SlideViewer toSlide, Type transitionUsed, int transitionDuration)
		{
			FromSlideIndex = fromSlideIndex;
			ToSlideIndex = toSlideIndex;
			FromSlide = fromSlide;
			ToSlide = toSlide;
			TransitionUsed = transitionUsed;
			TransitionDuration = transitionDuration;
		}
	}

	/// <summary>
	/// Base task to be queued in slideshow playback.
	/// </summary>
	public abstract class Task
	{
		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public int Duration { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether this instance is prioritized input.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is prioritized input; otherwise, <c>false</c>.
		/// </value>
		public bool IsPrioritizedInput { get; set; }
	}

	/// <summary>
	/// Task that pauses between transitions.
	/// </summary>
	public class WaitTask : Task
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WaitTask"/> class.
		/// </summary>
		/// <param name="duration">The duration.</param>
		/// <param name="isPrioritizedInput">if set to <c>true</c> [is prioritized input].</param>
		public WaitTask(int duration, bool isPrioritizedInput)
		{
			this.Duration = duration;
			this.IsPrioritizedInput = isPrioritizedInput;
		}
	}

	/// <summary>
	/// Task that transitions from one slide to another.
	/// </summary>
	public class TransitionTask : Task
	{
		/// <summary>
		/// Gets or sets the index of from slide.
		/// </summary>
		/// <value>The index of from slide.</value>
		public int FromSlideIndex { get; set; }
		
		/// <summary>
		/// Gets or sets the index of to slide.
		/// </summary>
		/// <value>The index of to slide.</value>
		public int ToSlideIndex { get; set; }
		
		/// <summary>
		/// Gets or sets the transition.
		/// </summary>
		/// <value>The transition.</value>
		public string Transition { get; set; }
		
		/// <summary>
		/// Gets or sets the e.
		/// </summary>
		/// <value>The e.</value>
		public TransitionEventArgs e { get; set; }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="TransitionTask"/> class.
		/// </summary>
		/// <param name="fromSlideIndex">Index of from slide.</param>
		/// <param name="toSlideIndex">Index of to slide.</param>
		/// <param name="transition">The transition.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="isPrioritizedInput">if set to <c>true</c> [is prioritized input].</param>
		public TransitionTask(int fromSlideIndex, int toSlideIndex, string transition, int duration, bool isPrioritizedInput)
		{
			int slideCount = DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides.Length;
			FromSlideIndex = TransitionManager.Mod(fromSlideIndex, slideCount);
			ToSlideIndex = TransitionManager.Mod(toSlideIndex, slideCount);
			Duration = duration;
			Transition = transition ?? DataHandler.Albums[DataHandler.CurrentAlbumIndex].Slides[ToSlideIndex].Transition;
			IsPrioritizedInput = isPrioritizedInput;
		}
	}

	/// <summary>
	/// Responsible for playing the slidshow and handling requests to play, pause, and go to specific slides.
	/// </summary>
	public class TransitionManager
	{
		private Dictionary<string, ITransition> TransitionInstances = new Dictionary<string, ITransition>();
		private TransitionEventArgs lastTransitionEventArgs;
		private Stack<Task> TaskStack = new Stack<Task>();
		private AlbumHandler album;
		private ITransition currentTransition;
		private Storyboard WaitStoryboard;
		private DoubleAnimation WaitAnimation;
		
		private int WaitTime { get; set; }
		private int TransitionDuration { get; set; }

		private int toSlideIndex;

		/// <summary>
		/// Gets the index of to slide.
		/// </summary>
		/// <value>The index of to slide.</value>
		public int ToSlideIndex
		{
			get 
			{
				return toSlideIndex;
			}

			private set
			{
				toSlideIndex = Mod(value, album.Slides.Length);
			}
		}

		public Canvas MediaRoot { get; private set; }
		
		/// <summary>
		/// Gets a value indicating whether this instance is waiting before transitioning to the next slide.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is waiting; otherwise, <c>false</c>.
		/// </value>
		public bool IsWaiting { get; private set; }
		
		/// <summary>
		/// Gets a value indicating whether this instance is paused.
		/// </summary>
		/// <value><c>true</c> if this instance is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TransitionManager"/> class.
		/// </summary>
		public TransitionManager()
		{
			album = DataHandler.Albums[DataHandler.CurrentAlbumIndex];
			ToSlideIndex = 0;
			MediaRoot = new Canvas();
			IsWaiting = false;
			IsPaused = true;

			Page page = App.Current.RootVisual as Page;
			if (page != null)
			{
				if (page.mediaRoot != null)
				{
					page.mediaRoot.Children.Add(MediaRoot);
				}
			}

			WaitTime = Configuration.Options.Transition.WaitTime;
			TransitionDuration = Configuration.Options.Transition.TransitionDuration;

			WaitStoryboard = new Storyboard();
			WaitAnimation = new DoubleAnimation();
			WaitAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(WaitTime));
			Storyboard.SetTarget(WaitAnimation, MediaRoot);
			Storyboard.SetTargetProperty((Timeline)WaitAnimation, new PropertyPath("(Canvas.Left)"));
			WaitStoryboard.Children.Add(WaitAnimation);
			MediaRoot.Resources.Add("WaitStoryboard", WaitStoryboard);
			WaitStoryboard.Completed += new EventHandler(WaitStoryboard_Completed);

			// Transition into first slide:
			TaskStack.Push(new TransitionTask(0, 0, null, 0, true));
		}

		/// <summary>
		/// Waits the specified wait time before executing next task.
		/// </summary>
		/// <param name="waitTime">The wait time (milliseconds).</param>
		private void Wait(int waitTime)
		{
			if (IsPaused)
			{
				return;
			}

			WaitAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(waitTime));
			WaitStoryboard.Begin();
			IsWaiting = true;
		}

		/// <summary>
		/// Handles the Completed event of the WaitStoryboard control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void WaitStoryboard_Completed(object sender, EventArgs e)
		{
			IsWaiting = false;
			WaitStoryboard.Stop();

			if (!IsPaused)
			{
				ExecuteNextTask();
			}
		}

		/// <summary>
		/// Handles the ExecutionCompleted event of the transition control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="Vertigo.SlideShow.Transitions.TransitionEventArgs"/> instance containing the event data.</param>
		private void transition_ExecutionCompleted(object sender, TransitionEventArgs e)
		{
			SlideViewer toSlide = e.ToSlide;

			e.ToSlide.MediaCompleted += new RoutedEventHandler(ToSlide_MediaCompleted);

			if (toSlide != null)
			{
				toSlide.Click += delegate
				{
					if (toSlide.Slide.Link == null)
					{
						Navigation.SkipToNextSlide(null);
					}
					else
					{
						System.Windows.Browser.HtmlPage.Window.Navigate(toSlide.Slide.Link, "_blank");
					}
				};
			}

			lastTransitionEventArgs = e;

			if (!IsPaused)
			{
				ExecuteNextTask();
			}
		}

		/// <summary>
		/// Handles the MediaCompleted event of the ToSlide control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void ToSlide_MediaCompleted(object sender, RoutedEventArgs e)
		{
			// If a new album has been selected while media was playing, 
			// do not continue old album's slideshow
			if (album == DataHandler.Albums[DataHandler.CurrentAlbumIndex])
			{
				Play();
			}
		}

		/// <summary>
		/// Executes the next task.
		/// </summary>
		private void ExecuteNextTask()
		{
			if (!TaskStack.Peek().IsPrioritizedInput)
			{
				if (TaskStack.Peek() is TransitionTask)
				{
					if (lastTransitionEventArgs.ToSlide is VideoViewer)
					{
						// SlideShow will continue when media finishes playing
						Pause();
					}

					TaskStack.Pop();
					TaskStack.Push(new WaitTask(WaitTime, false));
				}
				else
				{
					TaskStack.Pop();
					TaskStack.Push(new TransitionTask(ToSlideIndex, ++ToSlideIndex, null, TransitionDuration, false));
				}
			}

			DataHandler.Preloader.PreloadSlides();

			if (TaskStack.Peek().IsPrioritizedInput)
			{
				// Deprioritize the task so it doesn't get run twice
				Task lastTask = TaskStack.Pop();
				lastTask.IsPrioritizedInput = false;
				TaskStack.Push(lastTask);
			}

			if (TaskStack.Peek() is TransitionTask)
			{
				if (album.Slides[((TransitionTask)TaskStack.Peek()).ToSlideIndex].IsPreloaded)
				{
					TransitionToNextSlide();
				}
				else
				{
					album.Slides[ToSlideIndex].ToSlideFinishedLoading += new EventHandler(TransitionManager_ToSlideFinishedLoading);

					DataHandler.Preloader.DisplayProgressIndicator(((TransitionTask)TaskStack.Peek()).ToSlideIndex);
				}
			}
			else
			{
				Wait(TaskStack.Peek().Duration);
			}
		}

		/// <summary>
		/// Handles the ToSlideFinishedLoading event of the SlideHandler control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void TransitionManager_ToSlideFinishedLoading(object sender, EventArgs e)
		{
			DataHandler.Preloader.HideProgressIndicator();
			TransitionToNextSlide();
			((SlideHandler)sender).ToSlideFinishedLoading -= TransitionManager_ToSlideFinishedLoading;
		}

		/// <summary>
		/// Transitions to next slide in the task stack.
		/// </summary>
		private void TransitionToNextSlide()
		{
			TransitionTask task = (TransitionTask)TaskStack.Peek();

			if (!TransitionInstances.ContainsKey(task.Transition))
			{
				// TODO: #387 - Handle possible exception
				Type transitionType =
					Type.GetType("Vertigo.SlideShow.Transitions." + task.Transition, false) ??
					Type.GetType("Vertigo.SlideShow.Transitions." + Data.FALLBACK_TRANSITION, false) ??
					typeof(CrossFadeTransition);

				ITransition transition = (ITransition)Activator.CreateInstance(transitionType);

				TransitionInstances.Add(task.Transition, transition);

				TransitionInstances[task.Transition].ExecutionComplete += new TransitionCompletedEventHandler(transition_ExecutionCompleted);
			}

			Page page = App.Current.RootVisual as Page;

			if (page != null)
			{
				page.UpdateSlideDescription(album.Slides[task.ToSlideIndex].Title, album.Slides[task.ToSlideIndex].Description);

				List<PeopleTag> oldPeopleTags = new List<PeopleTag>();

				foreach (UIElement uiElement in page.mediaRoot.Children)
					if (uiElement is PeopleTag)
						oldPeopleTags.Add(uiElement as PeopleTag);

				foreach (PeopleTag oldTag in oldPeopleTags)
					page.mediaRoot.Children.Remove(oldTag);

				foreach (PeopleTagDefinition tagDefinition in album.Slides[task.ToSlideIndex].PeopleTagDefinitions)
				{
					PeopleTag peopleTag = new PeopleTag();
					peopleTag.TagDefinition = tagDefinition;
					peopleTag.ImageSize = album.Slides[task.ToSlideIndex].ImageSize;
					page.mediaRoot.Children.Add(peopleTag);
				}
			}

			currentTransition = TransitionInstances[task.Transition];
			currentTransition.Execute(task.FromSlideIndex, task.ToSlideIndex, task.Duration);
			DataHandler.Albums[DataHandler.CurrentAlbumIndex].ThumbnailViewer.HighlightThumbnail(task.ToSlideIndex);
		}
		
		/// <summary>
		/// Takes modulus of dividend w.r.t. divisor.
		/// (% operator can return a negative result, this arithmetic 
		/// guarantees a result within the bounds of an array)
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor.</param>
		/// <returns>The mod value as an int</returns>
		public static int Mod(int dividend, int divisor)
		{
			// TODO: #338 - if divisor == 0 throw exception
			divisor = Math.Abs(divisor);
			return ((dividend % divisor) + divisor) % divisor; // Will be on correct range
		}

		/// <summary>
		/// Selects the specified slide.
		/// </summary>
		/// <param name="index">The index of the slide to select.</param>
		/// <param name="transition">Overrides the transition to use. Specify null to use transition specified by the slide.</param>
		public void SelectSlide(int index, string transition)
		{
			TaskStack.Pop();
			TaskStack.Push(new TransitionTask(ToSlideIndex, index, transition, TransitionDuration, true));
			ToSlideIndex = index;
			if (currentTransition != null)
			{
				currentTransition.SkipToFill();
			}
			
			if (WaitStoryboard != null)
			{
				WaitStoryboard.SkipToFill();
			}

			if (IsPaused)
			{
				ExecuteNextTask();
			}
		}

		/// <summary>
		/// Selects the previous slide.
		/// </summary>
		/// <param name="transition">Overrides the transition to use. Specify null to use transition specified by the slide.</param>
		public void SelectPreviousSlide(string transition)
		{
			SelectSlide(ToSlideIndex - 1, transition);
		}

		/// <summary>
		/// Selects the next slide.
		/// </summary>
		/// <param name="transition">Overrides the transition to use. Specify null to use transition specified by the slide.</param>
		public void SelectNextSlide(string transition)
		{
			SelectSlide(ToSlideIndex + 1, transition);
		}

		/// <summary>
		/// Plays the slideshow.
		/// </summary>
		public void Play()
		{
			if (!IsPaused)
			{
				return;
			}

			IsPaused = false;

			if (IsWaiting)
			{
				WaitStoryboard.Resume();
			}
			else
			{
				ExecuteNextTask();
			}

			Page page = App.Current.RootVisual as Page;
			if (page != null)
			{
				page.DisplayPlayState();
			}
		}

		/// <summary>
		/// Pauses the slideshow.
		/// </summary>
		public void Pause()
		{
			if (IsPaused)
			{
				return;
			}
			
			IsPaused = true;
			Page page = App.Current.RootVisual as Page;
			
			if (page != null)
			{
				page.DisplayPausedState();
			}

			if (!IsWaiting && currentTransition != null)
			{
				currentTransition.SkipToFill();
			}
			else if (WaitStoryboard != null)
			{
				WaitStoryboard.Pause();
			}
		}

		/// <summary>
		/// Hides this slideshow.
		/// </summary>
		public void Hide()
		{
			Pause();
			MediaRoot.Visibility = Visibility.Collapsed;
		}

		/// <summary>
		/// Shows this slideshow.
		/// </summary>
		public void Show()
		{
			MediaRoot.Visibility = Visibility.Visible;
			Play();
		}
	}
}