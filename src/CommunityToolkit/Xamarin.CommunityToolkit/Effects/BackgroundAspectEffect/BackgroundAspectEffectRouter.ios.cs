using System;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Effects = Xamarin.CommunityToolkit.iOS.Effects;

[assembly: ExportEffect(typeof(Effects.BackgroundAspectEffectRouter), nameof(BackgroundAspectEffectRouter))]

namespace Xamarin.CommunityToolkit.iOS.Effects
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
