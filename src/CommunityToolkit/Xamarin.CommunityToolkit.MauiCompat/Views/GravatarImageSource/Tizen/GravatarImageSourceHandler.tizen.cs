using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;
using Application = Tizen.Applications.Application;
using Image = ElmSharp.Image;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public partial class GravatarImageSourceHandler : IImageSourceHandler
	{
		public async Task<bool> LoadImageAsync(Image image, ImageSource imageSource, CancellationToken cancelationToken = default)
		{
			var fileInfo = await LoadInternal(imageSource, 1, Application.Current.DirectoryInfo.Cache);

			try
			{
				await semaphore.WaitAsync();

				if (fileInfo?.Exists ?? false)
					return image.LoadFromFile(fileInfo.FullName);
			}
			finally
			{
				semaphore.Release();
			}

			return false;
		}
	}
}