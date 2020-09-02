using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	using System.Diagnostics;
	using System.Threading.Tasks;

	public class SnackBarPage : ContentPage
	{
		readonly Label labelResult;

		public SnackBarPage()
		{
			var button1 = new Button
			{
				Text = "Show snackbar with action button"
			};

			button1.Clicked += Button1_Clicked;

			var button2 = new Button
			{
				Text = "Show snackbar (no action button)"
			};

			button2.Clicked += Button2_Clicked;

			labelResult = new Label();

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = { button1, button2, labelResult }
			};
		}

		async void Button1_Clicked(object sender, EventArgs args)
		{
			var result = await this.DisplaySnackbar(GenerateLongText(5), 3000, "Run action", () =>
			{
				Debug.WriteLine("Snackbar action button clicked");
				return Task.CompletedTask;
			});
			labelResult.Text = result ? "Snackbar is closed by user" : "Snackbar is closed by timeout";
		}

		async void Button2_Clicked(object sender, EventArgs args)
		{
			var result = await this.DisplaySnackbar(GenerateLongText(5));
			labelResult.Text = result ? "Snackbar is closed by user" : "Snackbar is closed by timeout";
		}

		string GenerateLongText(int stringDuplicationTimes)
		{
			const string snackbarMessage = "It is a very long Snackbar mesage to test multiple strings. A B C D E F G H I I J K LO P Q R S T U V W X Y Z";
			var result = new StringBuilder();
			for (var i = 0; i < stringDuplicationTimes; i++)
			{
				result.AppendLine(snackbarMessage);
			}

			return result.ToString();
		}
	}
}