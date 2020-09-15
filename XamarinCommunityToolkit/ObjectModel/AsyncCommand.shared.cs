using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;

// Inspired by AsyncAwaitBestPractices.MVVM.AsyncCommand<T>: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncCommand<T> : BaseCommand, IAsyncCommand<T>
	{
		readonly Func<T, Task> execute;
		readonly Action<Exception> onException;
		readonly bool continueOnCapturedContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Xamarin.CommunityToolkit.ObjectModel.AsyncCommand`1"/> class.
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<T, Task> execute,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
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
		public async Task ExecuteAsync(T parameter)
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

		void ICommand.Execute(object parameter)
		{
			switch (parameter)
			{
				case T validParameter:
					Execute(validParameter);
					break;

				case null when !typeof(T).GetTypeInfo().IsValueType:
					Execute((T)parameter);
					break;

				case null:
					throw new InvalidCommandParameterException(typeof(T));

				default:
					throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
			}

			// Use local method to defer async void from ICommand.Execute, allowing InvalidCommandParameterException to be thrown on the calling thread context before reaching an async method
			async void Execute(T parameter) => await ExecuteAsync(parameter).ConfigureAwait(continueOnCapturedContext);
		}
	}

	/// <summary>
	/// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncCommand : BaseCommand, IAsyncCommand
	{
		readonly Func<Task> execute;
		readonly Action<Exception> onException;
		readonly bool continueOnCapturedContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncCommand"/> class.
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<Task> execute,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
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
		public async Task ExecuteAsync()
		{
			ExecutionCount++;

			try
			{
				await execute().ConfigureAwait(continueOnCapturedContext);
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

		async void ICommand.Execute(object parameter) => await ExecuteAsync().ConfigureAwait(continueOnCapturedContext);
	}
}
