using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Xamarin.CommunityToolkit.Helpers;

#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using UriImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.Android.ImageLoaderSourceHandler;
using StreamImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.Android.StreamImagesourceHandler;
#elif __IOS__
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UriImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.iOS.ImageLoaderSourceHandler;
using StreamImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.iOS.StreamImagesourceHandler;
#elif __MACOS__
using Microsoft.Maui.Controls.Compatibility.Platform.MacOS;
using UriImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.MacOS.ImageLoaderSourceHandler;
using StreamImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.MacOS.StreamImagesourceHandler;
#elif UAP10_0
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
#elif NET471
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers;
using StreamImageSourceHandler = Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers.StreamImagesourceHandler;
#elif TIZEN
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;
using NImage = Microsoft.Maui.Controls.Compatibility.Platform.Tizen.Native.Image;
using XForms = Xamarin.Forms.Forms;
#else
using Microsoft.Maui.Controls.Compatibility.Platform.WPF;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
#if ANDROID || IOS
	class ImageSourceValidator : IImageSourceValidator
	{
		public async Task<bool> IsImageSourceValidAsync(ImageSource? source)
		{
			var handler = GetHandler(source);
			if (handler == null)
				return false;

#if TIZEN
			return await handler.LoadImageAsync(new NImage(XForms.NativeParent), source).ConfigureAwait(false);
#elif ANDROID
			var imageSource = await handler.LoadImageAsync(source, XCT.Context).ConfigureAwait(false);
			return imageSource != null;
#else
			var imageSource = await handler.LoadImageAsync(source).ConfigureAwait(false);
			return imageSource != null;
#endif
		}

		IImageSourceHandler? GetHandler(ImageSource? source)
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

			if (source is FileImageSource fileSource)
			{
#if ANDROID
				if (!File.Exists(fileSource.File))
					return null;
#endif
				return new FileImageSourceHandler();
			}

			return null;
		}
	}
#endif
}