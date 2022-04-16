using System.Linq;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.SafeAreaEffectRouter), nameof(SafeAreaEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class SafeAreaEffectRouter : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		Thickness initialMargin;
		NSObject? orientationDidChangeNotificationObserver;

		new View Element => (View)base.Element;

		bool IsEligibleToConsumeEffect
			=> Element != null
				&& UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
				&& UIApplication.SharedApplication.Windows.Any();

		protected override void OnAttached()
		{
			if (!IsEligibleToConsumeEffect)
				return;

			orientationDidChangeNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(
				UIDevice.OrientationDidChangeNotification, _ => UpdateInsets());

			initialMargin = Element.Margin;
			UpdateInsets();
		}

		protected override void OnDetached()
		{
			if (!IsEligibleToConsumeEffect)
				return;

			if (orientationDidChangeNotificationObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(orientationDidChangeNotificationObserver);
				orientationDidChangeNotificationObserver?.Dispose();
				orientationDidChangeNotificationObserver = null;
			}

			Element.Margin = initialMargin;
		}

		void UpdateInsets()
		{
			// Double-check eligability something might've changed in regard to the windows
			if (!IsEligibleToConsumeEffect)
				return;

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