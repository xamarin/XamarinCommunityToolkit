using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel
{
	public class AsyncCommand_Tests
	{
		[Fact]
		public void CanExecute_NotNullable_Parameter()
		{
			// Arrange
			var asyncCommand = new AsyncCommand<int>(i => Task.FromResult(true), i => true);

			// Assert
			Assert.True(asyncCommand.CanExecute(123));
			Assert.False(asyncCommand.CanExecute(123M));
			Assert.False(asyncCommand.CanExecute("a"));
			Assert.False(asyncCommand.CanExecute(null));
			Assert.False(asyncCommand.CanExecute(new object()));
			Assert.False(asyncCommand.CanExecute(new List<int>()));
		}

		[Fact]
		public void CanExecute_Nullable_Parameter()
		{
			// Arrange
			var asyncCommand = new AsyncCommand<bool?>(i => Task.FromResult(true), i => true);

			// Assert
			Assert.True(asyncCommand.CanExecute(true));
			Assert.True(asyncCommand.CanExecute(null));
			Assert.False(asyncCommand.CanExecute(1));
			Assert.False(asyncCommand.CanExecute(new object()));
			Assert.False(asyncCommand.CanExecute(new List<int>()));
		}

		[Fact]
		public async Task Execute_MultipleExecution_Disabled()
		{
			// Arrange
			var canExecuteChangedCount = 0;
			var executeTcs = new TaskCompletionSource<bool>();
			var asyncCommand = new AsyncCommand<bool?>(i => executeTcs.Task, i => true)
			{
				AllowMultipleExecution = false
			};
			asyncCommand.CanExecuteChanged += (sender, args) =>
			{
				canExecuteChangedCount++;
			};

			// Act
			var executeTask = asyncCommand.ExecuteAsync(true);

			// Assert
			Assert.False(asyncCommand.CanExecute(true));
			Assert.Equal(1, canExecuteChangedCount);
			executeTcs.SetResult(true);
			await executeTask;
			Assert.True(asyncCommand.CanExecute(true));
			Assert.Equal(2, canExecuteChangedCount);
		}

		[Fact]
		public async Task Execute_MultipleExecution_Allowed()
		{
			// Arrange
			var executionCount = 0;
			var canExecuteChangedCount = 0;
			var executeTcs = new TaskCompletionSource<bool>();
			var asyncCommand = new AsyncCommand<bool?>(i =>
			{
				executionCount++;
				return executeTcs.Task;
			}, i => true)
			{
				AllowMultipleExecution = true
			};
			asyncCommand.CanExecuteChanged += (sender, args) =>
			{
				canExecuteChangedCount++;
			};

			// Act
			var executeTask1 = asyncCommand.ExecuteAsync(true);
			var executeTask2 = asyncCommand.ExecuteAsync(true);
			asyncCommand.Execute(true);

			// Assert
			Assert.True(asyncCommand.CanExecute(true));
			executeTcs.SetResult(true);
			await Task.WhenAll(executeTask1, executeTask2);
			Assert.True(asyncCommand.CanExecute(true));
			Assert.Equal(3, executionCount);
			Assert.Equal(0, canExecuteChangedCount);
		}

		[Fact]
		public async Task Execute_OnException()
		{
			// Arrange
			Exception onException = null;
			var executeTcs = new TaskCompletionSource<bool>();
			var asyncCommand = new AsyncCommand<bool?>(i => executeTcs.Task,
				onException: exception =>
				{
					onException = exception;
				});

			// Act
			var executeTask = asyncCommand.ExecuteAsync(null);
			var exceptionResult = new Exception();
			executeTcs.SetException(exceptionResult);

			// Assert
			await executeTask;
			Assert.Equal(exceptionResult, onException);
			Assert.True(asyncCommand.CanExecute(true));
		}

		[Fact]
		public async Task Execute_NoParameter()
		{
			// Arrange
			var executionCount = 0;
			var executeTcs = new TaskCompletionSource<bool>();
			var asyncCommand = new AsyncCommand(() =>
			{
				executionCount++;
				return executeTcs.Task;
			});

			// Act
			asyncCommand.Execute(null);
			asyncCommand.Execute(null);
			executeTcs.SetResult(true);

			// Assert
			await executeTcs.Task;
			Assert.True(asyncCommand.CanExecute(true));
			Assert.Equal(2, executionCount);
		}
	}
}