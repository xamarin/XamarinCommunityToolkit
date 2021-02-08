using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class IconTintColorEffectRouter : RoutingEffect
	{
		public IconTintColorEffectRouter()
			: base(EffectIds.IconTintColor)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.IconTintColorEffectRouter();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.IconTintColorEffectRouter();
#endif
		}
	}
}