using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	interface IImageSourceValidator
	{
		Task<bool> IsImageSourceValidAsync(ImageSource source);
	}
}