using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace XamarinCommunityToolkit.Behaviors
{
    public class TextValidationBehavior : ValidationBehavior
    {
        public static readonly BindableProperty MinimumLengthProperty = BindableProperty.Create(nameof(MinimumLength), typeof(int), typeof(TextValidationBehavior), 0, propertyChanged: OnValidationPropertyChanged);

        public static readonly BindableProperty MaximumLengthProperty = BindableProperty.Create(nameof(MaximumLength), typeof(int), typeof(TextValidationBehavior), int.MaxValue, propertyChanged: OnValidationPropertyChanged);

        public static readonly BindableProperty ShouldTrimValueProperty = BindableProperty.Create(nameof(ShouldTrimValue), typeof(bool), typeof(TextValidationBehavior), false, propertyChanged: OnValidationPropertyChanged);

        public static readonly BindableProperty ShouldConvertNullToEmptyValueProperty = BindableProperty.Create(nameof(ShouldConvertNullToEmptyValue), typeof(bool), typeof(TextValidationBehavior), true, propertyChanged: OnValidationPropertyChanged);

        public static readonly BindableProperty RegexPatternProperty = BindableProperty.Create(nameof(RegexPattern), typeof(string), typeof(TextValidationBehavior), defaultValueCreator: GetDefaultRegexPattern, propertyChanged: OnRegexPropertyChanged);

        public static readonly BindableProperty RegexOptionsProperty = BindableProperty.Create(nameof(RegexOptions), typeof(RegexOptions), typeof(TextValidationBehavior), defaultValueCreator: GetDefaultRegexOptions, propertyChanged: OnRegexPropertyChanged);

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

        public bool ShouldTrimValue
        {
            get => (bool)GetValue(ShouldTrimValueProperty);
            set => SetValue(ShouldTrimValueProperty, value);
        }

        public bool ShouldConvertNullToEmptyValue
        {
            get => (bool)GetValue(ShouldConvertNullToEmptyValueProperty);
            set => SetValue(ShouldConvertNullToEmptyValueProperty, value);
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

        protected virtual string DefaultRegexPattern => string.Empty;

        protected virtual RegexOptions DefaultRegexOptions => RegexOptions.None;

        protected override void OnAttachedTo(View bindable)
        {
            OnRegexPropertyChanged();
            base.OnAttachedTo(bindable);
        }

        protected override object DecorateValue()
        {
            var value = base.DecorateValue()?.ToString();

            if (ShouldTrimValue)
                value = value?.Trim();

            if (ShouldConvertNullToEmptyValue)
                value ??= string.Empty;

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
    }
}
