using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class StatusBar
	{
		static partial void PlatformSetColor(Color color)
		{
			if (!BarStyle.IsSupported())
				return;

			var activity = (FormsAppCompatActivity)ToolkitPlatform.Context.GetActivity();
			activity.SetStatusBarColor(color.ToAndroid());
		}

		static partial void PlatformSetStyle(StatusBarStyle style)
		{
			if (!BarStyle.IsSupported())
				return;

			switch (style)
			{
				case StatusBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
				default:
					BarStyle.RemoveBarAppearanceFlag((StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
			}
		}
	}
}