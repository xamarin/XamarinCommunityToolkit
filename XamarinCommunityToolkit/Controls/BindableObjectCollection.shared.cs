using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace XamarinCommunityToolkit.Controls
{
    [Preserve(AllMembers = true)]
    public class BindableObjectCollection : BindableObject, IEnumerable<BindableObject>, INotifyCollectionChanged
    {
        List<BindableObject> items = new List<BindableObject>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int IndexOf(BindableObject item)
            => items.IndexOf(item);

        public void Insert(int index, BindableObject item)
        {
            items.Insert(index, item);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public void RemoveAt(int index)
        {
            var oldItem = this[index];
            items.RemoveAt(index);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
        }

        public BindableObject this[int index]
        {
            get => this[index];
            set
            {
                var oldItem = this[index];
                this[index] = (BindableObject)value;
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
            }
        }

        public void Add(BindableObject item)
        {
            items.Add(item);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
        }

        public void Clear()
        {
            items.Clear();
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(BindableObject item)
            => items.Contains(item);

        public void CopyTo(BindableObject[] array, int arrayIndex)
            => items.CopyTo(array, arrayIndex);

        public bool Remove(BindableObject item)
        {
            var oldIndex = IndexOf(item);

            if (items.Remove(item))
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, oldIndex));
                return true;
            }

            return false;
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public IEnumerator<BindableObject> GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
