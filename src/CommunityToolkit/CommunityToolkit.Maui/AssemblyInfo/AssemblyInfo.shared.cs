using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("CommunityToolkit.Maui.UnitTests")]

#if !NETSTANDARD1_0
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "CommunityToolkit.Maui.Behaviors")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "CommunityToolkit.Maui.Converters")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "CommunityToolkit.Maui.Effects")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "CommunityToolkit.Maui.Extensions")]
[assembly: XmlnsDefinition("http://xamarin.com/schemas/2020/toolkit", "CommunityToolkit.Maui.UI.Views")]
#endif