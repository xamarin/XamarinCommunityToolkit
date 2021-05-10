using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryAsyncCommandTests : BaseCommandTests
	{
		[Test]
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

		[Test]
		public async Task AsyncCommand_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<AsyncCommand>(command);
			Assert.IsInstanceOf<IAsyncCommand>(command);
		}

		[Test]
		public async Task AsyncCommand_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<AsyncCommand>(command);
			Assert.IsInstanceOf<IAsyncCommand>(command);
		}

		[Test]
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

		[Test]
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
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand>(command);
			Assert.IsInstanceOf<IAsyncCommand>(command);
		}

		[Test]
		public async Task AsyncCommand_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, () => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand>(command);
			Assert.IsInstanceOf<IAsyncCommand>(command);
		}

		[Test]
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

		[Test]
		public async Task AsyncCommandT_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncCommand<int>>(command);
		}

		[Test]
		public async Task AsyncCommandT_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncCommand<int>>(command);
		}

		[Test]
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

		[Test]
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
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncCommand<int>>(command);
		}

		[Test]
		public async Task AsyncCommandT_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, () => true);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncCommand<int>>(command);
		}

		[Test]
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

		[Test]
		public async Task AsyncCommandTExecuteTCanExecute_NullCanExecuteParameter()
		{
			// Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			var command = CommandFactory.Create<int, bool>(IntParameterTask, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.IsTrue(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand<int, bool>>(command);
			Assert.IsInstanceOf<IAsyncCommand<int, bool>>(command);
		}

		[Test]
		public async Task AsyncCommandTExecuteTCanExecute_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.IsTrue(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncCommand<int, bool>>(command);
			Assert.IsInstanceOf<IAsyncCommand<int, bool>>(command);
		}
	}
}