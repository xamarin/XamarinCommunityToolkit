using System;using Microsoft.Extensions.Logging;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	sealed class ThumbFrame : Frame
	{
		public ThumbFrame()
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new ThumbFrameRenderer(XCT.Context ?? throw new NullReferenceException());
#endif
			#endregion
		}
	}
}