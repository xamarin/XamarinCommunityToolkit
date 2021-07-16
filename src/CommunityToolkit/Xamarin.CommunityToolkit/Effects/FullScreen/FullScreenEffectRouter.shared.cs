using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class FullScreenEffectRouter : RoutingEffect
	{
		public FullScreenEffectRouter()
			: base(EffectIds.FullScreenEffect)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.FullScreenEffectRouter();
#else
			throw new NotImplementedException();
#endif
		}
	}
}