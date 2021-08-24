using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// You can implement this interface to create your own animations to be used on the <see cref="BadgeView"/>. Create an implementation of <see cref="IBadgeAnimation"/>, assign the implemenatation to the <see cref="BadgeView.BadgeAnimation"/> and set <see cref="BadgeView.IsAnimated"/> to true.
	/// </summary>
	public interface IBadgeAnimation
	{
		Task OnAppearing(View badgeView);

		Task OnDisappering(View badgeView);
	}
}