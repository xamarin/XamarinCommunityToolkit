using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	interface IImageSourceValidator
	{
		Task<bool> IsImageSourceValidAsync(ImageSource? source);
	}
}