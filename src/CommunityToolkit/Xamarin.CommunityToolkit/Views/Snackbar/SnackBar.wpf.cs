using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Xamarin.CommunityToolkit.UI.Views.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms.Platform.WPF;
using Xamarin.Forms.Platform.WPF.Controls;
using Xamarin.Forms.Platform.WPF.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views
{
	internal partial class SnackBar
	{
		Timer? snackBarTimer;

		internal partial ValueTask Show(Forms.VisualElement visualElement, SnackBarOptions arguments)
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
			};
			snackBarTimer.Start();
			var border = new Border
			{
				CornerRadius = new CornerRadius(arguments.CornerRadius.Left, arguments.CornerRadius.Top, arguments.CornerRadius.Right, arguments.CornerRadius.Bottom)
			};
			border.Child = snackBar;
			formsAppBar.Content = border;
			return default;
		}
	}
}