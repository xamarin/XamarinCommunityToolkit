using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Helpers
{
	public static class CommandHelper
	{
		public static IAsyncCommand Create(
			Func<Task> action,
			Func<bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand(action, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncCommand<TExecute> Create<TExecute>(
			Func<TExecute, Task> action,
			Func<bool> canExecute,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			Create<TExecute>(action, ConvertCanExecute<TExecute>(canExecute), onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncCommand<TCanExecute> Create<TCanExecute>(
			Func<Task> action,
			Func<TCanExecute, bool> canExecute,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			Create<TCanExecute>(ConvertExecute<TCanExecute>(action), canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncCommand<T> Create<T>(
			Func<T, Task> action,
			Func<T, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			Create<T, T>(action, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncCommand<TExecute, TCanExecute> Create<TExecute, TCanExecute>(
			Func<TExecute, Task> action,
			Func<TCanExecute, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand<TExecute, TCanExecute>(action, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static Command Create(Action action) =>
			new Command(action);

		public static Command Create(Action action, Func<bool> canExecute) =>
			new Command(action, canExecute);

		public static Command<T> Create<T>(Action<T> action) =>
			new Command<T>(action);

		public static Command<T> Create<T>(Action<T> action, Func<T, bool> canExecute) =>
			new Command<T>(action, canExecute);

		static Func<T, Task> ConvertExecute<T>(Func<Task> execute)
		{
			if (execute == null)
				return null;

			return _ => execute();
		}

		static Func<T, bool> ConvertCanExecute<T>(Func<bool> canExecute)
		{
			if (canExecute == null)
				return null;

			return _ => canExecute();
		}
	}
}
