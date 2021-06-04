using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.BackgroundAspectEffectRouter), nameof(BackgroundAspectEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class BackgroundAspectEffectRouter : PlatformEffect
	{
		protected override void OnAttached()
		{
			Debugger.Break();
		}

		protected override void OnDetached()
		{

		}
	}
}
