using System;
using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// Factory for IAsyncCommand
	/// </summary>
	public partial class CommandFactory
	{
		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand Create(
			Func<Task> executeTask,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand(executeTask, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand Create(
			Func<Task> executeTask,
			Func<bool> canExecute,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand(executeTask, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncCommand<typeparamref name="TExecute"/></returns>
		public static IAsyncCommand<TExecute> Create<TExecute>(
			Func<TExecute, Task> executeTask,
			Func<object, bool> canExecute = null,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand<TExecute>(executeTask, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncCommand<typeparamref name="TExecute"/></returns>
		public static IAsyncCommand<TExecute> Create<TExecute>(
			Func<TExecute, Task> executeTask,
			Func<bool> canExecute,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand<TExecute>(executeTask, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand<TExecute, TCanExecute> Create<TExecute, TCanExecute>(
			Func<TExecute, Task> executeTask,
			Func<TCanExecute, bool> canExecute,
			Action<Exception> onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true) =>
			new AsyncCommand<TExecute, TCanExecute>(executeTask, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions);

		#region Helper Methods to Prevent Ambiguous Method Error When Using Anonymous Async Methods

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand Create(Func<Task> executeTask) =>
			Create(executeTask, (Func<object, bool>)null, null, false, true);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand Create(Func<Task> executeTask, Func<object, bool> canExecute) =>
			Create(executeTask, canExecute, null, false, true);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand Create(Func<Task> executeTask, Func<bool> canExecute) =>
			Create(executeTask, canExecute, null, false, true);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand<TExecute> Create<TExecute>(Func<TExecute, Task> executeTask) =>
			Create(executeTask, (Func<object, bool>)null, null, false, true);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand<TExecute> Create<TExecute>(Func<TExecute, Task> executeTask, Func<object, bool> canExecute) =>
			Create(executeTask, canExecute, null, false, true);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand<TExecute> Create<TExecute>(Func<TExecute, Task> execute, Func<bool> canExecute) =>
			Create(execute, canExecute, null, false, true);

		/// <summary>
		/// Initializes a new instance of IAsyncCommand
		/// </summary>
		/// <param name="executeTask">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <returns>IAsyncCommand</returns>
		public static IAsyncCommand<TExecute, TCanExecute> Create<TExecute, TCanExecute>(Func<TExecute, Task> executeTask, Func<TCanExecute, bool> canExecute) =>
			Create(executeTask, canExecute, null, false, true);
		#endregion
	}
}
