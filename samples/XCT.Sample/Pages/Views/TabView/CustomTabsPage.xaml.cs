using CommunityToolkit.Maui.UI.Views;

namespace CommunityToolkit.Maui.Sample.Pages.Views.TabView
{
	public partial class CustomTabsPage : BasePage
	{
		public CustomTabsPage() => InitializeComponent();

		void OnFabTabTapped(object? sender, TabTappedEventArgs e) => DisplayAlert("FabTabGallery", "Tab Tapped.", "Ok");
	}
}