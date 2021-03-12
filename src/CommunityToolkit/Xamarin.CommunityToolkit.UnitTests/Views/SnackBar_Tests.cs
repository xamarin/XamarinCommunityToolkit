#if !NETCOREAPP
using System;
using System.Threading.Tasks;
using NSubstitute;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class SnackBar_Tests
	{
		[Fact]
		public async void VisualElementExtension_DisplaySnackBarAsync_PlatformNotSupportedException()
		{
			var page = new ContentPage();
			await Assert.ThrowsAsync<PlatformNotSupportedException>(() => page.DisplaySnackBarAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Func<Task>>()));
		}

		[Fact]
		public async void VisualElementExtension_DisplaySnackBarAsyncWithOptions_PlatformNotSupportedException()
		{
			var page = new ContentPage();
			await Assert.ThrowsAsync<PlatformNotSupportedException>(() => page.DisplaySnackBarAsync(new SnackBarOptions()));
		}

		[Fact]
		public async void VisualElementExtension_DisplayToastAsync_PlatformNotSupportedException()
		{
			var page = new ContentPage();
			await Assert.ThrowsAsync<PlatformNotSupportedException>(() => page.DisplayToastAsync("message"));
		}

		[Fact]
		public async void VisualElementExtension_DisplayToastAsyncWithOptions_PlatformNotSupportedException()
		{
			var page = new ContentPage();
			await Assert.ThrowsAsync<PlatformNotSupportedException>(() => page.DisplayToastAsync(new ToastOptions()));
		}
	}
}
#endif