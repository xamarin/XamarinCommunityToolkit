using System;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Effects
{
	public class SafeAreaEffectRouter : RoutingEffect
	{
		public SafeAreaEffectRouter()
			: base(EffectIds.SafeArea)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.iOS.Effects.SafeAreaEffectRouter();
#endif
			#endregion
		}
	}
}