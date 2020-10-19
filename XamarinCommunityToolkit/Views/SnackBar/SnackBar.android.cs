using Xamarin.Forms;
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
			var snackBar = AndroidSnackBar.Make(view, arguments.MessageOptions.Message, arguments.Duration);
			var snackBarView = snackBar.View;
			snackBarView.SetBackgroundColor(arguments.BackgroundColor.ToAndroid());

			var snackTextView = snackBarView.FindViewById<TextView>(Resource.Id.snackbar_text);
			snackTextView.SetMaxLines(10);
			snackTextView.SetBackgroundColor(arguments.MessageOptions.Foreground.ToAndroid());
			snackTextView.SetTextSize(ComplexUnitType.Pt, (float)arguments.MessageOptions.FontSize);
			snackTextView.LayoutDirection = arguments.IsRtl
				? global::Android.Views.LayoutDirection.Rtl
				: global::Android.Views.LayoutDirection.Inherit;

			foreach (var action in arguments.Actions)
			{
				snackBar.SetAction(action.Text, async v => await action.Action());
				snackBar.SetActionTextColor(action.ForegroundColor.ToAndroid());
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