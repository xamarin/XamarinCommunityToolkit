using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
#if NETSTANDARD || __TVOS__ || __WATCHOS__
	class ImageSourceValidator : IImageSourceValidator
	{
		public Task<bool> IsImageSourceValidAsync(ImageSource? source) => Task.FromResult(false);
	}
#endif
}