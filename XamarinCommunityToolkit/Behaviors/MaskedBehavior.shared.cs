using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
    public class MaskedBehavior : BaseBehavior<View>
    {
        IDictionary<int, char> positions;
        bool applyingMask;

        public static readonly BindableProperty MaskProperty =
            BindableProperty.Create(nameof(Mask), typeof(string), typeof(MaskedBehavior), propertyChanged: OnMaskPropertyChanged);

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

        static void OnMaskPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((MaskedBehavior)bindable).SetPositions();

        static void OnUnMaskedCharacterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((MaskedBehavior)bindable).OnMaskChanged();

        void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
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

        void OnMaskChanged()
        {
            var inputView = (InputView)View;

            if (string.IsNullOrEmpty(Mask))
            {
                positions = null;
                return;
            }

            var originalText = RemoveMask(inputView?.Text);
            SetPositions();

            if (inputView == null) return;

            var maskedText = ApplyMask(originalText);
            if (inputView.Text != maskedText)
                inputView.Text = maskedText;
        }

        string RemoveMask(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var maskChars = positions
                .Select(c => c.Value)
                .Distinct()
                .ToArray();

            return string.Join(string.Empty, text.Split(maskChars));
        }

        string ApplyMask(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || positions == null)
                return text;

            if (text.Length > Mask.Length)
            {
                text = text.Remove(text.Length - 1);
            }

            text = RemoveMask(text);
            foreach (var position in positions)
            {
                if (text.Length < position.Key + 1) continue;

                var value = position.Value.ToString();

                //!important - If user types in masked value, don't add masked value
                if (text.Substring(position.Key, 1) != value)
                    text = text.Insert(position.Key, value);
            }

            return text;
        }

        /// <inheritdoc />
        protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewPropertyChanged(sender, e);

            if (e.PropertyName != nameof(InputView.Text) || applyingMask)
                return;

            try
            {
                applyingMask = true;

                var inputView = (InputView)View;
                var maskedText = ApplyMask(inputView.Text);
                if (inputView.Text != maskedText)
                    inputView.Text = maskedText;
            }
            finally
            {
                applyingMask = false;
            }
        }
    }
}
