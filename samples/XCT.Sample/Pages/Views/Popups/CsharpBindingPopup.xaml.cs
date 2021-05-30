using CommunityToolkit.Maui.Sample.ViewModels.Views.Popups;

namespace CommunityToolkit.Maui.Sample.Pages.Views.Popups
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