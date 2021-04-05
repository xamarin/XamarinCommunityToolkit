namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class NoLightDismissPopup
	{
		public NoLightDismissPopup() => InitializeComponent();

		void Button_Clicked(object? sender, System.EventArgs e) => Dismiss(null);
	}
}