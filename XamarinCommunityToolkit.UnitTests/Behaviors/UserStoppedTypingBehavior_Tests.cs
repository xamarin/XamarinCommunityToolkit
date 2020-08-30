using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class UserStoppedTypingBehavior_Tests
	{
		Entry entry;
		UserStoppedTypingBehavior behavior;
		int threshold = 1000;

		public UserStoppedTypingBehavior_Tests()
		{
			behavior = new UserStoppedTypingBehavior();
			behavior.StoppedTypingThreshold = threshold;

			entry = new Entry
			{
				Behaviors =
				{
					behavior
				}
			};
		}

		[Fact]
		public async void CommandExecutesAfterThresholdTimeExpires()
		{
			// arrange
			var commandHasBeenExecuted = false;
			behavior.Command = new Command<string>((s) => commandHasBeenExecuted = true);

			// act
			entry.Text = "1";
			await Task.Delay(threshold + 100);

			// assert
			Assert.True(commandHasBeenExecuted);
		}

		[Fact]
		public async void CommandNotExecutesYetBeforeThresholdTimeExpires()
		{
			// arrange
			var commandHasBeenExecuted = false;
			behavior.Command = new Command<string>((s) => commandHasBeenExecuted = true);

			// act
			entry.Text = "1";
			await Task.Delay(threshold - 100);

			// assert
			Assert.False(commandHasBeenExecuted);
		}

		[Fact]
		public async void CommandOnlyExecutesOnceWhenTextChangedOccurredMultipleTimes()
		{
			// arrange
			var timesExecuted = 0;
			behavior.Command = new Command<string>((s) => timesExecuted++);

			// act
			entry.Text = "1";
			entry.Text = "12";
			entry.Text = "123";
			entry.Text = "1234";
			await Task.Delay(threshold + 100);

			// assert
			Assert.Equal(1, timesExecuted);
		}

		[Fact]
		public async void KeyboardDimissesAfterThresholdTimeExpires()
		{
			// arrange
			behavior.AutoDismissKeyboard = true;

			// act
			entry.Focus();
			entry.Text = "1";
			
			await Task.Delay(threshold + 100);

			// assert
			Assert.False(entry.IsFocused);
		}
	}
}