using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Effects = Xamarin.CommunityToolkit.UWP.Effects;

[assembly: ExportEffect(typeof(Effects.SelectAllTextEffect), nameof(SelectAllTextEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class SelectAllTextEffect : PlatformEffect
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