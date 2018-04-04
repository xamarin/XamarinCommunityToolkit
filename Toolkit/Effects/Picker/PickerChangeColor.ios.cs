using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Color = Xamarin.Forms.Color;
using RoutingEffects = Xamarin.Toolkit.Effects;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.PickerChangeColor), nameof(RoutingEffects.PickerChangeColorEffect))]
namespace Xamarin.Toolkit.Effects.iOS
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