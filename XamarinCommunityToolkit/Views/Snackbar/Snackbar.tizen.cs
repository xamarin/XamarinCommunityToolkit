using System;
using EButton = ElmSharp.Button;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Forms.Page sender, SnackbarArguments arguments)
		{
			var snackbarDialog =
				Forms.Platform.Tizen.Native.Dialog.CreateDialog(Forms.Forms.NativeParent,
					arguments.ActionButtonText != null);

			snackbarDialog.Timeout = TimeSpan.FromMilliseconds(arguments.Duration).TotalSeconds;

			var message = arguments.Message.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
				.Replace(Environment.NewLine, "<br>");

			snackbarDialog.Message = message;

			if (!string.IsNullOrEmpty(arguments.ActionButtonText) && arguments.Action != null)
			{
				var ok = new EButton(snackbarDialog) { Text = arguments.ActionButtonText };
				snackbarDialog.NeutralButton = ok;
				ok.Clicked += async (s, evt) =>
				{
					snackbarDialog.Dismiss();
					await arguments.Action();
					arguments.SetResult(true);
				};
			}

			snackbarDialog.TimedOut += (s, evt) => { DismissSnackbar(); };

			snackbarDialog.BackButtonPressed += (s, evt) => { DismissSnackbar(); };

			snackbarDialog.Show();

			void DismissSnackbar()
			{
				snackbarDialog.Dismiss();
				arguments.SetResult(false);
			}
		}
	}
}