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
    }
}
