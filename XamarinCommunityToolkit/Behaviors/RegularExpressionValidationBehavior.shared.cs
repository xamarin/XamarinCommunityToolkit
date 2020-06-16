using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    /// <summary>
    /// Validates if the Entry text matches the regular expression rule
    /// </summary>
    public class RegularExpressionValidationBehavior : BaseBehavior<Entry>
    {
        Color currentTextColor;

        /// <summary>
        /// Bindable regular expression string to validate the Entry text against
        /// </summary>
        public static readonly BindableProperty RegularExpressionProperty =
            BindableProperty.Create(nameof(RegularExpression), typeof(string), typeof(RegularExpressionValidationBehavior));

        /// <summary>
        /// Regular expression string to validate the Entry text against
        /// </summary>
        public string RegularExpression
        {
            get => (string)GetValue(RegularExpressionProperty);
            set => SetValue(RegularExpressionProperty, value);
        }

        /// <summary>
        /// Bindable text color to apply when validation fails
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NumericValidationBehavior));

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
            currentTextColor = entry.TextColor;

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
        /// performs the validation against the regular expression string, and sets the text color
        /// </summary>
        /// <param name="sender">Entry control</param>
        /// <param name="e">Text changed event arguments</param>
        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var isValid = Regex.IsMatch(e.NewTextValue, RegularExpression);

            if (sender is Entry entry)
                entry.TextColor = isValid ? currentTextColor : (TextColor != null ? TextColor : Color.Red);
        }
    }
}
