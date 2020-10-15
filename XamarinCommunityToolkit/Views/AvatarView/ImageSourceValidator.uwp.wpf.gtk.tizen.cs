using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
#if UWP
using Xamarin.Forms.Platform.UWP;
#elif NET471
using Xamarin.Forms.Platform.GTK.Renderers;
#elif TIZEN
using Xamarin.Forms.Platform.Tizen;
#else
using Xamarin.Forms.Platform.WPF;
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
					handler = new UriImageSourceHandler();
					break;
				case FileImageSource f:
					if (!File.Exists(f.File))
					{
						return false;
					}

					handler = new FileImageSourceHandler();
					break;
				case StreamImageSource _:
#if NET471
					handler = new StreamImagesourceHandler();
#else
					handler = new StreamImageSourceHandler();
#endif
					break;
				default:
					return false;
			}

#if TIZEN
			var imageSource = await handler.LoadImageAsync(null, source) ? true : (bool?)null;
#else
			var imageSource = await handler.LoadImageAsync(source);
#endif
			return imageSource != null;
		}
	}
}