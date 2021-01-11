using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using AView = Android.Views.View;
using System;
using Android.Graphics.Drawables;
using Android.Widget;
using Color = Android.Graphics.Color;
using Android.Content.Res;
using Android.Views.Accessibility;
using Android.Content;
using AndroidOS = Android.OS;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Android.Effects;
using Android.OS;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformTouchEffect : PlatformEffect
	{
		static readonly Forms.Color defaultNativeAnimationColor = Forms.Color.FromRgba(128, 128, 128, 64);

		AccessibilityManager accessibilityManager;
		AccessibilityListener accessibilityListener;
		TouchEffect effect;
		bool isHoverSupported;
		RippleDrawable ripple;
		AView rippleView;
		float startX;
		float startY;
		Forms.Color rippleColor;
		int rippleRadius = -1;

		AView View => Control ?? Container;

		ViewGroup Group => Container ?? Control as ViewGroup;

		internal bool IsCanceled { get; set; }

		bool IsAccessibilityMode =>
			accessibilityManager != null &&
			accessibilityManager.IsEnabled &&
			accessibilityManager.IsTouchExplorationEnabled;

		protected override void OnAttached()
		{
			if (View == null)
				return;

			effect = TouchEffect.PickFrom(Element);
			if (effect?.IsDisabled ?? true)
				return;

			effect.Element = (VisualElement)Element;

			View.Touch += OnTouch;
			UpdateClickHandler();

			accessibilityManager = View.Context.GetSystemService(Context.AccessibilityService) as AccessibilityManager;
			if (accessibilityManager != null)
			{
				accessibilityListener = new AccessibilityListener(this);
				accessibilityManager.AddAccessibilityStateChangeListener(accessibilityListener);
				accessibilityManager.AddTouchExplorationStateChangeListener(accessibilityListener);
			}

			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop || !effect.NativeAnimation)
				return;

			View.Clickable = true;
			View.LongClickable = true;
			CreateRipple();

			if (Group != null)
			{
				rippleView = new FrameLayout(Group.Context)
				{
					LayoutParameters = new ViewGroup.LayoutParams(-1, -1),
					Clickable = false,
					Focusable = false,
				};
				View.LayoutChange += OnLayoutChange;
				rippleView.Background = ripple;
				Group.AddView(rippleView);
				rippleView.BringToFront();
			}
			else if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
			{
				rippleView = View;
				rippleView.Foreground = ripple;
			}
		}

		protected override void OnDetached()
		{
			if (effect?.Element == null)
				return;

			try
			{
				if (accessibilityManager != null)
				{
					accessibilityManager.RemoveAccessibilityStateChangeListener(accessibilityListener);
					accessibilityManager.RemoveTouchExplorationStateChangeListener(accessibilityListener);
					accessibilityListener.Dispose();
					accessibilityManager = null;
					accessibilityListener = null;
				}

				if (View != null)
				{
					View.LayoutChange -= OnLayoutChange;
					View.Touch -= OnTouch;
					View.Click -= OnClick;
				}

				effect.Element = null;
				effect = null;

				if (rippleView != null)
				{
					rippleView.Pressed = false;
					rippleView.Foreground = null;
					rippleView.Background = null;
					if (rippleView != View)
					{
						Group?.RemoveView(rippleView);
						rippleView.Dispose();
					}
					rippleView = null;
					ripple?.Dispose();
				}
			}
			catch (ObjectDisposedException)
			{
				// Suppress exception
			}
			isHoverSupported = false;
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == TouchEffect.IsAvailableProperty.PropertyName ||
				args.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
			{
				UpdateClickHandler();
			}
		}

		void UpdateClickHandler()
		{
			View.Click -= OnClick;
			if (IsAccessibilityMode || (effect.IsAvailable && effect.Element.IsEnabled))
			{
				View.Click += OnClick;
				return;
			}
		}

		void OnTouch(object sender, AView.TouchEventArgs e)
		{
			e.Handled = false;

			if (effect?.IsDisabled ?? true)
				return;

			if (IsAccessibilityMode)
				return;

			switch (e.Event.ActionMasked)
			{
				case MotionEventActions.Down:
					OnTouchDown(e);
					break;
				case MotionEventActions.Up:
					OnTouchUp();
					break;
				case MotionEventActions.Cancel:
					OnTouchCancel();
					break;
				case MotionEventActions.Move:
					OnTouchMove(sender, e);
					break;
				case MotionEventActions.HoverEnter:
					OnHoverEnter();
					break;
				case MotionEventActions.HoverExit:
					OnHoverExit();
					break;
			}
		}

		void OnTouchDown(AView.TouchEventArgs e)
		{
			IsCanceled = false;
			startX = e.Event.GetX();
			startY = e.Event.GetY();
			effect?.HandleUserInteraction(TouchInteractionStatus.Started);
			effect?.HandleTouch(TouchStatus.Started);
			StartRipple(e.Event.GetX(), e.Event.GetY());
			if (effect.DisallowTouchThreshold > 0)
				Group?.Parent?.RequestDisallowInterceptTouchEvent(true);
		}

		void OnTouchUp()
			=> HandleEnd(effect.Status == TouchStatus.Started ? TouchStatus.Completed : TouchStatus.Canceled);

		void OnTouchCancel()
			=> HandleEnd(TouchStatus.Canceled);

		void OnTouchMove(object sender, AView.TouchEventArgs e)
		{
			if (IsCanceled)
				return;

			var diffX = Math.Abs(e.Event.GetX() - startX) / View.Context.Resources.DisplayMetrics.Density;
			var diffY = Math.Abs(e.Event.GetY() - startY) / View.Context.Resources.DisplayMetrics.Density;
			var maxDiff = Math.Max(diffX, diffY);
			var disallowTouchThreshold = effect.DisallowTouchThreshold;
			if (disallowTouchThreshold > 0 && maxDiff > disallowTouchThreshold)
			{
				HandleEnd(TouchStatus.Canceled);
				return;
			}

			var view = sender as AView;
			if (view == null)
				return;

			var screenPointerCoords = new Point(view.Left + e.Event.GetX(), view.Top + e.Event.GetY());
			var viewRect = new Rectangle(view.Left, view.Top, view.Right - view.Left, view.Bottom - view.Top);
			var status = viewRect.Contains(screenPointerCoords) ? TouchStatus.Started : TouchStatus.Canceled;

			if (isHoverSupported && ((status == TouchStatus.Canceled && effect.HoverStatus == HoverStatus.Entered)
				|| (status == TouchStatus.Started && effect.HoverStatus == HoverStatus.Exited)))
				effect?.HandleHover(status == TouchStatus.Started ? HoverStatus.Entered : HoverStatus.Exited);

			if (effect.Status != status)
			{
				effect?.HandleTouch(status);
				if (status == TouchStatus.Started)
					StartRipple(e.Event.GetX(), e.Event.GetY());
				if (status == TouchStatus.Canceled)
					EndRipple();
			}
		}

		void OnHoverEnter()
		{
			isHoverSupported = true;
			effect?.HandleHover(HoverStatus.Entered);
		}

		void OnHoverExit()
		{
			isHoverSupported = true;
			effect?.HandleHover(HoverStatus.Exited);
		}

		void OnClick(object sender, EventArgs args)
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (!IsAccessibilityMode)
				return;

			IsCanceled = false;
			HandleEnd(TouchStatus.Completed);
		}

		void HandleEnd(TouchStatus status)
		{
			if (IsCanceled)
				return;

			IsCanceled = true;
			if (effect.DisallowTouchThreshold > 0)
				Group?.Parent?.RequestDisallowInterceptTouchEvent(false);

			effect?.HandleTouch(status);
			effect?.HandleUserInteraction(TouchInteractionStatus.Completed);
			EndRipple();
		}

		void StartRipple(float x, float y)
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (effect.CanExecute && effect.NativeAnimation && rippleView != null)
			{
				UpdateRipple();

				if (rippleView != View)
					rippleView.BringToFront();

				ripple.SetHotspot(x, y);
				rippleView.Pressed = true;
			}
		}

		void EndRipple()
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (rippleView?.Pressed ?? false)
				rippleView.Pressed = false;
		}

		void CreateRipple()
		{
			var background = View?.Background;

			if (background is RippleDrawable)
			{
				ripple = (RippleDrawable)background.GetConstantState().NewDrawable();
				return;
			}

			var noBackground = Element is Layout || background == null;
			ripple = new RippleDrawable(GetColorStateList(), noBackground ? null : background, noBackground ? new ColorDrawable(Color.White) : null);
			UpdateRipple();
		}

		void UpdateRipple()
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (effect.NativeAnimationColor == rippleColor && effect.NativeAnimationRadius == rippleRadius)
				return;

			rippleColor = effect.NativeAnimationColor;
			rippleRadius = effect.NativeAnimationRadius;
			ripple.SetColor(GetColorStateList());
			if (AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.M)
				ripple.Radius = (int)(View.Context.Resources.DisplayMetrics.Density * effect.NativeAnimationRadius);
		}

		ColorStateList GetColorStateList()
		{
			var nativeAnimationColor = effect.NativeAnimationColor;
			if (nativeAnimationColor == Forms.Color.Default)
				nativeAnimationColor = defaultNativeAnimationColor;

			return new ColorStateList(
				new[] { new int[] { } },
				new[] { (int)nativeAnimationColor.ToAndroid() });
		}

		void OnLayoutChange(object sender, AView.LayoutChangeEventArgs e)
		{
			var group = (ViewGroup)sender;
			if (group == null || (Group as IVisualElementRenderer)?.Element == null)
				return;

			if (rippleView != null)
			{
				rippleView.Right = group.Width;
				rippleView.Bottom = group.Height;
			}
		}

		sealed class AccessibilityListener : Java.Lang.Object,
											 AccessibilityManager.IAccessibilityStateChangeListener,
											 AccessibilityManager.ITouchExplorationStateChangeListener
		{
			PlatformTouchEffect platformTouchEffect;

			internal AccessibilityListener(PlatformTouchEffect platformTouchEffect)
				=> this.platformTouchEffect = platformTouchEffect;

			public void OnAccessibilityStateChanged(bool enabled)
				=> platformTouchEffect?.UpdateClickHandler();

			public void OnTouchExplorationStateChanged(bool enabled)
				=> platformTouchEffect?.UpdateClickHandler();

			protected override void Dispose(bool disposing)
			{
				if (disposing)
					platformTouchEffect = null;

				base.Dispose(disposing);
			}
		}
	}
}