using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Extensions
{
	static class JavaObjectExtensions
	{
		public static bool IsDisposed(this Java.Lang.Object obj)
			=> obj.Handle == IntPtr.Zero;

		public static bool IsAlive(this Java.Lang.Object obj)
			=> obj != null && !obj.IsDisposed();

		public static bool IsDisposed(this global::Android.Runtime.IJavaObject obj)
			=> obj.Handle == IntPtr.Zero;

		public static bool IsAlive(this global::Android.Runtime.IJavaObject obj)
			=> obj != null && !obj.IsDisposed();
	}
}