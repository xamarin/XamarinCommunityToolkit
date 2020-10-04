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
		WhiteSpace = 8,
		Symbol = 16,
		Any = LetterOrDigit | Symbol
	}
}