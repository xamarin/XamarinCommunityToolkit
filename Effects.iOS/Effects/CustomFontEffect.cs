using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using System.Linq;

[assembly: ExportEffect (typeof (CustomFontEffect), nameof (CustomFontEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class CustomFontEffect : PlatformEffect
    {
        protected override void OnAttached ()
        {
            var control = Control as UILabel;

			if (control == null)
				return;

			var effect = (FormsCommunityToolkit.Effects.CustomFontEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.CustomFontEffect);
			if (effect != null && !string.IsNullOrWhiteSpace(effect.FontPath))
				control.Font = UIFont.FromName(effect.FontFamilyName, control.Font.PointSize);
		}

        protected override void OnDetached ()
        {
        }
    }
}