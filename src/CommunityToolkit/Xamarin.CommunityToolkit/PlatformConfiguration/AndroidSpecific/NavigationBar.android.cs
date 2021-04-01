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
			if (!BarStyle.IsSupported())
				return;

			BarStyle.GetCurrentWindow().SetNavigationBarColor(color.ToAndroid());
		}

		static partial void SetStyle(NavigationBarStyle style)
		{
			if (!BarStyle.IsSupported())
				return;

			switch (style)
			{
				case NavigationBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
				default:
					BarStyle.RemoveBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
			}
		}
	}
}
