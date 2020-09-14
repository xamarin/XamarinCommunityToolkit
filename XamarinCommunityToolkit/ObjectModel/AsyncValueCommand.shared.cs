using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.Helpers;

// Inspired by AsyncAwaitBestPractices.MVVM.AsyncCommand<T>: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// An implementation of IAsyncValueCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncValueCommand<T> : IAsyncValueCommand<T>
	{
		readonly Func<T, ValueTask> execute;
		readonly Func<object, bool> canExecute;
		readonly Action<Exception> onException;
		readonly bool continueOnCapturedContext;
		readonly WeakEventManager weakEventManager = new WeakEventManager();

		int executionCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.AsyncCommand`1"/> class.
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<T, ValueTask> execute,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
			this.canExecute = canExecute ?? (_ => true);
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;

			AllowsMultipleExecutions = allowsMultipleExecutions;
		}

		/// <summary>
		/// Returns true when the Command is currently executing. Returns false when the Command is not executing
		/// </summary>
		public bool IsExecuting => ExecutionCount > 0;

		/// <summary>
		/// Returns true if the Command allows simultaneous executions
		/// </summary>
		public bool AllowsMultipleExecutions { get; }

		int ExecutionCount
		{
			get => executionCount;
			set
			{
				if (executionCount != value)
				{
					executionCount = value;
					RaiseCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => weakEventManager.AddEventHandler(value);
			remove => weakEventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Determines whether the command can execute in its current state
		/// </summary>
		/// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter) => (AllowsMultipleExecutions, IsExecuting) switch
		{
			(true, _) => canExecute(parameter),
			(false, true) => false,
			(false, false) => canExecute(parameter),
		};

		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		public void RaiseCanExecuteChanged() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public async ValueTask ExecuteAsync(T parameter)
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
	/// An implementation of IAsyncValueCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncValueCommand : IAsyncValueCommand
	{
		readonly Func<ValueTask> execute;
		readonly Func<object, bool> canExecute;
		readonly Action<Exception> onException;
		readonly bool continueOnCapturedContext;
		readonly WeakEventManager weakEventManager = new WeakEventManager();

		int executionCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.AsyncCommand`1"/> class.
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<ValueTask> execute,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
			this.canExecute = canExecute ?? (_ => true);
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;

			AllowsMultipleExecutions = allowsMultipleExecutions;
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => weakEventManager.AddEventHandler(value);
			remove => weakEventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Returns true when the Command is currently executing. Returns false when the Command is not executing
		/// </summary>
		public bool IsExecuting => ExecutionCount > 0;

		/// <summary>
		/// Returns true if the Command allows simultaneous executions
		/// </summary>
		public bool AllowsMultipleExecutions { get; }

		int ExecutionCount
		{
			get => executionCount;
			set
			{
				if (executionCount != value)
				{
					executionCount = value;
					RaiseCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// Determines whether the command can execute in its current state
		/// </summary>
		/// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(object parameter) => (AllowsMultipleExecutions, IsExecuting) switch
		{
			(true, _) => canExecute(parameter),
			(false, true) => false,
			(false, false) => canExecute(parameter),
		};

		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		public void RaiseCanExecuteChanged() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		public async ValueTask ExecuteAsync()
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

		async void ICommand.Execute(object parameter) => await execute().ConfigureAwait(continueOnCapturedContext);
	}
}
