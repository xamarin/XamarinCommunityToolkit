#if NETSTANDARD || __TVOS__ || __WATCHOS__
using System;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(VisualElement sender, SnackBarOptions arguments) => throw new PlatformNotSupportedException();
	}
}
#endif