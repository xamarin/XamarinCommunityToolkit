using System.Threading;
using System.Threading.Tasks;
using System.IO;

using UIKit;
using NativeImage = UIKit.UIImage;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using System;

namespace Xamarin.CommunityToolkit.Extensions.iOS
{
	static class ImageResourceExtention
	{
		internal static async ValueTask<NativeImage?> GetNativeImageAsync(this ImageSource source, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (source == null || source.IsEmpty)
				return null;

			var handler = Forms.Internals.Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(source);
			if (handler == null)
				return null;

			try
			{
				var scale = (float)UIScreen.MainScreen.Scale;

				return await handler.LoadImageAsync(source, scale: scale, cancelationToken: cancellationToken);
			}
			catch (OperationCanceledException)
			{
				Log.Warning("Image loading", "Image load cancelled");
			}
			catch (Exception ex)
			{
				Log.Warning("Image loading", $"Image load failed: {ex}");
			}

			return null;
		}
	}
}
