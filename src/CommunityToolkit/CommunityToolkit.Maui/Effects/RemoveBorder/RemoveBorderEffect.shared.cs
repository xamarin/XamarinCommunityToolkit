using System;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Effects
{
	public class RemoveBorderEffect : RoutingEffect
	{
		public RemoveBorderEffect()
			: base(EffectIds.RemoveBorder)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation

#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.iOS.Effects.RemoveBorderEffect();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.Android.Effects.RemoveBorderEffect();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.UWP.Effects.RemoveBorderEffect();
#endif
			#endregion
		}
	}
}