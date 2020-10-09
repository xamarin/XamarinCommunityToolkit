using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SegmentedView : View
	{
		public SegmentedView()
		{
		}

		public event EventHandler<SelectedItemChangedEventArgs> SelectedIndexChanged;
		public static BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(SegmentedView));
		public static BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SegmentedView), 0, propertyChanged: OnSegmentSelected);
		public static BindableProperty DisplayModeProperty = BindableProperty.Create(nameof(DisplayMode), typeof(SegmentMode), typeof(SegmentedView));

		public IList<string> Items { get; } = new LockableObservableListWrapper();

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SegmentedView), default(IList),
									propertyChanged: OnItemsSourceChanged);

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SegmentedView), null, BindingMode.TwoWay,
									propertyChanged: OnSelectedItemChanged);

		public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public SegmentMode DisplayMode
		{
			get { return (SegmentMode)GetValue(DisplayModeProperty); }
			set { SetValue(DisplayModeProperty, value); }
		}

		BindingBase _itemDisplayBinding;
		public BindingBase ItemDisplayBinding
		{
			get { return _itemDisplayBinding; }
			set
			{
				if (_itemDisplayBinding == value)
					return;

				OnPropertyChanging();
				_itemDisplayBinding = value;
				ResetItems();
				OnPropertyChanged();
			}
		}

		static readonly BindableProperty s_displayProperty =
			BindableProperty.Create("Display", typeof(string), typeof(SegmentedView), default(string));

		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((SegmentedView)bindable)?.OnItemsSourceChanged((IList)oldValue, (IList)newValue);
		}

		void OnItemsSourceChanged(IList oldValue, IList newValue)
		{
			var oldObservable = oldValue as INotifyCollectionChanged;
			if (oldObservable != null)
				oldObservable.CollectionChanged -= CollectionChanged;

			var newObservable = newValue as INotifyCollectionChanged;
			if (newObservable != null)
			{
				newObservable.CollectionChanged += CollectionChanged;
			}

			if (newValue != null)
				ResetItems();
		}

		void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					AddItems(e);
					break;
				case NotifyCollectionChangedAction.Remove:
					RemoveItems(e);
					break;
				default: //Move, Replace, Reset
					ResetItems();
					break;
			}
		}

		void AddItems(NotifyCollectionChangedEventArgs e)
		{
			var index = e.NewStartingIndex < 0 ? Items.Count : e.NewStartingIndex;
			foreach (var newItem in e.NewItems)
				((LockableObservableListWrapper)Items).InternalInsert(index++, GetDisplayMember(newItem));
		}

		void RemoveItems(NotifyCollectionChangedEventArgs e)
		{
			var index = e.OldStartingIndex < Items.Count ? e.OldStartingIndex : Items.Count;
			foreach (var _ in e.OldItems)
				((LockableObservableListWrapper)Items).InternalRemoveAt(index--);
		}

		void ResetItems()
		{
			if (ItemsSource == null)
				return;

			((LockableObservableListWrapper)Items).InternalClear();

			foreach (var item in ItemsSource)
				((LockableObservableListWrapper)Items).InternalAdd(GetDisplayMember(item));

			UpdateSelectedItem(SelectedIndex);
		}

		string GetDisplayMember(object item)
		{
			if (ItemDisplayBinding == null)
				return item.ToString();
			//TODO: Fix this
			//ItemDisplayBinding.Apply(item, this, s_displayProperty);
			//ItemDisplayBinding.Unapply();

			return (string)GetValue(s_displayProperty);
		}

		static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var segments = (SegmentedView)bindable;
			segments.SelectedItem = newValue;
		}

		void UpdateSelectedItem(int index)
		{
			if (index == -1)
			{
				SelectedItem = null;
				return;
			}

			if (ItemsSource != null)
			{
				SelectedItem = ItemsSource[index];
				return;
			}

			SelectedItem = Items[index];
		}

		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}

		private static void OnSegmentSelected(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is SegmentedView segment))
				return;
			int.TryParse(newValue?.ToString(), out var index);
			segment.SelectedIndexChanged?.Invoke(segment, new SelectedItemChangedEventArgs(segment?.Items[index], index));
			segment.SelectedItem = segment?.Items[index];
		}

		// IColorElement
		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set { SetValue(ColorProperty, value); }
		}

		public bool IsColorSet => IsSet(ColorProperty);
	}
}
