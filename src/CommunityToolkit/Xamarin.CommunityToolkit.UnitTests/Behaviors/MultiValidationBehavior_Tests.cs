using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class MultiValidationBehavior_Tests
	{
		[SetUp]
		public void Setup() => Device.PlatformServices = new MockPlatformServices();

		[TestCase(CharacterType.Any, 1, 2, "A", true)]
		[TestCase(CharacterType.Any, 0, int.MaxValue, "", true)]
		[TestCase(CharacterType.LowercaseLetter, 1, int.MaxValue, "WWWWWaWWWW", true)]
		[TestCase(CharacterType.UppercaseLetter, 1, int.MaxValue, "aaaaaaRRaaaa", true)]
		[TestCase(CharacterType.Letter, 4, int.MaxValue, "aaaaaaRRaaaa", true)]
		[TestCase(CharacterType.Digit, 1, int.MaxValue, "-1d", true)]
		[TestCase(CharacterType.Alphanumeric, 2, int.MaxValue, "@-3r", true)]
		[TestCase(CharacterType.NonAlphanumericSymbol, 10, int.MaxValue, "@-&^%!+()/", true)]
		[TestCase(CharacterType.LowercaseLatinLetter, 2, int.MaxValue, "HHHH a    r.", true)]
		[TestCase(CharacterType.UppercaseLatinLetter, 2, int.MaxValue, "aaaaaa....R.R.R.aaaa", true)]
		[TestCase(CharacterType.LatinLetter, 5, int.MaxValue, "12345bBbBb", true)]
		[TestCase(CharacterType.Whitespace, 0, int.MaxValue, ";lkjhgfd@+fasf", true)]
		[TestCase(CharacterType.Any, 2, 2, "A", false)]
		[TestCase(CharacterType.Any, 2, 2, "AaA", false)]
		[TestCase(CharacterType.Any, 1, int.MaxValue, "", false)]
		[TestCase(CharacterType.Any, 1, int.MaxValue, null, false)]
		[TestCase(CharacterType.LowercaseLetter, 1, int.MaxValue, "WWWWWW", false)]
		[TestCase(CharacterType.UppercaseLetter, 1, int.MaxValue, "aaaaaa", false)]
		[TestCase(CharacterType.Letter, 4, int.MaxValue, "wHo", false)]
		[TestCase(CharacterType.Digit, 1, int.MaxValue, "-d", false)]
		[TestCase(CharacterType.Alphanumeric, 2, int.MaxValue, "@-3", false)]
		[TestCase(CharacterType.NonAlphanumericSymbol, 1, int.MaxValue, "WWWWWWWW", false)]
		[TestCase(CharacterType.LowercaseLatinLetter, 1, int.MaxValue, "Кириллица", false)]
		[TestCase(CharacterType.UppercaseLatinLetter, 1, int.MaxValue, "КИРИЛЛИЦА", false)]
		[TestCase(CharacterType.LatinLetter, 1, int.MaxValue, "Это Кириллица!", false)]
		[TestCase(CharacterType.Whitespace, 0, 0, "WWWWWW WWWWW", false)]
		public async Task IsValid(CharacterType characterType, int minimumCharactersNumber, int maximumCharactersNumber, string value, bool expectedValue)
		{
			// Arrange
			var behavior = new CharactersValidationBehavior
			{
				CharacterType = characterType,
				MinimumCharacterCount = minimumCharactersNumber,
				MaximumCharacterCount = maximumCharactersNumber
			};

			var multiBehavior = new MultiValidationBehavior();
			multiBehavior.Children.Add(behavior);

			var entry = new Entry
			{
				Text = value
			};
			entry.Behaviors.Add(multiBehavior);

			// Act
			await multiBehavior.ForceValidate();

			// Assert
			Assert.AreEqual(expectedValue, multiBehavior.IsValid);
		}
	}
}