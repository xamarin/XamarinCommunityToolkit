using Xamarin.CommunityToolkit.Sample.ViewModels.Views.Popups;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class CsharpBindingPopup
	{
		public CsharpBindingPopup()
		{
			InitializeComponent();
			BindingContext = new CsharpBindingPopupViewModel();
		}
	}
}