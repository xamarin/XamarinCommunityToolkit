using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.VisualFeedbackEffectRouter), nameof(VisualFeedbackEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	[Preserve(AllMembers = true)]
	public class VisualFeedbackEffectRouter : PlatformEffect
	{
		AView view;
		RippleDrawable ripple;
		Drawable orgDrawable;
		FrameLayout rippleOverlay;
		FastRendererOnLayoutChangeListener fastListener;

		bool IsClickable => !(Element is Layout || Element is BoxView);

		protected override void OnAttached()
		{
			view = Control ?? Container;

			SetUpRipple();

			if (IsClickable)
				view.Touch += OnViewTouch;

			UpdateEffectColor();
		}

		protected override void OnDetached()
		{
			if (!IsClickable)
			{
				view.Touch -= OnOverlayTouch;
				view.RemoveOnLayoutChangeListener(fastListener);

				fastListener.Dispose();
				fastListener = null;
				rippleOverlay.Dispose();
				rippleOverlay = null;
			}
			else
			{
				view.Touch -= OnViewTouch;
				view.Background = orgDrawable;
				orgDrawable = null;
			}

			ripple?.Dispose();
			ripple = null;

			view = null;
		}

		protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (args.PropertyName == VisualFeedbackEffect.FeedbackColorProperty.PropertyName)
			{
				UpdateEffectColor();
			}
		}

		void UpdateEffectColor()
		{
			var color = VisualFeedbackEffect.GetFeedbackColor(Element);

			var nativeColor = color.ToAndroid();
			nativeColor.A = 80;

			ripple?.SetColor(GetPressedColorSelector(nativeColor));
		}

		void SetUpRipple()
		{
			ripple = CreateRipple(AColor.Transparent);

			if (!IsClickable)
			{
				rippleOverlay = new FrameLayout(view.Context)
				{
					Clickable = true,
					LongClickable = true,
					Foreground = ripple
				};
				fastListener = new FastRendererOnLayoutChangeListener(this);
				view.AddOnLayoutChangeListener(fastListener);
				view.RequestLayout();
			}
			else
			{
				orgDrawable = view.Background;
				view.Background = ripple;
			}
		}

		void SetUpOverlay()
		{
			var parent = view.Parent as ViewGroup;

			parent.AddView(rippleOverlay);

			rippleOverlay.BringToFront();
			rippleOverlay.Touch += OnOverlayTouch;
		}

		void OnViewTouch(object sender, AView.TouchEventArgs e) => e.Handled = false;

		void OnOverlayTouch(object sender, AView.TouchEventArgs e)
		{
			view?.DispatchTouchEvent(e.Event);

			e.Handled = false;
		}

		RippleDrawable CreateRipple(AColor color)
		{
			if (!IsClickable)
			{
				var mask = new ColorDrawable(AColor.White);
				return new RippleDrawable(GetPressedColorSelector(color), null, mask);
			}

			var back = view.Background;

			if (back == null)
			{
				var mask = new ColorDrawable(AColor.White);
				return new RippleDrawable(GetPressedColorSelector(color), null, mask);
			}
			else
				return new RippleDrawable(GetPressedColorSelector(color), back, null);
		}

		ColorStateList GetPressedColorSelector(int pressedColor) => new ColorStateList(
				new int[][]
				{
					new int[] { }
				},
				new int[]
				{
					pressedColor
				});

		internal class FastRendererOnLayoutChangeListener : Java.Lang.Object, AView.IOnLayoutChangeListener
		{
			bool hasParent = false;
			VisualFeedbackEffectRouter effect;

			public FastRendererOnLayoutChangeListener(VisualFeedbackEffectRouter effect)
			{
				this.effect = effect;
			}

			public void OnLayoutChange(AView v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
			{
				effect.rippleOverlay.Layout(v.Left, v.Top, v.Right, v.Bottom);

				if (hasParent)
				{
					return;
				}

				hasParent = true;
				effect.SetUpOverlay();
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
					effect = null;

				base.Dispose(disposing);
			}
		}
	}
}