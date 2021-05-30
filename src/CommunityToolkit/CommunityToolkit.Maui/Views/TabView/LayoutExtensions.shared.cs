using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
{
	public static class LayoutExtensions
	{
		public static IReadOnlyList<Element> GetChildren(this ILayoutController source)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			return source.Children;
		}
	}
}