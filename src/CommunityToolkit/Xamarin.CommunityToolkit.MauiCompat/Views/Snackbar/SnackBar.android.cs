using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;using Microsoft.Maui.Controls.Platform;using Microsoft.Extensions.DependencyInjection;using Font = Microsoft.Maui.Font;﻿using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Android.Util;
using Android.Graphics.Drawables;
#if ANDROID
using AndroidSnackBar = Google.Android.Material.Snackbar.Snackbar;
#else
using AndroidSnackBar = Android.Support.Design.Widget.Snackbar;
#endif

using Microsoft.Maui.Platform;namespace Xamarin.CommunityToolkit.UI.Views
{
	partial class SnackBar
	{
		internal partial async ValueTask Show(VisualElement sender, SnackBarOptions arguments)
		{
			var renderer = (await GetRendererWithRetries(sender))?.View ?? sender.ToPlatform(sender.Handler.MauiContext) ?? throw new ArgumentException("Provided VisualElement cannot be parent to SnackBar", nameof(sender));
			var snackBar = AndroidSnackBar.Make(renderer, arguments.MessageOptions.Message, (int)arguments.Duration.TotalMilliseconds);
			var snackBarView = snackBar.View;

			if (sender is not Page)
			{
				snackBar.SetAnchorView(renderer);
			}

			if (snackBar.View.Background is GradientDrawable shape)
			{
				if (arguments.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
				{
					shape?.SetColor(arguments.BackgroundColor.ToAndroid().ToArgb());
				}

				var density = renderer.Context?.Resources?.DisplayMetrics?.Density ?? 1;
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

			var snackTextView = snackBarView.FindViewById<TextView>(Xamarin.CommunityToolkit.MauiCompat.Resource.Id.snackbar_text) ?? throw new NullReferenceException();
			snackTextView.SetMaxLines(10);

			if (arguments.MessageOptions.Padding != MessageOptions.DefaultPadding)
			{
				snackBarView.SetPadding((int)arguments.MessageOptions.Padding.Left,
					(int)arguments.MessageOptions.Padding.Top,
					(int)arguments.MessageOptions.Padding.Right,
					(int)arguments.MessageOptions.Padding.Bottom);
			}

			if (arguments.MessageOptions.Foreground != default(Microsoft.Maui.Graphics.Color))
			{
				snackTextView.SetTextColor(arguments.MessageOptions.Foreground.ToAndroid());
			}

			var fontManager = sender.Handler?.MauiContext?.Services.GetRequiredService<IFontManager>();

			if (fontManager is null)
			{
				throw new ArgumentException("Unable to get IFontManager implementation");
			}
if (arguments.MessageOptions.Font != Font.Default)
			{
				if (arguments.MessageOptions.Font.Size > 0)
				{
					snackTextView.SetTextSize(ComplexUnitType.Dip, (float)arguments.MessageOptions.Font.Size);
				}

				snackTextView.SetTypeface(arguments.MessageOptions.Font.ToTypeface(fontManager), TypefaceStyle.Normal);
			}

			snackTextView.LayoutDirection = arguments.IsRtl
				? global::Android.Views.LayoutDirection.Rtl
				: global::Android.Views.LayoutDirection.Inherit;

			foreach (var action in arguments.Actions)
			{
				snackBar.SetAction(action.Text, async _ => await OnActionClick(action, arguments).ConfigureAwait(false));
				if (action.ForegroundColor != default(Microsoft.Maui.Graphics.Color))
				{
					snackBar.SetActionTextColor(action.ForegroundColor.ToAndroid());
				}

				var snackActionButtonView = snackBarView.FindViewById<TextView>(Xamarin.CommunityToolkit.MauiCompat.Resource.Id.snackbar_action) ?? throw new NullReferenceException();
				if (action.BackgroundColor != default(Microsoft.Maui.Graphics.Color))
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
					if (action.Font.Size > 0)
					{
						snackTextView.SetTextSize(ComplexUnitType.Dip, (float)action.Font.Size);
					}

					snackActionButtonView.SetTypeface(action.Font.ToTypeface(fontManager), TypefaceStyle.Normal);
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