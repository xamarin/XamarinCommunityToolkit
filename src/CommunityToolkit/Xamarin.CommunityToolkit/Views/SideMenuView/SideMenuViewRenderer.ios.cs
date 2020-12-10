using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.iOS.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static System.Math;

[assembly: ExportRenderer(typeof(SideMenuView), typeof(SideMenuViewRenderer))]

namespace Xamarin.CommunityToolkit.iOS.UI.Views
{
	[Preserve(AllMembers = true)]
	public class SideMenuViewRenderer : VisualElementRenderer<SideMenuView>
	{
		UISwipeGestureRecognizer leftSwipeGestureRecognizer;

		UISwipeGestureRecognizer rightSwipeGestureRecognizer;

		public SideMenuViewRenderer()
		{
			leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(OnSwiped)
			{
				Direction = UISwipeGestureRecognizerDirection.Left
			};
			AddGestureRecognizer(leftSwipeGestureRecognizer);
			rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(OnSwiped)
			{
				Direction = UISwipeGestureRecognizerDirection.Right
			};
			AddGestureRecognizer(rightSwipeGestureRecognizer);
		}

		bool IsPanGestureHandled
			=> Abs(Element?.CurrentGestureShift ?? 0) >= Element?.GestureThreshold;

		public override void AddGestureRecognizer(UIGestureRecognizer gestureRecognizer)
		{
			base.AddGestureRecognizer(gestureRecognizer);

			if (gestureRecognizer is UIPanGestureRecognizer panGestureRecognizer)
			{
				gestureRecognizer.ShouldBeRequiredToFailBy = ShouldBeRequiredToFailBy;
				gestureRecognizer.ShouldRecognizeSimultaneously = ShouldRecognizeSimultaneously;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Dispose(ref leftSwipeGestureRecognizer);
				Dispose(ref rightSwipeGestureRecognizer);
			}
			base.Dispose(disposing);
		}

		void Dispose(ref UISwipeGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer != null)
			{
				RemoveGestureRecognizer(gestureRecognizer);
				gestureRecognizer.Dispose();
				gestureRecognizer = null;
			}
		}

		void OnSwiped(UISwipeGestureRecognizer gesture)
		{
			var swipeDirection = gesture.Direction == UISwipeGestureRecognizerDirection.Left
				? SwipeDirection.Left
				: SwipeDirection.Right;

			Element?.OnSwiped(swipeDirection);
		}

		bool ShouldBeRequiredToFailBy(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
			=> IsPanGestureHandled && otherGestureRecognizer.View != this;

		bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
		{
			if (!(gestureRecognizer is UIPanGestureRecognizer panGesture))
				return true;

			var parent = Element?.Parent;
			while (parent != null)
			{
				if (parent is FlyoutPage)
				{
					var velocity = panGesture.VelocityInView(this);
					return Abs(velocity.Y) > Abs(velocity.X);
				}
				parent = parent.Parent;
			}
			return !IsPanGestureHandled;
		}
	}
}