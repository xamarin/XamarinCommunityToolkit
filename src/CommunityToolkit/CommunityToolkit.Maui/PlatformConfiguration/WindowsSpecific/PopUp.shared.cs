using CommunityToolkit.Maui.UI.Views;
using Xamarin.Forms;
using XCTElement = CommunityToolkit.Maui.UI.Views.BasePopup;
using XFPC = Xamarin.Forms.PlatformConfiguration;

namespace CommunityToolkit.Maui.PlatformConfiguration.WindowsSpecific
{
	public static class PopUp
	{
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
			"BorderColor", typeof(Color), typeof(BasePopup), default(Color));

		public static void SetBorderColor(BindableObject element, Color color) =>
			element.SetValue(BorderColorProperty, color);

		public static Color GetBorderColor(BindableObject element) =>
			(Color)element.GetValue(BorderColorProperty);

		public static IPlatformElementConfiguration<XFPC.Windows, XCTElement> SetBorderColor(this IPlatformElementConfiguration<XFPC.Windows, XCTElement> config, Color value)
		{
			SetBorderColor(config.Element, value);
			return config;
		}
	}
}