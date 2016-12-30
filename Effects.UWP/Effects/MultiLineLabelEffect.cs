using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FormsCommunityToolkit.Effects.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(MultiLineLabelEffect), nameof(MultiLineLabelEffect))]
namespace FormsCommunityToolkit.Effects.UWP
{
    public class MultiLineLabelEffect : PlatformEffect
    {
        int initialeLines;
        TextWrapping initialTextWrapping;

        protected override void OnAttached()
        {
            var control = Control as TextBlock;

			if (control == null)
				return;
			else
            {
                initialeLines = control.MaxLines;
                initialTextWrapping = control.TextWrapping;

                var effect = (Effects.MultiLineLabelEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.MultiLineLabelEffect);
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
                control.MaxLines = initialeLines;
                control.TextWrapping = initialTextWrapping;
            }
        }
    }
}