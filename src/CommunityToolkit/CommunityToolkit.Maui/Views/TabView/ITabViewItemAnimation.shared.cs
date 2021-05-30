using System.Threading.Tasks;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
{
	public interface ITabViewItemAnimation
	{
		Task OnSelected(View tabViewItem);

		Task OnDeSelected(View tabViewItem);
	}
}