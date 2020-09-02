using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Core;

namespace Xamarin.CommunityToolkit.Extensions
{
	using Actions.Snackbar;
	using Forms;

	public static class PageExtension
	{
		public const string SnackbarSignalName = "Xamarin.SendSnackbar";

		public static Task<bool> DisplaySnackbar(this Page page, string message, int duration = 3000, string actionButtonText = null, Func<Task> action = null)
		{
			var args = new SnackbarArguments(message, duration, actionButtonText, action);
			MessagingCenter.Send(page, SnackbarSignalName, args);
			return args.Result.Task;
		}
	}
}
