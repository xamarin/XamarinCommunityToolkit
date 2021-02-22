using System;
using System.Threading;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

		static bool IsMainThread
		{
			get
			{
				try
				{
					return Thread.CurrentThread == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread;
				}

				// TypeLoadException is thrown when not running WPF. This ocurrs when running Unit Tests in .NET Core 3.1 on macos.
				catch (TypeLoadException)
				{
					return SynchronizationContext.Current == synchronizationContext;
				}
			}
		}

		static void BeginInvokeOnMainThread(Action action)
		{
			try
			{
				System.Windows.Application.Current?.Dispatcher.BeginInvoke(action);
			}

			// TypeLoadException is thrown when not running WPF. This ocurrs when running Unit Tests in .NET Core 3.1 on macos.
			catch (TypeLoadException)
			{
				if (synchronizationContext != null && SynchronizationContext.Current != synchronizationContext)
					synchronizationContext.Post(_ => action(), null);
				else
					action();
			}
		}
	}
}