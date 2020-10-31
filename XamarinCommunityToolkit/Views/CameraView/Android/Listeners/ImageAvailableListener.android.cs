using System;
using Android.Media;
using Java.Nio;
using AImage = Android.Media.Image;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
	{
		public Action<byte[]> OnPhotoReady;

		public void OnImageAvailable(ImageReader reader)
		{
			AImage image = null;

			try
			{
				image = reader.AcquireNextImage();
				var buffer = image.GetPlanes()[0].Buffer;
				var imageData = new byte[buffer.Capacity()];
				buffer.Get(imageData);

				OnPhotoReady?.Invoke(imageData);
				buffer.Clear();
				buffer.Dispose();
			}
			catch (Exception)
			{
			}
			finally
			{
				image?.Close();
			}
		}
	}
}