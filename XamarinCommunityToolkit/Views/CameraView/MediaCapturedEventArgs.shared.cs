using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaCapturedEventArgs : EventArgs
	{
		internal MediaCapturedEventArgs(
			object data = null,
			ImageSource image = null,
			MediaSource video = null)
		{
			Data = data;
			Image = image;
			Video = video;
		}

		/// <summary>
		/// Raw image data, only filled when taking a picture
		/// </summary>
		public object Data { get; }
		public ImageSource Image { get; }
		public MediaSource Video { get; }
	}
}