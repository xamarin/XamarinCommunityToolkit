using Foundation;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(EntryClear), nameof(EntryClear))]
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