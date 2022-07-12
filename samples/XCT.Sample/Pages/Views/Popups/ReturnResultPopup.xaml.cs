namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class ReturnResultPopup
	{
		public ReturnResultPopup() =>
			InitializeComponent();

		protected override string GetLightDismissResult() => "Light Dismiss";

		void Button_Clicked(object? sender, System.EventArgs e) => Dismiss("Close button tapped");
	}
}