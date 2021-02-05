using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		public bool IsMainThread => throw new NotImplementedException();

		public void BeginInvokeOnMainThread(Action action)
		{
			GLib.Idle.Add(() =>
			{
				action();
				return false;
			});
		}
	}
}
