using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[Preserve(AllMembers = true)]
	public class TabBadgeTemplate : Grid
	{
		public TabBadgeTemplate()
		{
			BatchBegin();

			HorizontalOptions = LayoutOptions.Start;
			VerticalOptions = LayoutOptions.Start;

			var badgeBorder = new Frame
			{
				HasShadow = false,
				IsClippedToBounds = false,
				Padding = 2,
				Margin = 6
			};

			var badgeText = new Label
			{
				FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			badgeBorder.SetBinding(BackgroundColorProperty, new Binding("BackgroundColor", source: RelativeBindingSource.TemplatedParent));
			badgeBorder.SetBinding(Frame.BorderColorProperty, new Binding("BorderColor", source: RelativeBindingSource.TemplatedParent));

			badgeText.BatchBegin();

			badgeText.SetBinding(Label.TextProperty, new Binding("Text", source: RelativeBindingSource.TemplatedParent));
			badgeText.SetBinding(Label.TextColorProperty, new Binding("TextColor", source: RelativeBindingSource.TemplatedParent));

			badgeBorder.Content = badgeText;

			Children.Add(badgeBorder);

			badgeText.BatchCommit();

			BatchCommit();

			INameScope nameScope = new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName(TabBadgeView.ElementBorder, badgeBorder);
			nameScope.RegisterName(TabBadgeView.ElementText, badgeText);
		}
	}
}