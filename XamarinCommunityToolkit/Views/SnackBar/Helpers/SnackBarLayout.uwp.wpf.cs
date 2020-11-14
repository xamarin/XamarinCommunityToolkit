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
#if UWP
			Background = options.BackgroundColor.ToBrush();
			var messageLabel = new TextBlock
			{
				Text = options.MessageOptions.Message,
				FontSize = options.MessageOptions.FontSize,
				FontFamily = new FontFamily(options.MessageOptions.FontFamily),
				Foreground = options.MessageOptions.Foreground.ToBrush()
			};
#else
			Background = options.BackgroundColor.ToBrush();
			var messageLabel = new Label
			{
				Content = options.MessageOptions.Message,
				FontSize = options.MessageOptions.FontSize,
				FontFamily = new FontFamily(options.MessageOptions.FontFamily),
				Foreground = options.MessageOptions.Foreground.ToBrush()
			};
#endif
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
					Foreground = action.ForegroundColor.ToBrush(),
					Background = action.BackgroundColor.ToBrush(),
					FontSize = action.FontSize,
					FontFamily = new FontFamily(action.FontFamily),
					Command = new Forms.Command(async () =>
					{
						OnSnackBarActionExecuted?.Invoke();
						await action.Action();
					})
				};
				Children.Add(button);
				SetRow(button, 0);
				SetColumn(button, i + 1);
			}
		}

		public Action OnSnackBarActionExecuted;
	}
}