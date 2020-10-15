#if NETSTANDARD || __TVOS__ || __WATCHOS__
using System;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views.Options;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Page sender, SnackBarOptions arguments) => throw new PlatformNotSupportedException();
	}
}
#endif