using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class AttachedNumericValidationBehavior : BaseBehavior<View>
    {
        static Color currentTextColor;

        public static readonly BindableProperty AttachBehaviorProperty =
            BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(AttachedNumericValidationBehavior), false, propertyChanged: OnAttachBehaviorChanged);

        /// <summary>
        /// Bindable text color to apply when validation fails
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(NumericValidationBehavior));
        
        public static bool GetAttachBehavior(BindableObject view)
            => (bool)view.GetValue(AttachBehaviorProperty);

        public static void SetAttachBehavior(BindableObject view, bool value)
            => view.SetValue(AttachBehaviorProperty, value);

        public static Color GetTextColor(BindableObject view)
            => (Color)view.GetValue(TextColorProperty);

        public static void SetTextColor(BindableObject view, Color color)
            => view.SetValue(TextColorProperty, color);

        private static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            var entry = view as Entry;

            if (entry == null)
                return;

            var attachBehavior = (bool)newValue;

            if (attachBehavior)
            {
                currentTextColor = entry.TextColor;
                entry.TextChanged += OnEntryTextChanged;
            }
            else
            {
                entry.TextChanged -= OnEntryTextChanged;
            }
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var isValid = double.TryParse(args.NewTextValue, out _);
            Color textColor = GetTextColor((BindableObject)sender);

            if (sender is Entry entry)
                entry.TextColor = isValid ? currentTextColor : (textColor != null ? textColor : Color.Red);
        }
    }
}
