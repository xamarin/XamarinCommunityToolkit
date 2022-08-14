using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;
using Xamarin.Forms;
#if __IOS__
using UIKit;
using Xamarin.Forms.Platform.iOS;
#elif __MACOS__
using AppKit;
using Xamarin.Forms.Platform.MacOS;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	partial class SnackBar
	{
		internal partial ValueTask Show(VisualElement sender, SnackBarOptions arguments)
		{
			var snackBar = NativeSnackBar.MakeSnackBar(arguments.MessageOptions.Message)
							.SetDuration(arguments.Duration)
							.SetCornerRadius(arguments.CornerRadius)
							.SetTimeoutAction(() =>
							{
								arguments.SetResult(false);
								return Task.CompletedTask;
							});

#if __IOS__
			if (arguments.BackgroundColor != Color.Default)
			{
				snackBar.Appearance.Background = arguments.BackgroundColor.ToUIColor();
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackBar.Appearance.Font = arguments.MessageOptions.Font.ToUIFont();
			}

			if (arguments.MessageOptions.Foreground != Color.Default)
			{
				snackBar.Appearance.Foreground = arguments.MessageOptions.Foreground.ToUIColor();
			}

			if (arguments.MessageOptions.Padding != MessageOptions.DefaultPadding)
			{
				snackBar.Layout.PaddingTop = (nfloat)arguments.MessageOptions.Padding.Top;
				snackBar.Layout.PaddingLeft = (nfloat)arguments.MessageOptions.Padding.Left;
				snackBar.Layout.PaddingBottom = (nfloat)arguments.MessageOptions.Padding.Bottom;
				snackBar.Layout.PaddingRight = (nfloat)arguments.MessageOptions.Padding.Right;
			}

			snackBar.Appearance.TextAlignment = arguments.IsRtl ? UITextAlignment.Right : UITextAlignment.Left;

			if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
			{
				snackBar.Layout.PaddingTop = (nfloat)arguments.MessageOptions.Padding.Top;
				snackBar.Layout.PaddingLeft = (nfloat)arguments.MessageOptions.Padding.Left;
				snackBar.Layout.PaddingBottom = (nfloat)arguments.MessageOptions.Padding.Bottom;
				snackBar.Layout.PaddingRight = (nfloat)arguments.MessageOptions.Padding.Right;
			}

			snackBar.Appearance.TextAlignment = arguments.IsRtl ? UITextAlignment.Right : UITextAlignment.Left;
#elif __MACOS__
			if (arguments.BackgroundColor != Color.Default)
			{
				snackBar.Appearance.Background = arguments.BackgroundColor.ToNSColor();
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackBar.Appearance.Font = arguments.MessageOptions.Font.ToNSFont();
			}

			if (arguments.MessageOptions.Foreground != Color.Default)
			{
				snackBar.Appearance.Foreground = arguments.MessageOptions.Foreground.ToNSColor();
			}

			snackBar.Appearance.TextAlignment = arguments.IsRtl ? NSTextAlignment.Right : NSTextAlignment.Left;
#endif
			if (sender is not Page)
			{
				var renderer = Platform.GetRenderer(sender);
				snackBar.SetAnchor(renderer.NativeView);
			}

			foreach (var action in arguments.Actions)
			{
				var actionButton = new NativeSnackButton(action.Padding.Left,
					action.Padding.Top,
					action.Padding.Right,
					action.Padding.Bottom);
				actionButton.SetActionButtonText(action.Text);
#if __IOS__
				if (action.BackgroundColor != Color.Default)
				{
					actionButton.BackgroundColor = action.BackgroundColor.ToUIColor();
				}

				if (action.Font != Font.Default)
				{
					actionButton.TitleLabel.Font = action.Font.ToUIFont();
				}

				if (action.ForegroundColor != Color.Default)
				{
					actionButton.SetTitleColor(action.ForegroundColor.ToUIColor(), UIControlState.Normal);
				}
#elif __MACOS__
				if (action.BackgroundColor != Color.Default && actionButton.Layer != null)
				{
					actionButton.Layer.BackgroundColor = action.BackgroundColor.ToCGColor();
				}

				if (action.Font != Font.Default)
				{
					actionButton.Font = action.Font.ToNSFont();
				}
#endif
				actionButton.SetAction(async () =>
				{
					snackBar.Dismiss();
					await OnActionClick(action, arguments).ConfigureAwait(false);
				});

				snackBar.Actions.Add(actionButton);
			}

			snackBar.Show();

			return default;
		}
	}
}