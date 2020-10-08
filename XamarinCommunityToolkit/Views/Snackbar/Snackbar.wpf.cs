using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Controls;
using System.Windows.Controls;
using Xamarin.Forms.Platform.WPF.Helpers;
using RowDefinition = System.Windows.Controls.RowDefinition;
using ColumnDefinition = System.Windows.Controls.ColumnDefinition;
using Label = System.Windows.Controls.Label;
using Grid = System.Windows.Controls.Grid;
using Button = System.Windows.Controls.Button;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		Timer snackbarTimer;

		internal void Show(Xamarin.Forms.Page page, SnackbarArguments arguments)
		{
			var formsAppBar = System.Windows.Application.Current.MainWindow.FindChild<FormsAppBar>("PART_BottomAppBar");
			var currentContent = formsAppBar.Content;
			var snackBar = new SnackbarWpfLayout(arguments.Message, arguments.ActionButtonText, arguments.Action);
			snackbarTimer = new Timer { Interval = arguments.Duration };
			snackbarTimer.Tick += (sender, e) =>
			{
				formsAppBar.Content = currentContent;
				snackbarTimer.Stop();
				arguments.SetResult(false);
			};
			snackBar.OnSnackbarActionExecuted += () =>
			{
				formsAppBar.Content = currentContent;
				snackbarTimer.Stop();
				arguments.SetResult(true);
			};
			snackbarTimer.Start();
			formsAppBar.Content = snackBar;
		}

		class SnackbarWpfLayout : Grid
		{
			public SnackbarWpfLayout(string message, string actionButtonText, Func<Task> action)
			{
				RowDefinitions.Add(new RowDefinition());
				ColumnDefinitions.Add(new ColumnDefinition());
				ColumnDefinitions.Add(new ColumnDefinition() { Width = System.Windows.GridLength.Auto });
				var messageLabel = new Label() { Content = message };
				Children.Add(messageLabel);
				SetRow(messageLabel, 0);
				SetColumn(messageLabel, 0);
				if (!string.IsNullOrEmpty(actionButtonText) && action != null)
				{
					var button = new Button
					{
						Content = actionButtonText,
						Command = new Command(async () =>
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
}