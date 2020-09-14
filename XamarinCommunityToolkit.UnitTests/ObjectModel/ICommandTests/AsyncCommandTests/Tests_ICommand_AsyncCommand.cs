using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class Tests_ICommand_AsyncCommand : BaseAsyncCommandTests
	{
		[Theory]
		[InlineData(500)]
		[InlineData(default)]
		public async Task ICommand_Execute_IntParameter_Test(int parameter)
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask();

			// Assert
		}

		[Theory]
		[InlineData("Hello")]
		[InlineData(default)]
		public async Task ICommand_Execute_StringParameter_Test(string parameter)
		{
			// Arrange
			ICommand command = new AsyncCommand<string>(StringParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask();

			// Assert
		}

		[Fact]
		public void ICommand_ExecuteAsync_InvalidValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(string), typeof(int));

			ICommand command = new AsyncCommand<string>(StringParameterTask);

			// Act

			actualInvalidCommandParameterException = Assert.Throws<InvalidCommandParameterException>(() => command.Execute(Delay));

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.Equal(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Fact]
		public void ICommand_ExecuteAsync_InvalidReferenceTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int), typeof(string));

			ICommand command = new AsyncCommand<int>(IntParameterTask);

			// Act
			actualInvalidCommandParameterException = Assert.Throws<InvalidCommandParameterException>(() => command.Execute("Hello World"));

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.Equal(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Fact]
		public void ICommand_ExecuteAsync_ValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int));

			ICommand command = new AsyncCommand<int>(IntParameterTask);

			// Act
			actualInvalidCommandParameterException = Assert.Throws<InvalidCommandParameterException>(() => command.Execute(null));

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.Equal(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand(NoParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteDynamic_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteChanged_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}
	}
}
