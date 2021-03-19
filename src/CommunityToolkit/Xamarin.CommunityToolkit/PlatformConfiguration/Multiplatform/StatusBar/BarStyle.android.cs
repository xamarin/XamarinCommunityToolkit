using System;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class BarStyle
	{
		internal static void AddBarAppearanceFlag(FormsAppCompatActivity activity, StatusBarVisibility flag) =>
			SetBarAppearance(activity, barAppearance => barAppearance |= flag);

		internal static void RemoveBarAppearanceFlag(FormsAppCompatActivity activity, StatusBarVisibility flag) =>
			SetBarAppearance(activity, barAppearance => barAppearance &= ~flag);

		internal static void SetBarAppearance(FormsAppCompatActivity activity, Func<StatusBarVisibility, StatusBarVisibility> updateAppearance)
		{
			var currentWindow = GetCurrentWindow(activity);

			var appearance = currentWindow.DecorView.SystemUiVisibility;
			appearance = updateAppearance(appearance);
			currentWindow.DecorView.SystemUiVisibility = appearance;
		}

		internal static Window GetCurrentWindow(FormsAppCompatActivity activity)
		{
			var window = activity.Window ?? throw new NullReferenceException();
			window.ClearFlags(WindowManagerFlags.TranslucentStatus);
			window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			return window;
		}
	}
}