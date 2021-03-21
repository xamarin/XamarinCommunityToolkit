using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class StatusBar
	{
		static partial void SetColor(Color color)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			var activity = (FormsAppCompatActivity)ToolkitPlatform.Context.GetActivity();
			activity.SetStatusBarColor(color.ToAndroid());
		}

		static partial void SetStyle(StatusBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			switch (style)
			{
				case StatusBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
				case StatusBarStyle.LightContent:
				default:
					BarStyle.RemoveBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
			}
		}
	}
}