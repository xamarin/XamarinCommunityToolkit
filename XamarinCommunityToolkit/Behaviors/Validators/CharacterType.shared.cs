using System;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[Flags]
	public enum CharacterType
	{
		LowercaseLetter = 1,
		UppercaseLetter = 2,
		Letter = LowercaseLetter | UppercaseLetter,
		Digit = 4,
		LetterOrDigit = Letter | Digit,
		Whitespace = 8,
		Symbol = 16,
		LowercaseBasicLatinLetter = 32,
		UppercaseBasicLatinLetter = 64,
		BasicLatinLetter = LowercaseBasicLatinLetter | UppercaseBasicLatinLetter,
		Any = LetterOrDigit | Symbol | Whitespace
	}
}