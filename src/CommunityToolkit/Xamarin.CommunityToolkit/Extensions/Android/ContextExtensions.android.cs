using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.Extensions.Android
{
	static class ContextExtensions
	{
		internal static async ValueTask<Bitmap?> GetFormsBitmapAsync(this Context context, ImageSource imageSource, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (imageSource == null || imageSource.IsEmpty)
				return null;

			var handler = Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(imageSource);
			if (handler == null)
				return null;

			try
			{
				return await handler.LoadImageAsync(imageSource, context, cancellationToken).ConfigureAwait(false);
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
