using System;
using Android.Media;
using AImage = Android.Media.Image;

namespace CommunityToolkit.Maui.UI.Views
{
	class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
	{
		public event EventHandler<byte[]>? Photo;

		public void OnImageAvailable(ImageReader? reader)
		{
			AImage? image = null;

			try
			{
				image = reader?.AcquireNextImage();
				if (image == null)
					return;

				var buffer = image.GetPlanes()?[0].Buffer;
				if (buffer == null)
					return;

				var imageData = new byte[buffer.Capacity()];
				buffer.Get(imageData);

				Photo?.Invoke(this, imageData);
				buffer.Clear();
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