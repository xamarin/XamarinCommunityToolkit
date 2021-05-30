using System.Threading.Tasks;

namespace CommunityToolkit.Maui.UI.Views
{
	public interface ITabBadgeAnimation
	{
		Task OnAppearing(TabBadgeView badgeView);

		Task OnDisappering(TabBadgeView badgeView);
	}
}