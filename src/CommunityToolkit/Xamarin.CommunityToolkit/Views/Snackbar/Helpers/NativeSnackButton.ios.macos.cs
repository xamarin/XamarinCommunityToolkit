using System;
using System.Threading.Tasks;
#if __IOS__
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS;
#elif __MACOS__
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS;
#endif

namespace Xamarin.CommunityToolkit.Views.Snackbar.Helpers
{
	class NativeActionButton
	{
		public Func<Task> Action { get; protected set; }

		public string ActionButtonText { get; protected set; }

		public NativeSnackButtonAppearance Appearance { get; protected set; } = new NativeSnackButtonAppearance();

		public NativeActionButton SetAction(Func<Task> action)
		{
			Action = action;
			return this;
		}

		public NativeActionButton SetActionButtonText(string title)
		{
			ActionButtonText = title;
			return this;
		}
	}
}
