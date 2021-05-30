using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Effects
{
	public class SemanticEffectRouter : RoutingEffect
	{
		public SemanticEffectRouter()
			: base(EffectIds.Semantic)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __IOS__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.iOS.Effects.SemanticEffectRouter();
#elif __ANDROID__
			if (DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.Android.Effects.SemanticEffectRouter();
#endif
			#endregion
		}
	}
}