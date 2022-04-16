using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]
	class CupertinoTabViewItemTemplate : Microsoft.Maui.Controls.Compatibility.Grid
	{
		readonly VisualFeedbackEffect visualFeedback;

		readonly Image icon;
		readonly Label text;
		readonly TabBadgeView badge;

		public CupertinoTabViewItemTemplate()
		{
			visualFeedback = new VisualFeedbackEffect();
			Effects.Add(visualFeedback);

			RowSpacing = 0;

			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;

			RowDefinitions.Add(new RowDefinition { Height = Microsoft.Maui.GridLength.Star });
			RowDefinitions.Add(new RowDefinition { Height = Microsoft.Maui.GridLength.Auto });

			icon = new Image
			{
				Aspect = Aspect.AspectFit,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(0, 6)
			};

			text = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(0, 6)
			};

			badge = new TabBadgeView
			{
				PlacementTarget = icon,
				Margin = new Thickness(0)
			};

			Children.Add(icon);
			Children.Add(text);
			Children.Add(badge);

			SetRow(icon, 0);
			SetRow(text, 1);
			SetRow(badge, 0);
			SetRowSpan(badge, 2);
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();

			BindingContext = Parent;

			icon.SetBinding(Image.SourceProperty, "CurrentIcon");

			text.SetBinding(Label.TextProperty, "Text");
			text.SetBinding(Label.TextColorProperty, "CurrentTextColor");
			text.SetBinding(Label.FontSizeProperty, "CurrentFontSize");
			text.SetBinding(Label.FontAttributesProperty, "CurrentFontAttributes");

			badge.SetBinding(TabBadgeView.BackgroundColorProperty, "CurrentBadgeBackgroundColor");
			badge.SetBinding(TabBadgeView.BorderColorProperty, "CurrentBadgeBorderColor");
			badge.SetBinding(TabBadgeView.TextProperty, "BadgeText");
			badge.SetBinding(TabBadgeView.TextColorProperty, "BadgeTextColor");

			VisualFeedbackEffect.SetFeedbackColor(this, Colors.White);
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			UpdateLayout();
		}

		void UpdateLayout()
		{
			if (!(BindingContext is TabViewItem tabViewItem))
				return;

			if (tabViewItem.CurrentIcon == null)
			{
				SetRow(text, 0);
				SetRowSpan(text, 2);
			}
			else
			{
				SetRow(text, 1);
				SetRowSpan(text, 1);
			}
		}
	}
}