using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class ActionsPage : BasePage
	{
		public ActionsPage() => InitializeComponent();

		async void Button1_Clicked(object sender, EventArgs args)
		{
			var result = await this.DisplaySnackbar(GenerateLongText(5), 3000, "Run action", () =>
			{
				Debug.WriteLine("Snackbar action button clicked");
				return Task.CompletedTask;
			});
			StatusText.Text = result ? AppResources.SnackbarIsClosedByUser : AppResources.SnackbarIsClosedByTimeout;
		}

		async void Button2_Clicked(object sender, EventArgs args)
		{
			var result = await this.DisplaySnackbar(GenerateLongText(5));
			StatusText.Text = result ? AppResources.SnackbarIsClosedByUser : AppResources.SnackbarIsClosedByTimeout;
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