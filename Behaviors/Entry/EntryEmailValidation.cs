using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Behaviors
{
    /// <summary>
    /// Email validator behavior.
    /// </summary>
    public class EntryEmailValidation : BaseBehavior<Entry>
    {
        bool colorSet;
        Color color = Color.Default;


        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";


        static readonly BindablePropertyKey TextColorInvalidKey =
            BindableProperty.CreateReadOnly(nameof(TextColorInvalid), typeof(Color),
                                            typeof(EntryEmailValidation), Color.Default);

        /// <summary>
        /// The is valid property.
        /// </summary>
        public static readonly BindableProperty TextColorInvalidProperty =
            TextColorInvalidKey.BindableProperty;
        

        /// <summary>
        /// Gets or sets the text color invalid.
        /// </summary>
        /// <value>The text color invalid.</value>
        public Color TextColorInvalid
        {
            get { return (Color)GetValue(TextColorInvalidProperty); }
            set { SetValue(TextColorInvalidKey, value); }
        }

        static readonly BindablePropertyKey IsValidPropertyKey =
            BindableProperty.CreateReadOnly(nameof(IsValid), typeof(bool),
                                            typeof(EntryEmptyValidation), false);

        /// <summary>
        /// The is valid property.
        /// </summary>
        public static readonly BindableProperty IsValidProperty =
            IsValidPropertyKey.BindableProperty;

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        /// <param name="bindable">To be added.</param>
        /// <summary>
        /// Raises the attached to event.
        /// </summary>
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            HandleTextChanged(bindable, new TextChangedEventArgs(string.Empty, bindable.Text));
            base.OnAttachedTo(bindable);
        }

        /// <param name="bindable">To be added.</param>
        /// <summary>
        /// Raises the detaching from event.
        /// </summary>
        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e?.NewTextValue ?? string.Empty;
            IsValid = (Regex.IsMatch(text, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));

            var entry = sender as Entry;

            if (entry == null)
                return;
            
            if (!colorSet)
            {
                colorSet = true;
                color = entry.TextColor;
            }

            entry.TextColor = IsValid ? color : TextColorInvalid;
        }
    }
}
