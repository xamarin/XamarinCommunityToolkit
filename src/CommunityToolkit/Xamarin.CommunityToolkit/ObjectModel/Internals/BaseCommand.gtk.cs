using System;
using System.Threading;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static readonly Thread mainThread = Thread.CurrentThread;

		static bool IsMainThread => Thread.CurrentThread == mainThread;

		static void BeginInvokeOnMainThread(Action action)
		{
			GLib.Idle.Add(() =>
			{
				action();
				return false;
			});
		}
	}
}
