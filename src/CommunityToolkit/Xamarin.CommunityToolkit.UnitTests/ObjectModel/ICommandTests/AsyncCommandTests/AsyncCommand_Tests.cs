using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class AsyncCommandTests : BaseAsyncCommandTests
	{
		[Test]
		public void AsyncCommand_NullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncCommand(null));
			Assert.Throws<ArgumentNullException>(() => new AsyncCommand<string>(null));
			Assert.Throws<ArgumentNullException>(() => new AsyncCommand<string, string>(null));
#pragma warning restore CS8625
		}

		[TestCase(500)]
		[TestCase(0)]
		public async Task AsyncCommand_ExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask);

			// Act
			await command.ExecuteAsync(parameter).ConfigureAwait(false);

			// Assert
		}

		[TestCase("Hello")]
		[TestCase(default)]
		public async Task AsyncCommand_ExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			var command = new AsyncCommand<string>(StringParameterTask);

			// Act
			await command.ExecuteAsync(parameter).ConfigureAwait(false);

			// Assert
		}

		[Test]
		public void AsyncCommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert

			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_Parameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteTrue());

			// Act

			// Assert

			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_Parameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void AsyncCommand_NoParameter_NoCanExecute_Test()
		{
			// Arrange
			Func<bool>? canExecute = null;
#pragma warning disable CS8604 // Possible null reference argument.
			var command = new AsyncCommand(NoParameterTask, canExecute);
#pragma warning restore CS8604 // Possible null reference argument.

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncCommand_RaiseCanExecuteChanged_MainThreadCreation_MainThreadExecution_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var handleCanExecuteChangedTCS = new TaskCompletionSource<object?>();

			var command = new AsyncCommand(NoParameterTask, commandCanExecute);
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
		public Task AsyncCommand_RaiseCanExecuteChanged_BackgroundThreadCreation_BackgroundThreadExecution_Test() => Task.Run(async () =>
		{
			// Arrange

			// Use ConfigureAwait(false) to move to a background thread
			await Task.Delay(100).ConfigureAwait(false);

			var semaphoreSlim = new SemaphoreSlim(1, 1);

			var handleCanExecuteChangedTCS = new TaskCompletionSource<object?>();

			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncCommand(NoParameterTask, commandCanExecute);
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
			await handleCanExecuteChangedTCS.Task;

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
		});

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncCommand_RaiseCanExecuteChanged_MainThreadCreation_BackgroundThreadExecution_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var handleCanExecuteChangedTCS = new TaskCompletionSource<object?>();

			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncCommand(NoParameterTask, commandCanExecute);
			command.CanExecuteChanged += handleCanExecuteChanged;

			bool commandCanExecute(object? parameter) => canCommandExecute;

			Assert.False(command.CanExecute(null));

			// Act
			canCommandExecute = true;

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			await Task.Run(async () =>
			{
				// use ConfigureAwait(false) to return to a background thread
				await Task.Delay(100).ConfigureAwait(false);

				command.RaiseCanExecuteChanged();
				await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

				// Assert
				Assert.IsTrue(didCanExecuteChangeFire);
				Assert.IsTrue(command.CanExecute(null));
			}).ConfigureAwait(false);

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
		public async Task AsyncCommand_RaiseCanExecuteChanged_BackgroundThreadCreation_MainThreadExecution_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			AsyncCommand? command = null;
			var didCanExecuteChangeFire = false;
			var canCommandExecute = false;

			var handleCanExecuteChangedTCS = new TaskCompletionSource<object?>();

			await Task.Run(async () =>
			{
				await Task.Delay(100).ConfigureAwait(false);

				command = new AsyncCommand(NoParameterTask, commandCanExecute);
				command.CanExecuteChanged += handleCanExecuteChanged;

				bool commandCanExecute(object? parameter) => canCommandExecute;

				Assert.False(command.CanExecute(null));

				// Act
				canCommandExecute = true;

				// Assert
				Assert.IsTrue(command.CanExecute(null));
				Assert.False(didCanExecuteChangeFire);
			}).ConfigureAwait(true);

			// Act
			if (command is null)
				throw new NullReferenceException();

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
		public async Task AsyncCommand_ChangeCanExecute_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedTCS = new TaskCompletionSource<object?>();

			var canCommandExecute = false;
			var didCanExecuteChangeFire = false;

			var command = new AsyncCommand(NoParameterTask, commandCanExecute);
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
			await canExecuteChangedTCS.Task.ConfigureAwait(false);

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
					canExecuteChangedTCS.SetResult(null);
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncCommand_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

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

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task AsyncCommand_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedGreaterThan1TCS = new TaskCompletionSource<int>();

			var canExecuteChangedCount = 0;

			var command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);
			var canExecuteChangedGreaterThan1Result = await canExecuteChangedGreaterThan1TCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
			Assert.AreEqual(canExecuteChangedCount, canExecuteChangedGreaterThan1Result);

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					if (++canExecuteChangedCount is 2)
					{
						command.CanExecuteChanged -= handleCanExecuteChanged;
						canExecuteChangedGreaterThan1TCS.SetResult(canExecuteChangedCount);
					}
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}
		}
	}
}