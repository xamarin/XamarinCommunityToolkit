using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Maui.UI.Views.Helpers;
using CommunityToolkit.Maui.UI.Views.Options;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF.Controls;
using Xamarin.Forms.Platform.WPF.Helpers;

namespace CommunityToolkit.Maui.UI.Views
{
	class SnackBar
	{
		Timer? snackBarTimer;

		internal ValueTask Show(VisualElement visualElement, SnackBarOptions arguments)
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
			return default;
		}
	}
}