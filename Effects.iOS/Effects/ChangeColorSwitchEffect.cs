using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect (typeof (ChangeColorSwitchEffect), nameof (ChangeColorSwitchEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class ChangeColorSwitchEffect : PlatformEffect
    {
        Color trueColor;
        Color falseColor;

        public ChangeColorSwitchEffect ()
        {
        }

        protected override void OnAttached ()
        {
            trueColor = (Color)Element.GetValue (ChangeColorEffect.TrueColorProperty);
            falseColor = (Color)Element.GetValue (ChangeColorEffect.FalseColorProperty);

            if (falseColor != Color.Transparent)
			{
                (Control as UISwitch).TintColor = falseColor.ToUIColor ();
                (Control as UISwitch).Layer.CornerRadius = 16;
                (Control as UISwitch).BackgroundColor = falseColor.ToUIColor ();
            }

            if (trueColor != Color.Transparent)
                (Control as UISwitch).OnTintColor = trueColor.ToUIColor ();
        }

        protected override void OnDetached ()
        {
        }
    }
}
