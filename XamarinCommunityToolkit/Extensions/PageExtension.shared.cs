using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static class PageExtension
	{
		public static Task<bool> DisplaySnackbar(this Page page, string message, int duration = 3000, string actionButtonText = null, Func<Task> action = null)
		{
			var args = new SnackbarArguments(message, duration, actionButtonText, action);
			var snackBar = new SnackBar();
			snackBar.Show(page, args);
			return args.Result.Task;
		}
	}
}