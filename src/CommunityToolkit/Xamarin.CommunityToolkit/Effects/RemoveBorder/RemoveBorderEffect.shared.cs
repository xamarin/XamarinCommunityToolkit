using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class RemoveBorderEffect : RoutingEffect
	{
		public RemoveBorderEffect()
			: base(EffectIds.RemoveBorder)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation

#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.RemoveBorderEffect();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.RemoveBorderEffect();
#endif
			#endregion
		}
	}
}