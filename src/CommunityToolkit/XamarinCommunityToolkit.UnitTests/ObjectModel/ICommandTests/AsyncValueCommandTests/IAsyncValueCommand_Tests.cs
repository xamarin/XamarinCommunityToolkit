using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public class IAsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Fact]
		public void IAsyncCommand_CanExecute_InvalidReferenceParameter()
		{
			// Arrange
			IAsyncValueCommand<int, bool> command = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute("Hello World"));
		}

		[Fact]
		public void IAsyncCommand_Execute_InvalidValueTypeParameter()
		{
			// Arrange
			IAsyncValueCommand<string, bool> command = new AsyncValueCommand<string, bool>(StringParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(true));
		}

		[Fact]
		public void IAsyncCommand_Execute_InvalidReferenceParameter()
		{
			// Arrange
			IAsyncValueCommand<int, bool> command = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute("Hello World"));
		}

		[Fact]
		public void IAsyncCommand_CanExecute_InvalidValueTypeParameter()
		{
			// Arrange
			IAsyncValueCommand<int, string> command = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(true));
		}

		[Theory]
		[InlineData(500)]
		[InlineData(0)]
		public async Task AsyncValueCommand_ExecuteAsync_IntParameter_Test(int parameter)
		{
			// Arrange
			IAsyncValueCommand<int> command = new AsyncValueCommand<int>(IntParameterTask);
			IAsyncValueCommand<int> command2 = new AsyncValueCommand<int, string>(IntParameterTask);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Theory]
		[InlineData("Hello")]
		[InlineData(default)]
		public async Task AsyncValueCommand_ExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			IAsyncValueCommand<string> command = new AsyncValueCommand<string>(StringParameterTask);
			IAsyncValueCommand<string, int> command2 = new AsyncValueCommand<string, int>(StringParameterTask);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Fact]
		public void IAsyncValueCommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			IAsyncValueCommand<int> command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteTrue);
			IAsyncValueCommand<int, string> command2 = new AsyncValueCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command.CanExecute("Hello World"));
		}

		[Fact]
		public void IAsyncValueCommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			IAsyncValueCommand<int> command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteFalse);
			IAsyncValueCommand<int, bool> command2 = new AsyncValueCommand<int, bool>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute(true));
		}

		[Fact]
		public void IAsyncValueCommand_NoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			IAsyncValueCommand command = new AsyncValueCommand(NoParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void IAsyncValueCommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			IAsyncValueCommand command = new AsyncValueCommand(NoParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}
	}
}