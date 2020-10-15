using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
#if NETSTANDARD || __TVOS__ || __WATCHOS__
	class ImageSourceValidator : IImageSourceValidator
	{
		// we should still try to load image
		public Task<bool> IsImageSourceValid(ImageSource source) => Task.FromResult(true);
	}
#endif
}