using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class MaskedBehavior : BaseBehavior
    {
        IDictionary<int, char> positions;

        public static readonly BindableProperty MaskProperty =
            BindableProperty.Create(nameof(Mask), typeof(string), typeof(MaskedBehavior), string.Empty, propertyChanged: OnMaskPropertyChanged);

        public static readonly BindableProperty UnMaskedCharacterProperty =
            BindableProperty.Create(nameof(UnMaskedCharacter), typeof(char), typeof(MaskedBehavior), 'X', propertyChanged: OnUnMaskedCharacterPropertyChanged);

        public string Mask
        {
            get => (string)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }
        public char UnMaskedCharacter
        {
            get => (char)GetValue(UnMaskedCharacterProperty);
            set => SetValue(UnMaskedCharacterProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            var inputView = bindable as InputView;
            inputView.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }
        protected override void OnDetachingFrom(View bindable)
        {
            var inputView = bindable as InputView;
            inputView.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        static void OnMaskPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((MaskedBehavior)bindable).SetPositions();
        static void OnUnMaskedCharacterPropertyChanged(BindableObject bindable, object oldValue, object newValue) => ((MaskedBehavior)bindable).SetPositions();

        void SetPositions()
        {
            if (string.IsNullOrEmpty((string)GetValue(MaskProperty)))
            {
                positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
                if (Mask[i] != UnMaskedCharacter)
                    list.Add(i, Mask[i]);

            positions = list;
        }

        void OnEntryTextChanged(object sender, EventArgs args)
        {
            var inputView = sender as InputView;
            var text = inputView.Text;

            if (string.IsNullOrWhiteSpace(text) || positions == null) return;

            if (text.Length > Mask.Length)
            {
                inputView.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in positions)
            {
                if (text.Length < position.Key + 1) continue;

                var value = position.Value.ToString();

                //!important - If user types in masked value, don't add masked value
                if (text.Substring(position.Key, 1) != value)
                    text = text.Insert(position.Key, value);
            }

            inputView.Text = text;
        }
    }
}
