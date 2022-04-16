using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class RemoveBorderEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		UITextBorderStyle? oldBorderStyle;

		UITextField? TextField => Control as UITextField;

		protected override void OnAttached()
		{
			oldBorderStyle = TextField?.BorderStyle;
			SetBorderStyle(UITextBorderStyle.None);
		}

		protected override void OnDetached()
			=> SetBorderStyle(oldBorderStyle);

		void SetBorderStyle(UITextBorderStyle? borderStyle)
		{
			if (TextField != null && borderStyle.HasValue)
				TextField.BorderStyle = borderStyle.Value;
		}
	}
}