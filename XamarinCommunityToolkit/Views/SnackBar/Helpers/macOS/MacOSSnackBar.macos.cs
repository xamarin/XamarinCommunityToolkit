using System;
using System.Threading.Tasks;
using Foundation;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS
{
	class MacOSSnackBar
	{
		NSTimer timer;

		public Func<Task> Action { get; protected set; }

		public Func<Task> TimeoutAction { get; protected set; }

		public string ActionButtonText { get; protected set; }

		public SnackBarAppearance Appearance { get; } = new SnackBarAppearance();

		public double Duration { get; protected set; }

		public SnackBarLayout Layout { get; } = new SnackBarLayout();

		public string Message { get; protected set; }

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

		public static MacOSSnackBar MakeSnackBar(string message)
		{
			var snackbar = new MacOSSnackBar { Message = message };
			return snackbar;
		}

		public MacOSSnackBar SetAction(Func<Task> action)
		{
			Action = action;
			return this;
		}

		public MacOSSnackBar SetTimeoutAction(Func<Task> action)
		{
			TimeoutAction = action;
			return this;
		}

		public MacOSSnackBar SetActionButtonText(string title)
		{
			ActionButtonText = title;
			return this;
		}

		public MacOSSnackBar SetDuration(double duration)
		{
			Duration = duration;
			return this;
		}

		public MacOSSnackBar Show()
		{
			SnackBarView = GetSnackBarView();

			SnackBarView.ParentView.AddSubview(SnackBarView);

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