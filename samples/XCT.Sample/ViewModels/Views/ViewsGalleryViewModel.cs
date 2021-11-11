using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Views;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class ViewsGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(typeof(AvatarViewPage), "AvatarView",
				"The AvatarView represents a user's name by using the initials and a generated background color"),

			new SectionModel(typeof(BadgeViewPage), "BadgeView",
				"View used to notify users notifications, or status of something"),

			new SectionModel(typeof(DrawingViewPage), "DrawingView",
				"Draw & GO DrawingView makes capturing and displaying gestures extremely simple on all platforms that Xamarin.Forms supports. (Android, iOS, macOS, UWP, WPF, GTK and Tizen)"),

			new SectionModel(typeof(CameraViewPage), "CameraView",
				"The CameraView allows you to show a live preview from the camera. You can take pictures, record videos and much more!"),

			new SectionModel(typeof(DockLayoutPage), "DockLayout",
 				"Makes it easy to dock content in all four directions (top, bottom, left and right)"),

			new SectionModel(typeof(GravatarImagePage), "GravatarImageSource",
				"The GravatarImageSource allows you to easily utilize a users Gravatar image from Gravatar.com using nothing but their email address"),

			new SectionModel(typeof(ExpanderPage), "Expander",
				"The Expander control provides an expandable container to host any content"),

			new SectionModel(typeof(HexLayoutPage), "HexLayout",
				"A Layout that arranges the elements in a honeycomb pattern"),

			new SectionModel(typeof(MediaElementPage), "MediaElement",
				"MediaElement is a view for playing video and audio"),

			new SectionModel(typeof(RangeSliderPage), "RangeSlider",
				"The RangeSlider is a slider with two thumbs allowing to select numeric ranges"),

			new SectionModel(typeof(SemanticOrderViewPage), "SemanticOrderView",
				"Set accessiblity ordering on views"),

			new SectionModel(typeof(SnackBarPage), "SnackBar/Toast",
				"Show SnackBar, Toast etc"),

			new SectionModel(typeof(SideMenuViewPage), "SideMenuView",
				"SideMenuView is a simple and flexible Right/Left menu control"),

			new SectionModel(typeof(ShieldPage), "Shield",
				"Shields can show some status information or call-to-action in a badge-like way"),

			new SectionModel(typeof(StateLayoutPage), "StateLayout",
				"A collection of attached properties that let you specify one or more state views for any of your existing layouts."),

			new SectionModel(typeof(TabViewPage), "TabView",
				"A control to display a set of tabs and their respective content."),

			new SectionModel(typeof(UniformGridPage), "UniformGrid",
				"The UniformGrid is just like the Grid, with all rows and columns will have the same size."),

			new SectionModel(typeof(TextSwitcherPage), "TextSwitcher",
				"A TextSwitcher is useful to animate a label on screen. Whenever Text is updated, TextSwitcher animates the current text out and animates the new text in."),

			new SectionModel(typeof(PopupGalleryPage), "Popup",
				"The popup control renders native popups from the shared code. This page demonstrates a variety of different techniques for displaying native popups."),
			
			new SectionModel(typeof(SegmentedViewPage), "SegmentedView",
				"Segmented View allows you to have a simple segments control that can display text or images.")
		};
	}
}
