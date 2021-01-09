using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	/// <summary>
	/// Abstract Base Class used by AsyncValueCommand
	/// </summary>
	public class BaseAsyncCommand<TExecute, TCanExecute> : BaseCommand<TCanExecute>, ICommand
	{
		readonly Func<TExecute, Task> execute;
		readonly Action<Exception> onException;
		readonly bool continueOnCapturedContext;

		/// <summary>
		/// Initializes a new instance of BaseAsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public BaseAsyncCommand(
			Func<TExecute, Task> execute,
			Func<TCanExecute, bool> canExecute,
			Action<Exception> onException,
			bool continueOnCapturedContext,
			bool allowsMultipleExecutions)
			: base(canExecute, allowsMultipleExecutions)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;
		}

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		private protected async Task ExecuteAsync(TExecute parameter)
		{
			ExecutionCount++;

			try
			{
				await execute(parameter).ConfigureAwait(continueOnCapturedContext);
			}
			catch (Exception e) when (onException != null)
			{
				onException(e);
			}
			finally
			{
				if (--ExecutionCount <= 0)
					ExecutionCount = 0;
			}
		}

		bool ICommand.CanExecute(object parameter) => parameter switch
		{
			TCanExecute validParameter => CanExecute(validParameter),
			null when !typeof(TCanExecute).GetTypeInfo().IsValueType => CanExecute((TCanExecute)parameter),
			null => throw new InvalidCommandParameterException(typeof(TCanExecute)),
			_ => throw new InvalidCommandParameterException(typeof(TCanExecute), parameter.GetType()),
		};

		void ICommand.Execute(object parameter)
		{
			switch (parameter)
			{
				case TExecute validParameter:
					Execute(validParameter);
					break;

				case null when !typeof(TExecute).GetTypeInfo().IsValueType:
					Execute((TExecute)parameter);
					break;

				case null:
					throw new InvalidCommandParameterException(typeof(TExecute));

				default:
					throw new InvalidCommandParameterException(typeof(TExecute), parameter.GetType());
			}

			// Use local method to defer async void from ICommand.Execute, allowing InvalidCommandParameterException to be thrown on the calling thread context before reaching an async method
			async void Execute(TExecute parameter) => await ExecuteAsync(parameter).ConfigureAwait(continueOnCapturedContext);
		}
	}
}