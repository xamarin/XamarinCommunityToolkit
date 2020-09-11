using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
    public class Tests_AsyncValueCommand : BaseAsyncValueCommandTests
	{
        [Fact]
        public void AsyncValueCommandNullExecuteParameter()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
            Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand(null));
#pragma warning restore CS8625
        }

        [Fact]
        public void AsyncValueCommandT_NullExecuteParameter()
        {
            // Arrange

            // Act

            // Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
            Assert.Throws<ArgumentNullException>(() => new AsyncValueCommand<object>(null));
#pragma warning restore CS8625
        }

        [Theory]
        [InlineData(500)]
        [InlineData(0)]
        public async Task AsyncValueCommandExecuteAsync_IntParameter_Test(int parameter)
        {
            // Arrange
            var command = new AsyncValueCommand<int>(IntParameterTask);

            // Act
            await command.ExecuteAsync(parameter);

            // Assert
        }

        [Theory]
        [InlineData("Hello")]
        [InlineData(default)]
        public async Task AsyncValueCommandExecuteAsync_StringParameter_Test(string parameter)
        {
            // Arrange
            var command = new AsyncValueCommand<string>(StringParameterTask);

            // Act
            await command.ExecuteAsync(parameter);

            // Assert
        }

        [Fact]
        public void AsyncValueCommandParameter_CanExecuteTrue_Test()
        {
            // Arrange
            var command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteTrue);

            // Act

            // Assert

            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void AsyncValueCommandParameter_CanExecuteFalse_Test()
        {
            // Arrange
            var command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteFalse);

            // Act

            // Assert
            Assert.False(command.CanExecute(null));
        }

        [Fact]
        public void AsyncValueCommandNoParameter_CanExecuteTrue_Test()
        {
            // Arrange
            var command = new AsyncValueCommand(NoParameterTask, CanExecuteTrue);

            // Act

            // Assert
            Assert.True(command.CanExecute(null));
        }

        [Fact]
        public void AsyncValueCommandNoParameter_CanExecuteFalse_Test()
        {
            // Arrange
            var command = new AsyncValueCommand(NoParameterTask, CanExecuteFalse);

            // Act

            // Assert
            Assert.False(command.CanExecute(null));
        }

        [Fact]
        public void AsyncValueCommandCanExecuteChanged_Test()
        {
            // Arrange
            var canCommandExecute = false;
            var didCanExecuteChangeFire = false;

            var command = new AsyncValueCommand(NoParameterTask, commandCanExecute);
            command.CanExecuteChanged += handleCanExecuteChanged;

            void handleCanExecuteChanged(object sender, EventArgs e) => didCanExecuteChangeFire = true;
            bool commandCanExecute(object parameter) => canCommandExecute;

            Assert.False(command.CanExecute(null));

            // Act
            canCommandExecute = true;

            // Assert
            Assert.True(command.CanExecute(null));
            Assert.False(didCanExecuteChangeFire);

            // Act
            command.RaiseCanExecuteChanged();

            // Assert
            Assert.True(didCanExecuteChangeFire);
            Assert.True(command.CanExecute(null));
        }
    }
}
