using System;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[Flags]
	public enum CharacterType
	{
		LowerLetter = 1,
		UpperLetter = 2,
		Letter = LowerLetter | UpperLetter,
		Digit = 4,
		LetterOrDigit = Letter | Digit,
		Symbol = 8,
		WhiteSpace = 16,
		Any = LetterOrDigit | Symbol | WhiteSpace
	}
}