using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using XCTElement = Xamarin.CommunityToolkit.UI.Views.BasePopup;
using XFPC = Xamarin.Forms.PlatformConfiguration;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific
{
	public static class PopUp
	{
		public static readonly BindableProperty ArrowDirectionProperty = BindableProperty.Create(
			"ArrowDirection", typeof(PopoverArrowDirection), typeof(BasePopup), PopoverArrowDirection.None);

		public static void SetArrowDirection(BindableObject element, PopoverArrowDirection color) =>
			element.SetValue(ArrowDirectionProperty, color);

		public static PopoverArrowDirection GetArrowDirection(BindableObject element) =>
			(PopoverArrowDirection)element.GetValue(ArrowDirectionProperty);

		public static IPlatformElementConfiguration<XFPC.iOS, XCTElement> UseArrowDirection(this IPlatformElementConfiguration<XFPC.iOS, XCTElement> config, PopoverArrowDirection value)
		{
			SetArrowDirection(config.Element, value);
			return config;
		}
	}

	public enum PopoverArrowDirection
	{
		None,
		Up,
		Down,
		Left,
		Right,
		Any,
		Unknown
	}
}