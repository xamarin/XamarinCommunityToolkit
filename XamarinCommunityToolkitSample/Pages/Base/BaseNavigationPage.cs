using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace XamarinCommunityToolkitSample.Pages
{
    public class BaseNavigationPage : Xamarin.Forms.NavigationPage
    {
        public BaseNavigationPage(Xamarin.Forms.Page root) : base(root)
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
        }
    }
}

