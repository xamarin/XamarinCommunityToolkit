using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Xamarin.CommunityToolkit.Android.UI.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static System.Math;

[assembly: ExportRenderer(typeof(SideMenuView), typeof(SideMenuViewRenderer))]

namespace Xamarin.CommunityToolkit.Android.UI.Views
{
	[Preserve(AllMembers = true)]
	public class SideMenuViewRenderer : VisualElementRenderer<SideMenuView>
	{
		static Guid? lastTouchHandlerId;

		readonly float density;

		Guid elementId;

		bool panStarted;

		float? startX;

		float? startY;

		public SideMenuViewRenderer(Context context)
			: base(context)
			=> density = context.Resources.DisplayMetrics.Density;

		public override bool OnInterceptTouchEvent(MotionEvent ev)
		{
			if (Element.AllowInterceptGesture)
			{
				base.OnInterceptTouchEvent(ev);
				return false;
			}

			if (ev.ActionMasked == MotionEventActions.Move)
			{
				if (lastTouchHandlerId.HasValue && lastTouchHandlerId != elementId)
					return false;

				return CheckIsTouchHandled(GetTotalX(ev), GetTotalY(ev));
			}

			HandleDownUpEvents(ev);
			return false;
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (e.ActionMasked == MotionEventActions.Move)
			{
				var xDelta = GetTotalX(e);
				var yDelta = GetTotalY(e);
				CheckIsTouchHandled(xDelta, yDelta);

				if (Abs(yDelta) <= Abs(xDelta))
					UpdatePan(GestureStatus.Running, xDelta, yDelta);
			}

			HandleDownUpEvents(e);
			return true;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<SideMenuView> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement == null)
				return;

			panStarted = false;
			elementId = Element.Id;
		}

		bool CheckIsTouchHandled(float xDelta, float yDelta)
		{
			var xDeltaAbs = Abs(xDelta);
			var yDeltaAbs = Abs(yDelta);
			var isHandled = xDeltaAbs > yDeltaAbs && xDeltaAbs > Element.GestureThreshold;

			Parent?.RequestDisallowInterceptTouchEvent(isHandled);
			return isHandled;
		}

		void HandleDownUpEvents(MotionEvent ev)
		{
			HandleDownEvent(ev);
			HandleUpCancelEvent(ev);
		}

		void HandleUpCancelEvent(MotionEvent ev)
		{
			var action = ev.ActionMasked;
			var isUpAction = action == MotionEventActions.Up;
			var isCancelAction = action == MotionEventActions.Cancel;
			if (!panStarted || (!isUpAction && !isCancelAction))
				return;

			var xDelta = GetTotalX(ev);
			var yDelta = GetTotalY(ev);
			UpdatePan(isUpAction ? GestureStatus.Completed : GestureStatus.Canceled, xDelta, yDelta);
			panStarted = false;
			lastTouchHandlerId = null;

			Parent?.RequestDisallowInterceptTouchEvent(false);

			startX = null;
			startY = null;
		}

		void HandleDownEvent(MotionEvent ev)
		{
			if (ev.ActionMasked != MotionEventActions.Down)
				return;

			startX = ev.GetX();
			startY = ev.GetY();

			UpdatePan(GestureStatus.Started);
			panStarted = true;
			lastTouchHandlerId = elementId;
		}

		void UpdatePan(GestureStatus status, double totalX = 0, double totalY = 0)
			=> Element.OnPanUpdated(Element, GetPanUpdatedEventArgs(status, totalX, totalY));

		PanUpdatedEventArgs GetPanUpdatedEventArgs(GestureStatus status, double totalX = 0, double totalY = 0)
			=> new PanUpdatedEventArgs(status, 0, totalX, totalY);

		float GetTotalX(MotionEvent ev)
			=> (ev.GetX() - startX.GetValueOrDefault()) / density;

		float GetTotalY(MotionEvent ev)
			=> (ev.GetY() - startY.GetValueOrDefault()) / density;
	}
}