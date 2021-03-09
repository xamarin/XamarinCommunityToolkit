using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.iOSSpecific
{
    public static class Popup
    {
        public static readonly BindableProperty ArrowDirectionProperty = BindableProperty.Create(
            "ArrowDirection", typeof(PopoverArrowDirection), typeof(Views.Popup), PopoverArrowDirection.None);

        public static void SetArrowDirection(BindableObject element, PopoverArrowDirection color) =>
            element.SetValue(ArrowDirectionProperty, color);

        public static PopoverArrowDirection GetArrowDirection(BindableObject element) =>
            (PopoverArrowDirection)element.GetValue(ArrowDirectionProperty);
    }
}
