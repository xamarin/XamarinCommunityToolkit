using Xamarin.Forms.PlatformConfiguration;
using CommunityToolkit.Maui.PlatformConfiguration.iOSSpecific;
using CommunityToolkit.Maui.PlatformConfiguration.WindowsSpecific;

namespace CommunityToolkit.Maui.Sample.Pages.Views.Popups
{
	public partial class TransparentPopup
	{
		public TransparentPopup() => InitializeComponent();

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			On<iOS>().UseArrowDirection(PopoverArrowDirection.Right);
			On<Windows>().SetBorderColor(Xamarin.Forms.Color.Red);
		}
	}
}