using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryAsyncCommandTests : BaseCommandTests
	{
		[Fact]
		public void AsyncCommand_NullExecuteParameter()
		{
			// Arrange
			Func<Task>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Fact]
		public async Task AsyncCommand_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand>(command);
		}

		[Fact]
		public async Task AsyncCommand_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand>(command);
		}

		[Fact]
		public void AsyncCommand_FuncBool_NullExecuteParameter()
		{
			// Arrange
			Func<Task>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute, () => true));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Fact]
		public async Task AsyncCommand_FuncBool_NullCanExecuteParameter()
		{
			// Arrange
			Func<bool>? canExecute = null;
#pragma warning disable CS8604 // Possible null reference argument.
			var command = CommandFactory.Create(NoParameterTask, canExecute);
#pragma warning restore CS8604 // Possible null reference argument.

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand>(command);
		}

		[Fact]
		public async Task AsyncCommand_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, () => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand>(command);
		}

		[Fact]
		public void AsyncCommandT_NullExecuteParameter()
		{
			// Arrange
			Func<int, Task>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Fact]
		public async Task AsyncCommandT_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand<int>>(command);
		}

		[Fact]
		public async Task AsyncCommandT_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand<int>>(command);
		}

		[Fact]
		public void AsyncCommandT_FuncBool_NullExecuteParameter()
		{
			// Arrange
			Func<int, Task>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(execute, () => true));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Fact]
		public async Task AsyncCommandT_FuncBool_NullCanExecuteParameter()
		{
			// Arrange
			Func<bool>? canExecute = null;
#pragma warning disable CS8604 // Possible null reference argument.
			var command = CommandFactory.Create<int>(IntParameterTask, canExecute);
#pragma warning restore CS8604 // Possible null reference argument.

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand<int>>(command);
		}

		[Fact]
		public async Task AsyncCommandT_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, () => true);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<AsyncCommand<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand<int>>(command);
		}

		[Fact]
		public void AsyncCommandTExecuteTCanExecute_NullExecuteParameter()
		{
			// Arrange
			Func<int, Task>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Fact]
		public async Task AsyncCommandTExecuteTCanExecute_NullCanExecuteParameter()
		{
			// Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			var command = CommandFactory.Create<int, bool>(IntParameterTask, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsType<AsyncCommand<int, bool>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand<int, bool>>(command);
		}

		[Fact]
		public async Task AsyncCommandTExecuteTCanExecute_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.True(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsType<AsyncCommand<int, bool>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
			Assert.IsAssignableFrom<IAsyncCommand<int, bool>>(command);
		}
	}
}