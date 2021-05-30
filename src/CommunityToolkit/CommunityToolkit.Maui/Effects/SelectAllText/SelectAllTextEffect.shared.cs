using System;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Effects
{
	public class SelectAllTextEffect : RoutingEffect
	{
		public SelectAllTextEffect()
			: base(EffectIds.SelectAllText)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.iOS.Effects.SelectAllTextEffect();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.Android.Effects.SelectAllTextEffect();
#elif UWP
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.UWP.Effects.SelectAllTextEffect();
#endif
			#endregion
		}
	}
}