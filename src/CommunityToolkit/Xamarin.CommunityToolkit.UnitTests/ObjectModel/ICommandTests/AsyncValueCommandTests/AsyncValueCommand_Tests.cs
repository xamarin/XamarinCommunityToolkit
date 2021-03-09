using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public class AsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Fact]
		public void AsyncValueCommandNullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand(null));
#pragma warning restore CS8625
		}

		[Fact]
		public void AsyncValueCommandT_NullExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand<object>(null));
#pragma warning restore CS8625
		}

		[Theory]
		[InlineData(500)]
		[InlineData(0)]
		public async Task AsyncValueCommandExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask);
			var command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Theory]
		[InlineData("Hello")]
		[InlineData(default)]
		public async Task AsyncValueCommandExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			var command = new AsyncValueCommand<string>(StringParameterTask);
			var command2 = new AsyncValueCommand<string, bool>(StringParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Fact]
		public void AsyncValueCommandParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, parameter => CanExecuteTrue(parameter));
			var command2 = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert

			Assert.True(command.CanExecute(null));
			Assert.True(command2.CanExecute(true));
		}

		[Fact]
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

		[Fact]
		public void AsyncValueCommandParameter_CanExecuteTrue_NoParameterTest()
		{
			// Arrange
			var command = new AsyncValueCommand<int>(IntParameterTask, () => CanExecuteTrue());
			var command2 = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert

			Assert.True(command.CanExecute(null));
			Assert.True(command2.CanExecute(true));
		}

		[Fact]
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

		[Fact]
		public void AsyncValueCommandNoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void AsyncValueCommandNoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void AsyncValueCommandNoParameter_CanExecuteTrueNoParameter_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void AsyncValueCommandNoParameter_CanExecuteFalseNoParameter_Test()
		{
			// Arrange
			var command = new AsyncValueCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void AsyncValueCommandNoParameter_NoCanExecute_Test()
		{
			// Arrange
			Func<bool>? canExecute = null;
#pragma warning disable CS8604 // Possible null reference argument.
			var command = new AsyncValueCommand(NoParameterTask, canExecute);
#pragma warning restore CS8604 // Possible null reference argument.

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncValueCommand_RaiseCanExecuteChanged_Test()
		{
			// Arrange
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
		public async Task AsyncValueCommand_ChangeCanExecute_Test()
		{
			// Arrange
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
			Assert.True(command.CanExecute(null));
			Assert.False(didCanExecuteChangeFire);

			// Act
			command.ChangeCanExecute();
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

		[Fact]
		public async Task AsyncValueCommand_Parameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

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
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncValueCommand_Parameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			var command = new AsyncValueCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				command.CanExecuteChanged -= handleCanExecuteChanged;
				if (++canExecuteChangedCount is 2)
					handleCanExecuteChangedTCS.SetResult(canExecuteChangedCount);
			}

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask;
			await handleCanExecuteChangedTCS.Task;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(2, canExecuteChangedCount);
		}

		[Fact]
		public async Task AsyncValueCommand_NoParameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			var command = new AsyncValueCommand(() => IntParameterTask(Delay));
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			Assert.True(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync();

			// Assert
			Assert.True(command.IsExecuting);
			Assert.True(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Fact(Timeout = 2000)]
		public async Task AsyncValueCommand_NoParameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			var command = new AsyncValueCommand(() => IntParameterTask(Delay), allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				if (++canExecuteChangedCount is 2)
				{
					command.CanExecuteChanged -= handleCanExecuteChanged;
					handleCanExecuteChangedTCS.SetResult(canExecuteChangedCount);
				}
			}

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync();

			// Assert
			Assert.True(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask;
			var handleCanExecuteChangedResult = await handleCanExecuteChangedTCS.Task;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(2, canExecuteChangedCount);
			Assert.Equal(canExecuteChangedCount, handleCanExecuteChangedResult);
		}
	}
}