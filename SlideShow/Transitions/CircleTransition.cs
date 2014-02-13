using System;
using System.Windows;
using System.Windows.Media;

namespace Vertigo.SlideShow.Transitions
{
	/// <summary>
	/// Defines a circle transition, as a child of ShapeTransition.
	/// </summary>
	public abstract class CircleTransition : ShapeTransition
	{
		private double radius;

		/// <summary>
		/// Goes forward.
		/// </summary>
		/// <returns>boolean indicating whether to go forward</returns>
		protected abstract bool GoForward();

		private bool goForward { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CircleTransition"/> class.
		/// </summary>
		public CircleTransition()
		{
			radius = .5 * Math.Sqrt(Math.Pow(App.Current.Host.Content.ActualWidth, 2) + Math.Pow(App.Current.Host.Content.ActualHeight, 2));
		}

		/// <summary>
		/// Sets the clipping geometry.
		/// </summary>
		protected override void SetClippingGeometry()
		{
			EllipseGeometry ellipseGeometry = new EllipseGeometry();
			ellipseGeometry.Center = new Point(App.Current.Host.Content.ActualWidth / 2, App.Current.Host.Content.ActualHeight / 2);
			xTarget = ellipseGeometry;
			yTarget = ellipseGeometry;
			if (goForward)
			{
				fromSlide.Clip = ellipseGeometry;
			}
			else
			{
				toSlide.Clip = ellipseGeometry;
			}
		}

		/// <summary>
		/// Called when [resize].
		/// </summary>
		protected override void OnResize()
		{
			radius = .5 * Math.Sqrt(Math.Pow(App.Current.Host.Content.ActualWidth, 2) + Math.Pow(App.Current.Host.Content.ActualHeight, 2));
		}

		/// <summary>
		/// Adds the slides to media root.
		/// </summary>
		protected override void AddSlidesToMediaRoot()
		{
			goForward = GoForward();

			if (goForward)
			{
				MediaRoot.Children.Add(toSlide);
				MediaRoot.Children.Add(fromSlide);
			}
			else
			{
				MediaRoot.Children.Add(fromSlide);
				MediaRoot.Children.Add(toSlide);
			}
		}

		/// <summary>
		/// Gets the radius X from.
		/// </summary>
		/// <returns>Returns the X Radius from</returns>
		protected override double RadiusXFrom()
		{
			return
				goForward ?
				radius :
				0;
		}

		/// <summary>
		/// Gets the radius X to.
		/// </summary>
		/// <returns>Returns the X Radius to</returns>
		protected override double RadiusXTo()
		{
			return
				goForward ?
				0 :
				radius;
		}

		protected override string XPropertyPath()
		{
			return "RadiusX";
		}

		/// <summary>
		/// Gets or sets the x target.
		/// </summary>
		/// <value>The x target.</value>
		protected override DependencyObject xTarget { get; set; }

		/// <summary>
		/// Gets the radius Y from.
		/// </summary>
		/// <returns>Returns the Y radius from</returns>
		protected override double RadiusYFrom()
		{
			return
				goForward ?
				radius :
				0;
		}

		/// <summary>
		/// Gets the radius Y to.
		/// </summary>
		/// <returns>The Y radius to</returns>
		protected override double RadiusYTo()
		{
			return
				goForward ?
				0 :
				radius;
		}

		protected override string YPropertyPath()
		{
			return "RadiusY";
		}

		/// <summary>
		/// Gets or sets the y target.
		/// </summary>
		/// <value>The y target.</value>
		protected override DependencyObject yTarget { get; set; }
	}
}