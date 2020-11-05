using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

#if MONOANDROID
using Xamarin.Forms.Platform.Android;
#elif __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __MACOS__
using Xamarin.Forms.Platform.MacOS;
#elif UWP
using Xamarin.Forms.Platform.UWP;
#elif NET471
using Xamarin.Forms.Platform.GTK.Renderers;
#elif TIZEN
using Xamarin.Forms.Platform.Tizen;
using NImage = Xamarin.Forms.Platform.Tizen.Native.Image;
using XForms = Xamarin.Forms.Forms;
#else
using Xamarin.Forms.Platform.WPF;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
#if MONOANDROID || __IOS__ || __MACOS__
	using UriImageSourceHandler = ImageLoaderSourceHandler;
#endif

#if MONOANDROID || __IOS__ || __MACOS__ || NET471
	using StreamImageSourceHandler = StreamImagesourceHandler;
#endif

	class ImageSourceValidator : IImageSourceValidator
	{
		public async Task<bool> IsImageSourceValidAsync(ImageSource source)
		{
			var handler = GetHandler(source);
			if (handler == null)
				return false;

#if TIZEN
			return await handler.LoadImageAsync(new NImage(XForms.NativeParent), source).ConfigureAwait(false);
#elif MONOANDROID
			var imageSource = await handler.LoadImageAsync(source, null).ConfigureAwait(false);
			return imageSource != null;
#else
			var imageSource = await handler.LoadImageAsync(source).ConfigureAwait(false);
			return imageSource != null;
#endif
		}

		IImageSourceHandler GetHandler(ImageSource source)
		{
			if (source is UriImageSource)
				return new UriImageSourceHandler();

			if (source is StreamImageSource)
				return new StreamImageSourceHandler();

#if !NET471
			if (source is GravatarImageSource)
				return new GravatarImageSourceHandler();
#endif

#if !TIZEN
			if (source is FontImageSource)
				return new FontImageSourceHandler();
#endif

			if (source is FileImageSource fileSource && File.Exists(fileSource.File))
				return new FileImageSourceHandler();

			return null;
		}
	}
}