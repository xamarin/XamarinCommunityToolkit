using Foundation;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

[assembly: LinkerSafe]

#if XAMARIN_IOS || XAMARIN_MAC
[assembly: ExportImageSourceHandler(typeof(GravatarImageSource), typeof(GravatarImageSourceHandler))]
#endif