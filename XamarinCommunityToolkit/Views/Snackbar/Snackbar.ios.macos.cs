using System.Threading.Tasks;
using Xamarin.Forms;
#if __IOS__
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS;
#elif __MACOS__
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Page sender, SnackbarArguments arguments)
		{
#if __IOS__
			var snackbar = IOSSnackBar.MakeSnackbar(arguments.Message)
#elif __MACOS__

			var snackbar = MacOSSnackBar.MakeSnackbar(arguments.Message)
#endif
							.SetDuration(arguments.Duration)
							.SetTimeoutAction(() =>
							{
								arguments.SetResult(false);
								return Task.CompletedTask;
							});

#if __IOS__
			if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
			{
				var renderer = Platform.GetRenderer(sender);
				snackbar.SetParentController(renderer.ViewController);
			}
#endif

			if (!string.IsNullOrEmpty(arguments.ActionButtonText) && arguments.Action != null)
			{
				snackbar.SetActionButtonText(arguments.ActionButtonText);
				snackbar.SetAction(async () =>
				{
					snackbar.Dismiss();
					await arguments.Action();
					arguments.SetResult(true);
				});
			}

			snackbar.Show();
		}
	}
}