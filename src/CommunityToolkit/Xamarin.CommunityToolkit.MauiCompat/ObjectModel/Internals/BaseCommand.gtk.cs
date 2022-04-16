using System;using Microsoft.Extensions.Logging;
using System.Threading;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		readonly SynchronizationContext? synchronizationContext = SynchronizationContext.Current;

		bool IsMainThread => SynchronizationContext.Current == synchronizationContext;

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