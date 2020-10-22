using System;
using System.Linq;
using Xamarin.CommunityToolkit.UI.Views.Options;
using EButton = ElmSharp.Button;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Forms.Page sender, SnackBarOptions arguments)
		{
			var snackBarDialog =
				Forms.Platform.Tizen.Native.Dialog.CreateDialog(Forms.Forms.NativeParent,
					arguments.Actions.Any());

			snackBarDialog.Timeout = TimeSpan.FromMilliseconds(arguments.Duration).TotalSeconds;

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
					await action.Action();
					arguments.SetResult(true);
				};
			}

			snackBarDialog.TimedOut += (s, evt) => { DismissSnackBar(); };

			snackBarDialog.BackButtonPressed += (s, evt) => { DismissSnackBar(); };

			snackBarDialog.Show();

			void DismissSnackBar()
			{
				snackBarDialog.Dismiss();
				arguments.SetResult(false);
			}
		}
	}
}