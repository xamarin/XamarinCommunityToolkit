using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Snackbar;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackbarViews;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class IOSSnackBar
	{
		NSTimer timer;

		public Func<Task> Action { get; protected set; }

		public Func<Task> TimeoutAction { get; protected set; }

		public string ActionButtonText { get; protected set; }

		public SnackbarAppearance Appearance { get; } = new SnackbarAppearance();

		public double Duration { get; protected set; }

		public SnackbarLayout Layout { get; } = new SnackbarLayout();

		public string Message { get; protected set; }

		public UIViewController ParentController { get; protected set; }

		protected BaseSnackbarView SnackbarView { get; set; }

		public void Dismiss()
		{
			if (timer != null)
			{
				timer.Invalidate();
				timer.Dispose();
				timer = null;
			}

			SnackbarView?.Dismiss();
		}

		public static IOSSnackBar MakeSnackbar(string message)
		{
			var snackbar = new IOSSnackBar { Message = message };
			return snackbar;
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
			SnackbarView = GetSnackbarView();

			SnackbarView.ParentView.AddSubview(SnackbarView);
			SnackbarView.ParentView.BringSubviewToFront(SnackbarView);

			SnackbarView.Setup();

			timer = NSTimer.CreateScheduledTimer(TimeSpan.FromMilliseconds(Duration), async t =>
			{
				await TimeoutAction();
				Dismiss();
			});

			return this;
		}

		BaseSnackbarView GetSnackbarView()
		{
			if (Action != null && !string.IsNullOrEmpty(ActionButtonText))
			{
				return new ActionMessageSnackbarView(this);
			}

			return new MessageSnackbarView(this);
		}
	}
}