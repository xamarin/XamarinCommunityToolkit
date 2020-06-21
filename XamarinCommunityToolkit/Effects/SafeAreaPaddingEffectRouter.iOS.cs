using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinCommunityToolkit.Effects;
using Effects = XamarinCommunityToolkit.iOS.Effects;

[assembly: ResolutionGroupName(nameof(XamarinCommunityToolkit))]
[assembly: ExportEffect(typeof(Effects.SafeAreaPaddingEffectRouter), nameof(Effects.SafeAreaPaddingEffectRouter))]
namespace XamarinCommunityToolkit.iOS.Effects
{
    public class SafeAreaPaddingEffectRouter : PlatformEffect
    {
        Thickness initialPadding;

        protected override void OnAttached()
        {
            if (!(Element is Layout element))
                return;

            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || !UIApplication.SharedApplication.Windows.Any())
                return;

            var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;

            initialPadding = element.Padding;

            if (insets.Top <= 0)
                return;

            var safeArea = SafeAreaPaddingEffect.GetSafeAreaPadding(element);

            element.Padding = new Thickness(
                initialPadding.Left + (safeArea.Left ? insets.Left : 0),
                initialPadding.Top + (safeArea.Top ? insets.Top : 0),
                initialPadding.Right + (safeArea.Right ? insets.Right : 0),
                initialPadding.Bottom + (safeArea.Bottom ? insets.Bottom : 0)
            );
        }

        protected override void OnDetached()
        {
            if (Element is Layout element)
                element.Padding = initialPadding;
        }
    }
}
