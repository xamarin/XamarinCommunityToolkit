using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncCommandTests
{
	public class IAsyncCommandTests : BaseAsyncCommandTests
	{
		[Test]
		public void IAsyncCommand_CanExecute_InvalidReferenceParameter()
		{
			// Arrange
			IAsyncCommand<int, bool> command = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute("Hello World"));
		}

		[Test]
		public void IAsyncCommand_Execute_InvalidValueTypeParameter()
		{
			// Arrange
			IAsyncCommand<string, bool> command = new AsyncCommand<string, bool>(StringParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute(true));
		}

		[Test]
		public void IAsyncCommand_Execute_InvalidReferenceParameter()
		{
			// Arrange
			IAsyncCommand<int, bool> command = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.Execute("Hello World"));
		}

		[Test]
		public void IAsyncCommand_CanExecute_InvalidValueTypeParameter()
		{
			// Arrange
			IAsyncCommand<int, string> command = new AsyncCommand<int, string>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.Throws<InvalidCommandParameterException>(() => command.CanExecute(true));
		}

		[TestCase("Hello")]
		[TestCase(default)]
		public async Task AsyncCommand_ExecuteAsync_StringParameter_Test(string parameter)
		{
			// Arrange
			IAsyncCommand<string> command = new AsyncCommand<string>(StringParameterTask);
			IAsyncCommand<string, int> command2 = new AsyncCommand<string, int>(StringParameterTask);

			// Act
			await command.ExecuteAsync(parameter).ConfigureAwait(false);
			await command2.ExecuteAsync(parameter).ConfigureAwait(false);

			// Assert
		}

		[Test]
		public void IAsyncCommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteTrue(parameter));
			IAsyncCommand<int, bool> command2 = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command2.CanExecute(true));
		}

		[Test]
		public void IAsyncCommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, parameter => CanExecuteFalse(parameter));
			IAsyncCommand<int, string> command2 = new AsyncCommand<int, string>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute("Hello World"));
		}

		[Test]
		public void IAsyncCommand_NoParameter_CanExecuteTrue_Test()
		{
			// Arrange
			IAsyncCommand command = new AsyncCommand(NoParameterTask, parameter => CanExecuteTrue(parameter));

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void IAsyncCommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			IAsyncCommand command = new AsyncCommand(NoParameterTask, parameter => CanExecuteFalse(parameter));

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void IAsyncCommand_Parameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteTrue());
			IAsyncCommand<int, bool> command2 = new AsyncCommand<int, bool>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.IsTrue(command2.CanExecute(true));
		}

		[Test]
		public void IAsyncCommand_Parameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, () => CanExecuteFalse());
			IAsyncCommand<int, string> command2 = new AsyncCommand<int, string>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
			Assert.False(command2.CanExecute("Hello World"));
		}

		[Test]
		public void IAsyncCommand_NoParameter_CanExecuteTrue_NoParameter_Test()
		{
			// Arrange
			IAsyncCommand command = new AsyncCommand(NoParameterTask, () => CanExecuteTrue());

			// Act

			// Assert
			Assert.IsTrue(command.CanExecute(null));
		}

		[Test]
		public void IAsyncCommand_NoParameter_CanExecuteFalse_NoParameter_Test()
		{
			// Arrange
			IAsyncCommand command = new AsyncCommand(NoParameterTask, () => CanExecuteFalse());

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task IAsyncCommand_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.IsTrue(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.IsTrue(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);

			command.CanExecuteChanged -= handleCanExecuteChanged;

			void handleCanExecuteChanged(object? sender, EventArgs e) => canExecuteChangedCount++;
		}

		[Test]
		[Timeout(ICommandTestTimeout)]
		public async Task IAsyncCommand_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var semaphoreSlim = new SemaphoreSlim(1, 1);
			var canExecuteChangedCount = 0;
			var handleCanExecuteChangedTCS = new TaskCompletionSource<int>();

			IAsyncCommand<int> command = new AsyncCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			Assert.False(command.AllowsMultipleExecutions);

			// Act
			var asyncCommandTask = command.ExecuteAsync(Delay);

			// Assert
			Assert.IsTrue(command.IsExecuting);
			Assert.False(command.CanExecute(null));

			// Act
			await asyncCommandTask.ConfigureAwait(false);
			var handleCanExecuteChangedREsult = await handleCanExecuteChangedTCS.Task.ConfigureAwait(false);

			// Assert
			Assert.IsTrue(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
			Assert.AreEqual(canExecuteChangedCount, handleCanExecuteChangedREsult);

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
	}
}