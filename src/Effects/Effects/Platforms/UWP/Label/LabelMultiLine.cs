using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using RoutingEffects = Xamarin.Toolkit.Effects;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;

[assembly: ExportEffect(typeof(PlatformEffects.LabelMultiLine), nameof(RoutingEffects.LabelMultiLine))]
namespace Xamarin.Toolkit.Effects.UWP
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

                var effect = (RoutingEffects.LabelMultiLine)Element.Effects.FirstOrDefault(item => item is RoutingEffects.LabelMultiLine);
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