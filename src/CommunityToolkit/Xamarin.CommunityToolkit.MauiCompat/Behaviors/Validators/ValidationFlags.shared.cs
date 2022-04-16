using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[Flags]
	public enum ValidationFlags
	{
		None = 0,
		ValidateOnAttaching = 1,
		ValidateOnFocusing = 2,
		ValidateOnUnfocusing = 4,
		ValidateOnValueChanging = 8,
		ForceMakeValidWhenFocused = 16
	}
}