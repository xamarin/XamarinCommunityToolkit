using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
    public class IAsyncValueCommandTests : BaseAsyncValueCommandTests
    {
        [Theory]
        [InlineData(500)]
        [InlineData(0)]
        public async Task AsyncValueCommand_ExecuteAsync_IntParameter_Test(int parameter)
        {
            // Arrange
            IAsyncValueCommand<int> command = new AsyncValueCommand<int>(IntParameterTask);

            // Act
            await command.ExecuteAsync(parameter);

            // Assert
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData(default)]
        public async Task AsyncValueCommand_ExecuteAsync_StringParameter_Test(string parameter)
        {
            // Arrange
            IAsyncValueCommand<string> command = new AsyncValueCommand<string>(StringParameterTask);

            // Act
            await command.ExecuteAsync(parameter);

            // Assert
        }

        [Fact]
        public void IAsyncValueCommand_Parameter_CanExecuteTrue_Test()
        {
            // Arrange
            IAsyncValueCommand<int> command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteTrue);

            // Act

            // Assert
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void IAsyncValueCommand_Parameter_CanExecuteFalse_Test()
        {
            // Arrange
            IAsyncValueCommand<int> command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteFalse);

            // Act

            // Assert
            Assert.False(command.CanExecute(null));
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
