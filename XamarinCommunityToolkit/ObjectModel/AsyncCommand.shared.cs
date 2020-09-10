using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	public class AsyncCommand<T> : IAsyncCommand<T>
	{
		readonly Func<T, Task> execute;
		readonly Func<T, bool> canExecute;
		readonly Action<Exception> onException;
		readonly bool continueOnCapturedContext;
		readonly Lazy<bool> isNullableParameterType;
		readonly WeakEventManager weakEventManager = new WeakEventManager();

		int executionCount;
		bool isExecuting;
		bool allowMultipleExecution;

		bool IsExecuting
		{
			get => isExecuting;
			set
			{
				if (isExecuting == value)
					return;

				isExecuting = value;

				if (!AllowMultipleExecution)
					RaiseCanExecuteChanged();
			}
		}

		public bool AllowMultipleExecution
		{
			get => allowMultipleExecution;
			set
			{
				if (allowMultipleExecution == value)
					return;

				allowMultipleExecution = value;
				RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		/// Create a new AsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<T, Task> execute,
			Func<T, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false)
		{
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
			this.canExecute = canExecute;
			this.onException = onException;
			this.continueOnCapturedContext = continueOnCapturedContext;

			isNullableParameterType = new Lazy<bool>(() =>
			{
				var t = typeof(T);

				// The parameter is null. Is T Nullable?
				if (Nullable.GetUnderlyingType(t) != null)
					return true;

				// Not a Nullable, if it's a value type then null is not valid
				return !t.GetTypeInfo().IsValueType;
			});
		}

		/// <summary>
		/// Invoke the CanExecute method and return if it can be executed.
		/// </summary>
		/// <param name="parameter">Parameter to pass to CanExecute.</param>
		/// <returns>If it can be executed.</returns>
		public bool CanExecute(object parameter)
		{
			if (!IsValidParameter(parameter))
				return false;

			if (IsExecuting && !AllowMultipleExecution)
				return false;

			return canExecute?.Invoke((T) parameter) ?? true;
		}

		/// <summary>
		/// Event triggered when Can Execute changes.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => weakEventManager.AddEventHandler(value);
			remove => weakEventManager.RemoveEventHandler(value);
		}

		public async void Execute(object parameter) => await ExecuteAsync((T) parameter);

		/// <summary>
		/// Execute the command async.
		/// </summary>
		/// <returns>Task of action being executed that can be awaited.</returns>
		public async Task ExecuteAsync(T parameter)
		{
			try
			{
				IsExecuting = Interlocked.Increment(ref executionCount) > 0;
				await execute(parameter).ConfigureAwait(continueOnCapturedContext);
			}
			catch (Exception e) when (onException != null)
			{
				onException(e);
			}
			finally
			{
				IsExecuting = Interlocked.Decrement(ref executionCount) > 0;
			}
		}

		/// <summary>
		/// Raise a CanExecute change event.
		/// </summary>
		public void RaiseCanExecuteChanged() =>
			weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

		bool IsValidParameter(object o)
		{
			if (o != null)
				return o is T; // The parameter isn't null, so we don't have to worry whether null is a valid option

			return isNullableParameterType.Value;
		}
	}

	public class AsyncCommand : AsyncCommand<object>
	{
		public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null, Action<Exception> onException = null,
			bool continueOnCapturedContext = false) : base(o => execute(), o => canExecute?.Invoke() ?? true,
			onException, continueOnCapturedContext)
		{
			if (execute == null)
				throw new ArgumentNullException(nameof(execute));
		}
	}
}
