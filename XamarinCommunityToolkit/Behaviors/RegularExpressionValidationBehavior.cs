using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class RegularExpressionValidationBehavior : BaseBehavior<Entry>
    {
        public static readonly BindableProperty RegularExpressionProperty = 
            BindableProperty.Create(nameof(RegularExpression), typeof(string), typeof(RegularExpressionValidationBehavior));

        public string RegularExpression
        {
            get => (string)GetValue(RegularExpressionProperty);
            set => SetValue(RegularExpressionProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = 
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(NumericValidationBehavior));

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

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = false;
            isValid = (Regex.IsMatch(e.NewTextValue, RegularExpression));
            ((Entry)sender).TextColor = isValid ? Color.Default : (TextColor != null ? TextColor : Color.Red);
        }
    }
}
