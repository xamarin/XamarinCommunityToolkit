using System;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using Color = Android.Graphics.Color;
using XColor = Xamarin.Forms.Color;
using XView = Xamarin.Forms.View;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformTouchEffect : PlatformEffect
	{
		static readonly XColor defaultNativeAnimationColor = XColor.FromRgba(128, 128, 128, 64);

		AccessibilityManager? accessibilityManager;
		AccessibilityListener? accessibilityListener;
		TouchEffect? effect;
		bool isHoverSupported;
		RippleDrawable? ripple;
		AView? rippleView;
		float startX;
		float startY;
		XColor rippleColor;
		int rippleRadius = -1;

		AView View => Control ?? Container;

		ViewGroup? Group => Container ?? Control as ViewGroup;

		internal bool IsCanceled { get; set; }

		bool IsAccessibilityMode => accessibilityManager != null
			&& accessibilityManager.IsEnabled
			&& accessibilityManager.IsTouchExplorationEnabled;

		bool IsForegroundRippleWithTapGestureRecognizer
			=> ripple != null &&
				ripple.IsAlive() &&
				View.IsAlive() &&
				(XCT.SdkInt >= (int)BuildVersionCodes.M ? View.Foreground : View.Background) == ripple &&
				Element is XView view &&
				view.GestureRecognizers.Any(gesture => gesture is TapGestureRecognizer);

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

			accessibilityManager = View.Context?.GetSystemService(Context.AccessibilityService) as AccessibilityManager;
			if (accessibilityManager != null)
			{
				accessibilityListener = new AccessibilityListener(this);
				accessibilityManager.AddAccessibilityStateChangeListener(accessibilityListener);
				accessibilityManager.AddTouchExplorationStateChangeListener(accessibilityListener);
			}

			if (XCT.SdkInt < (int)BuildVersionCodes.Lollipop || !effect.NativeAnimation)
				return;

			View.Clickable = true;
			View.LongClickable = true;
			CreateRipple();
			ApplyRipple();

			View.LayoutChange += OnLayoutChange;
		}

		protected override void OnDetached()
		{
			if (effect?.Element == null)
				return;

			try
			{
				if (accessibilityManager != null && accessibilityListener != null)
				{
					accessibilityManager.RemoveAccessibilityStateChangeListener(accessibilityListener);
					accessibilityManager.RemoveTouchExplorationStateChangeListener(accessibilityListener);
					accessibilityListener.Dispose();
					accessibilityManager = null;
					accessibilityListener = null;
				}

				RemoveRipple();

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
					Group?.RemoveView(rippleView);
					rippleView.Dispose();
					rippleView = null;
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

			if (args.PropertyName == TouchEffect.NativeAnimationBorderlessProperty.PropertyName)
			{
				CreateRipple();
				ApplyRipple();
			}
		}

		void UpdateClickHandler()
		{
			if (!View.IsAlive())
				return;

			View.Click -= OnClick;
			if (IsAccessibilityMode || ((effect?.IsAvailable ?? false) && (effect?.Element?.IsEnabled ?? false)))
			{
				View.Click += OnClick;
				return;
			}
		}

		void OnTouch(object? sender, AView.TouchEventArgs e)
		{
			e.Handled = false;

			if (effect?.IsDisabled ?? true)
				return;

			if (IsAccessibilityMode)
				return;

			switch (e.Event?.ActionMasked)
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
			_ = e.Event ?? throw new NullReferenceException();

			IsCanceled = false;

			startX = e.Event.GetX();
			startY = e.Event.GetY();

			effect?.HandleUserInteraction(TouchInteractionStatus.Started);
			effect?.HandleTouch(TouchStatus.Started);

			StartRipple(e.Event.GetX(), e.Event.GetY());

			if (effect?.DisallowTouchThreshold > 0)
				Group?.Parent?.RequestDisallowInterceptTouchEvent(true);
		}

		void OnTouchUp()
			=> HandleEnd(effect?.Status == TouchStatus.Started ? TouchStatus.Completed : TouchStatus.Canceled);

		void OnTouchCancel()
			=> HandleEnd(TouchStatus.Canceled);

		void OnTouchMove(object? sender, AView.TouchEventArgs e)
		{
			if (IsCanceled || e.Event == null)
				return;

			var diffX = Math.Abs(e.Event.GetX() - startX) / View.Context?.Resources?.DisplayMetrics?.Density ?? throw new NullReferenceException();
			var diffY = Math.Abs(e.Event.GetY() - startY) / View.Context?.Resources?.DisplayMetrics?.Density ?? throw new NullReferenceException();
			var maxDiff = Math.Max(diffX, diffY);

			var disallowTouchThreshold = effect?.DisallowTouchThreshold;
			if (disallowTouchThreshold > 0 && maxDiff > disallowTouchThreshold)
			{
				HandleEnd(TouchStatus.Canceled);
				return;
			}

			if (sender is not AView view)
				return;

			var screenPointerCoords = new Point(view.Left + e.Event.GetX(), view.Top + e.Event.GetY());
			var viewRect = new Rectangle(view.Left, view.Top, view.Right - view.Left, view.Bottom - view.Top);
			var status = viewRect.Contains(screenPointerCoords) ? TouchStatus.Started : TouchStatus.Canceled;

			if (isHoverSupported && ((status == TouchStatus.Canceled && effect?.HoverStatus == HoverStatus.Entered)
				|| (status == TouchStatus.Started && effect?.HoverStatus == HoverStatus.Exited)))
				effect?.HandleHover(status == TouchStatus.Started ? HoverStatus.Entered : HoverStatus.Exited);

			if (effect?.Status != status)
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

		void OnClick(object? sender, EventArgs args)
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
			if (effect?.DisallowTouchThreshold > 0)
				Group?.Parent?.RequestDisallowInterceptTouchEvent(false);

			effect?.HandleTouch(status);

			effect?.HandleUserInteraction(TouchInteractionStatus.Completed);

			EndRipple();
		}

		void StartRipple(float x, float y)
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (!effect.NativeAnimation)
				return;

			if (effect.CanExecute)
			{
				UpdateRipple(effect.NativeAnimationColor);
				if (rippleView != null)
				{
					rippleView.Enabled = true;
					rippleView.BringToFront();
					ripple?.SetHotspot(x, y);
					rippleView.Pressed = true;
				}
				else if (IsForegroundRippleWithTapGestureRecognizer)
				{
					ripple?.SetHotspot(x, y);
					View.Pressed = true;
				}
			}
			else if (rippleView == null)
			{
				UpdateRipple(XColor.Transparent);
			}
		}

		void EndRipple()
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (rippleView != null)
			{
				if (rippleView.Pressed)
				{
					rippleView.Pressed = false;
					rippleView.Enabled = false;
				}
			}
			else if (IsForegroundRippleWithTapGestureRecognizer)
			{
				if (View.Pressed)
				{
					View.Pressed = false;
				}
			}
		}

		void CreateRipple()
		{
			RemoveRipple();

			var drawable = XCT.SdkInt >= (int)BuildVersionCodes.M && Group == null
				? View?.Foreground
				: View?.Background;

			var isBorderLess = effect?.NativeAnimationBorderless ?? false;
			var isEmptyDrawable = Element is Layout || drawable == null;
			var color = effect?.NativeAnimationColor ?? throw new NullReferenceException();

			if (drawable is RippleDrawable rippleDrawable && rippleDrawable.GetConstantState() is Drawable.ConstantState constantState)
				ripple = (RippleDrawable)constantState.NewDrawable();
			else
			{
				var content = isEmptyDrawable || isBorderLess ? null : drawable;
				var mask = isEmptyDrawable && !isBorderLess ? new ColorDrawable(Color.White) : null;

				ripple = new RippleDrawable(GetColorStateList(color), content, mask);
			}

			UpdateRipple(color);
		}

		void RemoveRipple()
		{
			if (ripple == null)
				return;

			if (View != null)
			{
				if (XCT.SdkInt >= (int)BuildVersionCodes.M && View.Foreground == ripple)
					View.Foreground = null;
				else if (View.Background == ripple)
					View.Background = null;
			}

			if (rippleView != null)
			{
				rippleView.Foreground = null;
				rippleView.Background = null;
			}

			ripple.Dispose();
			ripple = null;
		}

		void UpdateRipple(XColor color)
		{
			if (effect?.IsDisabled ?? true)
				return;

			if (color == rippleColor && effect.NativeAnimationRadius == rippleRadius)
				return;

			rippleColor = color;
			rippleRadius = effect.NativeAnimationRadius;
			ripple?.SetColor(GetColorStateList(color));
			if (XCT.SdkInt >= (int)BuildVersionCodes.M && ripple != null)
				ripple.Radius = (int)(View.Context?.Resources?.DisplayMetrics?.Density * effect?.NativeAnimationRadius ?? throw new NullReferenceException());
		}

		void ApplyRipple()
		{
			if (ripple == null || effect == null)
				return;

			var isBorderless = effect.NativeAnimationBorderless;

			if (Group == null)
			{
				if (XCT.SdkInt >= (int)BuildVersionCodes.M)
					View.Foreground = ripple;
				else
					View.Background = ripple;

				return;
			}

			if (rippleView == null)
			{
				rippleView = new FrameLayout(Group.Context ?? throw new NullReferenceException())
				{
					LayoutParameters = new ViewGroup.LayoutParams(-1, -1),
					Clickable = false,
					Focusable = false,
					Enabled = false,
				};

				Group.AddView(rippleView);
				rippleView.BringToFront();
			}

			Group.SetClipChildren(!isBorderless);

			if (isBorderless)
			{
				rippleView.Background = null;
				rippleView.Foreground = ripple;
			}
			else
			{
				rippleView.Foreground = null;
				rippleView.Background = ripple;
			}
		}

		ColorStateList GetColorStateList(XColor color)
		{
			var animationColor = color;
			if (animationColor == XColor.Default)
				animationColor = defaultNativeAnimationColor;

			return new ColorStateList(
				new[] { new int[] { } },
				new[] { (int)animationColor.ToAndroid() });
		}

		void OnLayoutChange(object? sender, AView.LayoutChangeEventArgs e)
		{
			if (sender is not AView view || (Group as IVisualElementRenderer)?.Element == null || rippleView == null)
				return;

			rippleView.Right = view.Width;
			rippleView.Bottom = view.Height;
		}

		sealed class AccessibilityListener : Java.Lang.Object,
											 AccessibilityManager.IAccessibilityStateChangeListener,
											 AccessibilityManager.ITouchExplorationStateChangeListener
		{
			PlatformTouchEffect? platformTouchEffect;

			internal AccessibilityListener(PlatformTouchEffect platformTouchEffect)
				=> this.platformTouchEffect = platformTouchEffect;

			public AccessibilityListener(IntPtr handle, JniHandleOwnership transfer)
				: base(handle, transfer)
			{
			}

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