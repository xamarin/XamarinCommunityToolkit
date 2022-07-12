using System;
using Xamarin.CommunityToolkit.UI.Views.Options;
using System.Linq;
#if UAP10_0
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xamarin.Forms.Platform.WPF;
#endif

namespace Xamarin.CommunityToolkit.UI.Views.Helpers
{
	class SnackBarLayout : Grid
	{
		public SnackBarLayout(SnackBarOptions options)
		{
			RowDefinitions.Add(new RowDefinition());
			ColumnDefinitions.Add(new ColumnDefinition());
			if (options.BackgroundColor != Forms.Color.Default)
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

			if (options.MessageOptions.Foreground != Forms.Color.Default)
			{
				messageLabel.Foreground = options.MessageOptions.Foreground.ToBrush();
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
					Command = new Forms.Command(async () =>
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
				if (action.Font != Forms.Font.Default)
				{
					button.FontSize = action.Font.FontSize;
					button.FontFamily = new FontFamily(action.Font.FontFamily);
				}

				if (action.BackgroundColor != Forms.Color.Default)
				{
					button.Background = action.BackgroundColor.ToBrush();
				}

				if (action.ForegroundColor != Forms.Color.Default)
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