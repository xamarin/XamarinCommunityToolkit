using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    /// <summary>
    /// Validates if the text value of the Entry is numeric
    /// </summary>
    public class AttachedNumericValidationBehavior : BaseBehavior<View>
    {
        static Color currentTextColor;

        /// <summary>
        /// Bindable boolean to indicate whether or not to attach behavior
        /// </summary>
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

        /// <summary>
        /// Triggered when the AttachBehavior bindable boolean changes
        /// </summary>
        /// <param name="view">Entry control to attach to</param>
        /// <param name="oldValue">Old boolean value</param>
        /// <param name="newValue">New boolean value</param>
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

        //// <summary>
        /// Handles the event when the text is changed on the Entry, 
        /// performs the validation if the Entry text is numeric, and sets the text color
        /// </summary>
        /// <param name="sender">Entry control</param>
        /// <param name="args">Text changed event arguments</param>
        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var isValid = double.TryParse(args.NewTextValue, out _);
            Color textColor = GetTextColor((BindableObject)sender);

            if (sender is Entry entry)
                entry.TextColor = isValid ? currentTextColor : (textColor != null ? textColor : Color.Red);
        }
    }
}
