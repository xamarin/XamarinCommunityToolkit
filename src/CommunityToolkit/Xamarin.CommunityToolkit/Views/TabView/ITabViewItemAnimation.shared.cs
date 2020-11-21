using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface ITabViewItemAnimation
	{
		Task OnSelected(View tabViewItem);

		Task OnDeSelected(View tabViewItem);
	}
}