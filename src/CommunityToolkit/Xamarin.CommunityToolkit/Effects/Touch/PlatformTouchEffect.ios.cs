using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformTouchEffect), nameof(TouchEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformTouchEffect : PlatformEffect
	{
		UIGestureRecognizer? touchGesture;

		UIGestureRecognizer? hoverGesture;

		TouchEffect? effect;

		UIView View => Container ?? Control;

		protected override void OnAttached()
		{
			effect = TouchEffect.PickFrom(Element);
			if (effect?.IsDisabled ?? true)
				return;

			effect.Element = (VisualElement)Element;

			if (View == null)
				return;

			touchGesture = new TouchUITapGestureRecognizer(effect);

			if (((View as IVisualNativeElementRenderer)?.Control ?? View) is UIButton button)
			{
				button.AllTouchEvents += PreventButtonHighlight;
				((TouchUITapGestureRecognizer)touchGesture).IsButton = true;
			}

			View.AddGestureRecognizer(touchGesture);

			if (XCT.IsiOS13OrNewer)
			{
				hoverGesture = new UIHoverGestureRecognizer(OnHover);
				View.AddGestureRecognizer(hoverGesture);
			}

			View.UserInteractionEnabled = true;
		}

		protected override void OnDetached()
		{
			if (effect?.Element == null)
				return;

			if (((View as IVisualNativeElementRenderer)?.Control ?? View) is UIButton button)
				button.AllTouchEvents -= PreventButtonHighlight;

			if (touchGesture != null)
			{
				View?.RemoveGestureRecognizer(touchGesture);
				touchGesture?.Dispose();
				touchGesture = null;
			}

			if (hoverGesture != null)
			{
				View?.RemoveGestureRecognizer(hoverGesture);
				hoverGesture?.Dispose();
				hoverGesture = null;
			}

			effect.Element = null;
			effect = null;
		}

		void OnHover()
		{
			if (effect == null || effect.IsDisabled)
				return;

			switch (hoverGesture?.State)
			{
				case UIGestureRecognizerState.Began:
				case UIGestureRecognizerState.Changed:
					effect?.HandleHover(HoverStatus.Entered);
					break;
				case UIGestureRecognizerState.Ended:
					effect?.HandleHover(HoverStatus.Exited);
					break;
			}
		}

		void PreventButtonHighlight(object? sender, EventArgs args)
		{
			if (sender is not UIButton button)
				throw new ArgumentException($"{nameof(sender)} must be Type {nameof(UIButton)}", nameof(sender));

			button.Highlighted = false;
		}
	}

	sealed class TouchUITapGestureRecognizer : UIGestureRecognizer
	{
		TouchEffect? effect;
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

		UIView? Renderer => (UIView?)effect?.Element.GetRenderer();

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			IsCanceled = false;
			startPoint = GetTouchPoint(touches);

			HandleTouch(TouchStatus.Started, TouchInteractionStatus.Started).SafeFireAndForget();

			State = UIGestureRecognizerState.Began;
			base.TouchesBegan(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			HandleTouch(effect?.Status == TouchStatus.Started ? TouchStatus.Completed : TouchStatus.Canceled, TouchInteractionStatus.Completed).SafeFireAndForget();

			IsCanceled = true;

			State = UIGestureRecognizerState.Ended;
			base.TouchesEnded(touches, evt);
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			if (effect?.IsDisabled ?? true)
				return;

			HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed).SafeFireAndForget();

			IsCanceled = true;

			State = UIGestureRecognizerState.Cancelled;
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
					HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed).SafeFireAndForget();
					IsCanceled = true;
					State = UIGestureRecognizerState.Cancelled;
					base.TouchesMoved(touches, evt);
					return;
				}
			}

			var status = point != null && Renderer?.Bounds.Contains(point.Value) is true
				? TouchStatus.Started
				: TouchStatus.Canceled;

			if (effect?.Status != status)
				HandleTouch(status).SafeFireAndForget();

			if (status == TouchStatus.Canceled)
				IsCanceled = true;

			State = UIGestureRecognizerState.Changed;
			base.TouchesMoved(touches, evt);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				effect = null;
				Delegate.Dispose();
			}

			base.Dispose(disposing);
		}

		CGPoint? GetTouchPoint(NSSet touches)
			=> Renderer != null ? (touches?.AnyObject as UITouch)?.LocationInView(Renderer) : null;

		public async Task HandleTouch(TouchStatus status, TouchInteractionStatus? interactionStatus = null)
		{
			if (IsCanceled || effect == null)
				return;

			if (effect?.IsDisabled ?? true)
				return;

			var canExecuteAction = effect.CanExecute;

			if (interactionStatus == TouchInteractionStatus.Started)
			{
				effect?.HandleUserInteraction(TouchInteractionStatus.Started);
				interactionStatus = null;
			}

			effect?.HandleTouch(status);
			if (interactionStatus.HasValue)
				effect?.HandleUserInteraction(interactionStatus.Value);

			if (effect == null || (!effect.NativeAnimation && !IsButton) || (!canExecuteAction && status == TouchStatus.Started))
				return;

			var control = effect.Element;
			if (control?.GetRenderer() is not UIView renderer)
				return;

			var color = effect.NativeAnimationColor;
			var radius = effect.NativeAnimationRadius;
			var shadowRadius = effect.NativeAnimationShadowRadius;
			var isStarted = status == TouchStatus.Started;
			defaultRadius = (float?)(defaultRadius ?? renderer.Layer.CornerRadius);
			defaultShadowRadius = (float?)(defaultShadowRadius ?? renderer.Layer.ShadowRadius);
			defaultShadowOpacity ??= renderer.Layer.ShadowOpacity;

			var tcs = new TaskCompletionSource<UIViewAnimatingPosition>();
			UIViewPropertyAnimator.CreateRunningPropertyAnimator(.2, 0, UIViewAnimationOptions.AllowUserInteraction,
				() =>
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
				}, endPos => tcs.SetResult(endPos));
			await tcs.Task;
		}
	}

	class TouchUITapGestureRecognizerDelegate : UIGestureRecognizerDelegate
	{
		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
		{
			if (gestureRecognizer is TouchUITapGestureRecognizer touchGesture && otherGestureRecognizer is UIPanGestureRecognizer &&
				otherGestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				touchGesture.HandleTouch(TouchStatus.Canceled, TouchInteractionStatus.Completed).SafeFireAndForget();
				touchGesture.IsCanceled = true;
			}

			return true;
		}

		public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
		{
			if (recognizer.View.IsDescendantOfView(touch.View))
				return true;

			if (recognizer.View is not IVisualNativeElementRenderer elementRenderer ||
				elementRenderer.Control == null)
				return false;

			if (elementRenderer.Control == touch.View ||
				elementRenderer.Control.Subviews.Any(view => view == touch.View))
				return true;

			return false;
		}
	}
}