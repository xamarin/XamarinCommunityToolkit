using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class AsyncCommandTests : BaseAsyncCommandTests
	{
		[Fact]
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

		[Theory]
		[InlineData(500)]
		[InlineData(0)]
		public async Task AsyncCommand_ExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask);

			// Act
			await command.ExecuteAsync(parameter);

			// Assert
		}

		[Theory]
		[InlineData("Hello")]
		[InlineData(default)]
		public async Task AsyncCommand_ExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			var command = new AsyncCommand<string>(StringParameterTask);

			// Act
			await command.ExecuteAsync(parameter);

			// Assert
		}

		[Fact]
		public void AsyncCommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert

			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_NoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_Parameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteTrue());

			// Act

			// Assert

			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_Parameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_NoParameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_NoParameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			var command = new AsyncCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void AsyncCommand_NoParameter_NoCanExecute_Test()
		{
			// Arrange
			Func<bool>? canExecute = null;
#pragma warning disable CS8604 // Possible null reference argument.
			var command = new AsyncCommand(NoParameterTask, canExecute);
#pragma warning restore CS8604 // Possible null reference argument.

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncCommand_RaiseCanExecuteChanged_MainThreadCreation_MainThreadExecution_Test()
		{
			// Arrange
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
			Assert.True(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.RaiseCanExecuteChanged();
			await handleCanExecuteChangedTCS.Task;

			// Assert
			Assert.True(didCanExecuteChangeFire);
			Assert.True(command.CanExecute(null));

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				command.CanExecuteChanged -= handleCanExecuteChanged;
				didCanExecuteChangeFire = true;
				handleCanExecuteChangedTCS.SetResult(null);
			}
		}

		[Fact(Timeout = 2000)]
		public Task AsyncCommand_RaiseCanExecuteChanged_BackgroundThreadCreation_BackgroundThreadExecution_Test() => Task.Run(async () =>
		{
			// Arrange

			// Use ConfigureAwait(false) to move to a background thread
			await Task.Delay(100).ConfigureAwait(false);

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
			Assert.True(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.RaiseCanExecuteChanged();
			await handleCanExecuteChangedTCS.Task;

			// Assert
			Assert.True(didCanExecuteChangeFire);
			Assert.True(command.CanExecute(null));

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				command.CanExecuteChanged -= handleCanExecuteChanged;
				didCanExecuteChangeFire = true;
				handleCanExecuteChangedTCS.SetResult(null);
			}
		});

		[Fact(Timeout = 2000)]
		public async Task AsyncCommand_RaiseCanExecuteChanged_MainThreadCreation_BackgroundThreadExecution_Test()
		{
			// Arrange
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
			Assert.True(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			await Task.Run(async () =>
			{
				// use ConfigureAwait(false) to return to a background thread
				await Task.Delay(100).ConfigureAwait(false);

				command.RaiseCanExecuteChanged();
				await handleCanExecuteChangedTCS.Task;

				// Assert
				Assert.True(didCanExecuteChangeFire);
				Assert.True(command.CanExecute(null));
			});

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				command.CanExecuteChanged -= handleCanExecuteChanged;
				didCanExecuteChangeFire = true;
				handleCanExecuteChangedTCS.SetResult(null);
			}
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncCommand_RaiseCanExecuteChanged_BackgroundThreadCreation_MainThreadExecution_Test()
		{
			// Arrange
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
				Assert.True(command.CanExecute(null));
				Assert.False(didCanExecuteChangeFire);
			}).ConfigureAwait(true);

			// Act
			if (command is null)
				throw new NullReferenceException();

			command.RaiseCanExecuteChanged();
			await handleCanExecuteChangedTCS.Task;

			// Assert
			Assert.True(didCanExecuteChangeFire);
			Assert.True(command.CanExecute(null));

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				command.CanExecuteChanged -= handleCanExecuteChanged;
				didCanExecuteChangeFire = true;
				handleCanExecuteChangedTCS.SetResult(null);
			}
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncCommand_ChangeCanExecute_Test()
		{
			// Arrange
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
			Assert.True(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.ChangeCanExecute();
			await canExecuteChangedTCS.Task;

			// Assert
			Assert.True(didCanExecuteChangeFire);
			Assert.True(command.CanExecute(null));

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				command.CanExecuteChanged -= handleCanExecuteChanged;
				didCanExecuteChangeFire = true;
				canExecuteChangedTCS.SetResult(null);
			}
		}

		[Fact]
		public async Task AsyncCommand_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.True(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.True(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncCommand_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedGreaterThan1TCS = new TaskCompletionSource<int>();

			var canExecuteChangedCount = 0;

			var command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask;
			var canExecuteChangedGreaterThan1Result = await canExecuteChangedGreaterThan1TCS.Task;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(2, canExecuteChangedCount);
			Assert.Equal(canExecuteChangedCount, canExecuteChangedGreaterThan1Result);

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				if (++canExecuteChangedCount is 2)
					canExecuteChangedGreaterThan1TCS.SetResult(canExecuteChangedCount);
			}
		}
	}
}