using System.Windows.Forms;
using Xamarin.CommunityToolkit.UI.Views.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Controls;
using Xamarin.Forms.Platform.WPF.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		Timer snackBarTimer;

		internal void Show(Page page, SnackBarOptions arguments)
		{
			var formsAppBar = System.Windows.Application.Current.MainWindow.FindChild<FormsAppBar>("PART_BottomAppBar");
			var currentContent = formsAppBar.Content;
			var snackBar = new SnackBarLayout(arguments);
			snackBarTimer = new Timer { Interval = (int)arguments.Duration.TotalMilliseconds };
			snackBarTimer.Tick += (sender, e) =>
			{
				formsAppBar.Content = currentContent;
				snackBarTimer.Stop();
				arguments.SetResult(false);
			};
			snackBar.OnSnackBarActionExecuted += () =>
			{
				formsAppBar.Content = currentContent;
				snackBarTimer.Stop();
				arguments.SetResult(true);
			};
			snackBarTimer.Start();
			formsAppBar.Content = snackBar;
		}
	}
}