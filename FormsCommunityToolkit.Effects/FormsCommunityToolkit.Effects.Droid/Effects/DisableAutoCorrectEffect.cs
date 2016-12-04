using System;
using Android.Widget;
using FormsCommunityToolkit.Effects.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(DisableAutoCorrectEffect), nameof(DisableAutoCorrectEffect))]

namespace FormsCommunityToolkit.Effects.Droid.Effects
{
	public class DisableAutoCorrectEffect : PlatformEffect
	{

		Android.Text.InputTypes _old;

		protected override void OnAttached()
		{
			var editText = Control as EditText;
			if (editText == null) return;

			_old = editText.InputType;
			editText.InputType = editText.InputType | Android.Text.InputTypes.TextFlagNoSuggestions;
		}

		protected override void OnDetached()
		{
			var editText = Control as EditText;
			if (editText == null) return;

			editText.InputType = _old;
		}
	}
}