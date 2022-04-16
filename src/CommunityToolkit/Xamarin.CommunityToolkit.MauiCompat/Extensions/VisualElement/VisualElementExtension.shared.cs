using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="VisualElement"/>.
	/// </summary>
	public static partial class VisualElementExtension
	{
		public static Task<bool> ColorTo(this VisualElement element, Color color, uint length = 250u, Easing? easing = null)
		{
			_ = element ?? throw new ArgumentNullException(nameof(element));

			var animationCompletionSource = new TaskCompletionSource<bool>();

			new Animation
			{
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color((float)v, element.BackgroundColor.Green, element.BackgroundColor.Blue, element.BackgroundColor.Alpha), element.BackgroundColor.Red, color.Red) },
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(element.BackgroundColor.Red, (float)v, element.BackgroundColor.Blue, element.BackgroundColor.Alpha), element.BackgroundColor.Green, color.Green) },
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(element.BackgroundColor.Red, element.BackgroundColor.Green, (float)v, element.BackgroundColor.Alpha), element.BackgroundColor.Blue, color.Blue) },
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(element.BackgroundColor.Red, element.BackgroundColor.Green, element.BackgroundColor.Blue, (float)v), element.BackgroundColor.Alpha, color.Alpha) },
			}.Commit(element, nameof(ColorTo), 16, length, easing, (d, b) => animationCompletionSource.SetResult(true));

			return animationCompletionSource.Task;
		}

		public static void AbortAnimations(this VisualElement element, params string[] otherAnimationNames)
		{
			_ = element ?? throw new ArgumentNullException(nameof(element));

			Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(element);
			element.AbortAnimation(nameof(ColorTo));

			if (otherAnimationNames == null)
				return;

			foreach (var name in otherAnimationNames)
				element.AbortAnimation(name);
		}

		/// <summary>
		/// Display toast with the default visual configuration
		/// </summary>
		/// <param name="visualElement">Toast anchor</param>
		/// <param name="message">Toast text</param>
		/// <param name="durationMilliseconds">Toast duration (milliseconds)</param>
		/// <returns>Task</returns>
		public static async Task DisplayToastAsync(this VisualElement visualElement, string message, int durationMilliseconds = 3000)
		{
			_ = visualElement ?? throw new ArgumentNullException(nameof(visualElement));

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
			await snackBar.Show(visualElement, args);
			await args.Result.Task;
		}

		/// <summary>
		/// Display toast with the custom options
		/// </summary>
		/// <param name="visualElement">Toast anchor</param>
		/// <param name="toastOptions">Toast options</param>
		/// <returns>Task</returns>
		public static async Task DisplayToastAsync(this VisualElement visualElement, ToastOptions toastOptions)
		{
			_ = visualElement ?? throw new ArgumentNullException(nameof(visualElement));

			var snackBar = new SnackBar();
			var options = new SnackBarOptions
			{
				MessageOptions = toastOptions.MessageOptions,
				CornerRadius = toastOptions.CornerRadius,
				Duration = toastOptions.Duration,
				BackgroundColor = toastOptions.BackgroundColor,
				IsRtl = toastOptions.IsRtl
			};

			await snackBar.Show(visualElement, options);

			await options.Result.Task;
		}

		/// <summary>
		/// Display snackbar with the default visual configuration
		/// </summary>
		/// <param name="visualElement">Anchor element</param>
		/// <param name="message">Text of the snackbar</param>
		/// <param name="actionButtonText">Text of the snackbar button</param>
		/// <param name="action">Action of the snackbar button</param>
		/// <param name="duration">Snackbar duration</param>
		/// <returns>True if snackbar action is executed. False if snackbar is closed by timeout</returns>
		public static async Task<bool> DisplaySnackBarAsync(this VisualElement visualElement, string message, string actionButtonText, Func<Task> action, TimeSpan? duration = null)
		{
			_ = visualElement ?? throw new ArgumentNullException(nameof(visualElement));

			var messageOptions = new MessageOptions { Message = message };
			var actionOptions = new List<SnackBarActionOptions>
			{
				new ()
				{
					Text = actionButtonText, Action = action
				}
			};
			var options = new SnackBarOptions
			{
				MessageOptions = messageOptions,
				Duration = duration ?? TimeSpan.FromSeconds(3),
				Actions = actionOptions,
#if NETSTANDARD1_0
				IsRtl = false,
#else
				IsRtl = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft,
#endif
			};
			var snackBar = new SnackBar();
			await snackBar.Show(visualElement, options);
			var result = await options.Result.Task;
			return result;
		}

		/// <summary>
		/// Display snackbar with custom configuration
		/// </summary>
		/// <param name="visualElement">Snackbar anchor</param>
		/// <param name="snackBarOptions">Snackbar options</param>
		/// <returns>True if snackbar action is executed. False if snackbar is closed by timeout</returns>
		public static async Task<bool> DisplaySnackBarAsync(this VisualElement visualElement, SnackBarOptions snackBarOptions)
		{
			_ = visualElement ?? throw new ArgumentNullException(nameof(visualElement));

			var snackBar = new SnackBar();
			await snackBar.Show(visualElement, snackBarOptions);

			var result = await snackBarOptions.Result.Task;
			return result;
		}

		internal static bool TryFindParentElementWithParentOfType<T>(this VisualElement? element, out VisualElement? result, out T? parent) where T : VisualElement
		{
			result = null;
			parent = null;

			while (element?.Parent != null)
			{
				if (element.Parent is not T parentElement)
				{
					element = element.Parent as VisualElement;
					continue;
				}

				result = element;
				parent = parentElement;

				return true;
			}

			return false;
		}

		internal static bool TryFindParentOfType<T>(this VisualElement? element, out T? parent) where T : VisualElement
			=> TryFindParentElementWithParentOfType(element, out _, out parent);
	}
}