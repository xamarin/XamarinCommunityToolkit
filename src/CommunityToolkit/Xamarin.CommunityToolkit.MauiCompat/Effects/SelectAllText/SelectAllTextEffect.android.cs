using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using Android.Widget;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.SelectAllTextEffect), nameof(SelectAllTextEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class SelectAllTextEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		EditText EditText => (EditText)Control;

		protected override void OnAttached()
			=> EditText?.SetSelectAllOnFocus(true);

		protected override void OnDetached()
			=> EditText?.SetSelectAllOnFocus(false);
	}
}