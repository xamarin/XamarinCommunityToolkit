using System.Timers;
using Gtk;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Xamarin.CommunityToolkit.UI.Views.Options;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		static Timer snackBarTimer;
		static bool isSnackBarActive;

		public void Show(Page page, SnackBarOptions arguments)
		{
			if (isSnackBarActive)
				return;

			isSnackBarActive = true;
			var platformRender = Platform.GetRenderer(page) as Widget;
			var snackBarLayout = GetSnackBarLayout(platformRender, arguments);
			AddPageContainer(platformRender);
			((platformRender.Toplevel as Window).Child as VBox).Add(snackBarLayout);
			(platformRender.Toplevel as Window).Child.ShowAll();
			snackBarTimer = new Timer(arguments.Duration);
			snackBarTimer.Elapsed += (sender, e) =>
			{
				((platformRender.Toplevel as Window).Child as VBox).Remove(snackBarLayout);
				snackBarTimer.Stop();
				arguments.SetResult(false);
				isSnackBarActive = false;
			};
			snackBarTimer.Start();
		}

		static HBox GetSnackBarLayout(Widget platformRender, SnackBarOptions arguments)
		{
			var snackBarLayout = new HBox();
			var message = new Gtk.Label(arguments.MessageOptions.Message);
			snackBarLayout.Add(message);
			foreach (var action in arguments.Actions)
			{
				var button = new Gtk.Button
				{
					Label = action.Text
				};
				button.Clicked += async (sender, e) =>
				{
					snackBarTimer.Stop();
					await action.Action();
					arguments.SetResult(true);
					(GetTopWindowContainer(platformRender) as VBox).Remove(snackBarLayout);
					isSnackBarActive = false;
				};
				snackBarLayout.Add(button);
			}

			return snackBarLayout;
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