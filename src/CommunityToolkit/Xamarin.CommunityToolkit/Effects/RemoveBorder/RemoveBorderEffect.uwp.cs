using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Platform.UWP;
using Effects = Xamarin.CommunityToolkit.UWP.Effects;

[assembly: Xamarin.Forms.ExportEffect(typeof(Effects.RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class RemoveBorderEffect : PlatformEffect
	{
		Thickness oldBorderThickness;

		protected override void OnAttached()
		{
			if (Control is Control uwpControl)
			{
				oldBorderThickness = uwpControl.BorderThickness;
				uwpControl.BorderThickness = new Thickness(0.0);
			}
		}

		protected override void OnDetached()
		{
			if (Control is Control uwpControl)
			{
				uwpControl.BorderThickness = oldBorderThickness;
			}
		}
	}
}