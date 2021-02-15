using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Xamarin.CommunityToolkit.UnitTests")]

#if !NETSTANDARD1_0
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "Xamarin.CommunityToolkit.AttachedProperties")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "Xamarin.CommunityToolkit.Behaviors")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "Xamarin.CommunityToolkit.Converters")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "Xamarin.CommunityToolkit.Effects")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "Xamarin.CommunityToolkit.Extensions")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "Xamarin.CommunityToolkit.UI.Views")]
#endif