using System;
using System.Threading;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static bool IsMainThread => Thread.CurrentThread == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread;

		static void BeginInvokeOnMainThread(Action action) => System.Windows.Application.Current?.Dispatcher.BeginInvoke(action);
	}
}
