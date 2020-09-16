using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class MotionEventHelper
	{
		VisualElement element;
		bool isInViewCell;

		public bool HandleMotionEvent(IViewParent parent, MotionEvent motionEvent)
		{
			if (isInViewCell || element == null || motionEvent == null || motionEvent.Action == MotionEventActions.Cancel)
				return false;

			var renderer = parent as VisualElementRenderer<Xamarin.Forms.View>;
			if (renderer == null || ShouldPassThroughElement())
				return false;

			// Let the container know that we're "fake" handling this event
			// renderer.NotifyFakeHandling();

			return true;
		}

		public void UpdateElement(VisualElement element)
		{
			isInViewCell = false;
			this.element = element;

			if (this.element == null)
				return;

			// Determine whether this control is inside a ViewCell;
			// we don't fake handle the events because ListView needs them for row selection
			// isInViewCell = element.IsInViewCell();
		}

		bool ShouldPassThroughElement()
		{
			if (element is Layout layout)
			{
				// If the layout is not input transparent, then the event should not pass through it
				if (!layout.InputTransparent)
					return false;

				// This is a layout, and it's transparent, and all its children are transparent, then the event
				// can just pass through
				if (layout.CascadeInputTransparent)
					return true;

				// This event isn't being bubbled up by a non-InputTransparent child layout
				return true;
			}

			// This is not a layout and it's transparent; the event can just pass through
			if (element.InputTransparent)
				return true;

			return false;
		}
	}
}