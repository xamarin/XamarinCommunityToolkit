using System;using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using EButton = ElmSharp.Button;

namespace Xamarin.CommunityToolkit.UI.Views
{
	internal partial class SnackBar
	{
		internal partial ValueTask Show(Forms.VisualElement sender, SnackBarOptions arguments)
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
				ok.Clicked += async (_, _) =>
				{
					snackBarDialog.Dismiss();
					await OnActionClick(action, arguments).ConfigureAwait(false);
				};
			}

			snackBarDialog.TimedOut += (_, _) => DismissSnackBar();
			snackBarDialog.BackButtonPressed += (_, _) => DismissSnackBar();
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