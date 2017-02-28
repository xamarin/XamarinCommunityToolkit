using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.EntryClear), nameof(RoutingEffects.EntryClear))]
namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class EntryClear : PlatformEffect
    {
        private UITextFieldViewMode _old;

        protected override void OnAttached()
        {
            ConfigureControl();
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            editText.ClearButtonMode = _old;
        }

        private void ConfigureControl()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            _old = editText.ClearButtonMode;
            editText.ClearButtonMode = UITextFieldViewMode.WhileEditing;
        }
    }
}