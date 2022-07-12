using Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific;
using Xamarin.CommunityToolkit.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.PlatformConfiguration;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
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