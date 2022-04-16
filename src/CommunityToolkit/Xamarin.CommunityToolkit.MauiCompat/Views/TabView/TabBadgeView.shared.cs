using System;using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]
	public class TabBadgeView : TemplatedView
	{
		internal const string ElementBorder = "PART_Border";
		internal const string ElementText = "PART_Text";

		Frame? badgeBorder;
		Label? badgeText;
		bool isVisible;

		public TabBadgeView() => ControlTemplate = new ControlTemplate(typeof(TabBadgeTemplate));

		public static BindableProperty PlacementTargetProperty =
			BindableProperty.Create(nameof(PlacementTarget), typeof(View), typeof(TabBadgeView), null);

		public View? PlacementTarget
		{
			get => (View?)GetValue(PlacementTargetProperty);
			set => SetValue(PlacementTargetProperty, value);
		}

		public static BindableProperty AutoHideProperty =
			BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(TabBadgeView), defaultValue: true,
				propertyChanged: OnAutoHideChanged);

		public bool AutoHide
		{
			get => (bool)GetValue(AutoHideProperty);
			set => SetValue(AutoHideProperty, value);
		}

		static async void OnAutoHideChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var tabBadgeView = (TabBadgeView)bindable;
			await tabBadgeView.UpdateVisibilityAsync();
		}

		public static BindableProperty IsAnimatedProperty =
		   BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(TabBadgeView), defaultValue: true);

		public bool IsAnimated
		{
			get => (bool)GetValue(IsAnimatedProperty);
			set => SetValue(IsAnimatedProperty, value);
		}

		public static BindableProperty BadgeAnimationProperty =
			BindableProperty.Create(nameof(BadgeAnimation), typeof(IBadgeAnimation), typeof(TabBadgeView), new BadgeAnimation());

		public IBadgeAnimation? BadgeAnimation
		{
			get => (IBadgeAnimation?)GetValue(BadgeAnimationProperty);
			set => SetValue(BadgeAnimationProperty, value);
		}

		public static new BindableProperty BackgroundColorProperty =
			BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(TabBadgeView), defaultValue: new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnBackgroundColorChanged);

		public new Color BackgroundColor
		{
			get => (Color)GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		static void OnBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabBadgeView)?.UpdateBackgroundColor((Color)newValue);

		public static BindableProperty BorderColorProperty =
		  BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(TabBadgeView), defaultValue: new Microsoft.Maui.Graphics.Color(),
			  propertyChanged: OnBorderColorChanged);

		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabBadgeView)?.UpdateBorderColor((Color)newValue);

		public static BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TabBadgeView), defaultValue: new Microsoft.Maui.Graphics.Color(),
				propertyChanged: OnTextColorChanged);

		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabBadgeView)?.UpdateTextColor((Color)newValue);

		public static BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(TabBadgeView), defaultValue: "0",
				propertyChanged: OnTextChanged);

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		static void OnTextChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabBadgeView)?.UpdateText((string)newValue);

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			badgeBorder = (Frame)GetTemplateChild(ElementBorder);
			badgeText = (Label)GetTemplateChild(ElementText);

			UpdateSize();
			UpdatePosition(badgeBorder);
			UpdateIsEnabled(badgeText);
		}

		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == IsEnabledProperty.PropertyName && badgeText is Label label)
				UpdateIsEnabled(label);
		}

		void UpdateIsEnabled(in Label badgeText)
		{
			if (IsEnabled)
				badgeText.PropertyChanged += OnBadgeTextPropertyChanged;
			else
				badgeText.PropertyChanged -= OnBadgeTextPropertyChanged;
		}

		void OnBadgeTextPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName is nameof(Height) or nameof(Width) && badgeBorder is Frame frame)
			{
				UpdateSize();
				UpdatePosition(frame);
			}
		}

		void UpdateSize()
		{
			if (badgeBorder == null || badgeText == null || badgeText.Width <= 0 || badgeText.Height <= 0)
				return;

			var badgeTextHeight = badgeText.Height + (badgeBorder.Padding.VerticalThickness / 2);
			var badgeTextWidth = Math.Max(badgeText.Width + (badgeBorder.Padding.HorizontalThickness / 2), badgeTextHeight);

			badgeBorder.HeightRequest = badgeTextHeight;
			badgeBorder.WidthRequest = badgeTextWidth;

			badgeBorder.CornerRadius = (int)Math.Round(badgeTextHeight / 2);
		}

		void UpdatePosition(Frame badgeBorder)
		{
			if (PlacementTarget == null)
				return;

			var x = PlacementTarget.X - PlacementTarget.Margin.HorizontalThickness;

			if (Device.RuntimePlatform != Device.Android)
				x += PlacementTarget.Width;

			badgeBorder.Margin = new Thickness(x, 0, 0, 0);
		}

		void UpdateBackgroundColor(Color backgroundColor)
		{
			if (badgeBorder != null)
				badgeBorder.BackgroundColor = backgroundColor;
		}

		void UpdateBorderColor(Color borderColor)
		{
			if (badgeBorder != null)
				badgeBorder.BorderColor = borderColor;
		}

		void UpdateTextColor(Color textColor)
		{
			if (badgeText != null)
				badgeText.TextColor = textColor;
		}

		async void UpdateText(string text)
		{
			if (badgeText != null)
			{
				badgeText.Text = text;
				await UpdateVisibilityAsync();
			}
		}

		async Task UpdateVisibilityAsync()
		{
			var badgeText = this.badgeText?.Text;

			if (badgeText == null || string.IsNullOrEmpty(badgeText))
			{
				IsVisible = false;
				return;
			}

			var badgeIsVisible = !AutoHide || !badgeText.Trim().Equals("0");

			if (IsAnimated)
			{
				if (badgeIsVisible == isVisible)
					return;

				if (badgeIsVisible)
				{
					IsVisible = true;

					if (BadgeAnimation != null)
						await BadgeAnimation.OnAppearing(this);
				}
				else
				{
					if (BadgeAnimation != null)
						await BadgeAnimation.OnDisappering(this);

					IsVisible = false;
				}

				isVisible = badgeIsVisible;
			}
			else
				IsVisible = badgeIsVisible;
		}
	}
}