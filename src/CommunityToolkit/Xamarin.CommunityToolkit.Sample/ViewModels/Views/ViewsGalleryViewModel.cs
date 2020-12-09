using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Views;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class ViewsGalleryViewModel : BaseGalleryViewModel
	{
		public override IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(typeof(AvatarViewPage), "AvatarView",
				"The AvatarView represents a user's name by using the initials and a generated background color"),

			new SectionModel(typeof(BadgeViewPage), "BadgeView",
				"View used to used to notify users notifications, or status of something"),

			new SectionModel(typeof(CameraViewPage), "CameraView",
				"The CameraView allows you to show a live preview from the camera. You can take pictures, record videos and much more!"),

			new SectionModel(typeof(DockLayoutPage), "DockLayout",
 				"Makes it easy to dock content in all four directions (top, bottom, left and right)"),

			new SectionModel(typeof(GravatarImagePage), "GravatarImageSource",
				"The GravatarImageSource allows you to easily utilize a users Gravatar image from Gravatar.com using nothing but their email address"),

			new SectionModel(typeof(ExpanderPage), "Expander",
				"The Expander control provides an expandable container to host any content"),

			new SectionModel(typeof(MediaElementPage), "MediaElement",
				"MediaElement is a view for playing video and audio"),

			new SectionModel(typeof(RangeSliderPage), "RangeSlider",
				"The RangeSlider is a slider with two thumbs allowing to select numeric ranges"),

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
				"The UniformGrid is just like the Grid, with all rows and columns will have the same size.")
		};
	}
}