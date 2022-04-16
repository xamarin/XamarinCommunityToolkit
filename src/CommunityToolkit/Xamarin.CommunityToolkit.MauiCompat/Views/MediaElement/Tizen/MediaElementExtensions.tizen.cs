using System;using Microsoft.Extensions.Logging;
using Tizen.Multimedia;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public static class MediaElementExtensions
	{
		public static PlayerDisplayMode ToNative(this DisplayAspectMode mode) => mode switch
		{
			DisplayAspectMode.AspectFill => PlayerDisplayMode.CroppedFull,
			DisplayAspectMode.AspectFit => PlayerDisplayMode.LetterBox,
			DisplayAspectMode.Fill => PlayerDisplayMode.FullScreen,
			DisplayAspectMode.OrignalSize => PlayerDisplayMode.OriginalOrFull,
			_ => throw new NotImplementedException()
		};

		public static DisplayAspectMode ToDisplayAspectMode(this Aspect aspect) => aspect switch
		{
			Aspect.AspectFill => DisplayAspectMode.AspectFill,
			Aspect.AspectFit => DisplayAspectMode.AspectFit,
			Aspect.Fill => DisplayAspectMode.Fill,
			_ => throw new NotImplementedException()
		};
	}
}