using Gtk;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		static Timer snackbarTimer;
		static bool isSnackBarActive;

		public void Show(Page page, SnackbarArguments arguments)
		{
			if (isSnackBarActive)
				return;

			isSnackBarActive = true;
			var platformRender = Platform.GetRenderer(page) as Widget;
			var snackBar = GetSnackbarLayout(platformRender, arguments);
			AddPageContainer(platformRender);
			((platformRender.Toplevel as Window).Child as VBox).Add(snackBar);
			(platformRender.Toplevel as Window).Child.ShowAll();
			snackbarTimer = new Timer(arguments.Duration);
			snackbarTimer.Elapsed += (sender, e) =>
			{
				((platformRender.Toplevel as Window).Child as VBox).Remove(snackBar);
				snackbarTimer.Stop();
				arguments.SetResult(false);
				isSnackBarActive = false;
			};
			snackbarTimer.Start();
		}

		static HBox GetSnackbarLayout(Widget platformRender, SnackbarArguments arguments)
		{
			var snackbarLayout = new HBox();
			var message = new Gtk.Label(arguments.Message);
			snackbarLayout.Add(message);
			var isActionDialog = !string.IsNullOrEmpty(arguments.ActionButtonText) && arguments.Action != null;
			if (isActionDialog)
			{
				var button = new Gtk.Button
				{
					Label = arguments.ActionButtonText
				};
				button.Clicked += async (sender, e) =>
				{
					snackbarTimer.Stop();
					await arguments.Action();
					arguments.SetResult(true);
					(GetTopWindowContainer(platformRender) as VBox).Remove(snackbarLayout);
					isSnackBarActive = false;
				};
				snackbarLayout.Add(button);
			}

			return snackbarLayout;
		}

		static Widget GetTopWindowContainer(Widget platformRenderer) => (platformRenderer.Toplevel as Window).Child;

		void AddPageContainer(Widget platformRenderer)
		{
			var container = new VBox();
			container.Add(platformRenderer);
			(platformRenderer.Toplevel as Window).Add(container);
		}
	}
}
