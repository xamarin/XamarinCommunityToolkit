using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Application = Tizen.Applications.Application;
using Image = ElmSharp.Image;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<bool> LoadImageAsync(Image image, ImageSource imageSource, CancellationToken cancelationToken = default)
		{
			var fileInfo = await LoadInternal(imageSource, 1, Application.Current.DirectoryInfo.Cache).ConfigureAwait(false);

			lock (locker)
			{
				if (fileInfo?.Exists ?? false)
					return image.LoadFromFile(fileInfo.FullName);
			}

			return false;
		}
	}
}