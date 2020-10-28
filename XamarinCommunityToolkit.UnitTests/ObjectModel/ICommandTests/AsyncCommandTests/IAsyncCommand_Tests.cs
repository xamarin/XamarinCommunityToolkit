using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class IAsyncCommandTests : BaseAsyncCommandTests
	{
		[Fact]
		public void IAsyncCommand_CanExecute_InvalidReferenceParameter()
		{
			// Arrange
			IAsyncCommand<int, bool> command = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute("Hello World"));
		}

		[Fact]
		public void IAsyncCommand_Execute_InvalidValueTypeParameter()
		{
			// Arrange
			IAsyncCommand<string, bool> command = new AsyncCommand<string, bool>(StringParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(true));
		}

		[Fact]
		public void IAsyncCommand_Execute_InvalidReferenceParameter()
		{
			// Arrange
			IAsyncCommand<int, bool> command = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute("Hello World"));
		}

		[Fact]
		public void IAsyncCommand_CanExecute_InvalidValueTypeParameter()
		{
			// Arrange
			IAsyncCommand<int, string> command = new AsyncCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(true));
		}

		[Theory]
		[InlineData("Hello")]
		[InlineData(default)]
		public async Task AsyncCommand_ExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			IAsyncCommand<string> command = new AsyncCommand<string>(StringParameterTask);
			IAsyncCommand<string, int> command2 = new AsyncCommand<string, int>(StringParameterTask);

			// Act
			await command.ExecuteAsync(parameter);
			await command2.ExecuteAsync(parameter);

			// Assert
		}

		[Fact]
		public void IAsyncCommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, CanExecuteTrue);
			IAsyncCommand<int, bool> command2 = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.True(command2.CanExecute(true));
		}

		[Fact]
		public void IAsyncCommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, CanExecuteFalse);
			IAsyncCommand<int, string> command2 = new AsyncCommand<int, string>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute("Hello World"));
		}

		[Fact]
		public void IAsyncCommand_NoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			IAsyncCommand command = new AsyncCommand(NoParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void IAsyncCommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			IAsyncCommand command = new AsyncCommand(NoParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public async Task IAsyncCommand_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.True(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.True(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(0, canExecuteChangedCount);

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;
		}

		[Fact]
		public async Task IAsyncCommand_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.True(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask;

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(2, canExecuteChangedCount);

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;
		}
	}
}