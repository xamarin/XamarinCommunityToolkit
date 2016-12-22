using System;
using Android.Widget;
using FormsCommunityToolkit.Effects.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(DisableAutoCorrectEffect), nameof(DisableAutoCorrectEffect))]

namespace FormsCommunityToolkit.Effects.Droid
{
	public class DisableAutoCorrectEffect : PlatformEffect
	{

		Android.Text.InputTypes old;

		protected override void OnAttached()
		{
			var editText = Control as EditText;
			if (editText == null) return;

			old = editText.InputType;
			editText.InputType = editText.InputType | Android.Text.InputTypes.TextFlagNoSuggestions;
		}

		protected override void OnDetached()
		{
			var editText = Control as EditText;
			if (editText == null) return;

			editText.InputType = old;
		}
	}
}