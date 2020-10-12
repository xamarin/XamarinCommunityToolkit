using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Actions.Snackbar
{
	public class Snackbar_Tests
	{
#if !NETCOREAPP
		[Fact]
		public async void PageExtension_DisplaySnackbar_PlatformNotSupportedException()
		{
			var page = new ContentPage();
			await Assert.ThrowsAsync<PlatformNotSupportedException>(() => page.DisplaySnackbar("message"));
		}
#endif
	}
}