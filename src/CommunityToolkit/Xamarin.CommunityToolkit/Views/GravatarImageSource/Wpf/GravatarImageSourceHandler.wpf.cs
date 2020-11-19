using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xamarin.Forms.Platform.WPF;
using FormsImageSource = Xamarin.Forms.ImageSource;
using SystemImageSource = System.Windows.Media.ImageSource;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<SystemImageSource> LoadImageAsync(FormsImageSource imagesource, CancellationToken cancellationToken = default)
		{
			var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var fileInfo = await LoadInternal(imagesource, 1, appdata);

			BitmapImage bitmap = null;
			try
			{
				await semaphore.WaitAsync();

				if (fileInfo?.Exists ?? false)
				{
					bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.StreamSource = fileInfo.OpenRead();
					bitmap.CacheOption = BitmapCacheOption.OnLoad;
					bitmap.EndInit();
					return bitmap;
				}
			}
			finally
			{
				semaphore.Release();
			}

			return bitmap;
		}
	}
}