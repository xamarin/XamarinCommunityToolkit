using System;using Microsoft.Extensions.Logging;
using Foundation;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static bool IsMainThread => NSThread.Current.IsMainThread;

		static void BeginInvokeOnMainThread(Action action) => NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
	}
}