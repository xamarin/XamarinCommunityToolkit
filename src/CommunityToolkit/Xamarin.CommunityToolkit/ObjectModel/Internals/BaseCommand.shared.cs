using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.ObjectModel.Internals
{
	/// <summary>
	/// Abstract Base Class used by AsyncCommand and AsyncValueCommand
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract partial class BaseCommand<TCanExecute>
	{
		readonly Func<TCanExecute?, bool> canExecute;
		readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
		readonly DelegateWeakEventManager weakEventManager = new DelegateWeakEventManager();

		int executionCount;

		/// <summary>
		/// Initializes BaseCommand
		/// </summary>
		/// <param name="canExecute"></param>
		/// <param name="allowsMultipleExecutions"></param>
		protected private BaseCommand(Func<TCanExecute?, bool>? canExecute, bool allowsMultipleExecutions)
		{
			this.canExecute = canExecute ?? (_ => true);
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

		protected int ExecutionCount
		{
			get => executionCount;
			set
			{
				var previousExecutionCount = executionCount;

				executionCount = value;

				HandleExecutionCountChanged(previousExecutionCount, value).SafeFireAndForget();
			}
		}

		/// <summary>
		/// Determines whether the command can execute in its current state
		/// </summary>
		/// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		public bool CanExecute(TCanExecute? parameter) => (AllowsMultipleExecutions, IsExecuting) switch
		{
			(true, _) => canExecute(parameter),
			(false, true) => false,
			(false, false) => canExecute(parameter),
		};

		/// <summary>
		/// Raises the `ICommand.CanExecuteChanged` event.
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			// Automatically marshall to the Main Thread to adhere to the way that Xamarin.Forms automatically marshalls binding events to Main Thread
			if (IsMainThread)
				weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
			else
				BeginInvokeOnMainThread(() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(CanExecuteChanged)));
		}

		/// <summary>
		/// Raises the `ICommand.CanExecuteChanged` event. Recommend using RaiseCanExecuteChanged() instead.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void ChangeCanExecute() => RaiseCanExecuteChanged();

		async Task HandleExecutionCountChanged(int updatedExecutionCount, int previousExecutionCount)
		{
			await semaphoreSlim.WaitAsync().ConfigureAwait(true);

			try
			{
				var shouldRaiseCanExecuteChanged = (AllowsMultipleExecutions, updatedExecutionCount, previousExecutionCount) switch
				{
					(true, _, _) => false,
					(false, 0, > 0) => true,
					(false, > 0, 0) => true,
					(false, _, _) => false
				};

				if (shouldRaiseCanExecuteChanged)
					RaiseCanExecuteChanged();
			}
			finally
			{
				semaphoreSlim.Release();
			}
		}
	}
}