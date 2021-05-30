using System;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
{
	sealed class ThumbFrame : Frame
	{
		public ThumbFrame()
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new ThumbFrameRenderer(ToolkitPlatform.Context ?? throw new NullReferenceException());
#endif
			#endregion
		}
	}
}