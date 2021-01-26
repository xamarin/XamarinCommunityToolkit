using System;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryCommandTests : BaseCommandTests
	{
		protected void NoParameterAction()
        {
        }

		protected void IntParameterAction(int i)
        {
        }

		[Fact]
		public void NullExecuteParameter()
		{
			// Arrange
			Action execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public void NullCanExecuteParameter()
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
		public void IntExecuteNullCanExecuteParameter()
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
		public void IntExecuteTrueCanExecuteParameter()
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
		public void IntExecuteFalseCanExecuteParameter()
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
		public void IntExecuteNullTypedCanExecuteParameter()
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
		public void IntExecuteBoolCanExecuteParameter()
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
	}
}