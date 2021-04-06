using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public class AsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Test]
		public void AsyncValueCommandNullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand(null));
#pragma warning restore CS8625
		}

		[Test]
		public void AsyncValueCommandT_NullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand<object>(null));
#pragma warning restore CS8625
		}

		[TestCase(500)]
		[TestCase(0)]
		public async Task AsyncValueCommandExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask);
			var command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(parameter).ConfigureAwait(false);
			await command2.ExecuteAsync(parameter).ConfigureAwait(false);

			// Assert
		}

		[TestCase("Hello")]
		[TestCase(default)]
		public async Task AsyncValueCommandExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			var command = new AsyncValueCommand<string>(StringParameterTask);
			var command2 = new AsyncValueCommand<string, bool>(StringParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(parameter).ConfigureAwait(false);
			await command2.ExecuteAsync(parameter).ConfigureAwait(false);

			// Assert
		}

		[Test]
		public void AsyncValueCommandParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, parameter => CanExecuteTrue(parameter));
			var command2 = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert

			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command2.CanExecute(true));
		}

		[Test]
		public void AsyncValueCommandParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, parameter => CanExecuteFalse(parameter));
			var command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute("Hello World"));
		}

		[Test]
		public void AsyncValueCommandParameter_CanExecuteTrue_NoParameterTest()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, () => CanExecuteTrue());
			var command2 = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert

			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command2.CanExecute(true));
		}

		[Test]
		public void AsyncValueCommandParameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, () => CanExecuteFalse());
			var command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute("Hello World"));
		}

		[Test]
		public void AsyncValueCommandNoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void AsyncValueCommandNoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncValueCommandNoParameter_CanExecuteTrueNoParameter_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void AsyncValueCommandNoParameter_CanExecuteFalseNoParameter_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncValueCommandNoParameter_NoCanExecute_Test()
		{
			// Arrange
			Func<bool>? canExecute = null;
#pragma warning disable CS8604 // Possible null reference argument.
			var command = new AsyncValueCommand(NoParameterTask, canExecute);
#pragma warning restore CS8604 // Possible null reference argument.

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncValueCommand_RaiseCanExecuteChanged_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var handleCanExecuteChangedTCS = new TaskCompletionSource<object?>();

			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncValueCommand(NoParameterTask, commandCanExecute);
			command.CanExecuteChanged += handleCanExecuteChanged;

			bool commandCanExecute(object? parameter) => canCommandExecute;

			Assert.False(command.CanExecute(null));

			// Act
			canCommandExecute = true;

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.RaiseCanExecuteChanged();
			await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(didCanExecuteChangeFire);
			Assert.IsTrue(command.CanExecute(null));

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					command.CanExecuteChanged -= handleCanExecuteChanged;
					didCanExecuteChangeFire = true;
					handleCanExecuteChangedTCS.SetResult(null);
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncValueCommand_ChangeCanExecute_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var handleCanExecuteChangedTCS = new TaskCompletionSource<object?>();

			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncValueCommand(NoParameterTask, commandCanExecute);
			command.CanExecuteChanged += handleCanExecuteChanged;

			bool commandCanExecute(object? parameter) => canCommandExecute;

			Assert.False(command.CanExecute(null));

			// Act
			canCommandExecute = true;

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.ChangeCanExecute();
			await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(didCanExecuteChangeFire);
			Assert.IsTrue(command.CanExecute(null));

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					command.CanExecuteChanged -= handleCanExecuteChanged;
					didCanExecuteChangeFire = true;
					handleCanExecuteChangedTCS.SetResult(null);
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncValueCommand_Parameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			Assert.IsTrue(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.IsTrue(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncValueCommand_Parameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			var command = new AsyncValueCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					if (++canExecuteChangedCount is 2)
					{
						command.CanExecuteChanged -= handleCanExecuteChanged;
						handleCanExecuteChangedTCS.SetResult(canExecuteChangedCount);
					}
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);
			var handleCanExecuteChangedResult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
			Assert.AreEqual(canExecuteChangedCount, handleCanExecuteChangedResult);
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncValueCommand_NoParameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand(() => IntParameterTask(Delay));
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			Assert.IsTrue(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.IsTrue(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncValueCommand_NoParameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			var command = new AsyncValueCommand(() => IntParameterTask(Delay), allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					if (++canExecuteChangedCount is 2)
					{
						command.CanExecuteChanged -= handleCanExecuteChanged;
						handleCanExecuteChangedTCS.SetResult(canExecuteChangedCount);
					}
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);
			var handleCanExecuteChangedResult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
			Assert.AreEqual(canExecuteChangedCount, handleCanExecuteChangedResult);
		}
	}
}