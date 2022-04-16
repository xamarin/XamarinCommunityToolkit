using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	internal partial class SnackBar
	{
		internal partial ValueTask Show(VisualElement sender, SnackBarOptions arguments) => throw new PlatformNotSupportedException();
	}
}