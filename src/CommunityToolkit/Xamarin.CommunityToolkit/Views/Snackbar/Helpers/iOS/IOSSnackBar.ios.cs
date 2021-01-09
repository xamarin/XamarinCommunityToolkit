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
		NSTimer timer;

		public List<NativeActionButton> Actions { get; protected set; } = new List<NativeActionButton>();

		public Func<Task> TimeoutAction { get; protected set; }

		public NativeSnackBarAppearance Appearance { get; protected set; } = new NativeSnackBarAppearance();

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

		public static NativeSnackBar MakeSnackBar(string message)
		{
			var snackBar = new NativeSnackBar { Message = message };
			return snackBar;
		}

		public NativeSnackBar SetTimeoutAction(Func<Task> action)
		{
			TimeoutAction = action;
			return this;
		}

		public NativeSnackBar SetDuration(double duration)
		{
			Duration = duration;
			return this;
		}

		public NativeSnackBar SetParentController(UIViewController controller)
		{
			ParentController = controller;
			return this;
		}

		public NativeSnackBar Show()
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
			if (Actions.Count() > 0)
			{
				return new ActionMessageSnackBarView(this);
			}

			return new MessageSnackBarView(this);
		}
	}
}