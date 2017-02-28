using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.EntryItalicPlaceholder), nameof(RoutingEffects.EntryItalicPlaceholder))]
namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class EntryItalicPlaceholder : PlatformEffect
    {
        private NSAttributedString _old;

        protected override void OnAttached()
        {
            var entry = Control as UITextField;
            if (entry == null || string.IsNullOrWhiteSpace(entry.Placeholder))
                return;

            _old = entry.AttributedPlaceholder;
            var entryFontSize = entry.Font.PointSize;
            entry.AttributedPlaceholder = new NSAttributedString(entry.Placeholder, font: UIFont.ItalicSystemFontOfSize(entryFontSize));
        }

        protected override void OnDetached()
        {
            var entry = Control as UITextField;
            if (entry == null)
                return;

            entry.AttributedPlaceholder = _old;
        }
    }
}
