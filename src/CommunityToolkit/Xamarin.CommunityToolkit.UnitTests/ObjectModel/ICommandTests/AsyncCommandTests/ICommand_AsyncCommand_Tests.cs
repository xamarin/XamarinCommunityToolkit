using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class ICommand_AsyncCommandTests : BaseAsyncCommandTests
	{
		[TestCase(500)]
		[TestCase(0)]
		public async Task ICommand_Execute_IntParameter_Test(int parameter)
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask().ConfigureAwait(false);

			// Assert
		}

		[TestCase("Hello")]
		[TestCase(default)]
		public async Task ICommand_Execute_StringParameter_Test(string parameter)
		{
			// Arrange
			ICommand command = new AsyncCommand<string>(StringParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask().ConfigureAwait(false);

			// Assert
		}

		[Test]
		public void ICommand_ExecuteAsync_InvalidValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException? actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(string), typeof(int));

			ICommand command = new AsyncCommand<string>(StringParameterTask);

			// Act

			actualInvalidCommandParameterException = Assert.Throws<InvalidCommandParameterException>(() => command.Execute(Delay));

			// Assert
			Assert.IsNotNull(actualInvalidCommandParameterException);
			Assert.AreEqual(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Test]
		public void ICommand_ExecuteAsync_InvalidReferenceTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException? actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int), typeof(string));

			ICommand command = new AsyncCommand<int>(IntParameterTask);

			// Act
			actualInvalidCommandParameterException = Assert.Throws<InvalidCommandParameterException>(() => command.Execute("Hello World"));

			// Assert
			Assert.IsNotNull(actualInvalidCommandParameterException);
			Assert.AreEqual(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Test]
		public void ICommand_ExecuteAsync_ValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException? actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int));

			ICommand command = new AsyncCommand<int>(IntParameterTask);

			// Act
			actualInvalidCommandParameterException = Assert.Throws<InvalidCommandParameterException>(() => command.Execute(null));

			// Assert
			Assert.IsNotNull(actualInvalidCommandParameterException);
			Assert.AreEqual(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Test]
		public void ICommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void ICommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand(NoParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void ICommand_NoParameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteDynamic_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteChanged_Test()
		{
			// Arrange
			ICommand command = new AsyncCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task ICommand_Parameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(Delay);

			// Assert
			Assert.IsTrue(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay).ConfigureAwait(false);
			await IntParameterTask(Delay).ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task ICommand_Parameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;

			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			ICommand command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			// Act
			command.Execute(Delay);

			// Assert
			Assert.False(command.CanExecute(null));

			// Act
			var handleCanExecuteChangedTCSResult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
			Assert.AreEqual(canExecuteChangedCount, handleCanExecuteChangedTCSResult);

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
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task ICommand_NoParameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncCommand(() => IntParameterTask(Delay));
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(null);

			// Assert
			Assert.IsTrue(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay).ConfigureAwait(false);
			await IntParameterTask(Delay).ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task ICommand_NoParameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			ICommand command = new AsyncCommand(() => IntParameterTask(Delay), allowsMultipleExecutions: false);
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
			var handleCanExecuteChangedREsult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
			Assert.AreEqual(canExecuteChangedCount, handleCanExecuteChangedREsult);
		}
	}
}