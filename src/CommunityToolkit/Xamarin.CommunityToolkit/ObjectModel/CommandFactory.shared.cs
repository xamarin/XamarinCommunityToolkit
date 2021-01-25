using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	public static class CommandFactory
	{
		public static Command Create(Action execute, Func<bool> canExecute = null) =>
			new Command(execute, canExecute ?? (() => true));

		public static Command Create<TExecute>(Action<TExecute> execute, Func<bool> canExecute = null) =>
			new Command(ConvertExecute(execute), ConvertCanExecute(canExecute));

		public static Command Create<TExecute, TCanExecute>(Action<TExecute> execute, Func<TCanExecute, bool> canExecute) =>
			new Command(ConvertExecute(execute), ConvertCanExecute(canExecute));

		public static IAsyncCommand Create(
			Func<Task> execute,
			Func<bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncCommand<TExecute> Create<TExecute>(
			Func<TExecute, Task> execute,
			Func<bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand<TExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncCommand<TExecute, TCanExecute> Create<TExecute, TCanExecute>(
			Func<TExecute, Task> execute,
			Func<TCanExecute, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand<TExecute, TCanExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		static Action<object> ConvertExecute<T>(Action<T> execute)
		{
			if (execute == null)
				return null;

			return e => execute((T)e);
		}

		static Func<object, bool> ConvertCanExecute(Func<bool> canExecute)
		{
			if (canExecute == null)
				return _ => true;

			return _ => canExecute();
		}

		static Func<object, bool> ConvertCanExecute<T>(Func<T, bool> canExecute)
		{
			if (canExecute == null)
				return _ => true;

			return e => canExecute((T)e);
		}
	}
}
