using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class MaskedBehavior : Behavior<Entry>
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

        void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
            {
                if (Mask[i] != 'X')
                {
                    list.Add(i, Mask[i]);
                }
            }

            positions = list;
        }

        void OnEntryTextChanged(object sender, EventArgs args)
        {
            var entry = sender as Entry;

            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text) || positions == null)
            {
                return;
            }

            if (text.Length > mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in positions)
            {
                if (text.Length < position.Key + 1) continue;

                var value = position.Value.ToString();

                //!important - If user types in masked value, don't masked value
                if (text.Substring(position.Key, 1) != value)
                {
                    text = text.Insert(position.Key, value);
                }
            }

            entry.Text = text;
        }
    }
}
