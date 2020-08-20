using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaCapturedEventArgs : EventArgs
	{
		public object Data { get; set; }
		public ImageSource Image { get; set; }
		public MediaSource Video { get; set; }
	}
}