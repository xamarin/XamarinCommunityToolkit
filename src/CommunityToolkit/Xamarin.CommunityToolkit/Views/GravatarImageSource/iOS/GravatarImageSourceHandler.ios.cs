using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<UIImage> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = default, float scale = 1)
		{
			var fileInfo = await LoadInternal(imagesource, scale, GetCacheDirectory());

			UIImage image = null;
			try
			{
				await semaphore.WaitAsync();

				if (fileInfo?.Exists ?? false)
					image = UIImage.FromFile(fileInfo.FullName);
			}
			finally
			{
				semaphore.Release();
			}

			return image;
		}
	}
}