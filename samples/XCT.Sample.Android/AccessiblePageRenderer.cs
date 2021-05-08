using Android.Content;
using Xamarin.CommunityToolkit.Sample.Droid;
using Xamarin.CommunityToolkit.Sample.Pages.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

// This is a temporary fix for an issue in forms that will be fixed in a later release of 5.0
// https://github.com/xamarin/Xamarin.Forms/pull/14089
[assembly: ExportRenderer(typeof(SemanticOrderViewPage), typeof(AccessiblePageRenderer))]

namespace Xamarin.CommunityToolkit.Sample.Droid
{
	public class AccessiblePageRenderer : PageRenderer
	{
		public AccessiblePageRenderer(Context context)
			: base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);
			Clickable = false;
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			DisableFocusableInTouchMode();
		}

		protected override void AttachViewToParent(global::Android.Views.View? child, int index, LayoutParams? @params)
		{
			base.AttachViewToParent(child, index, @params);
			DisableFocusableInTouchMode();
		}

		void DisableFocusableInTouchMode()
		{
			var view = Parent;
			var className = $"{view?.GetType().Name}";

			while (!className.Contains("PlatformRenderer") && view != null)
			{
				view = view.Parent;
				className = $"{view?.GetType().Name}";
			}

			if (view is global::Android.Views.View androidView)
			{
				androidView.Focusable = false;
				androidView.FocusableInTouchMode = false;
			}
		}
	}
}