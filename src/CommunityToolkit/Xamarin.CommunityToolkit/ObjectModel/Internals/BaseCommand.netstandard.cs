using System;
using System.Threading;

// Inspired by Prism.Commands https://github.com/PrismLibrary/Prism/blob/0fc56abbea733a319c9734c65d1110db07f7ebd4/src/Prism.Core/Commands/DelegateCommandBase.cs#L37-L47
namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	public abstract partial class BaseCommand<TCanExecute>
	{
		static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

		static bool IsMainThread => SynchronizationContext.Current == synchronizationContext;

		static void BeginInvokeOnMainThread(Action action)
		{
			if (synchronizationContext != null && SynchronizationContext.Current != synchronizationContext)
				synchronizationContext.Post((o) => action(), null);
			else
				action();
		}
	}
}