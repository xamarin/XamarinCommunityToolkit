using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class SemanticEffectRouter : RoutingEffect
	{
		public SemanticEffectRouter()
			: base(EffectIds.Semantic)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.SemanticEffectRouter();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.SemanticEffectRouter();
#endif
			#endregion
		}
	}
}