using System;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[Flags]
	public enum TextDecorationFlags
	{
		None = 0,
		TrimStart = 1,
		TrimEnd = 2,
		Trim = 3,
		NullToEmpty = 4,
		NormalizeWhiteSpace = 8
	}
}