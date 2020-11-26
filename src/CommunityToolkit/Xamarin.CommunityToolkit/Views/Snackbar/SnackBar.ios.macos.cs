using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
#if __IOS__
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS;
#elif __MACOS__
using AppKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS;
using Xamarin.Forms.Platform.MacOS;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Page sender, SnackBarOptions arguments)
		{
#if __IOS__
			var snackBar = IOSSnackBar.MakeSnackBar(arguments.MessageOptions.Message)
							.SetAppearance(new SnackBarAppearance
							{
								BackgroundColor = arguments.BackgroundColor.ToUIColor(),
								TextFontSize = (nfloat)arguments.MessageOptions.FontSize,
								TextForeground = arguments.MessageOptions.Foreground.ToUIColor(),
								TextFontName = arguments.MessageOptions.FontFamily,
								MessageTextAlignment = arguments.IsRtl ? UITextAlignment.Right : UITextAlignment.Left
							})
#elif __MACOS__

			var snackBar = MacOSSnackBar.MakeSnackBar(arguments.MessageOptions.Message)
							.SetAppearance(new SnackBarAppearance
							{
								BackgroundColor = arguments.BackgroundColor.ToNSColor(),
								TextFontSize = (nfloat)arguments.MessageOptions.FontSize,
								TextForeground = arguments.MessageOptions.Foreground.ToNSColor(),
								TextFontName = arguments.MessageOptions.FontFamily,
								MessageTextAlignment = arguments.IsRtl ? NSTextAlignment.Right : NSTextAlignment.Left
							})
#endif
							.SetDuration(arguments.Duration.TotalMilliseconds)
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
#if __IOS__
				snackBar.Appearance.ButtonBackgroundColor = action.BackgroundColor.ToUIColor();
				snackBar.Appearance.ButtonForegroundColor = action.ForegroundColor.ToUIColor();
#elif __MACOS__
				snackBar.Appearance.ButtonBackgroundColor = action.BackgroundColor.ToNSColor();
				snackBar.Appearance.ButtonForegroundColor = action.ForegroundColor.ToNSColor();
#endif
				snackBar.Appearance.ButtonFontSize = (nfloat)action.FontSize;
				snackBar.Appearance.ButtonFontName = action.FontFamily;
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