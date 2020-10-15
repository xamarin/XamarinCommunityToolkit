using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class AvatarView
	{
		async Task<bool> IsImageSourceValid(ImageSource source)
		{
			IImageSourceHandler handler;

			switch (source)
			{
				case UriImageSource _:
					handler = new UriImageSourceHandler();
					break;
				case FileImageSource f:
					if (!File.Exists(f.File))
					{
						return false;
					}

					handler = new FileImageSourceHandler();
					break;
				case StreamImageSource _:
					handler = new StreamImageSourceHandler();
					break;
				default:
					return false;
			}

			var imageSource = await handler.LoadImageAsync(source);
			return imageSource != null;
		}
	}
}