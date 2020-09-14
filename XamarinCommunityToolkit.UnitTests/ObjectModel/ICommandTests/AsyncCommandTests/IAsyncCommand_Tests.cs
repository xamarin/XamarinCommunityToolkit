using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
    public class Tests_IAsyncCommand : BaseAsyncCommandTests
    {
        [Theory]
        [InlineData(500)]
        [InlineData(0)]
        public async Task AsyncCommand_ExecuteAsync_IntParameter_Test(int parameter)
        {
            // Arrange
            var command = new AsyncCommand<int>(IntParameterTask);

            // Act
            await command.ExecuteAsync(parameter);

            // Assert
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData(default)]
        public async Task AsyncCommand_ExecuteAsync_StringParameter_Test(string parameter)
        {
            // Arrange
            IAsyncCommand<string> command = new AsyncCommand<string>(StringParameterTask);

            // Act
            await command.ExecuteAsync(parameter);

            // Assert
        }

        [Fact]
        public void IAsyncCommand_Parameter_CanExecuteTrue_Test()
        {
            // Arrange
            IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, CanExecuteTrue);

            // Act

            // Assert
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void IAsyncCommand_Parameter_CanExecuteFalse_Test()
        {
            // Arrange
            IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, CanExecuteFalse);

            // Act

            // Assert
            Assert.False(command.CanExecute(null));
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

            void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

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
        }

        [Fact]
        public async Task IAsyncCommand_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
        {
            // Arrange
            var canExecuteChangedCount = 0;

            IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
            command.CanExecuteChanged += handleCanExecuteChanged;

            void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

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
        }
    }
}
