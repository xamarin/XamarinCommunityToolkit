using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class MinMaxNumericValidationBehavior : BaseBehavior<Entry>
    {
        public static readonly BindableProperty MaxValueProperty = BindableProperty.
            Create(nameof(MaxValue), typeof(double), typeof(MinMaxNumericValidationBehavior), double.MaxValue);

        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public static readonly BindableProperty MinValueProperty = BindableProperty.
            Create(nameof(MinValue), typeof(double), typeof(MinMaxNumericValidationBehavior), double.MinValue);

        public int MinValue
        {
            get => (int)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NumericValidationBehavior));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is Entry entry)
            {
                double value;
                bool isDouble = double.TryParse(args.NewTextValue, out value);

                if (isDouble)
                {
                    bool isValid = ((value >= MinValue) && (value <= MaxValue));
                    Color currentTextColor = entry.TextColor;
                    entry.TextColor = isValid ? currentTextColor : (TextColor != null ? TextColor : Color.Red);
                }
            }
        }
    }
}
