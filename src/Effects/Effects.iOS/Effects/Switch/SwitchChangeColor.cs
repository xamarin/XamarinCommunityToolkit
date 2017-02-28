using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.iOS;

[assembly: ExportEffect (typeof (PlatformEffects.SwitchChangeColor), nameof (RoutingEffects.SwitchChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class SwitchChangeColor : PlatformEffect
    {
        private Color _trueColor;
        private Color _falseColor;

        protected override void OnAttached ()
        {
            var uiSwitch = Control as UISwitch;
            if (uiSwitch == null)
                return;

            _trueColor = (Color)Element.GetValue (RoutingEffects.SwitchChangeColor.TrueColorProperty);
            _falseColor = (Color)Element.GetValue (RoutingEffects.SwitchChangeColor.FalseColorProperty);

            if (_falseColor != Color.Transparent)
            {
                uiSwitch.TintColor = _falseColor.ToUIColor();
                uiSwitch.Layer.CornerRadius = 16;
                uiSwitch.BackgroundColor = _falseColor.ToUIColor();
            }

            if (_trueColor != Color.Transparent)
                uiSwitch.OnTintColor = _trueColor.ToUIColor();
        }

        protected override void OnDetached()
        {
            //Needed because of inheritance
        }
    }
}
