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
		public static async Task DisplayToastAsync(this Page page, string message, int durationMilliseconds = 3000)
		{
			_ = page ?? throw new ArgumentNullException(nameof(page));

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
			await args.Result.Task;
		}

		public static async Task DisplayToastAsync(this Page page, ToastOptions toastOptions)
		{
			_ = page ?? throw new ArgumentNullException(nameof(page));

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
			await options.Result.Task;
		}

		public static async Task<bool> DisplaySnackBarAsync(this Page page, string message, string actionButtonText, Func<Task> action, int durationMilliseconds = 3000)
		{
			_ = page ?? throw new ArgumentNullException(nameof(page));

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
			return await options.Result.Task;
		}

		public static async Task<bool> DisplaySnackBarAsync(this Page page, SnackBarOptions snackBarOptions)
		{
			_ = page ?? throw new ArgumentNullException(nameof(page));

			var snackBar = new SnackBar();
			var arguments = snackBarOptions ?? new SnackBarOptions();
			snackBar.Show(page, arguments);
			return await arguments.Result.Task;
		}
	}
}