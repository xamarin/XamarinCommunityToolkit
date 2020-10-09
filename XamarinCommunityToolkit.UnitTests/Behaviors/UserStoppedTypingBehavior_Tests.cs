using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class UserStoppedTypingBehavior_Tests
	{
		const int defaultTimeThreshold = 1000;
		const int defaultLengthThreshold = 0;

		[Fact]
		public async Task ExecuteCommandWhenTimeThresholdHasExpired()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true));

			// act
			entry.Text = "1";
			await Task.Delay(defaultTimeThreshold + 100);

			// assert
			Assert.True(commandHasBeenExecuted);
		}

		[Fact]
		public async Task ShouldNotExecuteCommandBeforeTimeThresholdHasExpired()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true));

			// act
			entry.Text = "1";
			await Task.Delay(10);
            
			// assert
			Assert.False(commandHasBeenExecuted);
		}

		[Fact]
		public async Task ShouldOnlyExectueCommandOnceWhenTextChangedHasOccurredMultipleTimes()
		{
			// arrange
			var timesExecuted = 0;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => timesExecuted++));

			// act
			entry.Text = "1";
			entry.Text = "12";
			entry.Text = "123";
			entry.Text = "1234";
			await Task.Delay(defaultTimeThreshold + 100);

			// assert
			Assert.Equal(1, timesExecuted);
		}

		[Fact]
		public async Task ShouldDismissKeyboardWhenTimeThresholdHasExpired()
		{
			// arrange
			var entry = CreateEntryWithBehavior(shouldDismissKeyboardAutomatically: true);

			// act
			entry.Focus();
			entry.Text = "1";

			await Task.Delay(defaultTimeThreshold + 100);

			// assert
			Assert.False(entry.IsFocused);
		}

		[Fact]
		public async Task ShouldExecuteCommandWhenMinimumLengthThreholdHasBeenReached()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true),
												lengthThreshold: 3);

			// act
			entry.Text = "1";
			entry.Text = "12";
			entry.Text = "123";
			await Task.Delay(defaultTimeThreshold + 100);

			// assert
			Assert.True(commandHasBeenExecuted);
		}

		[Fact]
		public async Task ShouldNotExecuteCommandWhenMinimumLengthThreholdHasNotBeenReached()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true),
												lengthThreshold: 2);

			// act
			entry.Text = "1";
			await Task.Delay(defaultTimeThreshold + 100);

			// assert
			Assert.False(commandHasBeenExecuted);
		}

		/// <summary>
		/// Due to Focus() not setting the Entry to IsFocused = true, we cannot test if the entry still got focus or not
		/// See for more information: https://forums.xamarin.com/discussion/181096/how-to-focus-an-entry-control-in-a-unit-test
		/// </summary>
		/// <returns></returns>
		//[Fact]
		//public async Task ShouldNotDismissKeyboardWhenMinimumLengthThreholdHasNotBeenReached()
		//{
		//	// arrange
		//	var entry = CreateEntryWithBehavior(lengthThreshold: 3,
		//										shouldDismissKeyboardAutomatically: true);

		//	// act
		//	entry.Focus();
		//	entry.Text = "1";
		//	await Task.Delay(defaultTimeThreshold + 100);

		//	// assert
		//	Assert.True(entry.IsFocused);
		//}

		[Fact]
		public async Task ShouldExecuteCommandImmediatelyWhenMinimumLengthThreholdHasNotBeenSet()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true));

			// act
			entry.Text = "1";
			await Task.Delay(defaultTimeThreshold + 100);

			// assert
			Assert.True(commandHasBeenExecuted);
		}

		public Entry CreateEntryWithBehavior(int timeThreshold = defaultTimeThreshold,
											 int lengthThreshold = defaultLengthThreshold,
											 bool shouldDismissKeyboardAutomatically = false,
											 ICommand command = null)
			=> new Entry
			{
				Behaviors =
				{
					new UserStoppedTypingBehavior
					{
						StoppedTypingTimeThreshold = timeThreshold,
						MinimumLengthThreshold = lengthThreshold,
						ShouldDismissKeyboardAutomatically = shouldDismissKeyboardAutomatically,
						Command = command
					}
				}
			};
	}
}