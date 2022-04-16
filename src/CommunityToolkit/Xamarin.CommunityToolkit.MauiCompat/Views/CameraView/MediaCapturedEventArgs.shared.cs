using System;using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using XCT = Xamarin.CommunityToolkit.Core;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaCapturedEventArgs : EventArgs
	{
		readonly string? path;
		readonly Lazy<ImageSource?> imageSource;
		readonly Lazy<XCT.FileMediaSource?> mediaSource;

		internal MediaCapturedEventArgs(
			string? path = null,
			byte[]? imageData = null,
			double rotation = 0)
		{
			// Path = path;
			this.path = path;
			Rotation = rotation;
			ImageData = imageData;
			imageSource = new Lazy<ImageSource?>(GetImageSource);
			mediaSource = new Lazy<XCT.FileMediaSource?>(GetMediaSource);
		}

		// TODO See note on CameraView.SavePhotoToFile.
		// Taken out Path for now, just return the stream data

		/// <summary>
		/// Path of the saved file, only filled when taking a video or a picture and SavePhotoToFile is true
		/// </summary>
		// public string Path { get; }

		/// <summary>
		/// Raw image data, only filled when taking a picture and SavePhotoToFile is false
		/// </summary>
		public byte[]? ImageData { get; }

		/// <summary>
		/// Applied image rotation for correct orientation on Android devices
		/// </summary>
		public double Rotation { get; }

		public ImageSource? Image => imageSource.Value;

		public XCT.FileMediaSource? Video => mediaSource.Value;

		ImageSource? GetImageSource()
		{
			if (ImageData != null)
				return ImageSource.FromStream(() => new MemoryStream(ImageData));

			return !string.IsNullOrEmpty(path) ? path : null;
		}

		XCT.FileMediaSource? GetMediaSource()
		{
			if (path != null && !string.IsNullOrEmpty(path))
				return path;
			else
				return null;
		}
	}
}