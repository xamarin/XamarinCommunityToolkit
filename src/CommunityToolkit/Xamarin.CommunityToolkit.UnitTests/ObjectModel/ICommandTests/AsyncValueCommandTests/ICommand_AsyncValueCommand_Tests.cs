using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public class ICommand_AsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Theory]
		[InlineData(500)]
		[InlineData(0)]
		public async Task ICommand_Execute_IntParameter_Test(int parameter)
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask().ConfigureAwait(false);

			// Assert
		}

		[Theory]
		[InlineData("Hello")]
		[InlineData(default)]
		public async Task ICommand_Execute_StringParameter_Test(string parameter)
		{
			// Arrange
			ICommand command = new AsyncValueCommand<string>(StringParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask().ConfigureAwait(false);

			// Assert
		}

		[Fact]
		public async Task ICommand_Execute_InvalidValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException? actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(string), typeof(int));

			ICommand command = new AsyncValueCommand<string>(StringParameterTask);

			// Act
			try
			{
				command.Execute(Delay);
				await NoParameterTask().ConfigureAwait(false);
				await NoParameterTask().ConfigureAwait(false);
			}
			catch (InvalidCommandParameterException e)
			{
				actualInvalidCommandParameterException = e;
			}

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.Equal(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Fact]
		public async Task ICommand_Execute_InvalidReferenceTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException? actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int), typeof(string));

			ICommand command = new AsyncValueCommand<int>(IntParameterTask);

			// Act
			try
			{
				command.Execute("Hello World");
				await NoParameterTask().ConfigureAwait(false);
				await NoParameterTask().ConfigureAwait(false);
			}
			catch (InvalidCommandParameterException e)
			{
				actualInvalidCommandParameterException = e;
			}

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.Equal(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Fact]
		public async Task ICommand_Execute_ValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException? actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int));

			ICommand command = new AsyncValueCommand<int>(IntParameterTask);

			// Act
			try
			{
				command.Execute(null);
				await NoParameterTask().ConfigureAwait(false);
				await NoParameterTask().ConfigureAwait(false);
			}
			catch (InvalidCommandParameterException e)
			{
				actualInvalidCommandParameterException = e;
			}

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.Equal(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, parameter => CanExecuteTrue());

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, parameter => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand(NoParameterTask, parameter => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_NoParameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteDynamic_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Fact]
		public void ICommand_Parameter_CanExecuteChanged_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Fact(Timeout = ICommandTestTimeout)]
		public async Task ICommand_Parameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncValueCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(Delay);

			// Assert
			Assert.True(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay).ConfigureAwait(false);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Fact(Timeout = ICommandTestTimeout)]
		public async Task ICommand_Parameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			ICommand command = new AsyncValueCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					if (++canExecuteChangedCount is 2)
					{
						command.CanExecuteChanged -= handleCanExecuteChanged;
						handleCanExecuteChangedTCS.SetResult(canExecuteChangedCount);
					}
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}

			// Act
			command.Execute(Delay);

			// Assert
			Assert.False(command.CanExecute(null));

			// Act
			var handleCanExecuteChangedResult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(2, canExecuteChangedCount);
			Assert.Equal(canExecuteChangedCount, handleCanExecuteChangedResult);
		}

		[Fact(Timeout = ICommandTestTimeout)]
		public async Task ICommand_NoParameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncValueCommand(() => IntParameterTask(Delay));
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(null);

			// Assert
			Assert.True(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay).ConfigureAwait(false);
			await IntParameterTask(Delay).ConfigureAwait(false);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Fact(Timeout = ICommandTestTimeout)]
		public async Task ICommand_NoParameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			ICommand command = new AsyncValueCommand(() => IntParameterTask(Delay), allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			async void handleCanExecuteChanged(object? sender, EventArgs e)
			{
				await semaphoreSlim.WaitAsync().ConfigureAwait(false);

				try
				{
					if (++canExecuteChangedCount is 2)
					{
						command.CanExecuteChanged -= handleCanExecuteChanged;
						handleCanExecuteChangedTCS.SetResult(canExecuteChangedCount);
					}
				}
				finally
				{
					semaphoreSlim.Release();
				}
			}

			// Act
			command.Execute(null);

			// Assert
			Assert.False(command.CanExecute(null));

			// Act
			var handleCanExecuteChangedResult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.Equal(2, canExecuteChangedCount);
			Assert.Equal(canExecuteChangedCount, handleCanExecuteChangedResult);
		}
	}
}