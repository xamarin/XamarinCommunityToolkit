﻿using System;
using Android.App;
using Android.OS;
using Android.Views;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Effects
{
	static class BarStyle
	{
		internal static bool IsSupported()
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.M)
			{
				Log.Warning(string.Empty, $"This functionality is not available. Minimum supported API is {BuildVersionCodes.M}");
				return false;
			}

			return true;
		}

		internal static void AddBarAppearanceFlag(Activity activity, StatusBarVisibility flag) =>
			SetBarAppearance(activity, barAppearance => barAppearance |= flag);

		internal static void RemoveBarAppearanceFlag(Activity activity, StatusBarVisibility flag) =>
			SetBarAppearance(activity, barAppearance => barAppearance &= ~flag);

		static void SetBarAppearance(Activity activity, Func<StatusBarVisibility, StatusBarVisibility> updateAppearance)
		{
			var window = GetCurrentWindow(activity);
			var appearance = window.DecorView.SystemUiVisibility;
			appearance = updateAppearance(appearance);
			window.DecorView.SystemUiVisibility = appearance;
		}

		internal static Window GetCurrentWindow(Activity activity)
		{
			var window = activity.Window ?? throw new NullReferenceException();
			window.ClearFlags(WindowManagerFlags.TranslucentStatus);
			window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
			return window;
		}
	}
}