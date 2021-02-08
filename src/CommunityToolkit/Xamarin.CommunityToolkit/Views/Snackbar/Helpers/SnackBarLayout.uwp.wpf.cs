using System;
using Xamarin.CommunityToolkit.UI.Views.Options;
using System.Linq;
#if UWP
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
#if UWP
			var messageLabel = new TextBlock
			{
				Text = options.MessageOptions.Message
			};
#else
			var messageLabel = new Label
			{
				Content = options.MessageOptions.Message,
			};
#endif
			if (options.MessageOptions.Font != Forms.Font.Default)
			{
				messageLabel.FontSize = options.MessageOptions.Font.FontSize;
				messageLabel.FontFamily = new FontFamily(options.MessageOptions.Font.FontFamily);
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
						await action.Action();
					})
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

		public Action OnSnackBarActionExecuted;
	}
}