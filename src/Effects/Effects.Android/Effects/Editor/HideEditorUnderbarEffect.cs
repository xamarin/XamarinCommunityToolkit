using System;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.Droid;

[assembly: ExportEffect(typeof(PlatformEffects.HideEditorUnderbarEffect), nameof(RoutingEffects.HideEditorUnderbar))]
namespace FormsCommunityToolkit.Effects.Droid
{
    public class HideEditorUnderbarEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var editText = Control as EditText;
			if (editText != null)
			{
				editText.SetBackgroundDrawable(null);
			}
		}

		protected override void OnDetached()
		{
		}
	}
}