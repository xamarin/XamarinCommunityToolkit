using Window = Android.Views.Window;using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using Android.App;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.Effects
{
	static class BarStyle
	{
		internal static bool IsSupported()
		{
			if (XCT.SdkInt < (int)(int)BuildVersionCodes.M)
			{
				(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogWarning(string.Empty, $"This functionality is not available. Minimum supported API is {(int)BuildVersionCodes.M}");
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