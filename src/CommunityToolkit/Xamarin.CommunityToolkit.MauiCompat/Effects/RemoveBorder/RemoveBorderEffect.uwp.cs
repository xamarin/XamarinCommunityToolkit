using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Effects = Xamarin.CommunityToolkit.UWP.Effects;

[assembly: Microsoft.Maui.Controls.ExportEffect(typeof(Effects.RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class RemoveBorderEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
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