using System;
using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit
{
	/// <summary>
	/// Platform extension methods.
	/// </summary>
	static class ToolkitPlatform
	{
		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		internal static Context Context
		{
			get
			{
				var page = Forms.Application.Current.MainPage;
				if (page != null)
				{
					var renderer = page.GetRenderer();
					return renderer.View.Context ?? throw new NullReferenceException($"{nameof(Context)} cannot be null");
				}

				// If MainPage isn't set yet, this is the only way to get Context
#pragma warning disable CS0618 // Type or member is obsolete
				return Forms.Forms.Context ?? throw new NullReferenceException($"{nameof(Context)} cannot be null");
#pragma warning restore CS0618 // Type or member is obsolete
			}
		}
	}
}