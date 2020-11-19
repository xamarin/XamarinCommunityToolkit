using System;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.VisualFeedbackEffectRouter), nameof(VisualFeedbackEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	[Foundation.Preserve(AllMembers = true)]
	public class VisualFeedbackEffectRouter : PlatformEffect
	{
		TouchEvents touchEvents;
		TouchEventsGestureRecognizer touchRecognizer;
		UIView view;
		UIView layer;
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
			touchEvents.TouchBegin -= OnTouchBegin;
			touchEvents.TouchEnd -= OnTouchEnd;
			touchEvents.TouchCancel -= OnTouchEnd;

			view.RemoveGestureRecognizer(touchRecognizer);
			touchRecognizer.Delegate?.Dispose();
			touchRecognizer.Delegate = null;
			touchRecognizer.Dispose();

			touchEvents = null;
			touchRecognizer = null;

			layer.RemoveFromSuperview();
			layer.Dispose();
			layer = null;

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
			layer.BackgroundColor = color.ToUIColor();
		}

		async void OnTouchBegin(object sender, EventArgs e)
		{
			if (!(Element is VisualElement visualElement) || !visualElement.IsEnabled)
				return;

			view.BecomeFirstResponder();

			await UIView.AnimateAsync(0.5, () =>
			{
				layer.Alpha = alpha;
			});
		}

		async void OnTouchEnd(object sender, EventArgs e)
		{
			if (!(Element is VisualElement visualElement) || !visualElement.IsEnabled) return;

			await UIView.AnimateAsync(0.5, () =>
			{
				layer.Alpha = 0;
			});
		}
	}

	public class ShouldRecognizeSimultaneouslyRecognizerDelegate : UIGestureRecognizerDelegate
	{
		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer) => true;
	}
}