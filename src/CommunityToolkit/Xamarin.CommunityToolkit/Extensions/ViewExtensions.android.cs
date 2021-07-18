using Android.OS;
using AView = Android.Views.View;

namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	public static class ViewExtensions
	{
		internal static void MaybeRequestLayout(this AView view)
		{
			var isInLayout = false;
			if ((int)Build.VERSION.SdkInt >= 18)
				isInLayout = view.IsInLayout;

			if (!isInLayout && !view.IsLayoutRequested)
				view.RequestLayout();
		}
	}
}