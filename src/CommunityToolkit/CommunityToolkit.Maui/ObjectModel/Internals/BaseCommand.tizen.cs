using System;
using ElmSharp;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace CommunityToolkit.Maui.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static bool IsMainThread => EcoreMainloop.IsMainThread;

		static void BeginInvokeOnMainThread(Action action)
		{
			if (IsMainThread)
				action();
			else
				EcoreMainloop.PostAndWakeUp(action);
		}
	}
}