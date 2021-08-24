using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class IconTintColorEffectRouter : RoutingEffect
	{
		public IconTintColorEffectRouter()
			: base(EffectIds.IconTintColor)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.IconTintColorEffectRouter();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.IconTintColorEffectRouter();
#endif
			#endregion
		}
	}
}