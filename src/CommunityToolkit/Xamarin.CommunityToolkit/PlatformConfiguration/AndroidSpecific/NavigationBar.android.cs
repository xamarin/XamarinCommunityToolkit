using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Xamarin.CommunityToolkit.Android.Effects.PlatformStatusBarEffect;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	public static partial class NavigationBar
	{
		static partial void SetColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			GetCurrentWindow().SetNavigationBarColor(color.ToAndroid());
		}

		static partial void SetStyle(NavigationBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			switch (style)
			{
				case NavigationBarStyle.Default:
				case NavigationBarStyle.LightContent:
					RemoveBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
				case NavigationBarStyle.DarkContent:
					AddBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
			}
		}

		static FormsAppCompatActivity Activity => PlatformStatusBarEffect.Activity;

		static Window GetCurrentWindow() => PlatformStatusBarEffect.GetCurrentWindow(Activity);
	}
}