using System;using Microsoft.Extensions.Logging;
using ElmSharp;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace Xamarin.CommunityToolkit.ObjectModel.Internals
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