using System.Linq;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformOtpEffect), nameof(OtpEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformOtpEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var effect = (OtpEffect)Element.Effects
				.FirstOrDefault(e => e is OtpEffect);
			if (effect != null
				&& UIDevice.CurrentDevice.CheckSystemVersion(12, 0)
				&& Control is UITextField textField)
			{
				textField.TextContentType = UITextContentType.OneTimeCode;
			}
		}

		protected override void OnDetached()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
				&& Control is UITextField textField)
			{
				textField.TextContentType = NSString.Empty;
			}
		}
	}
}
