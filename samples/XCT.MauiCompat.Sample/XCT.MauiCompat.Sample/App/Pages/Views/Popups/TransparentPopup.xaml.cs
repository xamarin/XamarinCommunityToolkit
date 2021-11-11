
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Graphics;
using Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific;
using Xamarin.CommunityToolkit.PlatformConfiguration.WindowsSpecific;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class TransparentPopup
	{
		public TransparentPopup() => InitializeComponent();

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().UseArrowDirection(PopoverArrowDirection.Right);
			On<Windows>().SetBorderColor(Colors.Red);
		}
	}
}