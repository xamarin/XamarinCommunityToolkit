using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Octokit;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class ByteArrayToImageSourceViewModel : BaseViewModel
	{
		readonly GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("XamarinCommunityToolkitSample"));

		byte[] avatar;

		public byte[] Avatar
		{
			get => avatar;
			set => SetProperty(ref avatar, value);
		}

		bool isBusy;

		public bool IsBusy
		{
			get => isBusy;
			set => SetProperty(ref isBusy, value);
		}

		public async Task OnAppearing()
		{
			try
			{
				IsBusy = true;

				var contributors = await gitHubClient.Repository.GetAllContributors("xamarin", "XamarinCommunityToolkit");
				var avatarUrl = contributors?.FirstOrDefault(c => c.Login == "almirvuk")?.AvatarUrl;

				if (avatarUrl == null)
					return;

				// Needed to produce some kind of byte array for sample testing
				using var client = new HttpClient();
				using var response = await client.GetAsync(avatarUrl);

				if (!response.IsSuccessStatusCode)
					return;

				var imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

				Avatar = imageBytes;
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}