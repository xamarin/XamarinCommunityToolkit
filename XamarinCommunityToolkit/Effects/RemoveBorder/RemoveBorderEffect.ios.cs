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
		UITextBorderStyle old;

		protected override void OnAttached()
		{
			var editText = Control as UITextField;
			if (editText == null)
				return;

			old = editText.BorderStyle;
			editText.BorderStyle = UITextBorderStyle.None;
		}

		protected override void OnDetached()
		{
			var editText = Control as UITextField;
			if (editText == null)
				return;

			editText.BorderStyle = old;
		}
	}
}
