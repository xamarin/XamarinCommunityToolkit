using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	public static partial class NavigationBar
	{
		static partial void SetColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			BarStyle.GetCurrentWindow().SetNavigationBarColor(color.ToAndroid());
		}

		static partial void SetStyle(NavigationBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			switch (style)
			{
				case NavigationBarStyle.Default:
				case NavigationBarStyle.LightContent:
					BarStyle.RemoveBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
				case NavigationBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
			}
		}
	}
}