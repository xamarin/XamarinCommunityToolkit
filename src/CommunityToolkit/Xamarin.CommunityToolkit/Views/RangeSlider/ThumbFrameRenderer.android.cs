using Android.Content;
using Android.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.FastRenderers;

[assembly: ExportRenderer(typeof(ThumbFrame), typeof(ThumbFrameRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	sealed class ThumbFrameRenderer : FrameRenderer
	{
		public ThumbFrameRenderer(Context context)
			: base(context)
		{
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			switch (e.ActionMasked)
			{
				case MotionEventActions.Down:
					Parent?.RequestDisallowInterceptTouchEvent(true);
					break;
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
					Parent?.RequestDisallowInterceptTouchEvent(false);
					break;
			}
			return base.OnTouchEvent(e);
		}
	}
}