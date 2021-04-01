using System;
using Android.OS;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Debug = System.Diagnostics.Debug;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	static class BarStyle
	{
		internal static bool IsSupported()
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
			{
				Debug.WriteLine($"This functionality is not available. Minimum supported API is {BuildVersionCodes.M}");
				return false;
			}

			return true;
		}

		internal static void AddBarAppearanceFlag(StatusBarVisibility flag) =>
			SetBarAppearance(barAppearance => barAppearance |= flag);

		internal static void RemoveBarAppearanceFlag(StatusBarVisibility flag) =>
			SetBarAppearance(barAppearance => barAppearance &= ~flag);

		internal static void SetBarAppearance(Func<StatusBarVisibility, StatusBarVisibility> updateAppearance)
		{
			var window = GetCurrentWindow();
			var appearance = window.DecorView.SystemUiVisibility;
			appearance = updateAppearance(appearance);
			window.DecorView.SystemUiVisibility = appearance;
		}

		internal static Window GetCurrentWindow()
		{
			var activity = ToolkitPlatform.Context.GetActivity();
			var window = activity.Window ?? throw new NullReferenceException();
			window.ClearFlags(WindowManagerFlags.TranslucentStatus);
			window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			return window;
		}
	}
}