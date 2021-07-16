using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Android.Util;
using Android.Graphics.Drawables;
#if MONOANDROID10_0
using AndroidSnackBar = Google.Android.Material.Snackbar.Snackbar;
#else
using AndroidSnackBar = Android.Support.Design.Widget.Snackbar;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal async ValueTask Show(VisualElement sender, SnackBarOptions arguments)
		{
			var renderer = await GetRendererWithRetries(sender) ?? throw new ArgumentException("Provided VisualElement cannot be parent to SnackBar", nameof(sender));
			var snackBar = AndroidSnackBar.Make(renderer.View, arguments.MessageOptions.Message, (int)arguments.Duration.TotalMilliseconds);
			var snackBarView = snackBar.View;

			if (sender is not Page)
			{
				snackBar.SetAnchorView(renderer.View);
			}

			if (snackBar.View.Background is GradientDrawable shape)
			{
				if (arguments.BackgroundColor != Forms.Color.Default)
				{
					shape?.SetColor(arguments.BackgroundColor.ToAndroid().ToArgb());
				}

				var density = renderer.View.Context?.Resources?.DisplayMetrics?.Density ?? 1;
				var defaultAndroidCornerRadius = 4 * density;
				arguments.CornerRadius = new Thickness(arguments.CornerRadius.Left * density,
					arguments.CornerRadius.Top * density,
					arguments.CornerRadius.Right * density,
					arguments.CornerRadius.Bottom * density);
				if (arguments.CornerRadius != new Thickness(defaultAndroidCornerRadius, defaultAndroidCornerRadius, defaultAndroidCornerRadius, defaultAndroidCornerRadius))
				{
					shape?.SetCornerRadii(new[]
					{
						(float)arguments.CornerRadius.Left, (float)arguments.CornerRadius.Left,
						(float)arguments.CornerRadius.Top, (float)arguments.CornerRadius.Top,
						(float)arguments.CornerRadius.Right, (float)arguments.CornerRadius.Right,
						(float)arguments.CornerRadius.Bottom, (float)arguments.CornerRadius.Bottom
					});
				}

				snackBarView.SetBackground(shape);
			}

			var snackTextView = snackBarView.FindViewById<TextView>(Resource.Id.snackbar_text) ?? throw new NullReferenceException();
			snackTextView.SetMaxLines(10);

			if (arguments.MessageOptions.Padding != MessageOptions.DefaultPadding)
			{
				snackBarView.SetPadding((int)arguments.MessageOptions.Padding.Left,
					(int)arguments.MessageOptions.Padding.Top,
					(int)arguments.MessageOptions.Padding.Right,
					(int)arguments.MessageOptions.Padding.Bottom);
			}

			if (arguments.MessageOptions.Foreground != Forms.Color.Default)
			{
				snackTextView.SetTextColor(arguments.MessageOptions.Foreground.ToAndroid());
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				if (arguments.MessageOptions.Font.FontSize > 0)
				{
					snackTextView.SetTextSize(ComplexUnitType.Dip, (float)arguments.MessageOptions.Font.FontSize);
				}

				snackTextView.SetTypeface(arguments.MessageOptions.Font.ToTypeface(), TypefaceStyle.Normal);
			}

			snackTextView.LayoutDirection = arguments.IsRtl
				? global::Android.Views.LayoutDirection.Rtl
				: global::Android.Views.LayoutDirection.Inherit;

			foreach (var action in arguments.Actions)
			{
				snackBar.SetAction(action.Text, async _ =>
				{
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
				});
				if (action.ForegroundColor != Forms.Color.Default)
				{
					snackBar.SetActionTextColor(action.ForegroundColor.ToAndroid());
				}

				var snackActionButtonView = snackBarView.FindViewById<TextView>(Resource.Id.snackbar_action) ?? throw new NullReferenceException();
				if (arguments.BackgroundColor != Forms.Color.Default)
				{
					snackActionButtonView.SetBackgroundColor(action.BackgroundColor.ToAndroid());
				}

				if (action.Padding != SnackBarActionOptions.DefaultPadding)
				{
					snackActionButtonView.SetPadding((int)action.Padding.Left,
						(int)action.Padding.Top,
						(int)action.Padding.Right,
						(int)action.Padding.Bottom);
				}

				if (action.Font != Font.Default)
				{
					if (action.Font.FontSize > 0)
					{
						snackTextView.SetTextSize(ComplexUnitType.Dip, (float)action.Font.FontSize);
					}

					snackActionButtonView.SetTypeface(action.Font.ToTypeface(), TypefaceStyle.Normal);
				}

				snackActionButtonView.LayoutDirection = arguments.IsRtl
					? global::Android.Views.LayoutDirection.Rtl
					: global::Android.Views.LayoutDirection.Inherit;
			}

			snackBar.AddCallback(new SnackBarCallback(arguments));
			snackBar.Show();
		}

		/// <summary>
		/// Tries to get renderer multiple times since it can be null while switching tabs in Shell.
		/// See this bug for more info: https://github.com/xamarin/Xamarin.Forms/issues/13950
		/// </summary>
		static async Task<IVisualElementRenderer?> GetRendererWithRetries(VisualElement element, int retryCount = 5)
		{
			var renderer = Platform.GetRenderer(element);
			if (renderer != null || retryCount <= 0)
				return renderer;

			await Task.Delay(50);
			return await GetRendererWithRetries(element, retryCount - 1);
		}

		class SnackBarCallback : AndroidSnackBar.BaseCallback
		{
			readonly SnackBarOptions arguments;

			public SnackBarCallback(SnackBarOptions arguments) => this.arguments = arguments;

			public override void OnDismissed(Java.Lang.Object transientBottomBar, int e)
			{
				base.OnDismissed(transientBottomBar, e);

				if (e == DismissEventAction)
					return;

				arguments.SetResult(false);
			}
		}
	}
}