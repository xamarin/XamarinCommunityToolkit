using System;
using System.IO;
using Xamarin.Forms;
using XCT = Xamarin.CommunityToolkit.Core;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaCapturedEventArgs : EventArgs
	{
		readonly Lazy<ImageSource> imageSource;
		readonly Lazy<XCT.MediaSource> mediaSource;

		internal MediaCapturedEventArgs(
			string path = null,
			byte[] imageData = null)
		{
			Path = path;
			ImageData = imageData;
			imageSource = new Lazy<ImageSource>(GetImageSource);
			mediaSource = new Lazy<XCT.MediaSource>(GetMediaSource);
		}

		/// <summary>
		/// Path of the saved file, only filled when taking a video or a picture and SavePhotoToFile is true
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Raw image data, only filled when taking a picture and SavePhotoToFile is false
		/// </summary>
		public byte[] ImageData { get; }

		public ImageSource Image => imageSource.Value;

		public XCT.MediaSource Video => mediaSource.Value;

		ImageSource GetImageSource()
		{
			if (ImageData != null)
				return ImageSource.FromStream(() => new MemoryStream(ImageData));
			return !string.IsNullOrEmpty(Path) ? Path : null;
		}

		XCT.MediaSource GetMediaSource() => !string.IsNullOrEmpty(Path) ? Path : null;
	}
}