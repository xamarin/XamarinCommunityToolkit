using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;
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
			var snackBar = NativeSnackBar.MakeSnackBar(arguments.MessageOptions.Message)
							.SetDuration(arguments.Duration.TotalMilliseconds)
							.SetTimeoutAction(() =>
							{
								arguments.SetResult(false);
								return Task.CompletedTask;
							});

#if __IOS__
			if (arguments.BackgroundColor != Color.Default)
			{
				snackBar.Appearance.BackgroundColor = arguments.BackgroundColor.ToUIColor();
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackBar.Appearance.TextFont = arguments.MessageOptions.Font.ToUIFont();
			}

			if (arguments.MessageOptions.Foreground != Color.Default)
			{
				snackBar.Appearance.TextForeground = arguments.MessageOptions.Foreground.ToUIColor();
			}

			snackBar.Appearance.MessageTextAlignment = arguments.IsRtl ? UITextAlignment.Right : UITextAlignment.Left;

			if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
			{
				var renderer = Platform.GetRenderer(sender);
				snackBar.SetParentController(renderer.ViewController);
			}
#elif __MACOS__
			if (arguments.BackgroundColor != Color.Default)
			{
				snackBar.Appearance.BackgroundColor = arguments.BackgroundColor.ToNSColor();
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackBar.Appearance.TextFont = arguments.MessageOptions.Font.ToNSFont();
			}

			if (arguments.MessageOptions.Foreground != Color.Default)
			{
				snackBar.Appearance.TextForeground = arguments.MessageOptions.Foreground.ToNSColor();
			}

			snackBar.Appearance.MessageTextAlignment = arguments.IsRtl ? NSTextAlignment.Right : NSTextAlignment.Left;
#endif

			foreach (var action in arguments.Actions)
			{
				var actionButton = new NativeActionButton();
				actionButton.SetActionButtonText(action.Text);
#if __IOS__
				if (action.BackgroundColor != Color.Default)
				{
					actionButton.Appearance.ButtonBackgroundColor = action.BackgroundColor.ToUIColor();
				}

				if (action.Font != Font.Default)
				{
					actionButton.Appearance.ButtonFont = action.Font.ToUIFont();
				}

				if (action.ForegroundColor != Color.Default)
				{
					actionButton.Appearance.ButtonForegroundColor = action.ForegroundColor.ToUIColor();
				}
#elif __MACOS__
				if (action.BackgroundColor != Color.Default)
				{
					actionButton.Appearance.ButtonBackgroundColor = action.BackgroundColor.ToNSColor();
				}

				if (action.Font != Font.Default)
				{
					actionButton.Appearance.ButtonFont = action.Font.ToNSFont();
				}

				if (action.ForegroundColor != Color.Default)
				{
					actionButton.Appearance.ButtonForegroundColor = action.ForegroundColor.ToNSColor();
				}
#endif
				actionButton.SetAction(async () =>
				{
					snackBar.Dismiss();
					await action.Action();
					arguments.SetResult(true);
				});

				snackBar.Actions.Add(actionButton);
			}

			snackBar.Show();
		}
	}
}