using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public static class AttachedNumericValidationBehavior
    {
        public static readonly BindableProperty AttachBehaviorProperty =
            BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(AttachedNumericValidationBehavior), false, propertyChanged: OnAttachBehaviorChanged);

        public static bool GetAttachBehavior(BindableObject view) => (bool)view.GetValue(AttachBehaviorProperty);

        public static void SetAttachBehavior(BindableObject view, bool value) => view.SetValue(AttachBehaviorProperty, value);

        static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            var entry = view as Entry;

            if (entry == null)
            {
                return;
            }

            var attachBehavior = (bool)newValue;

            if (attachBehavior)
            {
                entry.TextChanged += OnEntryTextChanged;
            }
            else
            {
                entry.TextChanged -= OnEntryTextChanged;
            }
        }

        static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            var isValid = double.TryParse(args.NewTextValue, out result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
