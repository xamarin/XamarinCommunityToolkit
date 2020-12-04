using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public class ReturnResultPopup : Popup<string>
	{
		public ReturnResultPopup()
		{
			Content = new StackLayout
			{
				Children = { Message(), CloseButton() }
			};

			Button CloseButton()
			{
				var button = new Button
				{
					Text = "Close"
				};

				button.Clicked += (s, e) => Dismiss("Close button tapped");
				return button;
			}

			Label Message() =>
				new Label
				{
					Text = "Hello World"
				};
		}

		protected override string GetLightDismissResult() => "Light Dismiss";
	}
}
