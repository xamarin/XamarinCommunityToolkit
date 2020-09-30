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
			var fileInfo = await GravatarHandlerUtil.Load(imagesource, scale, GetCacheDirectory()).ConfigureAwait(false);

			if (fileInfo?.Exists ?? false)
				return UIImage.FromFile(fileInfo.FullName);

			return null;
		}
	}
}