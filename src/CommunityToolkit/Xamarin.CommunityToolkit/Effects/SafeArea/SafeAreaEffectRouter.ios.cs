using System.Linq;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.SafeAreaEffectRouter), nameof(SafeAreaEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class SafeAreaEffectRouter : PlatformEffect
	{
		Thickness initialMargin;
		NSObject? didChangeStatusBarOrientationNotificationObserver;
		NSObject? didChangeStatusBarFrameNotificationObserver;

		new View Element => (View)base.Element;

		bool IsEligibleToConsumeEffect
			=> Element != null
				&& UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
				&& UIApplication.SharedApplication.Windows.Any();

		protected override void OnAttached()
		{
			if (!IsEligibleToConsumeEffect)
				return;

			didChangeStatusBarOrientationNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(
				UIApplication.DidChangeStatusBarOrientationNotification, _ => UpdateInsets());

			didChangeStatusBarFrameNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(
				UIApplication.DidChangeStatusBarFrameNotification, _ => UpdateInsets());

			initialMargin = Element.Margin;
			UpdateInsets();
		}

		protected override void OnDetached()
		{
			if (!IsEligibleToConsumeEffect)
				return;

			if (didChangeStatusBarOrientationNotificationObserver != null) {
				NSNotificationCenter.DefaultCenter.RemoveObserver(didChangeStatusBarOrientationNotificationObserver);
				didChangeStatusBarOrientationNotificationObserver?.Dispose();
				didChangeStatusBarOrientationNotificationObserver = null;
			}
			if (didChangeStatusBarFrameNotificationObserver != null) {
				NSNotificationCenter.DefaultCenter.RemoveObserver(didChangeStatusBarFrameNotificationObserver);
				didChangeStatusBarFrameNotificationObserver?.Dispose();
				didChangeStatusBarFrameNotificationObserver = null;
			}

			Element.Margin = initialMargin;
		}

		void UpdateInsets()
		{
			var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
			var safeArea = SafeAreaEffect.GetSafeArea(Element);

			Element.Margin = new Thickness(
				initialMargin.Left + CalculateInsets(insets.Left, safeArea.Left),
				initialMargin.Top + CalculateInsets(insets.Top, safeArea.Top),
				initialMargin.Right + CalculateInsets(insets.Right, safeArea.Right),
				initialMargin.Bottom + CalculateInsets(insets.Bottom, safeArea.Bottom));
		}

		double CalculateInsets(double insetsComponent, bool shouldUseInsetsComponent) => shouldUseInsetsComponent ? insetsComponent : 0;
	}
}
