namespace Xamarin.CommunityToolkit.UI.Views
{
	interface IViewSwitcher
	{
		uint TransitionDuration { get; set; }

		TransitionType TransitionType { get; set; }
	}
}