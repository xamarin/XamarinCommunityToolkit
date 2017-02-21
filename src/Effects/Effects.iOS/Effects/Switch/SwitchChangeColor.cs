using FormsCommunityToolkit.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SwitchChangeColor = FormsCommunityToolkit.Effects.iOS.SwitchChangeColor;

[assembly: ExportEffect (typeof (SwitchChangeColor), nameof (SwitchChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class SwitchChangeColor : PlatformEffect
    {
        private Color _trueColor;
        private Color _falseColor;

        public SwitchChangeColor ()
        {
        }

        protected override void OnAttached ()
        {
            _trueColor = (Color)Element.GetValue (FormsCommunityToolkit.Effects.SwitchChangeColor.TrueColorProperty);
            _falseColor = (Color)Element.GetValue (FormsCommunityToolkit.Effects.SwitchChangeColor.FalseColorProperty);

            if (_falseColor != Color.Transparent)
            {
                (Control as UISwitch).TintColor = _falseColor.ToUIColor ();
                (Control as UISwitch).Layer.CornerRadius = 16;
                (Control as UISwitch).BackgroundColor = _falseColor.ToUIColor ();
            }

            if (_trueColor != Color.Transparent)
                (Control as UISwitch).OnTintColor = _trueColor.ToUIColor ();
        }

        protected override void OnDetached ()
        {
        }
    }
}
