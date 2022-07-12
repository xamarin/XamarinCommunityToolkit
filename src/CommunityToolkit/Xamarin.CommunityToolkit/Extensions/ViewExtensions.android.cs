using Android.OS;
using Xamarin.CommunityToolkit.Helpers;
using AView = Android.Views.View;

namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	public static class ViewExtensions
	{
		internal static void MaybeRequestLayout(this AView view)
		{
			var isInLayout = false;
			if (XCT.SdkInt >= 18)
				isInLayout = view.IsInLayout;

			if (!isInLayout && !view.IsLayoutRequested)
				view.RequestLayout();
		}
	}
}