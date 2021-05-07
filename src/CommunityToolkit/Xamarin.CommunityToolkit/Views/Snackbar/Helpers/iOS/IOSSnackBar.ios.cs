using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class NativeSnackBar
	{
		NSTimer? timer;

		public List<NativeSnackButton> Actions { get; protected set; } = new List<NativeSnackButton>();

		public Func<Task>? TimeoutAction { get; protected set; }

		public NativeSnackBarAppearance Appearance { get; protected set; } = new NativeSnackBarAppearance();

		public TimeSpan Duration { get; protected set; }

		public SnackBarLayout Layout { get; } = new SnackBarLayout();

		public string Message { get; protected set; } = string.Empty;

		public UIView? Anchor { get; protected set; }

		protected BaseSnackBarView? SnackBarView { get; set; }

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

		public NativeSnackBar SetAnchor(UIView anchor)
		{
			Anchor = anchor;
			return this;
		}

		public NativeSnackBar Show()
		{
			SnackBarView = GetSnackBarView();
			SnackBarView.AnchorView = Anchor;

			SnackBarView.ParentView?.AddSubview(SnackBarView);
			SnackBarView.ParentView?.BringSubviewToFront(SnackBarView);

			SnackBarView.Setup();

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