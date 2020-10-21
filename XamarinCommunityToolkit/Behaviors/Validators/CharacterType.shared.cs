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
		Alphanumeric = Letter | Digit,
		Whitespace = 8,
		NonAlphanumericSymbol = 16,
		LowercaseLatinLetter = 32,
		UppercaseLatinLetter = 64,
		LatinLetter = LowercaseLatinLetter | UppercaseLatinLetter,
		Any = Alphanumeric | NonAlphanumericSymbol | Whitespace
	}
}