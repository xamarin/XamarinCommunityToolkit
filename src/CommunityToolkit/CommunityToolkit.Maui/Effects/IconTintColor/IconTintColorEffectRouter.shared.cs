using Xamarin.Forms;

namespace CommunityToolkit.Maui.Effects
{
	public class IconTintColorEffectRouter : RoutingEffect
	{
		public IconTintColorEffectRouter()
			: base(EffectIds.IconTintColor)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.Android.Effects.IconTintColorEffectRouter();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new CommunityToolkit.Maui.iOS.Effects.IconTintColorEffectRouter();
#endif
			#endregion
		}
	}
}