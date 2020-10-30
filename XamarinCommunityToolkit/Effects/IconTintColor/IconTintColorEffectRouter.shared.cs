using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class IconTintColorEffectRouter : RoutingEffect
	{
		public IconTintColorEffectRouter()
			: base($"{nameof(CommunityToolkit)}.{nameof(IconTintColorEffectRouter)}")
		{
#if MONOANDROID
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.IconTintColorEffectRouter();
#endif
		}
	}
}
