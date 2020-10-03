using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class TextCharactersValidationBehavior : TextValidationBehavior
	{
		IEnumerable<Predicate<char>> characterPredicates;

		public static readonly BindableProperty CharacterTypeProperty =
			BindableProperty.Create(nameof(CharacterType), typeof(CharacterType), typeof(TextCharactersValidationBehavior), CharacterType.Any, propertyChanged: OnCharacterTypePropertyChanged);

		public static readonly BindableProperty MinimumCharactersNumberProperty =
			BindableProperty.Create(nameof(MinimumCharactersNumber), typeof(int), typeof(TextCharactersValidationBehavior), 0, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MaximumCharactersNumberProperty =
			BindableProperty.Create(nameof(MaximumCharactersNumber), typeof(int), typeof(TextCharactersValidationBehavior), int.MaxValue, propertyChanged: OnValidationPropertyChanged);

		public TextCharactersValidationBehavior()
			=> OnCharacterTypePropertyChanged();

		public CharacterType CharacterType
		{
			get => (CharacterType)GetValue(CharacterTypeProperty);
			set => SetValue(CharacterTypeProperty, value);
		}

		public int MinimumCharactersNumber
		{
			get => (int)GetValue(MinimumCharactersNumberProperty);
			set => SetValue(MinimumCharactersNumberProperty, value);
		}

		public int MaximumCharactersNumber
		{
			get => (int)GetValue(MaximumCharactersNumberProperty);
			set => SetValue(MaximumCharactersNumberProperty, value);
		}

		protected override bool Validate(object value)
			=> base.Validate(value) && Validate(value?.ToString());

		static void OnCharacterTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((TextCharactersValidationBehavior)bindable).OnCharacterTypePropertyChanged();
			OnValidationPropertyChanged(bindable, oldValue, newValue);
		}

		void OnCharacterTypePropertyChanged()
			=> characterPredicates = GetCharacterPredicates();

		IEnumerable<Predicate<char>> GetCharacterPredicates()
		{
			if (CharacterType.HasFlag(CharacterType.LowerLetter))
				yield return char.IsLower;

			if (CharacterType.HasFlag(CharacterType.UpperLetter))
				yield return char.IsUpper;

			if (CharacterType.HasFlag(CharacterType.Digit))
				yield return char.IsDigit;

			if (CharacterType.HasFlag(CharacterType.Symbol))
				yield return char.IsSymbol;

			if (CharacterType.HasFlag(CharacterType.WhiteSpace))
				yield return char.IsWhiteSpace;
		}

		bool Validate(string value)
		{
			var count = value?.ToCharArray().Count(character => characterPredicates.Any(predicate => predicate.Invoke(character))) ?? 0;
			return count >= MinimumCharactersNumber
				&& count <= MaximumCharactersNumber;
		}
	}
}