using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

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
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(v, element.BackgroundColor.G, element.BackgroundColor.B, element.BackgroundColor.A), element.BackgroundColor.R, color.R) },
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(element.BackgroundColor.R, v, element.BackgroundColor.B, element.BackgroundColor.A), element.BackgroundColor.G, color.G) },
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(element.BackgroundColor.R, element.BackgroundColor.G, v, element.BackgroundColor.A), element.BackgroundColor.B, color.B) },
				{ 0, 1, new Animation(v => element.BackgroundColor = new Color(element.BackgroundColor.R, element.BackgroundColor.G, element.BackgroundColor.B, v), element.BackgroundColor.A, color.A) },
			}.Commit(element, nameof(ColorTo), 16, length, easing, (d, b) => animationCompletionSource.SetResult(true));

			return animationCompletionSource.Task;
		}

		public static void AbortAnimations(this VisualElement element, params string[] otherAnimationNames)
		{
			_ = element ?? throw new ArgumentNullException(nameof(element));

			ViewExtensions.CancelAnimations(element);
			element.AbortAnimation(nameof(ColorTo));

			foreach (var name in otherAnimationNames)
				element.AbortAnimation(name);
		}

		public static async Task DisplayToastAsync(this VisualElement visualElement, string message, int durationMilliseconds = 3000)
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
			snackBar.Show(visualElement, args);
			await args.Result.Task;
		}

		public static Task DisplayToastAsync(this VisualElement visualElement, ToastOptions toastOptions)
		{
			var snackBar = new SnackBar();
			var options = new SnackBarOptions
			{
				MessageOptions = toastOptions.MessageOptions,
				Duration = toastOptions.Duration,
				BackgroundColor = toastOptions.BackgroundColor,
				IsRtl = toastOptions.IsRtl
			};

			snackBar.Show(visualElement, options);

			return options.Result.Task;
		}

		public static async Task<bool> DisplaySnackBarAsync(this VisualElement visualElement, string message, string actionButtonText, Func<Task> action, TimeSpan? duration = null)
		{
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
			snackBar.Show(visualElement, options);
			var result = await options.Result.Task;
			return result;
		}

		public static async Task<bool> DisplaySnackBarAsync(this VisualElement visualElement, SnackBarOptions snackBarOptions)
		{
			var snackBar = new SnackBar();
			snackBar.Show(visualElement, snackBarOptions);

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