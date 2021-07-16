using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using EButton = ElmSharp.Button;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal ValueTask Show(Forms.VisualElement sender, SnackBarOptions arguments)
		{
			var snackBarDialog =
				Forms.Platform.Tizen.Native.Dialog.CreateDialog(Forms.Forms.NativeParent,
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