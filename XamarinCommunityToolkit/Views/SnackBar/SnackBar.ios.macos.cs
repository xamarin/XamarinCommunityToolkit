using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
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
		internal void Show(Page sender, SnackBarOptions arguments)
		{
#if __IOS__
			var snackBar = IOSSnackBar.MakeSnackBar(arguments.MessageOptions.Message)
#elif __MACOS__

			var snackBar = MacOSSnackBar.MakeSnackBar(arguments.MessageOptions.Message)
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
				snackBar.SetParentController(renderer.ViewController);
			}
#endif

			foreach (var action in arguments.Actions)
			{
				snackBar.SetActionButtonText(action.Text);
				snackBar.SetAction(async () =>
				{
					snackBar.Dismiss();
					await action.Action();
					arguments.SetResult(true);
				});
			}

			snackBar.Show();
		}
	}
}