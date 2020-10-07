using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Resx;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class ContentButtonPage : BasePage
	{
		public ContentButtonPage()
			=> InitializeComponent();

		async void ContentButton_Clicked(System.Object sender, System.EventArgs e)
		{
			var result = await this.DisplaySnackbar(AppResources.ContentButtonTapMessage, 3000, "Run action", () =>
			{
				Debug.WriteLine("Snackbar action button clicked");
				return Task.CompletedTask;
			});
		}
	}
}
