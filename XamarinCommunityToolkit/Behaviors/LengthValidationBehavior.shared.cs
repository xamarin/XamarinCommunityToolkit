using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class LengthValidationBehavior : BaseBehavior<Entry>
    {
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.
            Create(nameof(MaxLength), typeof(int), typeof(LengthValidationBehavior), int.MaxValue);

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public static readonly BindableProperty MinLengthProperty = BindableProperty.
            Create(nameof(MinLength), typeof(int), typeof(LengthValidationBehavior), 0);

        public int MinLength
        {
            get => (int)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NumericValidationBehavior));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
        }        

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = (e.NewTextValue.Length >= MinLength && e.NewTextValue.Length <= MaxLength);

            if (sender is Entry entry)
            {
                Color currentTextColor = entry.TextColor;
                entry.TextColor = isValid ? currentTextColor : (TextColor != null ? TextColor : Color.Red);
            }
        }
    }
}
