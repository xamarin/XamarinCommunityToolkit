using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	partial class SnackBar
	{
		internal partial ValueTask Show(VisualElement sender, SnackBarOptions arguments) => throw new PlatformNotSupportedException();
	}
}