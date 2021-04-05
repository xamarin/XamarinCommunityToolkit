using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SegmentedView : View
	{
		readonly WeakEventManager<SelectedItemChangedEventArgs> eventManager = new WeakEventManager<SelectedItemChangedEventArgs>();

		/// <summary>
		/// Invoked with the <see cref="SelectedIndex"/> changes
		/// </summary>
		public event EventHandler<SelectedItemChangedEventArgs> SelectedIndexChanged
		{
			add => eventManager.AddEventHandler(value);
			remove => eventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="Color"/> property.
		/// </summary>
		public static BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(SegmentedView));

		/// <summary>
		/// Backing BindableProperty for the <see cref="SelectedTextColor"/> property.
		/// </summary>
		public static BindableProperty SelectedTextColorProperty = BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(SegmentedView));

		/// <summary>
		/// Backing BindableProperty for the <see cref="NormalTextColor"/> property.
		/// </summary>
		public static BindableProperty NormalTextColorProperty = BindableProperty.Create(nameof(NormalTextColor), typeof(Color), typeof(SegmentedView));

		/// <summary>
		/// Backing BindableProperty for the <see cref="SelectedIndex"/> property.
		/// </summary>
		public static BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(SegmentedView), 0, propertyChanged: OnSegmentSelected);

		/// <summary>
		/// Backing BindableProperty for the <see cref="DisplayMode"/> property.
		/// </summary>
		public static BindableProperty DisplayModeProperty = BindableProperty.Create(nameof(DisplayMode), typeof(SegmentMode), typeof(SegmentedView));

		/// <summary>
		/// Segment items.
		/// </summary>
		internal IEnumerable<string> Items { get; } = new LockableObservableListWrapper();

		/// <summary>
		/// Backing BindableProperty for the <see cref="ItemsSource"/> property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SegmentedView), default(IEnumerable),
									propertyChanged: OnItemsSourceChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="SelectedItem"/> property.
		/// </summary>
		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SegmentedView), null, BindingMode.TwoWay,
									propertyChanged: OnSelectedItemChanged);

		public IEnumerable<object> ItemsSource
		{
			get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public object? SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public SegmentMode DisplayMode
		{
			get => (SegmentMode)GetValue(DisplayModeProperty);
			set => SetValue(DisplayModeProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="CornerRadius"/> property.
		/// </summary>
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SegmentedView), new CornerRadius(6.0));

		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		BindingBase? itemDisplayBinding;

		public BindingBase? ItemDisplayBinding
		{
			get => itemDisplayBinding;
			set
			{
				if (itemDisplayBinding == value)
					return;

				OnPropertyChanging();
				itemDisplayBinding = value;
				ResetItems();
				OnPropertyChanged();
			}
		}

		static readonly BindableProperty displayProperty =
			BindableProperty.Create("Display", typeof(string), typeof(SegmentedView), default(string));

		string GetDisplayMember(object? item)
		{
			if (ItemDisplayBinding == null)
				return item?.ToString() ?? string.Empty;

			return (string)GetValue(displayProperty);
		}

		static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) =>
			((SegmentedView)bindable)?.OnItemsSourceChanged((IEnumerable)oldValue, (IEnumerable)newValue);

		void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			if (oldValue is INotifyCollectionChanged oldObservable)
				oldObservable.CollectionChanged -= CollectionChanged;

			if (newValue is INotifyCollectionChanged newObservable)
				newObservable.CollectionChanged += CollectionChanged;

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
				default:
					ResetItems();
					break;
			}
		}

		void AddItems(NotifyCollectionChangedEventArgs e)
		{
			var index = e.NewStartingIndex < 0 ? Items.Count() : e.NewStartingIndex;
			foreach (var newItem in e.NewItems)
				((LockableObservableListWrapper)Items).InternalInsert(index++, GetDisplayMember(newItem));
		}

		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1312:Variable names should begin with lower-case letter", Justification = "Discard object")]
		void RemoveItems(NotifyCollectionChangedEventArgs e)
		{
			var itemsCount = Items.Count();

			var index = e.OldStartingIndex < itemsCount ? e.OldStartingIndex : itemsCount;

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
				SelectedItem = ItemsSource.ElementAt(index);
				return;
			}

			SelectedItem = Items.ElementAt(index);
		}

		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}

		static void OnSegmentSelected(BindableObject bindable, object oldValue, object newValue)
		{
			var segment = (SegmentedView)bindable;

			if (!int.TryParse(newValue?.ToString(), out var index))
				index = 0;

			segment.OnSelectedIndexChanged(segment, index);
			segment.SelectedItem = segment?.Items.ElementAt(index);
		}

		protected void OnSelectedIndexChanged(object segment, int index) =>
			eventManager.RaiseEvent(this, new SelectedItemChangedEventArgs(segment, index), nameof(SelectedIndexChanged));

		// IColorElement
		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		public Color NormalTextColor
		{
			get => (Color)GetValue(NormalTextColorProperty);
			set => SetValue(NormalTextColorProperty, value);
		}

		public Color SelectedTextColor
		{
			get => (Color)GetValue(SelectedTextColorProperty);
			set => SetValue(SelectedTextColorProperty, value);
		}

		public bool IsColorSet => IsSet(ColorProperty);

		public bool IsSelectedTextColorSet => IsSet(SelectedTextColorProperty);

		public bool IsNormalTextColorSet => IsSet(NormalTextColorProperty);
	}
}
