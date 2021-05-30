using Android.Widget;
using CommunityToolkit.Maui.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Effects = CommunityToolkit.Maui.Android.Effects;

[assembly: ExportEffect(typeof(Effects.SelectAllTextEffect), nameof(SelectAllTextEffect))]

namespace CommunityToolkit.Maui.Android.Effects
{
	public class SelectAllTextEffect : PlatformEffect
	{
		EditText EditText => (EditText)Control;

		protected override void OnAttached()
			=> EditText?.SetSelectAllOnFocus(true);

		protected override void OnDetached()
			=> EditText?.SetSelectAllOnFocus(false);
	}
}