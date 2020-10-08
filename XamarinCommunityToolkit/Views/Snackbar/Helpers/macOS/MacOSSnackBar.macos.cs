using System;
using System.Threading.Tasks;
using Foundation;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackbarViews;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS
{
	class MacOSSnackBar
	{
		NSTimer timer;

		public Func<Task> Action { get; protected set; }

		public Func<Task> TimeoutAction { get; protected set; }

		public string ActionButtonText { get; protected set; }

		public SnackbarAppearance Appearance { get; } = new SnackbarAppearance();

		public double Duration { get; protected set; }

		public SnackbarLayout Layout { get; } = new SnackbarLayout();

		public string Message { get; protected set; }

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

		public static MacOSSnackBar MakeSnackbar(string message)
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
			SnackbarView = GetSnackbarView();

			SnackbarView.ParentView.AddSubview(SnackbarView);

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