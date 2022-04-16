using System;using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Effects = Xamarin.CommunityToolkit.UWP.Effects;

[assembly: ExportEffect(typeof(Effects.SelectAllTextEffect), nameof(SelectAllTextEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class SelectAllTextEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		TextBox? EditText => Control as TextBox;

		protected override void OnAttached() => ApplyEffect(true);

		protected override void OnDetached() => ApplyEffect(false);

		void ApplyEffect(bool apply)
		{
			if (EditText == null)
				throw new NotSupportedException($"Control of type: {Control?.GetType()?.Name} is not supported by this effect.");

			EditText.GotFocus -= OnGotFocus;

			if (apply)
				EditText.GotFocus += OnGotFocus;
		}

		void OnGotFocus(object sender, RoutedEventArgs e) => EditText?.SelectAll();
	}
}