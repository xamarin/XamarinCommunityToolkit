using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Controls;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		Timer snackbarTimer;

		internal void Show(Page page, SnackbarArguments arguments)
		{
			if (System.Windows.Application.Current.MainWindow is MyFormsWindow window)
			{
				snackbarTimer = new Timer { Interval = arguments.Duration };
				snackbarTimer.Tick += delegate
				{
					window.HideSnackBar();
					snackbarTimer.Stop();
					arguments.SetResult(false);
				};
				window.OnSnackbarActionExecuted += delegate
				{
					window.HideSnackBar();
					snackbarTimer.Stop();
					arguments.SetResult(true);
				};
				snackbarTimer.Start();
				window.ShowSnackBar(arguments.Message, arguments.ActionButtonText, arguments.Action);
			}
			else
			{
				arguments.SetResult(false);
			}
		}

		class MyFormsWindow : FormsWindow
		{
			public static readonly DependencyProperty SnackbarActionCommandProperty = DependencyProperty.Register("SnackbarActionCommand", typeof(ICommand), typeof(FormsWindow));
			public static readonly DependencyProperty SnackbarActionButtonTextProperty = DependencyProperty.Register("SnackbarActionButtonText", typeof(string), typeof(FormsWindow));
			public static readonly DependencyProperty SnackbarMessageProperty = DependencyProperty.Register("SnackbarMessage", typeof(string), typeof(FormsWindow));

			public string SnackbarActionButtonText
			{
				get { return (string)GetValue(SnackbarActionButtonTextProperty); }
				private set { SetValue(SnackbarActionButtonTextProperty, value); }
			}

			public ICommand SnackbarActionCommand
			{
				get { return (ICommand)GetValue(SnackbarActionCommandProperty); }
				private set { SetValue(SnackbarActionCommandProperty, value); }
			}

			public string SnackbarMessage
			{
				get { return (string)GetValue(SnackbarMessageProperty); }
				private set { SetValue(SnackbarMessageProperty, value); }
			}
			public Action OnSnackbarActionExecuted;
			public void ShowSnackBar(string message, string actionButtonText, Func<Task> action)
			{
				SnackbarMessage = message;
				SnackbarActionButtonText = actionButtonText;
				if (action != null)
				{
					SnackbarActionCommand = new Command(async () =>
					{
						OnSnackbarActionExecuted?.Invoke();
						await action();
					});
				}
			}

			public void HideSnackBar()
			{
				SnackbarMessage = null;
				SnackbarActionButtonText = null;
				SnackbarActionCommand = null;
			}
		}
	}

}