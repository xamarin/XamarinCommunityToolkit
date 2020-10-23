using Android.Widget;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.SelectAllTextEffect), nameof(SelectAllTextEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class SelectAllTextEffect : PlatformEffect
	{
        protected override void OnAttached()
        {
            AttachEffect(true);
        }

        protected override void OnDetached()
        {
            AttachEffect(false);
        }

        void AttachEffect(bool apply)
		{
            var editText = Control as EditText;
            if (editText == null)
                return;

            editText.SetSelectAllOnFocus(apply);
        }
    }
}
