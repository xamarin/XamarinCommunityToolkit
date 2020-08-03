using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class MaskedBehavior : BaseBehavior
    {
        string mask = "";
        IDictionary<int, char> positions;

        public string Mask
        {
            get => mask;
            set
            {
                mask = value;
                SetPositions();
            }
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

        void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
                if (Mask[i] != 'X')
                    list.Add(i, Mask[i]);

            positions = list;
        }

        void OnEntryTextChanged(object sender, EventArgs args)
        {
            var inputView = sender as InputView;
            var text = inputView.Text;

            if (string.IsNullOrWhiteSpace(text) || positions == null) return;

            if (text.Length > mask.Length)
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
