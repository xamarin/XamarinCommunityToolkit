using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public static class LayoutExtensions
	{
		public static IReadOnlyList<Element> GetChildren(this ILayoutController source) => source.Children;
	}
}