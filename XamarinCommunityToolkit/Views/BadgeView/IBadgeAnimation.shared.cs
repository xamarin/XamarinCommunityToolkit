using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface IBadgeAnimation
	{
		Task OnAppearing(View badgeView);

		Task OnDisappering(View badgeView);
	}
}