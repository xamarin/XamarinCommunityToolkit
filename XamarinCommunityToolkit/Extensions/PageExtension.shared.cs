using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static class PageExtension
	{
		public static Task<bool> DisplayToastAsync(this Page page, string message, int duration = 3000)
		{
			var messageOptions = new MessageOptions { Message = message };
			var args = new SnackBarOptions(messageOptions,
				duration,
				Color.Default,
#if NETSTANDARD1_0
				false,
#else
				CultureInfo.CurrentCulture.TextInfo.IsRightToLeft,
#endif
				new List<SnackBarActionOptions>());
			var snackBar = new SnackBar();
			snackBar.Show(page, args);
			return args.Result.Task;
		}

		public static Task<bool> DisplayToastAsync(this Page page, ActionOptions actionOptions)
		{
			var snackBar = new SnackBar();
			var arguments = actionOptions ?? new ActionOptions();
			var options = new SnackBarOptions(arguments.MessageOptions, arguments.Duration, arguments.BackgroundColor, arguments.IsRtl, null);
			snackBar.Show(page, options);
			return options.Result.Task;
		}

		public static Task<bool> DisplaySnackBarAsync(this Page page, string message, string actionButtonText, Func<Task> action, int duration = 3000)
		{
			var messageOptions = new MessageOptions { Message = message };
			var actionOptions = new List<SnackBarActionOptions>
			{
				new SnackBarActionOptions
				{
					Text = actionButtonText, Action = action
				}
			};
			var args = new SnackBarOptions(messageOptions,
				duration,
				Color.Default,
#if NETSTANDARD1_0
				false,
#else
				CultureInfo.CurrentCulture.TextInfo.IsRightToLeft,
#endif
				actionOptions);
			var snackBar = new SnackBar();
			snackBar.Show(page, args);
			return args.Result.Task;
		}

		public static Task<bool> DisplaySnackBarAsync(this Page page, SnackBarOptions snackBarOptions)
		{
			var snackBar = new SnackBar();
			var arguments = snackBarOptions ?? new SnackBarOptions();
			snackBar.Show(page, arguments);
			return arguments.Result.Task;
		}
	}
}