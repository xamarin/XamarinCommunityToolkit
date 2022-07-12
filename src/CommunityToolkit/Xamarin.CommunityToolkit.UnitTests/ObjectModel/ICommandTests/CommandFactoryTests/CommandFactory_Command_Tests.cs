using System;
using System.Windows.Input;
using NUnit.Framework;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryCommandTests : BaseCommandTests
	{
		[Test]
		public void Action_NullExecuteParameter()
		{
			// Arrange
			Action? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Test]
		public void Action_ValidExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterAction);

			// Act
			command.Execute(null);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<Forms.Command>(command);
			Assert.IsInstanceOf<ICommand>(command);
		}

		[Test]
		public void Action_NullCanExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(NoParameterAction, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		}

		[Test]
		public void Action_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterAction, CanExecuteTrue);

			// Act
			command.Execute(null);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<Forms.Command>(command);
			Assert.IsInstanceOf<ICommand>(command);
		}

		[Test]
		public void ActionObject_NullExecuteParameter()
		{
			// Arrange
			Action<object>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Test]
		public void ActionObject_ValidExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(ObjectParameterAction);

			// Act
			command.Execute(null);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<Forms.Command>(command);
			Assert.IsInstanceOf<ICommand>(command);
		}

		[Test]
		public void ActionObject_NullCanExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(ObjectParameterAction, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		}

		[Test]
		public void ActionObject_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(ObjectParameterAction, _ => true);

			// Act
			command.Execute(1);
			command.Execute(null);
			command.Execute(string.Empty);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command.CanExecute(string.Empty));
			Assert.IsTrue(command.CanExecute(0));

			Assert.IsInstanceOf<Forms.Command>(command);
			Assert.IsInstanceOf<ICommand>(command);
		}

		[Test]
		public void ActionInt_NullExecuteParameter()
		{
			// Arrange
			Action<int>? execute = null;

			// Act

			// Assert
#pragma warning disable CS8604 // Possible null reference argument.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(execute, CanExecuteTrue));
#pragma warning restore CS8604 // Possible null reference argument.
		}

		[Test]
		public void ActionInt_NullCanExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(IntParameterAction, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		}

		[Test]
		public void ActionInt_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterAction, CanExecuteTrue);

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(0));
			Assert.False(command.CanExecute(null));
			Assert.False(command.CanExecute(string.Empty));

			Assert.IsInstanceOf<Forms.Command<int>>(command);
			Assert.IsInstanceOf<ICommand>(command);
		}
	}
}