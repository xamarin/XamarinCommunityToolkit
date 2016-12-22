using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Foundation;

[assembly: ExportEffect(typeof(RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
	public class RemoveBorderEffect : PlatformEffect
	{
        UITextBorderStyle old;

		protected override void OnAttached()
		{
            var editText = Control as UITextField;
			if (editText != null)
			{
                old = editText.BorderStyle;
                editText.BorderStyle = UITextBorderStyle.None;
			}
		}

		protected override void OnDetached()
		{
			var editText = Control as UITextField;
			if (editText != null)
				editText.BorderStyle = old;
		}
	}
}
