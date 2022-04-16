using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
#if __IOS__
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews;
using AnchorView = UIKit.UIView;
#else
using AppKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews;
using AnchorView = AppKit.NSView;
#endif
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers
{
	class NativeSnackBar
	{
		NSTimer? timer;

		public List<NativeSnackButton> Actions { get; protected set; } = new();

		public Func<Task>? TimeoutAction { get; protected set; }

		public NativeSnackBarAppearance Appearance { get; protected set; } = new();

		public TimeSpan Duration { get; protected set; }

		public SnackBarLayout Layout { get; } = new();

		public string Message { get; protected set; } = string.Empty;

		public AnchorView? Anchor { get; protected set; }

		protected BaseSnackBarView? SnackBarView { get; set; }

		public CGRect CornerRadius { get; set; } = new CGRect(10, 10, 10, 10);

		public void Dismiss()
		{
			if (timer != null)
			{
				timer.Invalidate();
				timer.Dispose();
				timer = null;
			}

			SnackBarView?.Dismiss();
		}

		public static NativeSnackBar MakeSnackBar(string message) => new NativeSnackBar { Message = message };

		public NativeSnackBar SetTimeoutAction(Func<Task> action)
		{
			TimeoutAction = action;
			return this;
		}

		public NativeSnackBar SetDuration(TimeSpan duration)
		{
			Duration = duration;
			return this;
		}

		public NativeSnackBar SetAnchor(AnchorView anchor)
		{
			Anchor = anchor;
			return this;
		}

		public NativeSnackBar SetCornerRadius(Thickness cornerRadius)
		{
			CornerRadius = new CGRect(cornerRadius.Left, cornerRadius.Top, cornerRadius.Right, cornerRadius.Bottom);
			return this;
		}

		public NativeSnackBar Show()
		{
			SnackBarView = GetSnackBarView();
			SnackBarView.AnchorView = Anchor;

			SnackBarView.ParentView?.AddSubview(SnackBarView);
#if __IOS__
			SnackBarView.ParentView?.BringSubviewToFront(SnackBarView);
#endif
			SnackBarView.Setup(CornerRadius);

			timer = NSTimer.CreateScheduledTimer(Duration, async t =>
			{
				if (TimeoutAction != null)
					await TimeoutAction();
				Dismiss();
			});

			return this;
		}

		BaseSnackBarView GetSnackBarView() => Actions.Any() ? new ActionMessageSnackBarView(this) : new MessageSnackBarView(this);
	}
}