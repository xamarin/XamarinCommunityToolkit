using System;using Microsoft.Extensions.Logging;
using Xamarin.CommunityToolkit.UI.Views.Options;
using System.Linq;
#if WINDOWS
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Button = Microsoft.UI.Xaml.Controls.Button;
using ColumnDefinition = Microsoft.UI.Xaml.Controls.ColumnDefinition;
using CornerRadius = Microsoft.UI.Xaml.CornerRadius;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using GridLength = Microsoft.UI.Xaml.GridLength;
using RowDefinition = Microsoft.UI.Xaml.Controls.RowDefinition;
using Thickness = Microsoft.UI.Xaml.Thickness;

#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Maui.Controls.Compatibility.Platform.WPF;
#endif

namespace Xamarin.CommunityToolkit.UI.Views.Helpers
{
	class SnackBarLayout : Grid
	{
		public SnackBarLayout(SnackBarOptions options)
		{
			RowDefinitions.Add(new RowDefinition());
			ColumnDefinitions.Add(new ColumnDefinition());
			if (options.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
			{
				Background = options.BackgroundColor.ToPlatform();
			}
#if WINDOWS
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

			if (options.MessageOptions.Font != Font.Default)
			{
				if (options.MessageOptions.Font.Size > 0)
				{
					messageLabel.FontSize = options.MessageOptions.Font.Size;
				}

				if (options.MessageOptions.Font.Family != null)
				{
					messageLabel.FontFamily = new FontFamily(options.MessageOptions.Font.Family);
				}
			}

			if (options.MessageOptions.Foreground != default(Microsoft.Maui.Graphics.Color))
			{
				messageLabel.Foreground = options.MessageOptions.Foreground.ToPlatform();
			}

			Children.Add(messageLabel);
			SetRow(messageLabel, 0);
			SetColumn(messageLabel, 0);
			for (var i = 0; i < options.Actions.Count(); i++)
			{
				ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
				var action = options.Actions.ToArray()[i];
				var button = new Button
				{
					Content = action.Text,
					Command = new Command(async () =>
					{
						OnSnackBarActionExecuted?.Invoke();
						try
						{
							if (action.Action != null)
								await action.Action();

							options.SetResult(true);
						}
						catch (Exception ex)
						{
							options.SetException(ex);
						}
					}),
					Padding = new Thickness(action.Padding.Left,
						action.Padding.Top,
						action.Padding.Right,
						action.Padding.Bottom)
				};
				if (action.Font != Font.Default)
				{
					button.FontSize = action.Font.Size;
					button.FontFamily = new FontFamily(action.Font.Family);
				}

				if (action.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
				{
					button.Background = action.BackgroundColor.ToPlatform();
				}

				if (action.ForegroundColor != default(Microsoft.Maui.Graphics.Color))
				{
					button.Foreground = action.ForegroundColor.ToPlatform();
				}

				Children.Add(button);
				SetRow(button, 0);
				SetColumn(button, i + 1);
			}
		}

		public Action? OnSnackBarActionExecuted;
	}
}