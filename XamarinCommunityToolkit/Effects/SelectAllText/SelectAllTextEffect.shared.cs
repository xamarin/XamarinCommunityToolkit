using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class SelectAllTextEffect : RoutingEffect
	{
		public SelectAllTextEffect()
			: base(EffectIds.SelectAllText)
		{
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.SelectAllTextEffect();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.SelectAllTextEffect();
#endif
		}
	}
}