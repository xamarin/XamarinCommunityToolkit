using System.Threading.Tasks;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
{
	interface IImageSourceValidator
	{
		Task<bool> IsImageSourceValidAsync(ImageSource? source);
	}
}