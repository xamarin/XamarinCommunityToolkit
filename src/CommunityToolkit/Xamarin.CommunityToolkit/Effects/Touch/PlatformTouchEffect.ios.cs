using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformTouchEffect : PlatformEffect
	{
		TouchUITapGestureRecognizer gesture;

		TouchEffect effect;

		protected override void OnAttached()
		{
			effect = TouchEffect.PickFrom(Element);
			if (effect?.IsDisabled ?? true)
				return;

			effect.Element = (VisualElement)Element;

			gesture = new TouchUITapGestureRecognizer(effect);

			if (Container != null)
			{
				if ((Container as IVisualNativeElementRenderer)?.Control is UIButton button)
				{
					button.AllTouchEvents += PreventButtonHighlight;
					gesture.IsButton = true;
				}

				Container.AddGestureRecognizer(gesture);
				Container.UserInteractionEnabled = true;
			}
		}

		protected override void OnDetached()
		{
			if (effect?.Element == null)
				return;

			if ((Container as IVisualNativeElementRenderer)?.Control is UIButton button)
				button.AllTouchEvents -= PreventButtonHighlight;

			Container?.RemoveGestureRecognizer(gesture);
			gesture?.Dispose();
			gesture = null;
			effect.Element = null;
			effect = null;
		}

		void PreventButtonHighlight(object sender, EventArgs args)
			=> ((UIButton)sender).Highlighted = false;
	}

	sealed class TouchUITapGestureRecognizer : UIGestureRecognizer
	{
		TouchEffect effect;
		float? defaultRadius;
		float? defaultShadowRadius;
		float? defaultShadowOpacity;
		CGPoint? startPoint;

		public TouchUITapGestureRecognizer(TouchEffect effect)
		{
			this.effect = effect;
			CancelsTouchesInView = false;
			Delegate = new TouchUITapGestureRecognizerDelegate();
		}

		public bool IsCanceled { get; set; } = true;

		public bool IsButton { get; set; }

		UIView Renderer => effect?.Element.GetRenderer() as UIView;

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			IsCanceled = false;
			startPoint = GetTouchPoint(touches);
			HandleTouch(TouchStatus.Started, TouchInteractionStatus.Started);
			base.TouchesBegan(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			HandleTouch(effect?.Status == TouchStatus.Started ? TouchStatus.Completed : TouchStatus.Canceled, TouchInteractionStatus.Completed);
			IsCanceled = true;
			base.TouchesEnded(touches, evt);
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed);
			IsCanceled = true;
			base.TouchesCancelled(touches, evt);
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			var disallowTouchThreshold = effect.DisallowTouchThreshold;
			var point = GetTouchPoint(touches);
			if (point != null && startPoint != null && disallowTouchThreshold > 0)
			{
				var diffX = Math.Abs(point.Value.X - startPoint.Value.X);
				var diffY = Math.Abs(point.Value.Y - startPoint.Value.Y);
				var maxDiff = Math.Max(diffX, diffY);
				if (maxDiff > disallowTouchThreshold)
				{
					HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed);
					IsCanceled = true;
					base.TouchesMoved(touches, evt);
					return;
				}
			}

			var status = point != null && Renderer.Bounds.Contains(point.Value)
				? TouchStatus.Started
				: TouchStatus.Canceled;

			if (effect?.Status != status)
				HandleTouch(status);

			base.TouchesMoved(touches, evt);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				effect = null;
				Delegate = null;
			}
			base.Dispose(disposing);
		}

		CGPoint? GetTouchPoint(NSSet touches)
			=> Renderer != null ? (touches?.AnyObject as UITouch)?.LocationInView(Renderer) : null;

		public void HandleTouch(TouchStatus status, TouchInteractionStatus? interactionStatus = null)
		{
			if (IsCanceled || effect == null)
				return;

			if (effect?.IsDisabled ?? true)
				return;

			if (interactionStatus == TouchInteractionStatus.Started)
			{
				effect?.HandleUserInteraction(TouchInteractionStatus.Started);
				interactionStatus = null;
			}

			effect.HandleTouch(status);
			if (interactionStatus.HasValue)
				effect?.HandleUserInteraction(interactionStatus.Value);

			if (effect == null || (!effect.NativeAnimation && !IsButton) || !effect.CanExecute)
				return;

			var control = effect.Element;
			if (!(control?.GetRenderer() is UIView renderer))
				return;

			var color = effect.NativeAnimationColor;
			var radius = effect.NativeAnimationRadius;
			var shadowRadius = effect.NativeAnimationShadowRadius;
			var isStarted = status == TouchStatus.Started;
			defaultRadius = (float?)(defaultRadius ?? renderer.Layer.CornerRadius);
			defaultShadowRadius = (float?)(defaultShadowRadius ?? renderer.Layer.ShadowRadius);
			defaultShadowOpacity ??= renderer.Layer.ShadowOpacity;

			UIView.AnimateAsync(.2, () =>
			{
				if (color == Color.Default)
					renderer.Layer.Opacity = isStarted ? 0.5f : (float)control.Opacity;
				else
					renderer.Layer.BackgroundColor = (isStarted ? color : control.BackgroundColor).ToCGColor();

				renderer.Layer.CornerRadius = isStarted ? radius : defaultRadius.GetValueOrDefault();

				if (shadowRadius >= 0)
				{
					renderer.Layer.ShadowRadius = isStarted ? shadowRadius : defaultShadowRadius.GetValueOrDefault();
					renderer.Layer.ShadowOpacity = isStarted ? 0.7f : defaultShadowOpacity.GetValueOrDefault();
				}
			});
		}
	}

	class TouchUITapGestureRecognizerDelegate : UIGestureRecognizerDelegate
	{
		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
		{
			if (gestureRecognizer is TouchUITapGestureRecognizer touchGesture && otherGestureRecognizer is UIPanGestureRecognizer &&
				otherGestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				touchGesture.HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed);
				touchGesture.IsCanceled = true;
			}

			return true;
		}
	}
}
