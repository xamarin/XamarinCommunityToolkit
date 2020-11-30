using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	sealed class ThumbFrame : Frame
	{
		public ThumbFrame()
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new ThumbFrameRenderer(null);
#endif
		}
	}
}