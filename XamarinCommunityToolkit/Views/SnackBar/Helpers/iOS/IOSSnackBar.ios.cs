using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class IOSSnackBar
	{
		NSTimer timer;

		public Func<Task> Action { get; protected set; }

		public Func<Task> TimeoutAction { get; protected set; }

		public string ActionButtonText { get; protected set; }

		public SnackBarAppearance Appearance { get; } = new SnackBarAppearance();

		public double Duration { get; protected set; }

		public SnackBarLayout Layout { get; } = new SnackBarLayout();

		public string Message { get; protected set; }

		public UIViewController ParentController { get; protected set; }

		protected BaseSnackBarView SnackBarView { get; set; }

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

		public static IOSSnackBar MakeSnackBar(string message)
		{
			var snackBar = new IOSSnackBar { Message = message };
			return snackBar;
		}

		public IOSSnackBar SetAction(Func<Task> action)
		{
			Action = action;
			return this;
		}

		public IOSSnackBar SetTimeoutAction(Func<Task> action)
		{
			TimeoutAction = action;
			return this;
		}

		public IOSSnackBar SetActionButtonText(string title)
		{
			ActionButtonText = title;
			return this;
		}

		public IOSSnackBar SetDuration(double duration)
		{
			Duration = duration;
			return this;
		}

		public IOSSnackBar SetParentController(UIViewController controller)
		{
			ParentController = controller;
			return this;
		}

		public IOSSnackBar Show()
		{
			SnackBarView = GetSnackBarView();

			SnackBarView.ParentView.AddSubview(SnackBarView);
			SnackBarView.ParentView.BringSubviewToFront(SnackBarView);

			SnackBarView.Setup();

			timer = NSTimer.CreateScheduledTimer(TimeSpan.FromMilliseconds(Duration), async t =>
			{
				await TimeoutAction();
				Dismiss();
			});

			return this;
		}

		BaseSnackBarView GetSnackBarView()
		{
			if (Action != null && !string.IsNullOrEmpty(ActionButtonText))
			{
				return new ActionMessageSnackBarView(this);
			}

			return new MessageSnackBarView(this);
		}
	}
}