using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
#if NETSTANDARD || __TVOS__ || __WATCHOS__
	class ImageSourceValidator : IImageSourceValidator
	{
		// We should still try to load an image
		public Task<bool> IsImageSourceValidAsync(ImageSource source) => Task.FromResult(true);
	}
#endif
}
