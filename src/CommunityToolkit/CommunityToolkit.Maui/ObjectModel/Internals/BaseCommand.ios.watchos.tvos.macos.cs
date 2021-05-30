using System;
using Foundation;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace CommunityToolkit.Maui.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static bool IsMainThread => NSThread.Current.IsMainThread;

		static void BeginInvokeOnMainThread(Action action) => NSRunLoop.Main.BeginInvokeOnMainThread(action.Invoke);
	}
}