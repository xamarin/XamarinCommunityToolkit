using System;
using UIKit;
using CommunityToolkit.Maui.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = CommunityToolkit.Maui.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.VisualFeedbackEffectRouter), nameof(VisualFeedbackEffect))]

namespace CommunityToolkit.Maui.iOS.Effects
{
	[Foundation.Preserve(AllMembers = true)]
	public class VisualFeedbackEffectRouter : PlatformEffect
	{
		TouchEvents? touchEvents;
		TouchEventsGestureRecognizer? touchRecognizer;
		UIView? view;
		UIView? layer;
		float alpha;

		protected override void OnAttached()
		{
			view = Control ?? Container;

			view.UserInteractionEnabled = true;

			layer = new UIView
			{
				Alpha = 0,
				Opaque = false,
				UserInteractionEnabled = false
			};
			view.AddSubview(layer);

			layer.TranslatesAutoresizingMaskIntoConstraints = false;

			layer.TopAnchor.ConstraintEqualTo(view.TopAnchor).Active = true;
			layer.LeftAnchor.ConstraintEqualTo(view.LeftAnchor).Active = true;
			layer.BottomAnchor.ConstraintEqualTo(view.BottomAnchor).Active = true;
			layer.RightAnchor.ConstraintEqualTo(view.RightAnchor).Active = true;

			view.BringSubviewToFront(layer);

			touchEvents = new TouchEvents();

			touchRecognizer = new TouchEventsGestureRecognizer(touchEvents);
			touchRecognizer.Delegate = new ShouldRecognizeSimultaneouslyRecognizerDelegate();

			view.AddGestureRecognizer(touchRecognizer);

			touchEvents.TouchBegin += OnTouchBegin;
			touchEvents.TouchEnd += OnTouchEnd;
			touchEvents.TouchCancel += OnTouchEnd;

			UpdateEffectColor();
		}

		protected override void OnDetached()
		{
			if (touchEvents != null)
			{
				touchEvents.TouchBegin -= OnTouchBegin;
				touchEvents.TouchEnd -= OnTouchEnd;
				touchEvents.TouchCancel -= OnTouchEnd;
			}

			if (view != null && touchRecognizer != null)
			{
				view.RemoveGestureRecognizer(touchRecognizer);
				touchRecognizer.Delegate.Dispose();
				touchRecognizer.Dispose();
			}

			if (layer != null)
			{
				layer.RemoveFromSuperview();
				layer.Dispose();
			}

			layer = null;
			touchRecognizer = null;
			touchEvents = null;
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
			alpha = color.A < 1.0f ? 1f : 0.1f;

			if (layer != null)
				layer.BackgroundColor = color.ToUIColor();
		}

		async void OnTouchBegin(object? sender, EventArgs e)
		{
			if (Element is not VisualElement visualElement || !visualElement.IsEnabled)
				return;

			view?.BecomeFirstResponder();

			await UIView.AnimateAsync(0.5, () =>
			{
				if (layer != null)
					layer.Alpha = alpha;
			});
		}

		async void OnTouchEnd(object? sender, EventArgs e)
		{
			if (Element is not VisualElement visualElement || !visualElement.IsEnabled)
				return;

			await UIView.AnimateAsync(0.5, () =>
			{
				if (layer != null)
					layer.Alpha = 0;
			});
		}
	}

	public class ShouldRecognizeSimultaneouslyRecognizerDelegate : UIGestureRecognizerDelegate
	{
		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer) => true;
	}
}