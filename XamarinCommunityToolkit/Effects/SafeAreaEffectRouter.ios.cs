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
		Thickness? initialMargin;

		protected override void OnAttached()
		{
			if (!(Element is View element))
				return;

			if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || !UIApplication.SharedApplication.Windows.Any())
				return;

			var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;

			if (insets.Top <= 0)
				return;

			initialMargin = element.Margin;

			var safeArea = SafeAreaEffect.GetSafeArea(element);

			element.Margin = new Thickness(
				element.Margin.Left + (safeArea.Left ? insets.Left : 0),
				element.Margin.Top + (safeArea.Top ? insets.Top : 0),
				element.Margin.Right + (safeArea.Right ? insets.Right : 0),
				element.Margin.Bottom + (safeArea.Bottom ? insets.Bottom : 0));
		}

		protected override void OnDetached()
		{
			if (Element is Layout element && initialMargin.HasValue)
			{
				element.Margin = initialMargin.Value;
				initialMargin = null;
			}
		}
	}
}