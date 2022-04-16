using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Effects.Semantic
{
	public enum SemanticInclusion
	{
		Default = 0,
		Include = 1,
		Exclude = 2,
		ExcludeWithChildren = 3,
	}
}