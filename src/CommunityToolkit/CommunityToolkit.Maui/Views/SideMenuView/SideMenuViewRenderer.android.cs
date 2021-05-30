using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using CommunityToolkit.Maui.Android.UI.Views;
using CommunityToolkit.Maui.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static System.Math;

[assembly: ExportRenderer(typeof(SideMenuView), typeof(SideMenuViewRenderer))]

namespace CommunityToolkit.Maui.Android.UI.Views
{
	[Preserve(Conditional = true)]
	public class SideMenuViewRenderer : VisualElementRenderer<SideMenuView>
	{
		const double defaultGestureThreshold = 30.0;

		static Guid? lastTouchHandlerId;

		readonly float density;

		Guid elementId;

		bool panStarted;

		float? startX;

		float? startY;

		public SideMenuViewRenderer(Context context)
			: base(context)
			=> density = context.Resources?.DisplayMetrics?.Density ?? throw new NullReferenceException();

		double GestureThreshold => Element.GestureThreshold >= 0
			? Element.GestureThreshold
			: defaultGestureThreshold;

		public override bool OnInterceptTouchEvent(MotionEvent ev)
		{
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
			var isHandled = xDeltaAbs > yDeltaAbs
				&& xDeltaAbs > GestureThreshold
				&& ((xDelta > 0 && Element.CheckGestureEnabled(SideMenuPosition.LeftMenu))
				|| (xDelta < 0 && Element.CheckGestureEnabled(SideMenuPosition.RightMenu)));

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