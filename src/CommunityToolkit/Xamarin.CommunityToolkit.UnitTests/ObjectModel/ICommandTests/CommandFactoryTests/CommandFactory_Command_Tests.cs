using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryCommandTests : BaseCommandTests
	{
		[Fact]
		public void Action_NullExecuteParameter()
		{
			// Arrange
			Action? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public void Action_ValidExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterAction);

			// Act
			command.Execute(null);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<Forms.Command>(command);
			Assert.IsAssignableFrom<ICommand>(command);
		}

		[Fact]
		public void Action_NullCanExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(NoParameterAction, null));
		}

		[Fact]
		public void Action_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterAction, CanExecuteTrue);

			// Act
			command.Execute(null);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<Forms.Command>(command);
			Assert.IsAssignableFrom<ICommand>(command);
		}

		[Fact]
		public void ActionObject_NullExecuteParameter()
		{
			// Arrange
			Action<object>? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public void ActionObject_ValidExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(ObjectParameterAction);

			// Act
			command.Execute(null);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<Forms.Command>(command);
			Assert.IsAssignableFrom<ICommand>(command);
		}

		[Fact]
		public void ActionObject_NullCanExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(ObjectParameterAction, null));
		}

		[Fact]
		public void ActionObject_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(ObjectParameterAction, _ => true);

			// Act
			command.Execute(1);
			command.Execute(null);
			command.Execute(string.Empty);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			Assert.IsType<Forms.Command>(command);
			Assert.IsAssignableFrom<ICommand>(command);
		}

		[Fact]
		public void ActionInt_NullExecuteParameter()
		{
			// Arrange
			Action<int>? execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(execute, CanExecuteTrue));
		}

		[Fact]
		public void ActionInt_NullCanExecuteParameter()
		{
			// Arrange

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create<int>(IntParameterAction, null));
		}

		[Fact]
		public void ActionInt_ValidCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterAction, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(0));
			Assert.False(command.CanExecute(null));
			Assert.False(command.CanExecute(string.Empty));

			Assert.IsType<Forms.Command<int>>(command);
			Assert.IsAssignableFrom<ICommand>(command);
		}
	}
}