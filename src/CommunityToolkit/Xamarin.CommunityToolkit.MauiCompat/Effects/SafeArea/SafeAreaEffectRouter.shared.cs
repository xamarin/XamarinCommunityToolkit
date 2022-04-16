using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class SafeAreaEffectRouter : RoutingEffect
	{
		public SafeAreaEffectRouter()
			: base(EffectIds.SafeArea)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.SafeAreaEffectRouter();
#endif
			#endregion
		}
	}
}