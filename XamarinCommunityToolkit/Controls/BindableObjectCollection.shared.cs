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
        List<BindableObject> Items = new List<BindableObject>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int IndexOf(BindableObject item)
        {
            return Items.IndexOf(item);
        }

        public void Insert(int index, BindableObject item)
        {
            Items.Insert(index, item);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public void RemoveAt(int index)
        {
            var oldItem = this[index];
            Items.RemoveAt(index);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
        }

        public BindableObject this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                var oldItem = this[index];
                this[index] = (BindableObject)value;
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem));
            }
        }

        public void Add(BindableObject item)
        {
            Items.Add(item);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1));
        }

        public void Clear()
        {
            Items.Clear();
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(BindableObject item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(BindableObject[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(BindableObject item)
        {
            var oldIndex = IndexOf(item);

            if (Items.Remove(item))
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, oldIndex));
                return true;
            }

            return false;
        }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<BindableObject> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
