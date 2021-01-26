using System.Text;
using System.Text.RegularExpressions;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class TextValidationBehavior : ValidationBehavior
	{
		public static readonly BindableProperty MinimumLengthProperty =
			BindableProperty.Create(nameof(MinimumLength), typeof(int), typeof(TextValidationBehavior), 0, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MaximumLengthProperty =
			BindableProperty.Create(nameof(MaximumLength), typeof(int), typeof(TextValidationBehavior), int.MaxValue, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty DecorationFlagsProperty =
			BindableProperty.Create(nameof(DecorationFlags), typeof(TextDecorationFlags), typeof(TextValidationBehavior), TextDecorationFlags.None, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty RegexPatternProperty =
			BindableProperty.Create(nameof(RegexPattern), typeof(string), typeof(TextValidationBehavior), defaultValueCreator: GetDefaultRegexPattern, propertyChanged: OnRegexPropertyChanged);

		public static readonly BindableProperty RegexOptionsProperty =
			BindableProperty.Create(nameof(RegexOptions), typeof(RegexOptions), typeof(TextValidationBehavior), defaultValueCreator: GetDefaultRegexOptions, propertyChanged: OnRegexPropertyChanged);

		Regex regex;

		public int MinimumLength
		{
			get => (int)GetValue(MinimumLengthProperty);
			set => SetValue(MinimumLengthProperty, value);
		}

		public int MaximumLength
		{
			get => (int)GetValue(MaximumLengthProperty);
			set => SetValue(MaximumLengthProperty, value);
		}

		public TextDecorationFlags DecorationFlags
		{
			get => (TextDecorationFlags)GetValue(DecorationFlagsProperty);
			set => SetValue(DecorationFlagsProperty, value);
		}

		public string RegexPattern
		{
			get => (string)GetValue(RegexPatternProperty);
			set => SetValue(RegexPatternProperty, value);
		}

		public RegexOptions RegexOptions
		{
			get => (RegexOptions)GetValue(RegexOptionsProperty);
			set => SetValue(RegexOptionsProperty, value);
		}

		public TextValidationBehavior()
		{
			OnRegexPropertyChanged();
		}

		protected virtual string DefaultRegexPattern => string.Empty;

		protected virtual RegexOptions DefaultRegexOptions => RegexOptions.None;

		protected override object DecorateValue()
		{
			var value = base.DecorateValue()?.ToString();
			var flags = DecorationFlags;

			if (flags.HasFlag(TextDecorationFlags.NullToEmpty))
				value ??= string.Empty;

			if (value == null)
				return null;

			if (flags.HasFlag(TextDecorationFlags.TrimStart))
				value = value.TrimStart();

			if (flags.HasFlag(TextDecorationFlags.TrimEnd))
				value = value.TrimEnd();

			if (flags.HasFlag(TextDecorationFlags.NormalizeWhiteSpace))
				value = NormalizeWhiteSpace(value);

			return value;
		}

		protected override bool Validate(object value)
		{
			var text = value?.ToString();
			if (text == null)
				return false;

			var length = text.Length;
			return length >= MinimumLength &&
				length <= MaximumLength &&
				(regex?.IsMatch(text) ?? false);
		}

		static void OnRegexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((TextValidationBehavior)bindable).OnRegexPropertyChanged();
			OnValidationPropertyChanged(bindable, oldValue, newValue);
		}

		static object GetDefaultRegexPattern(BindableObject bindable)
			=> ((TextValidationBehavior)bindable).DefaultRegexPattern;

		static object GetDefaultRegexOptions(BindableObject bindable)
			=> ((TextValidationBehavior)bindable).DefaultRegexOptions;

		void OnRegexPropertyChanged()
			=> regex = RegexPattern != null
				? new Regex(RegexPattern, RegexOptions)
				: null;

		// This method trims down multiple consecutive whitespaces
		// back to one whitespace.
		// I.e. "Hello    World" will become "Hello World"
		string NormalizeWhiteSpace(string value)
		{
			var builder = new StringBuilder();
			var isSpace = false;
			foreach (var ch in value)
			{
				var wasSpace = isSpace;
				isSpace = char.IsWhiteSpace(ch);
				if (wasSpace && isSpace)
					continue;

				builder.Append(ch);
			}
			return builder.ToString();
		}
	}
}