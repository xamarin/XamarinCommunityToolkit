using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryAsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Test]
		public void AsyncValueCommand_NullExecuteParameter()
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
		public async Task AsyncValueCommand_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<AsyncValueCommand>(command);
			Assert.IsInstanceOf<IAsyncValueCommand>(command);
		}

		[Test]
		public async Task AsyncValueCommand_ObjectCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, parameter => true);

			// Act
			await command.ExecuteAsync();

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<AsyncValueCommand>(command);
			Assert.IsInstanceOf<IAsyncValueCommand>(command);
		}

		[Test]
		public void AsyncValueCommand_FuncBool_NullExecuteParameter()
		{
			// Arrange
			Func<ValueTask>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute, () => true));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Test]
		public async Task AsyncValueCommand_FuncBool_NullCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand>(command);
			Assert.IsInstanceOf<IAsyncValueCommand>(command);
		}

		[Test]
		public async Task AsyncValueCommand_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand>(command);
			Assert.IsInstanceOf<IAsyncValueCommand>(command);
		}

		[Test]
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

		[Test]
		public async Task AsyncValueCommandT_NullCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncValueCommand<int>>(command);
		}

		[Test]
		public async Task AsyncValueCommandT_ObjectCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncValueCommand<int>>(command);
		}

		[Test]
		public void AsyncValueCommandT_FuncBool_NullExecuteParameter()
		{
			// Arrange
			Func<int, ValueTask>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(execute, () => true));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Test]
		public async Task AsyncValueCommandT_FuncBool_NullCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncValueCommand<int>>(command);
		}

		[Test]
		public async Task AsyncValueCommandT_FuncBool_ValidExecuteParameter_ValidCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand<int>>(command);
			Assert.IsInstanceOf<IAsyncValueCommand<int>>(command);
		}

		[Test]
		public void AsyncValueCommandTExecuteTCanExecute_NullExecuteParameter()
		{
			// Arrange
			Func<int, ValueTask>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Test]
		public async Task AsyncValueCommandTExecuteTCanExecute_NullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, null);

			// Act
			await command.ExecuteAsync(0);

			// Assert
			Assert.IsTrue(command.CanExecute(true));

			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));

			Assert.IsInstanceOf<ICommand>(command);
			Assert.IsInstanceOf<AsyncValueCommand<int, bool>>(command);
			Assert.IsInstanceOf<IAsyncValueCommand<int, bool>>(command);
		}

		[Test]
		public async Task AsyncValueCommandTExecuteTCanExecute_ObjectCanExecuteParameter()
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
			Assert.IsInstanceOf<AsyncValueCommand<int, bool>>(command);
			Assert.IsInstanceOf<IAsyncValueCommand<int, bool>>(command);
		}
	}
}