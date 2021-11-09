using System;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.Helpers
{
	static partial class XCT
	{
		static Context? context;
		static int? sdkInt;

		/// <summary>
		/// Gets the <see cref="Context"/>.
		/// </summary>
		internal static Context Context
		{
			get
			{
				var page = Application.Current.MainPage ?? throw new NullReferenceException($"{nameof(Application.MainPage)} cannot be null");
				var renderer = page.GetRenderer();

				if (renderer?.View.Context is not null)
					context = renderer.View.Context;

				return renderer?.View.Context ?? context ?? throw new NullReferenceException($"{nameof(Context)} cannot be null");
			}
		}

		internal static int SdkInt => sdkInt ??= (int)Build.VERSION.SdkInt;
	}
}