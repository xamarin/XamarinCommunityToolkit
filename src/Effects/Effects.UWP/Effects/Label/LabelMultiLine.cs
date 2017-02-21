using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FormsCommunityToolkit.Effects.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(LabelMultiLine), nameof(LabelMultiLine))]
namespace FormsCommunityToolkit.Effects.UWP
{
    public class LabelMultiLine : PlatformEffect
    {
        private int _initialeLines;
        private TextWrapping _initialTextWrapping;

        protected override void OnAttached()
        {
            var control = Control as TextBlock;

            if (control == null)
                return;
            else
            {
                _initialeLines = control.MaxLines;
                _initialTextWrapping = control.TextWrapping;

                var effect = (FormsCommunityToolkit.Effects.LabelMultiLine)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.LabelMultiLine);
                if (effect != null && effect.Lines > 0)
                {
                    control.MaxLines = effect.Lines;
                    control.TextWrapping = TextWrapping.WrapWholeWords;
                }
            }            
        }

        protected override void OnDetached()
        {
            var control = Control as TextBlock;

            if (control == null)
                return;
            else
            {
                control.MaxLines = _initialeLines;
                control.TextWrapping = _initialTextWrapping;
            }
        }
    }
}