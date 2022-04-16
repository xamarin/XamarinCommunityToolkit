using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// Factory for IAsyncValueCommand
	/// </summary>
	public static partial class CommandFactory
	{
		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncValueCommand</returns>
		public static IAsyncValueCommand Create(
			Func<ValueTask> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncValueCommand</returns>
		public static IAsyncValueCommand Create(
			Func<ValueTask> execute,
			Func<bool> canExecute,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncValueCommand<typeparamref name="TExecute"/></returns>
		public static IAsyncValueCommand<TExecute> Create<TExecute>(
			Func<TExecute?, ValueTask> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand<TExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncValueCommand<typeparamref name="TExecute"/></returns>
		public static IAsyncValueCommand<TExecute> Create<TExecute>(
			Func<TExecute?, ValueTask> execute,
			Func<bool> canExecute,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand<TExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncValueCommand></returns>
		public static IAsyncValueCommand<TExecute, TCanExecute> Create<TExecute, TCanExecute>(
			Func<TExecute?, ValueTask> execute,
			Func<TCanExecute?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncValueCommand<TExecute, TCanExecute>(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);
	}
}