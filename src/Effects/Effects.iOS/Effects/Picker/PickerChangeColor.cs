using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Color = Xamarin.Forms.Color;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.PickerChangeColor), nameof(RoutingEffects.PickerChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class PickerChangeColor : PlatformEffect
    {
        private Color _color;

        protected override void OnAttached()
        {
            /*
             * Text Color change when I select a value
             */ 
            _color = (Color)Element.GetValue(RoutingEffects.PickerChangeColor.ColorProperty);
            (Control as UITextField).AttributedPlaceholder = new Foundation.NSAttributedString((Control as UITextField).AttributedPlaceholder.Value, foregroundColor: _color.ToUIColor());
        }

        protected override void OnDetached()
        {
        }
    }
}