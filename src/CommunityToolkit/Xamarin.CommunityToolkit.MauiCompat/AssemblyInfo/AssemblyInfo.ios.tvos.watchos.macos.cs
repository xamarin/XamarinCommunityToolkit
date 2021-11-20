using Foundation;
using Xamarin.CommunityToolkit.UI.Views;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: LinkerSafe]
#pragma warning restore CS0618 // Type or member is obsolete

#if XAMARIN_IOS || XAMARIN_MAC
[assembly: ExportImageSourceHandler(typeof(GravatarImageSource), typeof(GravatarImageSourceHandler))]
#endif