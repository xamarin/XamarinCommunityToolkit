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
			var fileInfo = await LoadInternal(imagesource, 1, GetCacheDirectory()).ConfigureAwait(false);

			lock (locker)
			{
				if (fileInfo?.Exists ?? false)
					return new NSImage(fileInfo.FullName);
			}

			return null;
		}
	}
}