using Xamarin.Forms;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Android.Util;
#if MONOANDROID10_0
using AndroidSnackBar = Google.Android.Material.Snackbar.Snackbar;
#else
using AndroidSnackBar = Android.Support.Design.Widget.Snackbar;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Page sender, SnackBarOptions arguments)
		{
			var view = Platform.GetRenderer(sender).View;
			var snackBar = AndroidSnackBar.Make(view, arguments.MessageOptions.Message, (int)arguments.Duration.TotalMilliseconds);
			var snackBarView = snackBar.View;
			if (arguments.BackgroundColor != Forms.Color.Default)
			{
				snackBarView.SetBackgroundColor(arguments.BackgroundColor.ToAndroid());
			}

			var snackTextView = snackBarView.FindViewById<TextView>(Resource.Id.snackbar_text);
			snackTextView.SetMaxLines(10);
			if (arguments.MessageOptions.Foreground != Forms.Color.Default)
			{
				snackTextView.SetTextColor(arguments.MessageOptions.Foreground.ToAndroid());
			}

			if (arguments.MessageOptions.Font != Font.Default)
			{
				snackTextView.SetTextSize(ComplexUnitType.Dip, (float)arguments.MessageOptions.Font.FontSize);
				snackTextView.SetTypeface(arguments.MessageOptions.Font.ToTypeface(), TypefaceStyle.Normal);
			}

			snackTextView.LayoutDirection = arguments.IsRtl
				? global::Android.Views.LayoutDirection.Rtl
				: global::Android.Views.LayoutDirection.Inherit;

			foreach (var action in arguments.Actions)
			{
				snackBar.SetAction(action.Text, async v => await action.Action());
				if (action.ForegroundColor != Forms.Color.Default)
				{
					snackBar.SetActionTextColor(action.ForegroundColor.ToAndroid());
				}

				var snackActionButtonView = snackBarView.FindViewById<TextView>(Resource.Id.snackbar_action);
				if (arguments.BackgroundColor != Forms.Color.Default)
				{
					snackActionButtonView.SetBackgroundColor(action.BackgroundColor.ToAndroid());
				}

				if (action.Font != Forms.Font.Default)
				{
					snackActionButtonView.SetTextSize(ComplexUnitType.Dip, (float)action.Font.FontSize);
					snackActionButtonView.SetTypeface(action.Font.ToTypeface(), TypefaceStyle.Normal);
				}

				snackActionButtonView.LayoutDirection = arguments.IsRtl
					? global::Android.Views.LayoutDirection.Rtl
					: global::Android.Views.LayoutDirection.Inherit;
			}

			snackBar.AddCallback(new SnackBarCallback(arguments));
			snackBar.Show();
		}

		class SnackBarCallback : AndroidSnackBar.BaseCallback
		{
			readonly SnackBarOptions arguments;

			public SnackBarCallback(SnackBarOptions arguments) => this.arguments = arguments;

			public override void OnDismissed(Java.Lang.Object transientBottomBar, int e)
			{
				base.OnDismissed(transientBottomBar, e);
				switch (e)
				{
					case DismissEventTimeout:
						arguments.SetResult(false);
						break;
					case DismissEventAction:
						arguments.SetResult(true);
						break;
				}
			}
		}
	}
}
