using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface IImageSourceValidator
	{
		Task<bool> IsImageSourceValid(ImageSource source);
	}
}