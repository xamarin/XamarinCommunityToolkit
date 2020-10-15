using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

#if MONOANDROID
using Xamarin.Forms.Platform.Android;
#elif __IOS__
using Xamarin.Forms.Platform.iOS;
#else
using Xamarin.Forms.Platform.MacOS;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class ImageSourceValidator : IImageSourceValidator
	{
		public async Task<bool> IsImageSourceValid(ImageSource source)
		{
			IImageSourceHandler handler;

			switch (source)
			{
				case UriImageSource _:
					handler = new ImageLoaderSourceHandler();
					break;
				case FileImageSource _:
					handler = new FileImageSourceHandler();
					break;
				case StreamImageSource _:
					handler = new StreamImagesourceHandler();
					break;
				default:
					return false;
			}

#if MONOANDROID
			var imageSource = await handler.LoadImageAsync(source, null);
#else
			var imageSource = await handler.LoadImageAsync(source);
#endif
			return imageSource != null;
		}
	}
}