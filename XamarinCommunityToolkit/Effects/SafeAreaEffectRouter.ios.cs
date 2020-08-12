using System.Linq;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ResolutionGroupName(nameof(Xamarin.CommunityToolkit))]
[assembly: ExportEffect(typeof(Effects.SafeAreaEffectRouter), nameof(SafeAreaEffectRouter))]
namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class SafeAreaEffectRouter : PlatformEffect
	{
		Thickness initialMargin;

		protected override void OnAttached()
		{
			if (!(Element is View element))
				return;

			if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || !UIApplication.SharedApplication.Windows.Any())
				return;

			var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;

			initialMargin = element.Margin;

			if (insets.Top <= 0)
				return;

			var safeArea = SafeAreaEffect.GetSafeArea(element);

			element.Margin = new Thickness(
				initialMargin.Left + (safeArea.Left ? insets.Left : 0),
				initialMargin.Top + (safeArea.Top ? insets.Top : 0),
				initialMargin.Right + (safeArea.Right ? insets.Right : 0),
				initialMargin.Bottom + (safeArea.Bottom ? insets.Bottom : 0)
			);
		}

		protected override void OnDetached()
		{
			if (Element is Layout element)
				element.Margin = initialMargin;
		}
	}
}