using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    /// <summary>
    /// Validates the numeric value of the Entry to a minimum and maximum value
    /// </summary>
    public class MinMaxNumericValidationBehavior : BaseBehavior<Entry>
    {
        /// <summary>
        /// Bindable maximum value property
        /// </summary>
        public static readonly BindableProperty MaxValueProperty = BindableProperty.
            Create(nameof(MaxValue), typeof(double), typeof(MinMaxNumericValidationBehavior), double.MaxValue);

        /// <summary>
        /// Maximum value property to validate against
        /// </summary>
        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Bindable minimum value property
        /// </summary>
        public static readonly BindableProperty MinValueProperty = BindableProperty.
            Create(nameof(MinValue), typeof(double), typeof(MinMaxNumericValidationBehavior), double.MinValue);

        /// <summary>
        /// Minimum value property to validate against
        /// </summary>
        public int MinValue
        {
            get => (int)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        /// <summary>
        /// Bindable text color to apply when validation fails
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NumericValidationBehavior));

        /// <summary>
        /// Text color to apply when validation fails
        /// </summary>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        /// <summary>
        /// Attaches the OnEntryTextChanged handler to the TextChanged event
        /// </summary>
        /// <param name="bindable">Entry control to which the handler is attached</param>
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        /// <summary>
        /// Detaches the OnEntryTextChanged handler from the TextChanged event
        /// </summary>
        /// <param name="bindable">Entry control from which the handler is detached</param>
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        /// <summary>
        /// Handles the event when the text is changed on the Entry, 
        /// performs the validation if the Entry text is between the MinValue and MaxValue, 
        /// and sets the text color
        /// </summary>
        /// <param name="sender">Entry control</param>
        /// <param name="e">Text changed event arguments</param>
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
