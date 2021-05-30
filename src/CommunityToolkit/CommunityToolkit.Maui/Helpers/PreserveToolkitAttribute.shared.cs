using System;
using System.ComponentModel;

namespace CommunityToolkit.Maui.Helpers
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class PreserveToolkitAttribute : Attribute
	{
	}
}