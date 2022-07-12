namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class ButtonPopup
	{
		public ButtonPopup() => InitializeComponent();

		void Button_Clicked(object? sender, System.EventArgs e) => Dismiss(null);
	}
}