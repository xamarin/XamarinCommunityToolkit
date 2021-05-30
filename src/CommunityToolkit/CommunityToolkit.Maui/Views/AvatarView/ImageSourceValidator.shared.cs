using System.Threading.Tasks;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
{
#if NETSTANDARD || __TVOS__ || __WATCHOS__
	class ImageSourceValidator : IImageSourceValidator
	{
		public Task<bool> IsImageSourceValidAsync(ImageSource? source) => Task.FromResult(false);
	}
#endif
}