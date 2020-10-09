using System.Windows.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Controls;
using Xamarin.Forms.Platform.WPF.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		Timer snackbarTimer;

		internal void Show(Page page, SnackbarArguments arguments)
		{
			var formsAppBar = System.Windows.Application.Current.MainWindow.FindChild<FormsAppBar>("PART_BottomAppBar");
			var currentContent = formsAppBar.Content;
			var snackBar = new SnackbarLayout(arguments.Message, arguments.ActionButtonText, arguments.Action);
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
	}
}