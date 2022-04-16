using Microsoft.Maui.Controls.Platform;using Microsoft.Extensions.DependencyInjection;using Font = Microsoft.Maui.Font;using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
#if __IOS__
using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#elif __MACOS__
using AppKit;
using Microsoft.Maui.Controls.Compatibility.Platform.MacOS;
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
			if (arguments.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
			{
				snackBar.Appearance.Background = arguments.BackgroundColor.ToUIColor();
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackBar.Appearance.Font = arguments.MessageOptions.Font.ToUIFont(sender.Handler?.MauiContext?.Services.GetRequiredService<IFontManager>());
			}

			if (arguments.MessageOptions.Foreground != default(Microsoft.Maui.Graphics.Color))
			{
				snackBar.Appearance.Foreground = arguments.MessageOptions.Foreground.ToUIColor();
			}

			if (arguments.MessageOptions.Padding != MessageOptions.DefaultPadding)
			{
				snackBar.Layout.PaddingTop = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Top;
				snackBar.Layout.PaddingLeft = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Left;
				snackBar.Layout.PaddingBottom = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Bottom;
				snackBar.Layout.PaddingRight = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Right;
			}

			snackBar.Appearance.TextAlignment = arguments.IsRtl ? UITextAlignment.Right : UITextAlignment.Left;

			if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
			{
				snackBar.Layout.PaddingTop = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Top;
				snackBar.Layout.PaddingLeft = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Left;
				snackBar.Layout.PaddingBottom = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Bottom;
				snackBar.Layout.PaddingRight = (System.Runtime.InteropServices.NFloat)arguments.MessageOptions.Padding.Right;
			}

			snackBar.Appearance.TextAlignment = arguments.IsRtl ? UITextAlignment.Right : UITextAlignment.Left;
#elif __MACOS__
			if (arguments.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
			{
				snackBar.Appearance.Background = arguments.BackgroundColor.ToNSColor();
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackBar.Appearance.Font = arguments.MessageOptions.Font.ToNSFont();
			}

			if (arguments.MessageOptions.Foreground != default(Microsoft.Maui.Graphics.Color))
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
				if (action.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
				{
					actionButton.BackgroundColor = action.BackgroundColor.ToUIColor();
				}

				if (action.Font != Font.Default)
				{
					actionButton.TitleLabel.Font = action.Font.ToUIFont(sender.Handler?.MauiContext?.Services.GetRequiredService<IFontManager>());
				}

				if (action.ForegroundColor != default(Microsoft.Maui.Graphics.Color))
				{
					actionButton.SetTitleColor(action.ForegroundColor.ToUIColor(), UIControlState.Normal);
				}
#elif __MACOS__
				if (action.BackgroundColor != default(Microsoft.Maui.Graphics.Color) && actionButton.Layer != null)
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