using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.CommunityToolkit.Extensions;
#if MONOANDROID10_0
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;
using AppCompatActivity = AndroidX.AppCompat.App.AppCompatActivity;
using AndroidSnackbar = Google.Android.Material.Snackbar.Snackbar;
#else
using AppCompatAlertDialog = global::Android.Support.V7.App.AlertDialog;
using AppCompatActivity = global::Android.Support.V7.App.AppCompatActivity;
using AndroidSnackbar = global::Android.Support.Design.Widget.Snackbar;
#endif

namespace Xamarin.CommunityToolkit.Actions.Snackbar
{
	using global::Android.App;

	public class SnackBar
	{
		public SnackBar(Activity subscriber)
		{
			MessagingCenter.Subscribe<Page, SnackbarArguments>(subscriber, PageExtension.SnackbarSignalName, OnSnackbarRequested);
		}

		void OnSnackbarRequested(Page sender, SnackbarArguments arguments)
		{
			var view = Platform.GetRenderer(sender).View;
			var snackbar = AndroidSnackbar.Make(view, arguments.Message, arguments.Duration);
			var snackbarView = snackbar.View;
			var snackTextView = snackbarView.FindViewById<TextView>(Resource.Id.snackbar_text);
			snackTextView.SetMaxLines(10);

			if (!string.IsNullOrEmpty(arguments.ActionButtonText) && arguments.Action != null)
			{
				snackbar.SetAction(arguments.ActionButtonText, async (v) => await arguments.Action());
			}

			snackbar.AddCallback(new SnackbarCallback(arguments));
			snackbar.Show();
		}
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
