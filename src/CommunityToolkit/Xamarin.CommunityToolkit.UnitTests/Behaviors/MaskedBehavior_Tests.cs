using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class MaskedBehavior_Tests
	{
		[Test]
		public void ShouldMaskUsersInput()
		{
			// arrange
			var entry = CreateEntry("AA-AA-AA", 'A');

			// act
			entry.Text = "112233";

			// assert
			Assert.IsTrue(entry.Text.Equals("11-22-33"));
		}

		[Test]
		public void MaskNullOrEmptyReturns()
		{
			// arrange
			var entry = CreateEntry(string.Empty, 'A');

			// act
			entry.Text = "112233";

			// assert
			Assert.IsTrue(entry.Text.Equals("112233"));
		}

		[Test]
		public void MaskChangedShouldUpdateText()
		{
			// arrange
			var entry = CreateEntry("AA-AA-AA", 'A');

			// act
			entry.Text = "112233";
			Assert.IsTrue(entry.Text.Equals("11-22-33"));

			var behavior = (MaskedBehavior)entry.Behaviors.First();
			behavior.Mask = "AA-(AA)-AA";

			// assert
			Assert.IsTrue(entry.Text.Equals("11-(22)-33"));
		}

		[Test]
		public void NumbersInMaskShouldBeAllowed()
		{
			// arrange
			var entry = CreateEntry("+1 (AAA) AAA-AAAA", 'A');

			// act
			entry.Text = "12345";

			// assert
			Assert.IsTrue(entry.Text.Equals("+1 (123) 45"));
		}

		[Test]
		public void NumbersInMaskShouldBeAllowedTypeByType()
		{
			// arrange
			var entry = CreateEntry("+1 (AAA) AAA-AAAA", 'A');

			// act
			entry.Text = string.Empty;

			entry.Text += "1";
			entry.Text += "2";
			entry.Text += "3";
			entry.Text += "4";
			entry.Text += "5";

			// assert
			Assert.IsTrue(entry.Text.Equals("+1 (123) 45"));
		}

		Entry CreateEntry(string mask, char unMaskedCharacter)
		{
			var behavior = new MaskedBehavior
			{
				Mask = mask,
				UnMaskedCharacter = unMaskedCharacter
			};

			var entry = new Entry
			{
				Behaviors =
				{
					behavior
				}
			};

			// We simulate Focus/Unfocus behavior ourselves
			// because unit tests doesn't have "platform-specific" part
			// where IsFocused is controlled in the real app
			entry.FocusChangeRequested += (s, e) => entry.SetValue(VisualElement.IsFocusedPropertyKey, e.Focus);

			return entry;
		}
	}
}