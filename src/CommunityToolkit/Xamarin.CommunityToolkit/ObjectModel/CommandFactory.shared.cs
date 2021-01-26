using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Exceptions;
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

		public static IAsyncValueCommand Create(
			Func<ValueTask> execute,
			Func<bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncValueCommand<TExecute> Create<TExecute>(
			Func<TExecute, ValueTask> execute,
			Func<bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand<TExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		public static IAsyncValueCommand<TExecute, TCanExecute> Create<TExecute, TCanExecute>(
			Func<TExecute, ValueTask> execute,
			Func<TCanExecute, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand<TExecute, TCanExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		static Action<object> ConvertExecute<T>(Action<T> execute)
		{
			if (execute == null)
				return null;

			return p => Execute(execute, p);
		}

		static void Execute<T>(Action<T> execute, object parameter)
		{
			switch (parameter)
			{
				case T validParameter:
					execute(validParameter);
					break;

				case null when !typeof(T).GetTypeInfo().IsValueType:
					execute((T)parameter);
					break;

				case null:
					throw new InvalidCommandParameterException(typeof(T));

				default:
					throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
			}
		}

		static Func<object, bool> ConvertCanExecute(Func<bool> canExecute)
		{
			if (canExecute == null)
				return _ => true;

			return _ => canExecute();
		}

		static Func<object, bool> ConvertCanExecute<T>(Func<T, bool> canExecute)
		{
			canExecute ??= _ => true;
			return p => CanExecute(canExecute, p);
		}

		static bool CanExecute<T>(Func<T, bool> canExecute, object parameter) => parameter switch
		{
			T validParameter => canExecute(validParameter),
			null when !typeof(T).GetTypeInfo().IsValueType => canExecute((T)parameter),
			null => throw new InvalidCommandParameterException(typeof(T)),
			_ => throw new InvalidCommandParameterException(typeof(T), parameter.GetType()),
		};
	}
}
