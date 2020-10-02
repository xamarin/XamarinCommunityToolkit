using Xamarin.Forms;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
#if MONOANDROID10_0
using AndroidSnackbar = Google.Android.Material.Snackbar.Snackbar;
#else
using AndroidSnackbar = global::Android.Support.Design.Widget.Snackbar;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		internal void Show(Page sender, SnackbarArguments arguments)
		{
			var view = Platform.GetRenderer(sender).View;
			var snackbar = AndroidSnackbar.Make(view, arguments.Message, arguments.Duration);
			var snackbarView = snackbar.View;
			var snackTextView = snackbarView.FindViewById<TextView>(Resource.Id.snackbar_text);
			snackTextView.SetMaxLines(10);

			if (!string.IsNullOrEmpty(arguments.ActionButtonText) && arguments.Action != null)
				snackbar.SetAction(arguments.ActionButtonText, async (v) => await arguments.Action());

			snackbar.AddCallback(new SnackbarCallback(arguments));
			snackbar.Show();
		}

		class SnackbarCallback : AndroidSnackbar.BaseCallback
		{
			readonly SnackbarArguments arguments;

			public SnackbarCallback(SnackbarArguments arguments) => this.arguments = arguments;

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