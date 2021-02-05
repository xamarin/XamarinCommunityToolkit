using System;
using Android.OS;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static volatile Handler handler;

		static bool IsMainThread
		{
			get
			{
				if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
					return Looper.MainLooper.IsCurrentThread;

				return Looper.MyLooper() == Looper.MainLooper;
			}
		}

		static void BeginInvokeOnMainThread(Action action)
		{
			if (handler?.Looper != Looper.MainLooper)
				handler = new Handler(Looper.MainLooper);

			handler.Post(action);
		}
	}
}
