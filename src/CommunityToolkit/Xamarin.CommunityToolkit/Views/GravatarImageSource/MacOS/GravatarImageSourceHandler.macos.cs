using System.Threading;
using System.Threading.Tasks;
using AppKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<NSImage> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = default, float scale = 1)
		{
			var fileInfo = await LoadInternal(imagesource, 1, GetCacheDirectory());

			NSImage image = null;
			try
			{
				await semaphore.WaitAsync();

				if (fileInfo?.Exists ?? false)
					image = new NSImage(fileInfo.FullName);
			}
			finally
			{
				semaphore.Release();
			}

			return image;
		}
	}
}