using System;
using System.Threading.Tasks;
#if UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Xamarin.CommunityToolkit.UI.Views.Helpers
{
	class SnackbarLayout : Grid
	{
		public SnackbarLayout(string message, string actionButtonText, Func<Task> action)
		{
			RowDefinitions.Add(new RowDefinition());
			ColumnDefinitions.Add(new ColumnDefinition());
			ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
#if UWP
			var messageLabel = new TextBlock() { Text = message };
#else
			var messageLabel = new Label() { Content = message };
#endif
			Children.Add(messageLabel);
			SetRow(messageLabel, 0);
			SetColumn(messageLabel, 0);
			if (!string.IsNullOrEmpty(actionButtonText) && action != null)
			{
				var button = new Button
				{
					Content = actionButtonText,
					Command = new Forms.Command(async () =>
					{
						OnSnackbarActionExecuted?.Invoke();
						await action();
					})
				};
				Children.Add(button);
				SetRow(button, 0);
				SetColumn(button, 1);
			}
		}

		public Action OnSnackbarActionExecuted;
	}
}