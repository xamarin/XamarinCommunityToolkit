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

			Activity.SetStatusBarColor(color.ToAndroid());
		}

		static partial void SetStyle(StatusBarStyle style)
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
				return;

			switch (style)
			{
				case StatusBarStyle.Default:
				case StatusBarStyle.LightContent:
					BarStyle.RemoveBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
				case StatusBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
			}
		}

		static FormsAppCompatActivity Activity => (FormsAppCompatActivity)ToolkitPlatform.Context.GetActivity();
	}
}