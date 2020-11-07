using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface ITabBadgeAnimation
	{
		Task OnAppearing(TabBadgeView badgeView);

		Task OnDisappering(TabBadgeView badgeView);
	}
}