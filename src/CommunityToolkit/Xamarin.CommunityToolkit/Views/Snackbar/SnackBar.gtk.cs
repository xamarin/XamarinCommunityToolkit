using System;
using System.Linq;
using System.Threading.Tasks;
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
		Timer? snackBarTimer;

		public ValueTask Show(VisualElement visualElement, SnackBarOptions arguments)
		{
			var mainWindow = (Platform.GetRenderer(visualElement).Container.Child as Forms.Platform.GTK.Controls.Page)?.Children[0] as VBox;
			var snackBarLayout = GetSnackBarLayout(mainWindow, arguments);

			AddSnackBarContainer(mainWindow, snackBarLayout);

			snackBarTimer = new Timer(arguments.Duration.TotalMilliseconds);
			snackBarTimer.Elapsed += (sender, e) =>
			{
				mainWindow?.Remove(snackBarLayout);
				snackBarTimer.Stop();
				arguments.SetResult(false);
			};

			snackBarTimer.Start();
			return default;
		}

		HBox GetSnackBarLayout(Container? container, SnackBarOptions arguments)
		{
			var snackBarLayout = new HBox();
			snackBarLayout.ModifyBg(StateType.Normal, arguments.BackgroundColor.ToGtkColor());

			var message = new Gtk.Label(arguments.MessageOptions.Message);
			message.ModifyFont(new FontDescription { AbsoluteSize = arguments.MessageOptions.Font.FontSize, Family = arguments.MessageOptions.Font.FontFamily });
			message.ModifyFg(StateType.Normal, arguments.MessageOptions.Foreground.ToGtkColor());
			message.SetPadding((int)arguments.MessageOptions.Padding.Left, (int)arguments.MessageOptions.Padding.Top);
			snackBarLayout.Add(message);
			snackBarLayout.SetChildPacking(message, false, false, 0, PackType.Start);

			foreach (var action in arguments.Actions)
			{
				var button = new Gtk.Button
				{
					Label = action.Text
				};
				button.ModifyFont(new FontDescription { AbsoluteSize = action.Font.FontSize, Family = action.Font.FontFamily });
				button.ModifyBg(StateType.Normal, action.BackgroundColor.ToGtkColor());
				button.ModifyFg(StateType.Normal, action.ForegroundColor.ToGtkColor());

				button.Clicked += async (sender, e) =>
				{
					snackBarTimer?.Stop();
                    try
                    {
						if (action.Action != null)
							await action.Action();

						arguments.SetResult(true);
					}
					catch (Exception ex)
					{
						arguments.SetException(ex);
					}
					container?.Remove(snackBarLayout);
				};

				snackBarLayout.Add(button);
				snackBarLayout.SetChildPacking(button, false, false, 0, PackType.End);
			}

			return snackBarLayout;
		}

		void AddSnackBarContainer(Container? mainWindow, Widget snackBarLayout)
		{
			var children = mainWindow?.Children ?? Enumerable.Empty<Widget>();
			foreach (var child in children)
			{
				mainWindow?.Remove(child);
			}

			foreach (var child in children)
			{
				mainWindow?.Add(child);
			}

			mainWindow?.Add(snackBarLayout);
			snackBarLayout.ShowAll();
		}
	}
}