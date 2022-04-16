using System;using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.Helpers
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class PreserveToolkitAttribute : Attribute
	{
	}
}