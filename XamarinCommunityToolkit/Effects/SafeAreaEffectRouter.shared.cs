using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class SafeAreaEffectRouter : RoutingEffect
	{
		public SafeAreaEffectRouter()
			: base($"{nameof(CommunityToolkit)}.{nameof(SafeAreaEffectRouter)}")
		{
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.SafeAreaEffectRouter();
#endif
		}
	}
}