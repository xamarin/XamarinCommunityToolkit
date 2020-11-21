using System.Linq;
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

		new View Element
			=> base.Element as View;

		bool IsEligibleToConsumeEffect
			=> Element != null
				&& UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
				&& UIApplication.SharedApplication.Windows.Any();

		protected override void OnAttached()
		{
			if (!IsEligibleToConsumeEffect)
				return;

			var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;
			var safeArea = SafeAreaEffect.GetSafeArea(Element);

			initialMargin = Element.Margin;
			Element.Margin = new Thickness(
				initialMargin.Left + CalculateInsets(insets.Left, safeArea.Left),
				initialMargin.Top + CalculateInsets(insets.Top, safeArea.Top),
				initialMargin.Right + CalculateInsets(insets.Right, safeArea.Right),
				initialMargin.Bottom + CalculateInsets(insets.Bottom, safeArea.Bottom));
		}

		protected override void OnDetached()
		{
			if (IsEligibleToConsumeEffect)
				Element.Margin = initialMargin;
		}

		double CalculateInsets(double insetsComponent, bool shouldUseInsetsComponent)
			=> shouldUseInsetsComponent
				? insetsComponent
				: 0;
	}
}