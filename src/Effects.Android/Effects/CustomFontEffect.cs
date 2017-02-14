using System.Linq;
using Android.Graphics;
using Android.Widget;
using FormsCommunityToolkit.Effects.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(CustomFontEffect), nameof(CustomFontEffect))]
namespace FormsCommunityToolkit.Effects.Droid
{
    public class CustomFontEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control as TextView;

            if (control == null)
                return;
            else
            {
                var effect = (FormsCommunityToolkit.Effects.CustomFontEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.CustomFontEffect);
                if (effect != null && !string.IsNullOrWhiteSpace(effect.FontPath))
                {
                    var font = Typeface.CreateFromAsset(Forms.Context.Assets, effect.FontPath);
                    control.Typeface = font;
                }
            }
        }

        protected override void OnDetached()
        {
        }
    }
}