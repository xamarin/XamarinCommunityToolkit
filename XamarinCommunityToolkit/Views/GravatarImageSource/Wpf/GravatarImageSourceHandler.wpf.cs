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
			var fileInfo = await LoadInternal(imagesource, 1, appdata).ConfigureAwait(false);

			lock (locker)
			{
				if (fileInfo?.Exists ?? false)
				{
					var bitmapimage = new BitmapImage();
					bitmapimage.BeginInit();
					bitmapimage.StreamSource = fileInfo.OpenRead();
					bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapimage.EndInit();
					return bitmapimage;
				}
			}

			return null;
		}
	}
}