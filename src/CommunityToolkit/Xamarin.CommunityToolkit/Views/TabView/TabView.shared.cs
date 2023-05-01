using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[Preserve(AllMembers = true)]
	[ContentProperty(nameof(TabItems))]
	public class TabView : ContentView, IDisposable
	{
		const uint tabIndicatorAnimationDuration = 100;

		readonly Grid mainContainer;
		readonly Grid tabStripContainer;
		readonly Grid tabStripBackground;
		readonly BoxView tabStripBorder;
		readonly ScrollView tabStripContainerScroll;
		readonly Grid tabStripIndicator;
		readonly Grid tabStripContent;
		readonly Grid tabStripContentContainer;
		readonly CarouselView contentContainer;

		readonly List<double> contentWidthCollection;
		IList? tabItemsSource;
		ObservableCollection<TabViewItem>? contentTabItems;

		public TabView()
		{
			TabItems = new ObservableCollection<TabViewItem>();

			contentWidthCollection = new List<double>();

			BatchBegin();

			tabStripBackground = new Grid
			{
				BackgroundColor = TabStripBackgroundColor,
				HeightRequest = TabStripHeight,
				VerticalOptions = LayoutOptions.Start
			};

			tabStripBorder = new BoxView
			{
				Color = TabStripBorderColor,
				HeightRequest = 1,
				VerticalOptions = LayoutOptions.Start
			};

			tabStripBackground.Children.Add(tabStripBorder);

			tabStripIndicator = new Grid
			{
				BackgroundColor = TabIndicatorColor,
				HeightRequest = TabIndicatorHeight,
				HorizontalOptions = LayoutOptions.Start
			};

			UpdateTabIndicatorPlacement(TabIndicatorPlacement);

			tabStripContent = new Grid
			{
				BackgroundColor = Color.Transparent,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Start,
				ColumnSpacing = 0
			};

			tabStripContentContainer = new Grid
			{
				BackgroundColor = Color.Transparent,
				Children = { tabStripContent, tabStripIndicator },
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Start
			};

			tabStripContainerScroll = new ScrollView()
			{
				BackgroundColor = Color.Transparent,
				Orientation = ScrollOrientation.Horizontal,
				Content = tabStripContentContainer,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Start
			};

			if (Device.RuntimePlatform == Device.macOS || Device.RuntimePlatform == Device.UWP)
				tabStripContainerScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Default;

			tabStripContainer = new Grid
			{
				BackgroundColor = Color.Transparent,
				Children = { tabStripBackground, tabStripContainerScroll }
			};

			contentContainer = new CarouselView
			{
				BackgroundColor = Color.Transparent,
				ItemsSource = TabItems.Where(t => t.Content != null),
				ItemTemplate = new DataTemplate(() =>
				{
					var tabViewItemContent = new ContentView();
					tabViewItemContent.SetBinding(ContentProperty, "CurrentContent");
					return tabViewItemContent;
				}),
				IsSwipeEnabled = IsSwipeEnabled,
				IsScrollAnimated = IsTabTransitionEnabled,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
				VerticalScrollBarVisibility = ScrollBarVisibility.Never,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			// Workaround to fix a Xamarin.Forms CarouselView issue that create a wrong 1px margin.
			if (Device.RuntimePlatform == Device.iOS)
				contentContainer.Margin = new Thickness(-1, -1, 0, 0);

			mainContainer = new Grid
			{
				BackgroundColor = Color.Transparent,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = { contentContainer, tabStripContainer },
				RowSpacing = 0
			};

			mainContainer.RowDefinitions.Add(new RowDefinition { Height = TabStripHeight > 0 ? TabStripHeight : GridLength.Auto });
			mainContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			mainContainer.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

			Grid.SetRow(tabStripContainer, 0);
			Grid.SetRowSpan(tabStripContainer, 2);

			Grid.SetRow(contentContainer, 1);
			Grid.SetRowSpan(contentContainer, 2);

			Content = mainContainer;

			BatchCommit();

			DisableLoop();
			UpdateIsEnabled();
		}

		void DisableLoop()
		{
			// If TabView is used with Xamarin.Forms >= 5.0, the default value of the CarouselView Loop property is true,
			// whereas in TabView we are not yet ready to support it. Access the property and disable the loop.
			var loopProperty = contentContainer.GetType().GetProperty("Loop");

			if (loopProperty != null && loopProperty.CanWrite)
				loopProperty.SetValue(contentContainer, false, null);
		}

		public void Dispose()
		{
			if (contentContainer != null)
			{
				contentContainer.PropertyChanged -= OnContentContainerPropertyChanged;
				contentContainer.Scrolled -= OnContentContainerScrolled;
			}

			if (tabItemsSource is INotifyCollectionChanged notifyTabItemsSource)
				notifyTabItemsSource.CollectionChanged -= OnTabItemsSourceCollectionChanged;

			if (TabItems != null)
				TabItems.CollectionChanged -= OnTabItemsCollectionChanged;

			var lazyView = ((contentContainer?.CurrentItem as TabViewItem)?.Content as BaseLazyView) ?? (TabItems?[SelectedIndex].Content as BaseLazyView);
			lazyView?.Dispose();
		}

		public ObservableCollection<TabViewItem> TabItems { get; set; }

		public static readonly BindableProperty TabItemsSourceProperty =
			BindableProperty.Create(nameof(TabItemsSource), typeof(IList), typeof(TabView), null,
				propertyChanged: OnTabItemsSourceChanged);

		public IList? TabItemsSource
		{
			get => (IList?)GetValue(TabItemsSourceProperty);
			set => SetValue(TabItemsSourceProperty, value);
		}

		static void OnTabItemsSourceChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabItemsSource();

		public static readonly BindableProperty TabViewItemDataTemplateProperty =
			BindableProperty.Create(nameof(TabViewItemDataTemplate), typeof(DataTemplate), typeof(TabView), null);

		public DataTemplate? TabViewItemDataTemplate
		{
			get => (DataTemplate?)GetValue(TabViewItemDataTemplateProperty);
			set => SetValue(TabViewItemDataTemplateProperty, value);
		}

		public static readonly BindableProperty TabContentDataTemplateProperty =
		   BindableProperty.Create(nameof(TabContentDataTemplate), typeof(DataTemplate), typeof(TabView), null);

		public DataTemplate? TabContentDataTemplate
		{
			get => (DataTemplate?)GetValue(TabContentDataTemplateProperty);
			set => SetValue(TabContentDataTemplateProperty, value);
		}

		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TabView), -1, BindingMode.TwoWay,
				propertyChanged: OnSelectedIndexChanged);

		public int SelectedIndex
		{
			get => (int)GetValue(SelectedIndexProperty);
			set => SetValue(SelectedIndexProperty, value);
		}

		static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is TabView tabView && tabView.TabItems != null)
			{
				var selectedIndex = (int)newValue;

				if (selectedIndex < 0)
				{
					return;
				}
				if ((int)oldValue != selectedIndex)
					tabView.UpdateSelectedIndex(selectedIndex);
			}
		}

		public static readonly BindableProperty TabStripPlacementProperty =
			BindableProperty.Create(nameof(TabStripPlacement), typeof(TabStripPlacement), typeof(TabView), TabStripPlacement.Top,
				propertyChanged: OnTabStripPlacementChanged);

		public TabStripPlacement TabStripPlacement
		{
			get => (TabStripPlacement)GetValue(TabStripPlacementProperty);
			set => SetValue(TabStripPlacementProperty, value);
		}

		static void OnTabStripPlacementChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabStripPlacement((TabStripPlacement)newValue);

		public static readonly BindableProperty TabStripBackgroundColorProperty =
			BindableProperty.Create(nameof(TabStripBackgroundColor), typeof(Color), typeof(TabView), Color.Default,
				propertyChanged: OnTabStripBackgroundColorChanged);

		public Color TabStripBackgroundColor
		{
			get => (Color)GetValue(TabStripBackgroundColorProperty);
			set => SetValue(TabStripBackgroundColorProperty, value);
		}

		static void OnTabStripBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabStripBackgroundColor((Color)newValue);

		public static readonly BindableProperty TabStripBackgroundViewProperty =
		   BindableProperty.Create(nameof(TabStripBackgroundColor), typeof(View), typeof(TabView), null,
			   propertyChanged: OnTabStripBackgroundViewChanged);

		public View? TabStripBackgroundView
		{
			get => (View?)GetValue(TabStripBackgroundViewProperty);
			set => SetValue(TabStripBackgroundViewProperty, value);
		}

		static void OnTabStripBackgroundViewChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabStripBackgroundView((View)newValue);

		public static readonly BindableProperty TabStripBorderColorProperty =
			BindableProperty.Create(nameof(TabStripBorderColor), typeof(Color), typeof(TabView), Color.Default,
				propertyChanged: OnTabStripBorderColorChanged);

		public Color TabStripBorderColor
		{
			get => (Color)GetValue(TabStripBorderColorProperty);
			set => SetValue(TabStripBorderColorProperty, value);
		}

		static void OnTabStripBorderColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabStripBorderColor((Color)newValue);

		public static readonly BindableProperty TabContentBackgroundColorProperty =
			BindableProperty.Create(nameof(TabContentBackgroundColor), typeof(Color), typeof(TabView), Color.Default,
				propertyChanged: OnTabContentBackgroundColorChanged);

		public Color TabContentBackgroundColor
		{
			get => (Color)GetValue(TabContentBackgroundColorProperty);
			set => SetValue(TabContentBackgroundColorProperty, value);
		}

		static void OnTabContentBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabContentBackgroundColor((Color)newValue);

		public static readonly BindableProperty TabStripHeightProperty =
		  BindableProperty.Create(nameof(TabStripHeight), typeof(double), typeof(TabView), 48d,
			  propertyChanged: OnTabStripHeightChanged);

		public double TabStripHeight
		{
			get => (double)GetValue(TabStripHeightProperty);
			set => SetValue(TabStripHeightProperty, value);
		}

		static void OnTabStripHeightChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabStripHeight((double)newValue);

		public static readonly BindableProperty IsTabStripVisibleProperty =
		  BindableProperty.Create(nameof(IsTabStripVisible), typeof(bool), typeof(TabView), true,
			  propertyChanged: OnIsTabStripVisibleChanged);

		public bool IsTabStripVisible
		{
			get => (bool)GetValue(IsTabStripVisibleProperty);
			set => SetValue(IsTabStripVisibleProperty, value);
		}

		static void OnIsTabStripVisibleChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateIsTabStripVisible((bool)newValue);

		public static readonly BindableProperty TabContentHeightProperty =
			BindableProperty.Create(nameof(TabContentHeight), typeof(double), typeof(TabView), -1d,
			   propertyChanged: OnTabContentHeightChanged);

		public double TabContentHeight
		{
			get => (double)GetValue(TabContentHeightProperty);
			set => SetValue(TabContentHeightProperty, value);
		}

		static void OnTabContentHeightChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabContentHeight((double)newValue);

		public static readonly BindableProperty TabIndicatorColorProperty =
			BindableProperty.Create(nameof(TabIndicatorColor), typeof(Color), typeof(TabView), Color.Default,
			   propertyChanged: OnTabIndicatorColorChanged);

		public Color TabIndicatorColor
		{
			get => (Color)GetValue(TabIndicatorColorProperty);
			set => SetValue(TabIndicatorColorProperty, value);
		}

		static void OnTabIndicatorColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabIndicatorColor((Color)newValue);

		public static readonly BindableProperty TabIndicatorHeightProperty =
			BindableProperty.Create(nameof(TabIndicatorHeight), typeof(double), typeof(TabView), 3d,
				propertyChanged: OnTabIndicatorHeightChanged);

		public double TabIndicatorHeight
		{
			get => (double)GetValue(TabIndicatorHeightProperty);
			set => SetValue(TabIndicatorHeightProperty, value);
		}

		static void OnTabIndicatorHeightChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabIndicatorHeight((double)newValue);

		public static readonly BindableProperty TabIndicatorWidthProperty =
		   BindableProperty.Create(nameof(TabIndicatorWidth), typeof(double), typeof(TabView), default(double),
			   propertyChanged: OnTabIndicatorWidthChanged);

		public double TabIndicatorWidth
		{
			get => (double)GetValue(TabIndicatorWidthProperty);
			set => SetValue(TabIndicatorWidthProperty, value);
		}

		static void OnTabIndicatorWidthChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabIndicatorWidth((double)newValue);

		public static readonly BindableProperty TabIndicatorViewProperty =
			BindableProperty.Create(nameof(TabIndicatorView), typeof(View), typeof(TabView), null,
				propertyChanged: OnTabIndicatorViewChanged);

		public View? TabIndicatorView
		{
			get => (View?)GetValue(TabIndicatorViewProperty);
			set => SetValue(TabIndicatorViewProperty, value);
		}

		static void OnTabIndicatorViewChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabIndicatorView((View)newValue);

		public static readonly BindableProperty TabIndicatorPlacementProperty =
		  BindableProperty.Create(nameof(TabIndicatorPlacement), typeof(TabIndicatorPlacement), typeof(TabView), TabIndicatorPlacement.Bottom,
			 propertyChanged: OnTabIndicatorPlacementChanged);

		public TabIndicatorPlacement TabIndicatorPlacement
		{
			get => (TabIndicatorPlacement)GetValue(TabIndicatorPlacementProperty);
			set => SetValue(TabIndicatorPlacementProperty, value);
		}

		static void OnTabIndicatorPlacementChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateTabIndicatorPlacement((TabIndicatorPlacement)newValue);

		public static readonly BindableProperty IsTabTransitionEnabledProperty =
		   BindableProperty.Create(nameof(IsTabTransitionEnabled), typeof(bool), typeof(TabView), true,
			   propertyChanged: OnIsTabTransitionEnabledChanged);

		public bool IsTabTransitionEnabled
		{
			get => (bool)GetValue(IsTabTransitionEnabledProperty);
			set => SetValue(IsTabTransitionEnabledProperty, value);
		}

		static void OnIsTabTransitionEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateIsTabTransitionEnabled((bool)newValue);

		public static readonly BindableProperty IsSwipeEnabledProperty =
			BindableProperty.Create(nameof(IsSwipeEnabled), typeof(bool), typeof(TabView), true,
			   propertyChanged: OnIsSwipeEnabledChanged);

		public bool IsSwipeEnabled
		{
			get => (bool)GetValue(IsSwipeEnabledProperty);
			set => SetValue(IsSwipeEnabledProperty, value);
		}

		static void OnIsSwipeEnabledChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabView)?.UpdateIsSwipeEnabled((bool)newValue);

		public delegate void TabSelectionChangedEventHandler(object? sender, TabSelectionChangedEventArgs e);

		public event TabSelectionChangedEventHandler? SelectionChanged;

		public delegate void TabViewScrolledEventHandler(object? sender, ItemsViewScrolledEventArgs e);

		public event TabViewScrolledEventHandler? Scrolled;

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == IsEnabledProperty.PropertyName)
				UpdateIsEnabled();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (TabItems == null || TabItems.Count == 0)
				return;

			foreach (var tabViewItem in TabItems)
				UpdateTabViewItemBindingContext(tabViewItem);
		}

		void OnTabViewItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (sender is TabViewItem tabViewItem)
			{
				if (e.PropertyName == TabViewItem.TabWidthProperty.PropertyName)
					UpdateTabViewItemTabWidth(tabViewItem);
			}
		}

		void OnTabItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (var tabViewItem in e.OldItems.OfType<TabViewItem>())
				{
					ClearTabViewItem(tabViewItem);
				}
			}

			if (e.NewItems != null)
			{
				foreach (var tabViewItem in e.NewItems.OfType<TabViewItem>())
				{
					AddTabViewItem(tabViewItem, TabItems.IndexOf(tabViewItem));
				}
			}
		}

		void OnContentContainerPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(CarouselView.ItemsSource)
				|| e.PropertyName == nameof(CarouselView.VisibleViews))
			{
				var items = contentContainer.ItemsSource;

				UpdateItemsSource(items);
			}
			else if (e.PropertyName == nameof(CarouselView.Position))
			{
				var selectedIndex = contentContainer.Position;
				if (SelectedIndex != selectedIndex)
					UpdateSelectedIndex(selectedIndex, true);
			}
		}

		void OnContentContainerScrolled(object? sender, ItemsViewScrolledEventArgs args)
		{
			for (var i = 0; i < TabItems.Count; i++)
				TabItems[i].UpdateCurrentContent();

			UpdateTabIndicatorPosition(args);

			OnTabViewScrolled(args);
		}

		void ClearTabStrip()
		{
			foreach (var tabViewItem in TabItems)
				ClearTabViewItem(tabViewItem);

			if (tabStripContent.Children.Count > 0)
				tabStripContent.Children.Clear();

			tabStripContent.ColumnDefinitions.Clear();

			var hasItems = TabItems.Count > 0 || TabItemsSource?.Count > 0;
			tabStripContainer.IsVisible = hasItems;
		}

		void ClearTabViewItem(TabViewItem tabViewItem)
		{
			tabViewItem.PropertyChanged -= OnTabViewItemPropertyChanged;
			tabStripContent.Children.Remove(tabViewItem);
		}

		void AddTabViewItem(TabViewItem tabViewItem, int index = -1)
		{
			tabViewItem.PropertyChanged -= OnTabViewItemPropertyChanged;
			tabViewItem.PropertyChanged += OnTabViewItemPropertyChanged;

			if (tabViewItem.ControlTemplate == null)
			{
				if (Device.RuntimePlatform == Device.Android)
					tabViewItem.ControlTemplate = new ControlTemplate(typeof(MaterialTabViewItemTemplate));
				else if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.macOS)
					tabViewItem.ControlTemplate = new ControlTemplate(typeof(CupertinoTabViewItemTemplate));
				else if (Device.RuntimePlatform == Device.UWP)
					tabViewItem.ControlTemplate = new ControlTemplate(typeof(WindowsTabViewItemTemplate));
				else
				{
					// Default ControlTemplate for other platforms
					tabViewItem.ControlTemplate = new ControlTemplate(typeof(MaterialTabViewItemTemplate));
				}
			}

			AddSelectionTapRecognizer(tabViewItem);

			AddTabViewItemToTabStrip(tabViewItem, index);

			UpdateTabContentSize();
			UpdateTabStripSize();

			if (SelectedIndex != 0)
				UpdateSelectedIndex(0);
		}

		void UpdateTabStripSize()
		{
			var tabStripSize = tabStripContent.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);

			if (tabStripContainer.HeightRequest != tabStripSize.Request.Height)
				tabStripContainer.HeightRequest = tabStripSize.Request.Height;
		}

		void UpdateTabContentSize()
		{
			var items = contentContainer.ItemsSource;

			var count = 0;

			var enumerator = items.GetEnumerator();

			while (enumerator.MoveNext())
				count++;

			VerticalOptions = count != 0 ? LayoutOptions.FillAndExpand : LayoutOptions.Start;
			mainContainer.HeightRequest = count != 0 ? (TabContentHeight + TabStripHeight) : TabStripHeight;
			UpdateTabContentHeight(count != 0 ? TabContentHeight : 0);
		}

		void AddTabViewItemFromTemplate(object? item, int index = -1) => AddTabViewItemFromTemplateToTabStrip(item, index);

		void UpdateTabViewItemBindingContext(TabViewItem tabViewItem)
		{
			if (tabViewItem == null || tabViewItem.Content == null)
				return;

			tabViewItem.Content.BindingContext ??= BindingContext;
		}

		void AddSelectionTapRecognizer(View view)
		{
			var tapRecognizer = new TapGestureRecognizer();

			tapRecognizer.Tapped += (object? sender, EventArgs args) =>
			{
				if (sender is not View view)
					return;

				var capturedIndex = tabStripContent.Children.IndexOf(view);

				if (view is TabViewItem tabViewItem)
				{
					var tabTappedEventArgs = new TabTappedEventArgs(capturedIndex);
					tabViewItem.OnTabTapped(tabTappedEventArgs);
				}

				if (CanUpdateSelectedIndex(capturedIndex))
				{
					if (SelectedIndex != capturedIndex)
						UpdateSelectedIndex(capturedIndex);
				}
			};

			view.GestureRecognizers.Add(tapRecognizer);
		}

		void AddTabViewItemToTabStrip(View item, int index = -1)
		{
			var tabViewItemSizeRequest = item.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);

			if (tabViewItemSizeRequest.Request.Height < TabStripHeight)
			{
				item.HeightRequest = TabStripHeight;
				item.VerticalOptions = TabStripPlacement == TabStripPlacement.Top ? LayoutOptions.Start : LayoutOptions.End;
			}

			tabStripContent.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = (item is TabViewItem tabViewItem && tabViewItem.TabWidth > 0) ? tabViewItem.TabWidth : GridLength.Star
			});

			if (index >= 0)
			{
				tabStripContent.Children.Insert(index, item);

				for (var i = index; i < tabStripContent.Children.Count; i++)
					Grid.SetColumn(tabStripContent.Children[i], i);
			}
			else
			{
				tabStripContent.Children.Add(item);
				var count = tabStripContent.Children.Count - 1;
				item.SetValue(Grid.ColumnProperty, count);
			}

			UpdateTabViewItemTabWidth(item as TabViewItem);
		}

		void AddTabViewItemFromTemplateToTabStrip(object? item, int index = -1)
		{
			var view = TabViewItemDataTemplate is not DataTemplateSelector tabItemDataTemplate
						? (View)(TabViewItemDataTemplate?.CreateContent() ?? throw new NullReferenceException())
						: (View)tabItemDataTemplate.SelectTemplate(item, this).CreateContent();

			view.BindingContext = item;

			view.Effects.Add(new VisualFeedbackEffect());

			AddSelectionTapRecognizer(view);
			AddTabViewItemToTabStrip(view, index);
		}

		void UpdateIsEnabled()
		{
			if (IsEnabled)
			{
				contentContainer.PropertyChanged += OnContentContainerPropertyChanged;
				contentContainer.Scrolled += OnContentContainerScrolled;

				TabItems.CollectionChanged += OnTabItemsCollectionChanged;
			}
			else
			{
				contentContainer.PropertyChanged -= OnContentContainerPropertyChanged;
				contentContainer.Scrolled -= OnContentContainerScrolled;

				TabItems.CollectionChanged -= OnTabItemsCollectionChanged;
			}

			tabStripContent.IsEnabled = IsEnabled;
			contentContainer.IsEnabled = IsEnabled;
		}

		void UpdateTabViewItemTabWidth(TabViewItem? tabViewItem)
		{
			if (tabViewItem == null)
				return;

			var index = tabStripContent.Children.IndexOf(tabViewItem);
			var colummns = tabStripContent.ColumnDefinitions;

			ColumnDefinition? column = null;

			if (index < colummns.Count)
				column = colummns[index];

			if (column == null)
				return;

			column.Width = tabViewItem.TabWidth > 0 ? tabViewItem.TabWidth : GridLength.Star;
			UpdateTabIndicatorPosition(SelectedIndex);
		}

		void UpdateTabItemsSource()
		{
			if (TabItemsSource == null || TabViewItemDataTemplate == null)
				return;

			if (tabItemsSource is INotifyCollectionChanged oldnNotifyTabItemsSource)
				oldnNotifyTabItemsSource.CollectionChanged -= OnTabItemsSourceCollectionChanged;

			tabItemsSource = TabItemsSource;

			if (tabItemsSource is INotifyCollectionChanged newNotifyTabItemsSource)
				newNotifyTabItemsSource.CollectionChanged += OnTabItemsSourceCollectionChanged;

			ClearTabStrip();

			contentContainer.ItemTemplate = TabContentDataTemplate;
			contentContainer.ItemsSource = TabItemsSource;

			foreach (var item in TabItemsSource)
			{
				AddTabViewItemFromTemplate(item);
			}

			UpdateTabContentSize();
			UpdateTabStripSize();
			if (SelectedIndex != 0)
				UpdateSelectedIndex(0);
		}

		void OnTabItemsSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => UpdateTabItemsSource();

		void UpdateItemsSource(IEnumerable items)
		{
			contentWidthCollection.Clear();

			if (contentContainer.VisibleViews.Count == 0)
				return;

			var contentWidth = contentContainer.VisibleViews.FirstOrDefault().Width;
			var tabItemsCount = items.Cast<object>().Count();

			for (var i = 0; i < tabItemsCount; i++)
				contentWidthCollection.Add(contentWidth * i);
		}

		bool CanUpdateSelectedIndex(int selectedIndex)
		{
			if (TabItems == null || TabItems.Count == 0)
				return true;

			var tabItem = TabItems[selectedIndex];

			if (tabItem != null && tabItem.Content == null)
			{
				var itemsCount = TabItems.Count;
				var contentItemsCount = TabItems.Count(t => t.Content == null);

				return itemsCount == contentItemsCount;
			}

			return true;
		}

		void UpdateSelectedIndex(int position, bool hasCurrentItem = false)
		{
			if (position < 0)
				return;
			var oldposition = SelectedIndex;

			var newPosition = position;

			Device.BeginInvokeOnMainThread(async () =>
			{
				if (contentTabItems == null || contentTabItems.Count != TabItems.Count)
					contentTabItems = new ObservableCollection<TabViewItem>(TabItems.Where(t => t.Content != null));

				var contentIndex = position;
				var tabStripIndex = position;

				if (TabItems.Count > 0)
				{
					TabViewItem? currentItem = null;

					if (hasCurrentItem)
						currentItem = (TabViewItem)contentContainer.CurrentItem;

					var tabViewItem = TabItems[position];

					var lazyView = (currentItem?.Content as BaseLazyView) ?? (tabViewItem.Content as BaseLazyView);

					contentIndex = contentTabItems.IndexOf(currentItem ?? tabViewItem);
					tabStripIndex = TabItems.IndexOf(currentItem ?? tabViewItem);

					position = tabStripIndex;

					for (var index = 0; index < TabItems.Count; index++)
					{
						if (index == position)
							TabItems[position].IsSelected = true;
						else
							TabItems[index].IsSelected = false;
					}

					if (lazyView != null && !lazyView.IsLoaded)
						await lazyView.LoadViewAsync();

					var currentTabItem = TabItems[position];
					currentTabItem.SizeChanged += OnCurrentTabItemSizeChanged;
					UpdateTabIndicatorPosition(currentTabItem);
				}
				else
					UpdateTabIndicatorPosition(position);

				if (contentIndex >= 0)
					contentContainer.Position = contentIndex;

				if (tabStripContent.Children.Count > 0)
					await tabStripContainerScroll.ScrollToAsync(tabStripContent.Children[tabStripIndex], ScrollToPosition.MakeVisible, false);

				SelectedIndex = position;
				if (oldposition != SelectedIndex)
				{
					var selectionChangedArgs = new TabSelectionChangedEventArgs()
					{
						NewPosition = newPosition,
						OldPosition = oldposition
					};

					OnTabSelectionChanged(selectionChangedArgs);
				}
			});
		}

		void OnCurrentTabItemSizeChanged(object? sender, EventArgs e)
		{
			if (sender is not View view)
				return;

			var currentTabItem = view;
			UpdateTabIndicatorWidth(TabIndicatorWidth > 0 ? TabIndicatorWidth : currentTabItem.Width);
			UpdateTabIndicatorPosition(currentTabItem);
			currentTabItem.SizeChanged -= OnCurrentTabItemSizeChanged;
		}

		void UpdateTabStripPlacement(TabStripPlacement tabStripPlacement)
		{
			if (tabStripPlacement == TabStripPlacement.Top)
			{
				tabStripBackground.VerticalOptions = LayoutOptions.Start;

				Grid.SetRow(tabStripContainer, 0);
				Grid.SetRowSpan(tabStripContainer, 2);

				mainContainer.RowDefinitions[0].Height = TabStripHeight > 0 ? TabStripHeight : GridLength.Auto;
				mainContainer.RowDefinitions[1].Height = GridLength.Auto;
				mainContainer.RowDefinitions[2].Height = GridLength.Star;

				tabStripBorder.VerticalOptions = LayoutOptions.End;
			}

			if (tabStripPlacement == TabStripPlacement.Bottom)
			{
				tabStripBackground.VerticalOptions = LayoutOptions.End;

				Grid.SetRow(tabStripContainer, 1);
				Grid.SetRowSpan(tabStripContainer, 2);

				mainContainer.RowDefinitions[0].Height = GridLength.Star;
				mainContainer.RowDefinitions[1].Height = GridLength.Auto;
				mainContainer.RowDefinitions[2].Height = TabStripHeight > 0 ? TabStripHeight : GridLength.Auto;

				tabStripBorder.VerticalOptions = LayoutOptions.Start;
			}

			UpdateTabContentLayout();
			UpdateTabIndicatorMargin();
		}

		void UpdateTabContentLayout()
		{
			if (tabStripContainer.IsVisible)
			{
				if (TabStripPlacement == TabStripPlacement.Top)
				{
					Grid.SetRow(contentContainer, 1);
					Grid.SetRowSpan(contentContainer, 2);
				}
				else
				{
					Grid.SetRow(contentContainer, 0);
					Grid.SetRowSpan(contentContainer, 2);
				}
			}
			else
			{
				Grid.SetRow(contentContainer, 0);
				Grid.SetRowSpan(contentContainer, 3);
			}

			if (TabStripBackgroundView != null)
			{
				var tabStripBackgroundViewHasCornerRadius =
					(TabStripBackgroundView is IBorderElement borderElement && borderElement.CornerRadius != default) ||
					(TabStripBackgroundView is BoxView boxView && boxView.CornerRadius != default);

				if (tabStripBackgroundViewHasCornerRadius)
				{
					Grid.SetRow(contentContainer, 0);
					Grid.SetRowSpan(contentContainer, 3);
				}
			}
		}

		void UpdateTabStripBackgroundColor(Color tabStripBackgroundColor)
		{
			tabStripBackground.BackgroundColor = tabStripBackgroundColor;

			if (Device.RuntimePlatform == Device.macOS)
				tabStripContainerScroll.BackgroundColor = tabStripBackgroundColor;
		}

		void UpdateTabStripBackgroundView(View tabStripBackgroundView)
		{
			if (tabStripBackgroundView != null)
				tabStripBackground.Children.Add(tabStripBackgroundView);
			else
				tabStripBackground.Children.Clear();

			UpdateTabContentLayout();
		}

		void UpdateTabStripBorderColor(Color tabStripBorderColor)
		{
			tabStripBorder.Color = tabStripBorderColor;

			UpdateTabIndicatorMargin();
		}

		void UpdateTabIndicatorMargin()
		{
			if (TabStripBorderColor == Color.Default)
				return;

			if (TabStripPlacement == TabStripPlacement.Top && TabIndicatorPlacement == TabIndicatorPlacement.Bottom)
				tabStripIndicator.Margin = new Thickness(0, 0, 0, 1);

			if (TabStripPlacement == TabStripPlacement.Bottom && TabIndicatorPlacement == TabIndicatorPlacement.Top)
				tabStripIndicator.Margin = new Thickness(0, 1, 0, 0);
		}

		void UpdateTabContentBackgroundColor(Color tabContentBackgroundColor) => contentContainer.BackgroundColor = tabContentBackgroundColor;

		void UpdateTabStripHeight(double tabStripHeight) => tabStripBackground.HeightRequest = tabStripHeight;

		void UpdateIsTabStripVisible(bool isTabStripVisible)
		{
			tabStripContainer.IsVisible = isTabStripVisible;

			UpdateTabContentLayout();
		}

		void UpdateTabContentHeight(double tabContentHeight) => contentContainer.HeightRequest = tabContentHeight;

		void UpdateTabIndicatorColor(Color tabIndicatorColor)
		{
			if (tabStripIndicator != null)
				tabStripIndicator.BackgroundColor = tabIndicatorColor;
		}

		void UpdateTabIndicatorHeight(double tabIndicatorHeight)
		{
			if (tabStripIndicator != null)
			{
				tabStripIndicator.HeightRequest = tabIndicatorHeight;
			}
		}

		void UpdateTabIndicatorWidth(double tabIndicatorWidth)
		{
			if (tabStripIndicator != null)
			{
				tabStripIndicator.WidthRequest = TabIndicatorWidth > 0 ? TabIndicatorWidth : tabIndicatorWidth;
			}
		}

		void UpdateTabIndicatorView(View tabIndicatorView)
		{
			if (tabIndicatorView != null)
				tabStripIndicator.Children.Add(tabIndicatorView);
			else
				tabStripIndicator.Children.Clear();
		}

		void UpdateTabIndicatorPlacement(TabIndicatorPlacement tabIndicatorPlacement)
		{
			switch (tabIndicatorPlacement)
			{
				case TabIndicatorPlacement.Top:
					tabStripIndicator.VerticalOptions = LayoutOptions.Start;
					break;
				case TabIndicatorPlacement.Center:
					tabStripIndicator.VerticalOptions = LayoutOptions.Center;
					break;
				case TabIndicatorPlacement.Bottom:
				default:
					tabStripIndicator.VerticalOptions = LayoutOptions.End;
					break;
			}

			UpdateTabIndicatorMargin();
		}

		void UpdateIsSwipeEnabled(bool isSwipeEnabled)
		{
			if (contentContainer != null)
			{
				contentContainer.IsSwipeEnabled = isSwipeEnabled;
			}
		}

		void UpdateIsTabTransitionEnabled(bool isTabTransitionEnabled)
		{
			if (contentContainer != null)
			{
				contentContainer.IsScrollAnimated = isTabTransitionEnabled;
			}
		}

		void UpdateTabIndicatorPosition(int tabViewItemIndex)
		{
			if (tabStripContent == null || tabStripContent.Children.Count == 0 || tabViewItemIndex == -1)
				return;

			var currentTabViewItem = tabStripContent.Children[tabViewItemIndex];

			if (currentTabViewItem.Width <= 0)
				currentTabViewItem.SizeChanged += OnCurrentTabItemSizeChanged;
			else
				UpdateTabIndicatorWidth(currentTabViewItem.Width);

			UpdateTabIndicatorPosition(currentTabViewItem);
		}

		void UpdateTabIndicatorPosition(ItemsViewScrolledEventArgs args)
		{
			if (args.HorizontalOffset == 0)
			{
				UpdateTabIndicatorPosition(SelectedIndex);
				return;
			}

			if (tabStripContent == null || TabItems.Count == 0)
				return;

			if (contentWidthCollection.Count == 0)
				UpdateItemsSource(contentContainer.ItemsSource);

			var offset = args.HorizontalOffset;
			var toRight = args.HorizontalDelta > 0;

			var nextIndex = toRight ? contentWidthCollection.FindIndex(c => c > offset) : contentWidthCollection.FindLastIndex(c => c < offset);
			var previousIndex = toRight ? nextIndex - 1 : nextIndex + 1;

			if (previousIndex < 0 || nextIndex < 0)
				return;

			var itemsCount = TabItems.Count;

			if (previousIndex >= 0 && previousIndex < itemsCount)
			{
				var currentTabViewItem = TabItems[previousIndex];
				var currentTabViewItemWidth = currentTabViewItem.Width;

				UpdateTabIndicatorWidth(currentTabViewItemWidth);

				var contentItemsCount = contentWidthCollection.Count;

				if (previousIndex >= 0 && previousIndex < contentItemsCount)
				{
					var progress = (offset - contentWidthCollection[previousIndex]) / (contentWidthCollection[nextIndex] - contentWidthCollection[previousIndex]);
					var position = toRight ? currentTabViewItem.X + (currentTabViewItemWidth * progress) : currentTabViewItem.X - (currentTabViewItemWidth * progress);
				}
			}
		}

		void UpdateTabIndicatorPosition(View currentTabViewItem)
		{
			var width = TabIndicatorWidth > 0 ? (currentTabViewItem.Width - tabStripIndicator.Width) : 0;
			var position = currentTabViewItem.X + (width / 2) - 1;
			tabStripIndicator.TranslateTo(position, 0, tabIndicatorAnimationDuration, Easing.Linear);
		}

		internal virtual void OnTabSelectionChanged(TabSelectionChangedEventArgs e)
		{
			var handler = SelectionChanged;
			handler?.Invoke(this, e);
		}

		internal virtual void OnTabViewScrolled(ItemsViewScrolledEventArgs e)
		{
			var handler = Scrolled;
			handler?.Invoke(this, e);
		}
	}
}
