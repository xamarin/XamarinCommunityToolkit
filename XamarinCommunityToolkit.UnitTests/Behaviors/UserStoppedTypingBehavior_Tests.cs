using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class UserStoppedTypingBehavior_Tests
	{
		const int defaultThreshold = 1000;

		[Fact]
		public async void CommandExecutesAfterTimeThresholdExpires()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true));

			// act
			entry.Text = "1";
			await Task.Delay(defaultThreshold + 100);

			// assert
			Assert.True(commandHasBeenExecuted);
		}

		[Fact]
		public async void CommandNotExecutesYetBeforeTimeThresholdExpires()
		{
			// arrange
			var commandHasBeenExecuted = false;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => commandHasBeenExecuted = true));

			// act
			entry.Text = "1";
			await Task.Delay(defaultThreshold - 100);

			// assert
			Assert.False(commandHasBeenExecuted);
		}

		[Fact]
		public async void CommandOnlyExecutesOnceWhenTextChangedOccurredMultipleTimes()
		{
			// arrange
			var timesExecuted = 0;
			var entry = CreateEntryWithBehavior(command: new Command<string>((s) => timesExecuted++));

			// act
			entry.Text = "1";
			entry.Text = "12";
			entry.Text = "123";
			entry.Text = "1234";
			await Task.Delay(defaultThreshold + 100);

			// assert
			Assert.Equal(1, timesExecuted);
		}

		[Fact]
		public async void KeyboardDimissesAfterTimeThresholdExpires()
		{
			// arrange
			var entry = CreateEntryWithBehavior(shouldDismissKeyboardAutomatically: true);

			// act
			entry.Focus();
			entry.Text = "1";

			await Task.Delay(defaultThreshold + 100);

			// assert
			Assert.False(entry.IsFocused);
		}

		public Entry CreateEntryWithBehavior(int threshold = defaultThreshold,
											 bool shouldDismissKeyboardAutomatically = false,
											 ICommand command = null)
			=> new Entry
			{
				Behaviors =
				{
					new UserStoppedTypingBehavior
					{
						StoppedTypingTimeThreshold = threshold,
						ShouldDismissKeyboardAutomatically = shouldDismissKeyboardAutomatically,
						Command = command
					}
				}
			};
	}
}