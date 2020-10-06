using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<Bitmap> LoadImageAsync(ImageSource imagesource, Context context, CancellationToken cancelationToken = default)
		{
			var fileInfo = await LoadInternal(imagesource, 1, Application.Context.CacheDir.AbsolutePath);

			Bitmap bitmap = null;
			try
			{
				await semaphore.WaitAsync();

				if (fileInfo?.Exists ?? false)
					bitmap = await BitmapFactory.DecodeFileAsync(fileInfo.FullName);
			}
			finally
			{
				semaphore.Release();
			}

			return bitmap;
		}
	}
}