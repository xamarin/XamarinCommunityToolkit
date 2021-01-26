using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class CharactersValidationBehavior : TextValidationBehavior
	{
		List<Predicate<char>> characterPredicates;

		public static readonly BindableProperty CharacterTypeProperty =
			BindableProperty.Create(nameof(CharacterType), typeof(CharacterType), typeof(CharactersValidationBehavior), CharacterType.Any, propertyChanged: OnCharacterTypePropertyChanged);

		public static readonly BindableProperty MinimumCharacterCountProperty =
			BindableProperty.Create(nameof(MinimumCharacterCount), typeof(int), typeof(CharactersValidationBehavior), 0, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MaximumCharacterCountProperty =
			BindableProperty.Create(nameof(MaximumCharacterCount), typeof(int), typeof(CharactersValidationBehavior), int.MaxValue, propertyChanged: OnValidationPropertyChanged);

		public CharactersValidationBehavior()
			=> OnCharacterTypePropertyChanged();

		public CharacterType CharacterType
		{
			get => (CharacterType)GetValue(CharacterTypeProperty);
			set => SetValue(CharacterTypeProperty, value);
		}

		public int MinimumCharacterCount
		{
			get => (int)GetValue(MinimumCharacterCountProperty);
			set => SetValue(MinimumCharacterCountProperty, value);
		}

		public int MaximumCharacterCount
		{
			get => (int)GetValue(MaximumCharacterCountProperty);
			set => SetValue(MaximumCharacterCountProperty, value);
		}

		protected override bool Validate(object value)
			=> base.Validate(value) && Validate(value?.ToString());

		static void OnCharacterTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((CharactersValidationBehavior)bindable).OnCharacterTypePropertyChanged();
			OnValidationPropertyChanged(bindable, oldValue, newValue);
		}

		static IEnumerable<Predicate<char>> GetCharacterPredicates(CharacterType characterType)
		{
			if (characterType.HasFlag(CharacterType.LowercaseLetter))
				yield return char.IsLower;

			if (characterType.HasFlag(CharacterType.UppercaseLetter))
				yield return char.IsUpper;

			if (characterType.HasFlag(CharacterType.Digit))
				yield return char.IsDigit;

			if (characterType.HasFlag(CharacterType.Whitespace))
				yield return char.IsWhiteSpace;

			if (characterType.HasFlag(CharacterType.NonAlphanumericSymbol))
				yield return c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c);

			if (characterType.HasFlag(CharacterType.LowercaseLatinLetter))
				yield return c => c >= 'a' && c <= 'z';

			if (characterType.HasFlag(CharacterType.UppercaseLatinLetter))
				yield return c => c >= 'A' && c <= 'Z';
		}

		void OnCharacterTypePropertyChanged()
			=> characterPredicates = GetCharacterPredicates(CharacterType).ToList();

		bool Validate(string value)
		{
			var count = value?.ToCharArray().Count(character => characterPredicates.Any(predicate => predicate.Invoke(character))) ?? 0;
			return count >= MinimumCharacterCount
				&& count <= MaximumCharacterCount;
		}
	}
}