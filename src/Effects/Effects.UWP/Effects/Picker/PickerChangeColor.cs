using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Media;
using FormsCommunityToolkit.Effects;
using PickerChangeColor = FormsCommunityToolkit.Effects.UWP.Effects.PickerChangeColor;

[assembly: ExportEffect(typeof(PickerChangeColor), nameof(PickerChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.UWP.Effects
{
    [Preserve]
    public class PickerChangeColor : PlatformEffect
    {
        private Windows.UI.Color _color;

        protected override void OnAttached()
        {
            var color = (Color)Element.GetValue(FormsCommunityToolkit.Effects.PickerChangeColor.ColorProperty);
            _color = ConvertColor(color);
            (Control as ComboBox).Foreground = new SolidColorBrush( _color);
        }

        protected override void OnDetached()
        {
        }

        private Windows.UI.Color ConvertColor(Xamarin.Forms.Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
        }
    }
}