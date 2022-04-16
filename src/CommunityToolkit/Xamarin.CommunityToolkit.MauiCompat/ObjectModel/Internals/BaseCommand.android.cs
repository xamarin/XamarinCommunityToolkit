using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using Android.OS;
using Xamarin.CommunityToolkit.Helpers;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static volatile Handler? handler;

		static bool IsMainThread
		{
			get
			{
				if (XCT.SdkInt >= (int)BuildVersionCodes.M)
					return Looper.MainLooper?.IsCurrentThread ?? false;

				return Looper.MyLooper() == Looper.MainLooper;
			}
		}

		static void BeginInvokeOnMainThread(Action action)
		{
			if (handler == null || handler.Looper != Looper.MainLooper)
			{
				handler = new Handler(Looper.MainLooper ?? throw new NullReferenceException($"{nameof(Looper.MainLooper)} cannot be null"));
			}

			handler.Post(action);
		}
	}
}