using System.Timers;
using Gtk;
using Pango;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Extensions;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		Timer snackBarTimer;

		public void Show(Page page, SnackBarOptions arguments)
		{
			var mainWindow = (Platform.GetRenderer(page).Container.Child as Forms.Platform.GTK.Controls.Page)?.Children[0] as VBox;
			var snackBarLayout = GetSnackBarLayout(mainWindow, arguments);
			AddSnackBarContainer(mainWindow, snackBarLayout);
			snackBarTimer = new Timer(arguments.Duration);
			snackBarTimer.Elapsed += (sender, e) =>
			{
				mainWindow.Remove(snackBarLayout);
				snackBarTimer.Stop();
				arguments.SetResult(false);
			};
			snackBarTimer.Start();
		}

		HBox GetSnackBarLayout(Container container, SnackBarOptions arguments)
		{
			var snackBarLayout = new HBox();
			snackBarLayout.ModifyBg(StateType.Normal, arguments.BackgroundColor.ToGtkColor());

			var message = new Gtk.Label(arguments.MessageOptions.Message);
			message.ModifyFont(new FontDescription { AbsoluteSize = arguments.MessageOptions.FontSize, Family = arguments.MessageOptions.FontFamily });
			message.ModifyFg(StateType.Normal, arguments.MessageOptions.Foreground.ToGtkColor());
			snackBarLayout.Add(message);
			snackBarLayout.SetChildPacking(message, false, false, 0, PackType.Start);

			foreach (var action in arguments.Actions)
			{
				var button = new Gtk.Button
				{
					Label = action.Text
				};
				button.ModifyFont(new FontDescription { AbsoluteSize = action.FontSize, Family = action.FontFamily });
				button.ModifyBg(StateType.Normal, action.BackgroundColor.ToGtkColor());
				button.ModifyFg(StateType.Normal, action.ForegroundColor.ToGtkColor());

				button.Clicked += async (sender, e) =>
				{
					snackBarTimer.Stop();
					await action.Action();
					arguments.SetResult(true);
					container.Remove(snackBarLayout);
				};

				snackBarLayout.Add(button);
				snackBarLayout.SetChildPacking(button, false, false, 0, PackType.End);
			}

			return snackBarLayout;
		}

		void AddSnackBarContainer(Container mainWindow, Widget snackBarLayout)
		{
			var children = mainWindow.Children;
			foreach (var child in mainWindow.Children)
			{
				mainWindow.Remove(child);
			}

			foreach (var child in children)
			{
				mainWindow.Add(child);
			}

			mainWindow.Add(snackBarLayout);
			snackBarLayout.ShowAll();
		}
	}
}