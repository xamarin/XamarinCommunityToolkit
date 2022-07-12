﻿using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel.Internals;

// Inspired by AsyncAwaitBestPractices.MVVM.AsyncCommand<T>: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncCommand<TExecute, TCanExecute> : BaseAsyncCommand<TExecute, TCanExecute>, IAsyncCommand<TExecute, TCanExecute>
	{
		/// <summary>
		/// Initializes a new instance of the AsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<TExecute?, Task> execute,
			Func<TCanExecute?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: base(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		public new Task ExecuteAsync(TExecute parameter) => base.ExecuteAsync(parameter);
	}

	/// <summary>
	/// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncCommand<T> : BaseAsyncCommand<T, object>, IAsyncCommand<T>
	{
		/// <summary>
		/// Initializes a new instance of AsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<T?, Task> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: base(execute, canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Initializes a new instance of AsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<T?, Task> execute,
			Func<bool> canExecute,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: this(execute, ConvertCanExecute(canExecute), onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		public new Task ExecuteAsync(T parameter) => base.ExecuteAsync(parameter);
	}

	/// <summary>
	/// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
	/// </summary>
	public class AsyncCommand : BaseAsyncCommand<object, object>, IAsyncCommand
	{
		/// <summary>
		/// Initializes a new instance of AsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<Task> execute,
			Func<object?, bool>? canExecute = null,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: base(ConvertExecute(execute), canExecute, onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Initializes a new instance of AsyncCommand
		/// </summary>
		/// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
		/// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
		/// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
		public AsyncCommand(
			Func<Task> execute,
			Func<bool> canExecute,
			Action<Exception>? onException = null,
			bool continueOnCapturedContext = false,
			bool allowsMultipleExecutions = true)
			: this(execute, ConvertCanExecute(canExecute), onException, continueOnCapturedContext, allowsMultipleExecutions)
		{
		}

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The executed Task</returns>
		public Task ExecuteAsync() => ExecuteAsync(null);
	}
}