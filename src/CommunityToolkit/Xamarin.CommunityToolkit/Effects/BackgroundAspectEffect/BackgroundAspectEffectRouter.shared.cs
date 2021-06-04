using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class BackgroundAspectEffectRouter : RoutingEffect
	{
		public BackgroundAspectEffectRouter()
			: base(EffectIds.BackgroundAspect)
		{
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.BackgroundAspectEffectRouter();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.BackgroundAspectEffectRouter();
#endif
		}
	}
}
