using System;using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

// Inspired by Xamarin.Essentials.MainThread  https://github.com/xamarin/Essentials/tree/main/Xamarin.Essentials/MainThread
namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static bool IsMainThread
		{
			get
			{
				// if there is no main window, then this is either a service
				// or the UI is not yet constructed, so the main thread is the
				// current thread
				try
				{
					if (CoreApplication.MainView?.CoreWindow == null)
						return true;
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Unable to validate MainView creation. {ex.Message}");
					return true;
				}

				return CoreApplication.MainView.CoreWindow.Dispatcher?.HasThreadAccess ?? false;
			}
		}

		static async void BeginInvokeOnMainThread(Action action)
		{
			var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;

			if (dispatcher == null)
				throw new InvalidOperationException("Unable to find main thread.");

			await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
		}
	}
}