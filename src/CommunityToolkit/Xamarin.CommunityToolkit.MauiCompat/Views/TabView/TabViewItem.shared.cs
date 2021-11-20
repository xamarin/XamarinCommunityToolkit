using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]
	[ContentProperty(nameof(Content))]
	public class TabViewItem : TemplatedView
	{
		public const string SelectedVisualState = "Selected";
		public const string UnselectedVisualState = "Unselected";

		bool isOnScreen;

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(TabViewItem), string.Empty);

		public string? Text
		{
			get => (string?)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTabViewItemPropertyChanged);

		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		public static readonly BindableProperty TextColorSelectedProperty =
			BindableProperty.Create(nameof(TextColorSelected), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color());

		public Color TextColorSelected
		{
			get => (Color)GetValue(TextColorSelectedProperty);
			set => SetValue(TextColorSelectedProperty, value);
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TabViewItem), Device.GetNamedSize(NamedSize.Small, typeof(Label)),
				propertyChanged: OnTabViewItemPropertyChanged);

		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		public static readonly BindableProperty FontSizeSelectedProperty =
			BindableProperty.Create(nameof(FontSizeSelected), typeof(double), typeof(TabViewItem), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

		public double FontSizeSelected
		{
			get => (double)GetValue(FontSizeSelectedProperty);
			set => SetValue(FontSizeSelectedProperty, value);
		}

		public static readonly BindableProperty FontFamilyProperty =
			BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(TabViewItem), string.Empty,
				propertyChanged: OnTabViewItemPropertyChanged);

		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		public static readonly BindableProperty FontFamilySelectedProperty =
			BindableProperty.Create(nameof(FontFamilySelected), typeof(string), typeof(TabViewItem), string.Empty,
				propertyChanged: OnTabViewItemPropertyChanged);

		public string FontFamilySelected
		{
			get => (string)GetValue(FontFamilySelectedProperty);
			set => SetValue(FontFamilySelectedProperty, value);
		}

		public static readonly BindableProperty FontAttributesProperty =
			BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(TabViewItem), FontAttributes.None,
				propertyChanged: OnTabViewItemPropertyChanged);

		public FontAttributes FontAttributes
		{
			get => (FontAttributes)GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		public static readonly BindableProperty FontAttributesSelectedProperty =
			BindableProperty.Create(nameof(FontAttributesSelected), typeof(FontAttributes), typeof(TabViewItem), FontAttributes.None,
				propertyChanged: OnTabViewItemPropertyChanged);

		public FontAttributes FontAttributesSelected
		{
			get => (FontAttributes)GetValue(FontAttributesSelectedProperty);
			set => SetValue(FontAttributesSelectedProperty, value);
		}

		public static readonly BindableProperty ContentProperty =
			BindableProperty.Create(nameof(Content), typeof(View), typeof(TabViewItem));

		public View? Content
		{
			get => (View?)GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		public static readonly BindableProperty IconProperty =
		  BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(TabViewItem), null,
			  propertyChanged: OnTabViewItemPropertyChanged);

		public ImageSource? Icon
		{
			get => (ImageSource?)GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}

		public static readonly BindableProperty IconSelectedProperty =
		  BindableProperty.Create(nameof(IconSelected), typeof(ImageSource), typeof(TabViewItem), null,
			  propertyChanged: OnTabViewItemPropertyChanged);

		public ImageSource? IconSelected
		{
			get => (ImageSource?)GetValue(IconSelectedProperty);
			set => SetValue(IconSelectedProperty, value);
		}

		public static readonly BindableProperty IsSelectedProperty =
			BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TabViewItem), false,
				propertyChanged: OnIsSelectedChanged);

		public bool IsSelected
		{
			get => (bool)GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}

		static async void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is TabViewItem tabViewItem)
			{
				tabViewItem.UpdateCurrent();
				await tabViewItem.UpdateTabAnimationAsync();
			}
		}

		public static readonly BindableProperty BadgeTextProperty =
		   BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(TabViewItem), string.Empty);

		public double TabWidth
		{
			get => (double)GetValue(TabWidthProperty);
			set => SetValue(TabWidthProperty, value);
		}

		public static readonly BindableProperty TabWidthProperty =
			BindableProperty.Create(nameof(TabWidth), typeof(double), typeof(TabViewItem), -1d);

		public static BindableProperty TabAnimationProperty =
			BindableProperty.Create(nameof(TabAnimation), typeof(ITabViewItemAnimation), typeof(TabViewItem), null);

		public ITabViewItemAnimation? TabAnimation
		{
			get => (ITabViewItemAnimation?)GetValue(TabAnimationProperty);
			set => SetValue(TabAnimationProperty, value);
		}

		public string BadgeText
		{
			get => (string)GetValue(BadgeTextProperty);
			set => SetValue(BadgeTextProperty, value);
		}

		public static readonly BindableProperty BadgeTextColorProperty =
			BindableProperty.Create(nameof(BadgeTextColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color());

		public Color BadgeTextColor
		{
			get => (Color)GetValue(BadgeTextColorProperty);
			set => SetValue(BadgeTextColorProperty, value);
		}

		public static readonly BindableProperty BadgeBackgroundColorProperty =
			BindableProperty.Create(nameof(BadgeBackgroundColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTabViewItemPropertyChanged);

		public Color BadgeBackgroundColor
		{
			get => (Color)GetValue(BadgeBackgroundColorProperty);
			set => SetValue(BadgeBackgroundColorProperty, value);
		}

		public static readonly BindableProperty BadgeBackgroundColorSelectedProperty =
			BindableProperty.Create(nameof(BadgeBackgroundColorSelected), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTabViewItemPropertyChanged);

		public Color BadgeBackgroundColorSelected
		{
			get => (Color)GetValue(BadgeBackgroundColorSelectedProperty);
			set => SetValue(BadgeBackgroundColorSelectedProperty, value);
		}

		public static readonly BindableProperty BadgeBorderColorProperty =
		   BindableProperty.Create(nameof(BadgeBorderColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color(),
			   propertyChanged: OnTabViewItemPropertyChanged);

		public Color BadgeBorderColor
		{
			get => (Color)GetValue(BadgeBorderColorProperty);
			set => SetValue(BadgeBorderColorProperty, value);
		}

		public static readonly BindableProperty BadgeBorderColorSelectedProperty =
			BindableProperty.Create(nameof(BadgeBorderColorSelected), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTabViewItemPropertyChanged);

		public Color BadgeBorderColorSelected
		{
			get => (Color)GetValue(BadgeBorderColorSelectedProperty);
			set => SetValue(BadgeBorderColorSelectedProperty, value);
		}

		public static readonly BindableProperty TapCommandProperty =
		   BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(TabViewItem), null);

		public ICommand TapCommand
		{
			get => (ICommand)GetValue(TapCommandProperty);
			set => SetValue(TapCommandProperty, value);
		}

		static void OnTabViewItemPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabViewItem)?.UpdateCurrent();

		internal static readonly BindablePropertyKey CurrentTextColorPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentTextColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color());

		public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

		public Color CurrentTextColor
		{
			get => (Color)GetValue(CurrentTextColorProperty);
			private set => SetValue(CurrentTextColorPropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentFontSizePropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentFontSize), typeof(double), typeof(TabViewItem), null);

		public static readonly BindableProperty CurrentFontSizeProperty = CurrentFontSizePropertyKey.BindableProperty;

		public double CurrentFontSize
		{
			get => (double)GetValue(CurrentFontSizeProperty);
			private set => SetValue(CurrentFontSizePropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentIconPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentIcon), typeof(ImageSource), typeof(TabViewItem), null);

		public static readonly BindableProperty CurrentIconProperty = CurrentIconPropertyKey.BindableProperty;

		public ImageSource? CurrentIcon
		{
			get => (ImageSource?)GetValue(CurrentIconProperty);
			private set => SetValue(CurrentIconPropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentFontFamilyPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentFontFamily), typeof(string), typeof(TabViewItem), string.Empty);

		public static readonly BindableProperty CurrentFontFamilyProperty = CurrentFontFamilyPropertyKey.BindableProperty;

		public string CurrentFontFamily
		{
			get => (string)GetValue(CurrentFontFamilyProperty);
			private set => SetValue(CurrentFontFamilyPropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentFontAttributesPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentFontAttributes), typeof(FontAttributes), typeof(TabViewItem), FontAttributes.None);

		public static readonly BindableProperty CurrentFontAttributesProperty = CurrentFontAttributesPropertyKey.BindableProperty;

		public FontAttributes CurrentFontAttributes
		{
			get => (FontAttributes)GetValue(CurrentFontAttributesProperty);
			private set => SetValue(CurrentFontAttributesPropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentBadgeBackgroundColorPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentBadgeBackgroundColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color());

		public static readonly BindableProperty CurrentBadgeBackgroundColorProperty = CurrentBadgeBackgroundColorPropertyKey.BindableProperty;

		public Color CurrentBadgeBackgroundColor
		{
			get => (Color)GetValue(CurrentBadgeBackgroundColorProperty);
			private set => SetValue(CurrentBadgeBackgroundColorPropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentBadgeBorderColorPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentBadgeBorderColor), typeof(Color), typeof(TabViewItem), new Microsoft.Maui.Graphics.Color());

		public static readonly BindableProperty CurrentBadgeBorderColorProperty = CurrentBadgeBorderColorPropertyKey.BindableProperty;

		public Color CurrentBadgeBorderColor
		{
			get => (Color)GetValue(CurrentBadgeBorderColorProperty);
			private set => SetValue(CurrentBadgeBorderColorPropertyKey, value);
		}

		internal static readonly BindablePropertyKey CurrentContentPropertyKey = BindableProperty.CreateReadOnly(nameof(CurrentContent), typeof(View), typeof(TabViewItem), null);

		public static readonly BindableProperty CurrentContentProperty = CurrentContentPropertyKey.BindableProperty;

		public View? CurrentContent
		{
			get => (View?)GetValue(CurrentContentProperty);
			private set => SetValue(CurrentContentPropertyKey, value);
		}

		public delegate void TabTappedEventHandler(object? sender, TabTappedEventArgs e);

		public event TabTappedEventHandler? TabTapped;

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == "Renderer")
				UpdateCurrent();
		}

		internal virtual void OnTabTapped(TabTappedEventArgs e)
		{
			if (IsEnabled)
			{
				var handler = TabTapped;
				handler?.Invoke(this, e);

				if (TapCommand != null)
					TapCommand.Execute(null);
			}
		}

		internal void UpdateCurrentContent(bool isOnScreen = true)
		{
			this.isOnScreen = isOnScreen;
			var newCurrentContent = this.isOnScreen ? Content : null;

			if (newCurrentContent != CurrentContent)
				CurrentContent = newCurrentContent;
		}

		void UpdateCurrent()
		{
			CurrentTextColor = !IsSelected || TextColorSelected == new Microsoft.Maui.Graphics.Color() ? TextColor : TextColorSelected;
			CurrentFontSize = !IsSelected || FontSizeSelected == FontSize ? FontSize : FontSizeSelected;
			CurrentIcon = !IsSelected || IconSelected == null ? Icon : IconSelected;
			CurrentFontFamily = !IsSelected || string.IsNullOrEmpty(FontFamilySelected) ? FontFamily : FontFamilySelected;
			CurrentFontAttributes = !IsSelected || FontAttributesSelected == FontAttributes.None ? FontAttributes : FontAttributesSelected;
			CurrentBadgeBackgroundColor = !IsSelected || BadgeBackgroundColorSelected == new Microsoft.Maui.Graphics.Color() ? BadgeBackgroundColor : BadgeBackgroundColorSelected;
			CurrentBadgeBorderColor = !IsSelected || BadgeBorderColorSelected == new Microsoft.Maui.Graphics.Color() ? BadgeBorderColor : BadgeBorderColorSelected;

			UpdateCurrentContent();
			ApplyIsSelectedState();
		}

		async Task UpdateTabAnimationAsync()
		{
			if (TabAnimation == null)
				return;

			var tabViewItemChildrens = this.GetChildren();

			if (tabViewItemChildrens.Count == 0 || !(tabViewItemChildrens[0] is View view))
				return;

			if (IsSelected)
				await TabAnimation.OnSelected(view);
			else
				await TabAnimation.OnDeSelected(view);
		}

		void ApplyIsSelectedState()
		{
			if (IsSelected)
				VisualStateManager.GoToState(this, SelectedVisualState);
			else
				VisualStateManager.GoToState(this, UnselectedVisualState);
		}
	}
}