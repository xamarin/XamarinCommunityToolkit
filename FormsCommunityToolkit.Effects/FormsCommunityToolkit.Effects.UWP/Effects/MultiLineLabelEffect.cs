using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FormsCommunityToolkit.Effects.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(MultiLineLabelEffect), nameof(MultiLineLabelEffect))]
namespace FormsCommunityToolkit.Effects.UWP.Effects
{
    public class MultiLineLabelEffect : PlatformEffect
    {
        private int _initialeLines;
        private TextWrapping _initialTextWrapping;

        protected override void OnAttached()
        {
            TextBlock control = Control as TextBlock;

            if (control != null)
            {
                _initialeLines = control.MaxLines;
                _initialTextWrapping = control.TextWrapping;

                var effect = (FormsCommunityToolkit.Effects.MultiLineLabelEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.MultiLineLabelEffect);
                if (effect != null && effect.Lines > 0)
                {
                    control.MaxLines = effect.Lines;
                    control.TextWrapping = TextWrapping.WrapWholeWords;
                }
            }            
        }

        protected override void OnDetached()
        {
            TextBlock control = Control as TextBlock;

            if (control != null)
            {
                control.MaxLines = _initialeLines;
                control.TextWrapping = _initialTextWrapping;
            }
        }
    }
}