using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Linq;
using RoutingEffects = XamarinCommunityToolkit.Effects;
using PlatformEffects = XamarinCommunityToolkit.Effects.iOS;

[assembly: ExportEffect (typeof (PlatformEffects.LabelCustomFont), nameof (RoutingEffects.LabelCustomFont))]
namespace XamarinCommunityToolkit.Effects.iOS
{
    public class LabelCustomFont : PlatformEffect
    {
        private RoutingEffects.LabelCustomFont _effect;
        protected override void OnAttached ()
        {
            var control = Control as UILabel;

            if (control == null)
                return;

            _effect = (RoutingEffects.LabelCustomFont)Element.Effects.FirstOrDefault (item => item is RoutingEffects.LabelCustomFont);
            if (_effect != null && !string.IsNullOrWhiteSpace (_effect.FontPath))
                control.Font = UIFont.FromName (_effect.FontFamilyName, control.Font.PointSize);

            // After one of these properties change, reapply the custom font
            // As per https://bugzilla.xamarin.com/show_bug.cgi?id=33666
            Element.PropertyChanged += (sender, e) => {
				if (e.PropertyName == Label.TextColorProperty.PropertyName
				    || e.PropertyName == Label.FontProperty.PropertyName
				    || e.PropertyName == Label.TextProperty.PropertyName
				    || e.PropertyName == Label.FormattedTextProperty.PropertyName)
				{
                    control.Font = UIFont.FromName (_effect.FontFamilyName, control.Font.PointSize);
                }
            };
        }

        protected override void OnDetached ()
        {
        }
    }
}