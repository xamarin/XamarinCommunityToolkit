using Gtk;
using System.Linq;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Extensions;

namespace Xamarin.CommunityToolkit.Actions.Snackbar
{
	public class SnackBar
	{
		static Timer snackbarTimer;
		static bool isSnackBarActive;

		public void Show(Page sender, SnackbarArguments arguments)
		{
			if (isSnackBarActive)
				return;

			isSnackBarActive = true;
			var platformRender = Platform.GetRenderer(sender) as Widget;
			var snackBar = GetSnackbarLayout(platformRender, arguments);
			AddPageContainer(platformRender);
			((platformRender.Toplevel as Window).Child as VBox).Add(snackBar);
			(platformRender.Toplevel as Window).Child.ShowAll();
			snackbarTimer = new Timer(arguments.Duration);
			snackbarTimer.Elapsed += delegate
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
				var button = new Gtk.Button();
				button.Label = arguments.ActionButtonText;
				button.Clicked += async delegate
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

		static Widget GetTopWindowContainer(Widget platformRenderer)
		{
			return (platformRenderer.Toplevel as Window).Child;
		}

		void AddPageContainer(Widget platformRenderer)
		{
			var container = new VBox();
			container.Add(platformRenderer);
			(platformRenderer.Toplevel as Window).Add(container);
		}
	}
}
