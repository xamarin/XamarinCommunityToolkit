using System;
using Android.OS;

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
				if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
					return Looper.MainLooper?.IsCurrentThread ?? false;

				return Looper.MyLooper() == Looper.MainLooper;
			}
		}

		static void BeginInvokeOnMainThread(Action action)
		{
			if (handler == null || handler.Looper != Looper.MainLooper)
			{
#pragma warning disable CS8604 // Possible null reference argument.
				handler = new Handler(Looper.MainLooper);
#pragma warning restore CS8604 // Possible null reference argument.
			}

			handler.Post(action);
		}
	}
}