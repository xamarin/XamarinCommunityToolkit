using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryRemoveBorder), nameof(RoutingEffects.EntryRemoveBorder))]
namespace Xamarin.Toolkit.Effects.UWP
{
    public class EntryRemoveBorder : PlatformEffect
    {
        Windows.UI.Xaml.Thickness old;

        protected override void OnAttached()
        {
            var textBox = Control as TextBox;
            if (textBox == null)
                return;
            old = textBox.BorderThickness;
            textBox.BorderThickness = new Windows.UI.Xaml.Thickness(0);
        }

        protected override void OnDetached()
        {
            var textBox = Control as TextBox;
            if (textBox == null)
                return;

            textBox.BorderThickness = old;
        }
    }
}
