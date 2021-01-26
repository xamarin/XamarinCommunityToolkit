using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryTests : BaseCommandTests
	{
		protected void NoParameterAction()
        {
        }

		protected void IntParameterAction(int i)
        {
        }

		[Fact]
		public void CommandNullExecuteParameter()
		{
			// Arrange
			Action execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public void AsyncCommandNullExecuteParameter()
		{
			// Arrange
			Func<Task> execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public void CommandNullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterAction, null);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));
		}

		[Fact]
		public void AsyncCommandNullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create(NoParameterTask, null);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));
		}

		[Fact]
		public void IntCommandNullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterAction, null);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			command.Execute(0);
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(null));
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(string.Empty));
		}

		[Fact]
		public void IntAsyncCommandNullCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, null);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));

			command.Execute(0);
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(null));
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(string.Empty));
		}

		[Fact]
		public void IntCommandTrueCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterAction, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));
		}

		[Fact]
		public void IntAsyncCommandTrueCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute(string.Empty));
			Assert.True(command.CanExecute(0));
		}

		[Fact]
		public void IntCommandFalseCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterAction, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command.CanExecute(string.Empty));
			Assert.False(command.CanExecute(0));
		}

		[Fact]
		public void IntAsyncCommandFalseCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command.CanExecute(string.Empty));
			Assert.False(command.CanExecute(0));
		}

		[Fact]
		public void IntCommandNullTypedCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterAction, null);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(null));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
		}

		[Fact]
		public void IntAsyncCommandNullTypedCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, null);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(null));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
		}

		[Fact]
		public void IntCommandBoolCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterAction, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(null));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
		}

		[Fact]
		public void IntAsyncCommandBoolCanExecuteParameter()
		{
			// Arrange
			var command = CommandFactory.Create<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(null));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(string.Empty));
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(0));
		}
	}
}