using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using Android.Content;
using Android.OS;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;

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