using System;
using Xamarin.CommunityToolkit.UI.Views.Options;
using System.Linq;
#if UAP10_0
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Maui.Controls.Compatibility.Platform.WPF;
#endif

namespace Xamarin.CommunityToolkit.UI.Views.Helpers
{
	class SnackBarLayout : Microsoft.Maui.Controls.Grid
	{
		public SnackBarLayout(SnackBarOptions options)
		{
			RowDefinitions.Add(new RowDefinition());
			ColumnDefinitions.Add(new ColumnDefinition());
			if (options.BackgroundColor != new Color()))
			{
				Background = options.BackgroundColor.ToBrush();
			}
#if UAP10_0
			CornerRadius = new CornerRadius(options.CornerRadius.Left, options.CornerRadius.Top, options.CornerRadius.Right, options.CornerRadius.Bottom);
			var messageLabel = new TextBlock
			{
				Text = options.MessageOptions.Message
			};
#else
			var messageLabel = new Label
			{
				Content = options.MessageOptions.Message
			};
#endif
			messageLabel.Padding = new Thickness(options.MessageOptions.Padding.Left,
					options.MessageOptions.Padding.Top,
					options.MessageOptions.Padding.Right,
					options.MessageOptions.Padding.Bottom);

			if (options.MessageOptions.Font != Forms.Font.Default)
			{
				if (options.MessageOptions.Font.FontSize > 0)
				{
					messageLabel.FontSize = options.MessageOptions.Font.FontSize;
				}

				if (options.MessageOptions.Font.FontFamily != null)
				{
					messageLabel.FontFamily = new FontFamily(options.MessageOptions.Font.FontFamily);
				}
			}

			if (options.MessageOptions.Foreground != new Color()))
			{
				messageLabel.Foreground = options.MessageOptions.Foreground.ToBrush();
			}

			Children.Add(messageLabel);
			SetRow(messageLabel, 0);
			SetColumn(messageLabel, 0);
			for (var i = 0; i < options.Actions.Count(); i++)
			{
				ColumnDefinitions.Add(new ColumnDefinition() { Width = Microsoft.Maui.Controls.GridLength.Auto });
				var action = options.Actions.ToArray()[i];
				var button = new Button
				{
					Content = action.Text,
					Command = new Forms.Command(async () =>
					{
						OnSnackBarActionExecuted?.Invoke();
						if (action.Action != null)
							await action.Action();
					}),
					Padding = new Thickness(action.Padding.Left,
						action.Padding.Top,
						action.Padding.Right,
						action.Padding.Bottom)
				};
				if (action.Font != Forms.Font.Default)
				{
					button.FontSize = action.Font.FontSize;
					button.FontFamily = new FontFamily(action.Font.FontFamily);
				}

				if (action.BackgroundColor != new Color()))
				{
					button.Background = action.BackgroundColor.ToBrush();
				}

				if (action.ForegroundColor != new Color()))
				{
					button.Foreground = action.ForegroundColor.ToBrush();
				}

				Children.Add(button);
				SetRow(button, 0);
				SetColumn(button, i + 1);
			}
		}

		public Action? OnSnackBarActionExecuted;
	}
}