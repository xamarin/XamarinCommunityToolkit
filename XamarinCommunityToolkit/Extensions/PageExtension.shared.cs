using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views.Options;
using System.Globalization;
using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static class PageExtension
	{
		public static Task<bool> DisplayToastAsync(this Page page, string message, int duration = 3000)
		{
			var messageOptions = new MessageOptions {Message = message};
			var args = new SnackBarOptions(messageOptions, duration, Color.Default,
				CultureInfo.CurrentCulture.TextInfo.IsRightToLeft, new List<SnackBarActionOptions>());
			var snackBar = new SnackBar();
			snackBar.Show(page, args);
			return args.Result.Task;
		}

		public static Task<bool> DisplaySnackBarAsync(this Page page, string message, string actionButtonText, Func<Task> action, int duration = 3000)
		{
			var messageOptions = new MessageOptions{Message = message};
			var actionOptions = new List<SnackBarActionOptions>
			{
				new SnackBarActionOptions
				{
					Text = actionButtonText, Action = action
				}
			};
			var args = new SnackBarOptions(messageOptions, duration, Color.Default, CultureInfo.CurrentCulture.TextInfo.IsRightToLeft, actionOptions);
			var snackBar = new SnackBar();
			snackBar.Show(page, args);
			return args.Result.Task;
		}

		public static Task<bool> DisplaySnackBarAsync(this Page page, SnackBarOptions arguments)
		{
			var snackBar = new SnackBar();
			snackBar.Show(page, arguments);
			return arguments.Result.Task;
		}
	}
}