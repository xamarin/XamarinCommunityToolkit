using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.UI.Views.Options;
using EButton = ElmSharp.Button;

namespace CommunityToolkit.Maui.UI.Views
{
	class SnackBar
	{
		internal ValueTask Show(Xamarin.Forms.VisualElement sender, SnackBarOptions arguments)
		{
			var snackBarDialog =
				Xamarin.Forms.Platform.Tizen.Native.Dialog.CreateDialog(Xamarin.Forms.Forms.NativeParent,
					arguments.Actions.Any());

			snackBarDialog.Timeout = arguments.Duration.TotalSeconds;

			var message = arguments.MessageOptions.Message.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
				.Replace(Environment.NewLine, "<br>");

			snackBarDialog.Message = message;

			foreach (var action in arguments.Actions)
			{
				var ok = new EButton(snackBarDialog) { Text = action.Text };
				snackBarDialog.NeutralButton = ok;
				ok.Clicked += async (s, evt) =>
				{
					snackBarDialog.Dismiss();

					if (action.Action != null)
						await action.Action();

					arguments.SetResult(true);
				};
			}

			snackBarDialog.TimedOut += (s, evt) => DismissSnackBar();
			snackBarDialog.BackButtonPressed += (s, evt) => DismissSnackBar();
			snackBarDialog.Show();

			return default;

			void DismissSnackBar()
			{
				snackBarDialog.Dismiss();
				arguments.SetResult(false);
			}
		}
	}
}