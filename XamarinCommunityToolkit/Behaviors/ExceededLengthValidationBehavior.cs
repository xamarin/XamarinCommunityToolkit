using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class ExceededLengthValidationBehavior : BaseBehavior<Entry>
    {
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.
            Create(nameof(MaxLength), typeof(int), typeof(ExceededLengthValidationBehavior));

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
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
            bool isValid = (e.NewTextValue.Length < MaxLength);
            ((Entry)sender).TextColor = isValid ? Color.Default : (TextColor != null ? TextColor : Color.Red);
        }
    }
}
