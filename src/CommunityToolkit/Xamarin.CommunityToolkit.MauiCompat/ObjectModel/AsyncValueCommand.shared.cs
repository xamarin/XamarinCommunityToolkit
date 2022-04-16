using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel.Internals;

// Inspired by AsyncAwaitBestPractices.MVVM.AsyncCommand<T>: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Xamarin.CommunityToolkit.ObjectModel
{
	public class AsyncValueCommand<TExecute, TCanExecute> : BaseAsyncValueCommand<TExecute, TCanExecute>, IAsyncValueCommand<TExecute, TCanExecute>
	{
		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<TExecute?, ValueTask> execute,
			Func<TCanExecute?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: base(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Executes the Command as a ValueTask
		/// </summary>
		/// <returns>The executed ValueTask</returns>
		public new ValueTask ExecuteAsync(TExecute parameter) => base.ExecuteAsync(parameter);
	}

	/// <summary>
	/// An implementation of IAsyncValueCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncValueCommand<T> : BaseAsyncValueCommand<T, object>, IAsyncValueCommand<T>
	{
		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<T?, ValueTask> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: base(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<T?, ValueTask> execute,
			Func<bool> canExecute,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: this(execute, ConvertCanExecute(canExecute), onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Executes the Command as a ValueTask
		/// </summary>
		/// <returns>The executed ValueTask</returns>
		public new ValueTask ExecuteAsync(T parameter) => base.ExecuteAsync(parameter);
	}

	/// <summary>
	/// An implementation of IAsyncValueCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncValueCommand : BaseAsyncValueCommand<object, object>, IAsyncValueCommand
	{
		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<ValueTask> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: base(ConvertExecute(execute), canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Initializes a new instance of AsyncValueCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncValueCommand(
			Func<ValueTask> execute,
			Func<bool> canExecute,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: this(execute, ConvertCanExecute(canExecute), onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Executes the Command as a ValueTask
		/// </summary>
		/// <returns>The executed ValueTask</returns>
		public ValueTask ExecuteAsync() => ExecuteAsync(null);
	}
}