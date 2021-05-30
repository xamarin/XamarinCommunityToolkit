using CommunityToolkit.Maui.UI.Views;

namespace CommunityToolkit.Maui.Sample.Pages.Views.Popups
{
	public partial class OpenedEventSimplePopup
	{
		public OpenedEventSimplePopup()
		{
			InitializeComponent();
			Opened += OnOpened;
		}

		void OnOpened(object? sender, PopupOpenedEventArgs e)
		{
			Opened -= OnOpened;

			Title.Text = "Opened Event Popup";
			Message.Text = "The content of this popup was updated after the popup was rendered";
		}
	}
}