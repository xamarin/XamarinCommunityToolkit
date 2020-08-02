using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Toolkit.Xamarin.Forms.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// Adds a range of items to a <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">Collection where the items will be added to.</param>
        /// <param name="items">Items to add to the collection.</param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Sets a range of items in a <paramref name="collection"/> and clears existing items.
        /// </summary>
        /// <param name="collection">Collection where the items will be added to.</param>
        /// <param name="items">Items to set in the collection.</param>
        public static void SetRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.AddRange(items);
        }
    }
}