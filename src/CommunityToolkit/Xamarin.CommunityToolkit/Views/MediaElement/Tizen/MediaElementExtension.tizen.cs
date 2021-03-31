using Xamarin.Forms;
using Tizen.Multimedia;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public static class MediaElementExtension
	{
		public static PlayerDisplayMode ToNative(this DisplayAspectMode mode)
		{
			return mode switch
			{
				DisplayAspectMode.AspectFill => PlayerDisplayMode.CroppedFull,
				DisplayAspectMode.AspectFit => PlayerDisplayMode.LetterBox,
				DisplayAspectMode.Fill => PlayerDisplayMode.FullScreen,
				DisplayAspectMode.OrignalSize => PlayerDisplayMode.OriginalOrFull,
				_ => PlayerDisplayMode.LetterBox
			};
		}

		public static DisplayAspectMode ToDisplayAspectMode(this Aspect aspect)
		{
			return aspect switch
			{
				Aspect.AspectFill => DisplayAspectMode.AspectFill,
				Aspect.AspectFit => DisplayAspectMode.AspectFit,
				Aspect.Fill => DisplayAspectMode.Fill,
				_ => DisplayAspectMode.AspectFit
			};
		}
	}
}
