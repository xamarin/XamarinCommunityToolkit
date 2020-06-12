using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    /// <summary>
    /// Validates if the text value of the Entry is numeric
    /// </summary>
    public class NumericValidationBehavior : BaseBehavior<Entry>
    {
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
        /// performs the validation if the Entry text is numeric, and sets the text color
        /// </summary>
        /// <param name="sender">Entry control</param>
        /// <param name="e">Text changed event arguments</param>
        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            var isValid = double.TryParse(args.NewTextValue, out result);

            if (sender is Entry entry)
            {
                Color currentTextColor = entry.TextColor;
                entry.TextColor = isValid ? currentTextColor : (TextColor != null ? TextColor : Color.Red);
            }
        }
    }
}
