using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.Views.Popups;
using Xamarin.CommunityToolkit.Sample.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases.Popups
{
	public partial class GH_BUG_1761
	{
		public GH_BUG_1761() => InitializeComponent();

		private async void ButtonAB_Clicked(object sender, EventArgs e)
		{
			var popupA = new PopupThatReturnRandomString();
			var result = await Navigation.ShowPopupAsync(popupA);
			var popupB = new PopupThatDisplayString(result);
			Navigation.ShowPopup(popupB);
		}


		public class PopupThatReturnRandomString : Popup<string?>
		{
			protected override string GetLightDismissResult() => "Light Dismiss";

			void Button_Clicked(object? sender, System.EventArgs e) => Dismiss(Value);

			private string Value = string.Empty;
			public PopupThatReturnRandomString()
			{
				Value = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant();
				Size = new Size(200, 200);
				var sl = new StackLayout();
				var label = new Label() { Text = $"Generated value: {Value}" };
				var btn = new Button() { Text = $"DONE" };
				btn.Pressed += Button_Clicked;
				sl.Children.Add(label);
				sl.Children.Add(btn);
				Content = sl;
			}
		}

		public class PopupThatDisplayString : Popup
		{
			public PopupThatDisplayString(string? value)
			{
				Size = new Size(200, 200);
				var sl = new StackLayout();
				var btn = new Label() { Text = $"Received value: {value ?? "value is null"}" };
				sl.Children.Add(btn);
				Content = sl;
			}
		}
	}


	public class GH_BUG_1761_ViewModel : BaseViewModel
	{
		public GH_BUG_1761_ViewModel()
		{

		}

		INavigation Navigation => Application.Current.MainPage.Navigation;
	}
}