using UIKit;

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	[Foundation.Preserve(AllMembers = true)]
	class TouchEventsGestureRecognizer : UIGestureRecognizer
	{
		readonly TouchEvents events;

		public TouchEventsGestureRecognizer(TouchEvents events) => this.events = events;

		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			events.OnTouchBegin();

			base.TouchesBegan(touches, evt);
		}

		public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
		{
			events.OnTouchMove();

			base.TouchesMoved(touches, evt);
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			events.OnTouchEnd();

			base.TouchesEnded(touches, evt);
		}

		public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
		{
			events.OnTouchCancel();

			base.TouchesCancelled(touches, evt);
		}
	}
}