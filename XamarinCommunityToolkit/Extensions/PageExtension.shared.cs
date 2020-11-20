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
		public static Task DisplayToastAsync(this Page page, string message, int durationMilliseconds = 3000)
		{
			var messageOptions = new MessageOptions { Message = message };
			var args = new SnackBarOptions
			{
				MessageOptions = messageOptions,
				Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
#if NETSTANDARD1_0
				IsRtl = false,
#else
				IsRtl = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft,
#endif
			};
			var snackBar = new SnackBar();
			snackBar.Show(page, args);
			return args.Result.Task;
		}

		public static Task DisplayToastAsync(this Page page, ToastOptions toastOptions)
		{
			var snackBar = new SnackBar();
			var arguments = toastOptions ?? new ToastOptions();
			var options = new SnackBarOptions
			{
				MessageOptions = arguments.MessageOptions,
				Duration = arguments.Duration,
				BackgroundColor = arguments.BackgroundColor,
				IsRtl = arguments.IsRtl
			};
			snackBar.Show(page, options);
			return options.Result.Task;
		}

		public static Task<bool> DisplaySnackBarAsync(this Page page, string message, string actionButtonText, Func<Task> action, int durationMilliseconds = 3000)
		{
			var messageOptions = new MessageOptions { Message = message };
			var actionOptions = new List<SnackBarActionOptions>
			{
				new SnackBarActionOptions
				{
					Text = actionButtonText, Action = action
				}
			};
			var options = new SnackBarOptions
			{
				MessageOptions = messageOptions,
				Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
				Actions = actionOptions,
#if NETSTANDARD1_0
				IsRtl = false,
#else
				IsRtl = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft,
#endif
			};
			var snackBar = new SnackBar();
			snackBar.Show(page, options);
			return options.Result.Task;
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