using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class TextCharactersValidationBehavior_Tests
	{
		public TextCharactersValidationBehavior_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Theory]
		[InlineData(CharacterType.Any, 1, 2, "A", true)]
		[InlineData(CharacterType.Any, 0, int.MaxValue, "", true)]
		[InlineData(CharacterType.LowercaseLetter, 1, int.MaxValue, "WWWWWaWWWW", true)]
		[InlineData(CharacterType.UppercaseLetter, 1, int.MaxValue, "aaaaaaRRaaaa", true)]
		[InlineData(CharacterType.Letter, 4, int.MaxValue, "aaaaaaRRaaaa", true)]
		[InlineData(CharacterType.Digit, 1, int.MaxValue, "-1d", true)]
		[InlineData(CharacterType.LetterOrDigit, 2, int.MaxValue, "@-3r", true)]
		[InlineData(CharacterType.Symbol, 10, int.MaxValue, "@-&^%!+()/", true)]
		[InlineData(CharacterType.WhiteSpace, 0, int.MaxValue, ";lkjhgfd@+fasf", true)]
		[InlineData(CharacterType.Any, 2, 2, "A", false)]
		[InlineData(CharacterType.Any, 2, 2, "AaA", false)]
		[InlineData(CharacterType.Any, 1, int.MaxValue, "", false)]
		[InlineData(CharacterType.Any, 1, int.MaxValue, null, false)]
		[InlineData(CharacterType.LowercaseLetter, 1, int.MaxValue, "WWWWWW", false)]
		[InlineData(CharacterType.UppercaseLetter, 1, int.MaxValue, "aaaaaa", false)]
		[InlineData(CharacterType.Letter, 4, int.MaxValue, "wHo", false)]
		[InlineData(CharacterType.Digit, 1, int.MaxValue, "-d", false)]
		[InlineData(CharacterType.LetterOrDigit, 2, int.MaxValue, "@-3", false)]
		[InlineData(CharacterType.Symbol, 1, int.MaxValue, "WWWWWWWW", false)]
		[InlineData(CharacterType.WhiteSpace, 0, 0, "WWWWWW WWWWW", false)]
		public void IsValid(CharacterType characterType, int minimumCharactersNumber, int maximumCharactersNumber, string value, bool expectedValue)
		{
			var behavior = new TextCharactersValidationBehavior
			{
				CharacterType = characterType,
				MinimumCharacterCount = minimumCharactersNumber,
				MaximumCharacterCount = maximumCharactersNumber
			};
			var entry = new Entry
			{
				Text = value
			};
			entry.Behaviors.Add(behavior);
			behavior.ForceValidate();
			Assert.Equal(expectedValue, behavior.IsValid);
		}
	}
}
