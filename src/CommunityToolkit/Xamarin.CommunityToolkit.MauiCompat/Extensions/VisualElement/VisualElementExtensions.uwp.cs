using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;

namespace Xamarin.CommunityToolkit.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="VisualElement"/>.
	/// </summary>
	public static partial class VisualElementExtensions
	{
		/// <summary>
		/// <see cref="VisualElement"/> cleanup object to dispose and
		/// destroy resources.
		/// </summary>
		/// <param name="self">
		/// The <see cref="VisualElement"/> to cleanup.
		/// </param>
		/// <remarks>
		/// This extension method is ported from Xamarin.Forms and should remain in sync.
		/// </remarks>
		internal static void Cleanup(this VisualElement self)
		{
			if (self == null)
				throw new ArgumentNullException("self");

			var renderer = Platform.GetRenderer(self);

			foreach (var element in self.Descendants())
			{
				var visual = element as VisualElement;
				if (visual == null)
					continue;

				var childRenderer = Platform.GetRenderer(visual);
				if (childRenderer != null)
				{
					childRenderer.Dispose();
					Platform.SetRenderer(visual, null);
				}
			}

			if (renderer != null)
			{
				renderer.Dispose();
				Platform.SetRenderer(self, null);
			}
		}
	}
}