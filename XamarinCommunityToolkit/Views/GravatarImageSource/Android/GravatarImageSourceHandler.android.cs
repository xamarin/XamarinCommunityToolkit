using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<Bitmap> LoadImageAsync(ImageSource imagesource, Context context, CancellationToken cancelationToken = default)
		{
			var fileInfo = await GravatarHandlerUtil.Load(imagesource, 1, Application.Context.CacheDir.AbsolutePath).ConfigureAwait(false);

			if (fileInfo?.Exists ?? false)
				return await BitmapFactory.DecodeFileAsync(fileInfo.FullName);

			return null;
		}
	}
}