using System;

namespace Xamarin.CommunityToolkit.Effects
{
	[Flags]
	public enum FullScreenMode
	{
		Disabled = -1,
		Enabled = 1,
		ImmersiveDroid = 2,
		StickyImmersiveDroid = 3,
		LeanBackDroid = 4
	}
}
