using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryAsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Fact]
		public void AsyncValueCommand_NullExecuteParameter()
		{
			// Arrange
			Func<Task>? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public async Task AsyncValueCommand_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand>(command);
		}

		[Fact]
		public async Task AsyncValueCommand_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand>(command);
		}

		[Fact]
		public void AsyncValueCommand_FuncBool_NullExecuteParameter()
		{
			// Arrange
			Func<ValueTask>? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute, () => true));
		}

		[Fact]
		public async Task AsyncValueCommand_FuncBool_NullCanExecuteParameter()
		{
			// Arrange
			Func<bool>? canExecute = null;
			var command = CommandFactory.Create(NoParameterTask, canExecute);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand>(command);
		}

		[Fact]
		public async Task AsyncValueCommand_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, () => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand>(command);
		}

		[Fact]
		public void AsyncValueCommandT_NullExecuteParameter()
		{
			// Arrange
			Func<int, ValueTask>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Fact]
		public async Task AsyncValueCommandT_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand<int>>(command);
		}

		[Fact]
		public async Task AsyncValueCommandT_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand<int>>(command);
		}

		[Fact]
		public void AsyncValueCommandT_FuncBool_NullExecuteParameter()
		{
			// Arrange
			Func<int, ValueTask>? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(execute, () => true));
		}

		[Fact]
		public async Task AsyncValueCommandT_FuncBool_NullCanExecuteParameter()
		{
			// Arrange
			Func<bool>? canExecute = null;
			var command = CommandFactory.Create<int>(IntParameterTask, canExecute);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand<int>>(command);
		}

		[Fact]
		public async Task AsyncValueCommandT_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, () => true);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncValueCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand<int>>(command);
		}

		[Fact]
		public void AsyncValueCommandTExecuteTCanExecute_NullExecuteParameter()
		{
			// Arrange
			Func<int, ValueTask>? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public async Task AsyncValueCommandTExecuteTCanExecute_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, null);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsType<AsyncValueCommand<int, bool>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand<int, bool>>(command);
		}

		[Fact]
		public async Task AsyncValueCommandTExecuteTCanExecute_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsType<AsyncValueCommand<int, bool>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncValueCommand<int, bool>>(command);
		}
	}
}