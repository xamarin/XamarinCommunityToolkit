#if NETSTANDARD || __TVOS__ || __WATCHOS__
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal Task Show(Page sender, SnackBarOptions arguments) => throw new PlatformNotSupportedException();
	}
}
#endif