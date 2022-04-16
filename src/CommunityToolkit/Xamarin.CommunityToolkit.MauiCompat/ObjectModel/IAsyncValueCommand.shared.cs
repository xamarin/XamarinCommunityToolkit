﻿// Inspired by AsyncAwaitBestPractices.MVVM.IAsyncValueCommand: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// An Async implementation of ICommand for ValueTask
	/// </summary>
	public interface IAsyncValueCommand<in TExecute, in TCanExecute> : IAsyncValueCommand<TExecute>
	{
		/// <summary>
		/// Determines whether the command can execute in its current state
		/// </summary>
		/// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		bool CanExecute(TCanExecute parameter);
	}

	/// <summary>
	/// An Async implementation of ICommand for ValueTask
	/// </summary>
	public interface IAsyncValueCommand<in T> : System.Windows.Input.ICommand
	{
		/// <summary>
		/// Returns true when the Command is currently executing. Returns false when the Command is not executing
		/// </summary>
		bool IsExecuting { get; }

		/// <summary>
		/// Returns true if the Command allows simultaneous executions
		/// </summary>
		bool AllowsMultipleExecutions { get; }

		/// <summary>
		/// Executes the Command as a ValueTask
		/// </summary>
		/// <returns>The ValueTask to execute</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		System.Threading.Tasks.ValueTask ExecuteAsync(T parameter);

		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		void RaiseCanExecuteChanged();
	}

	/// <summary>
	/// An Async implementation of ICommand for ValueTask
	/// </summary>
	public interface IAsyncValueCommand : System.Windows.Input.ICommand
	{
		/// <summary>
		/// Returns true when the Command is currently executing. Returns false when the Command is not executing
		/// </summary>
		bool IsExecuting { get; }

		/// <summary>
		/// Returns true if the Command allows simultaneous executions
		/// </summary>
		bool AllowsMultipleExecutions { get; }

		/// <summary>
		/// Executes the Command as a ValueTask
		/// </summary>
		/// <returns>The ValueTask to execute</returns>
		System.Threading.Tasks.ValueTask ExecuteAsync();

		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		void RaiseCanExecuteChanged();
	}
}