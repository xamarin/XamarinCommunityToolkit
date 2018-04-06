using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.PickerChangeColor), nameof(RoutingEffects.PickerChangeColorEffect))]
namespace Xamarin.Toolkit.Effects.UWP
{
    [Preserve]
    public class PickerChangeColor : PlatformEffect
    {
        Windows.UI.Color color;

        protected override void OnAttached()
        {
            var color = (Color)Element.GetValue(RoutingEffects.PickerChangeColor.ColorProperty);
            this.color = ConvertColor(color);
            (Control as ComboBox).Foreground = new SolidColorBrush(this.color);
        }

        protected override void OnDetached()
        {
        }

        Windows.UI.Color ConvertColor(Color color) =>
            Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
    }
}
