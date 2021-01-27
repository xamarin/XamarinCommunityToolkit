using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.CommandFactoryTests
{
	public class CommandFactoryAsyncCommandTests : BaseCommandTests
	{
		[Fact]
		public void NullExecuteParameter()
		{
			// Arrange
			Func<Task> execute = null;

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => CommandFactory.Create(execute));
		}

		[Fact]
		public void NullCanExecuteParameter()
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
		public void IntExecuteNullCanExecuteParameter()
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
		public void IntExecuteTrueCanExecuteParameter()
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
		public void IntExecuteFalseCanExecuteParameter()
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
		public void IntExecuteNullTypedCanExecuteParameter()
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
		public void IntExecuteBoolCanExecuteParameter()
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

		[Fact]
		public void LambdaExecuteTiebreakerTest()
		{
			// Arrange
			CommandFactory.Create(async () => await NoParameterTask());
			CommandFactory.Create(async () => await NoParameterTask(), CanExecuteTrue);
			CommandFactory.Create(executeTask: async () => await NoParameterTask(), CanExecuteTrue, null);
			CommandFactory.Create<int>(async p => await IntParameterTask(p));
			CommandFactory.Create<int>(async p => await IntParameterTask(p), CanExecuteTrue);
			CommandFactory.Create<int>(executeTask: async p => await IntParameterTask(p), CanExecuteTrue, null);
			CommandFactory.Create<int, bool>(async p => await IntParameterTask(p));
			CommandFactory.Create<int, bool>(async p => await IntParameterTask(p), CanExecuteTrue);
			CommandFactory.Create<int, bool>(executeTask: async p => await IntParameterTask(p), CanExecuteTrue, null);

			// Act

			// Assert
		}
	}
}