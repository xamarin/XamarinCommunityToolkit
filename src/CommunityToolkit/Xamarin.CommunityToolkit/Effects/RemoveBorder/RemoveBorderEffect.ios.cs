using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class RemoveBorderEffect : PlatformEffect
	{
		UITextBorderStyle? oldBorderStyle;

		UITextField TextField => Control as UITextField;

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