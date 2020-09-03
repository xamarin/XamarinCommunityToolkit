using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace Xamarin.CommunityToolkit.Actions.Snackbar
{
	public class SnackBar
	{
		static DispatcherTimer snackbarTimer;
		public void Show(Forms.Page page, SnackbarArguments arguments)
		{
			var pageControl = Platform.GetRenderer(page).ContainerElement.Parent as PageControl;
			var sender = new ExtendedPageControl(pageControl);
			snackbarTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(arguments.Duration) };
			snackbarTimer.Tick += delegate
			{
				sender.HideSnackBar();
				snackbarTimer.Stop();
				arguments.SetResult(false);
			};
			sender.OnSnackbarActionExecuted += delegate
			{
				sender.HideSnackBar();
				snackbarTimer.Stop();
				arguments.SetResult(true);
			};
			snackbarTimer.Start();
			sender.ShowSnackBar(arguments.Message, arguments.ActionButtonText, arguments.Action);
		}

		class ExtendedPageControl : ContentControl
		{
			public ExtendedPageControl(PageControl pageControl)
			{

			}

			public static readonly DependencyProperty SnackbarActionCommandProperty = DependencyProperty.Register("SnackbarActionCommand", typeof(ICommand), typeof(PageControl), new PropertyMetadata(null));
			public static readonly DependencyProperty SnackbarActionButtonTextProperty = DependencyProperty.Register("SnackbarActionButtonText", typeof(string), typeof(PageControl), new PropertyMetadata(null));
			public static readonly DependencyProperty SnackbarMessageProperty = DependencyProperty.Register("SnackbarMessage", typeof(string), typeof(PageControl), new PropertyMetadata(null));
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
