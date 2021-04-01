﻿using System;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.ObjectModel.Extensions
{
	public static class INotifyPropertyChangedExtension
	{
		[Obsolete("This method shouldn't be used week events (like one in ObservableObject). This method is deprecated due to high probability of misuse.")]
		public static void WeakSubscribe<T>(this INotifyPropertyChanged target, T subscriber, Action<T, object?, PropertyChangedEventArgs> action)
		{
			_ = target ?? throw new ArgumentNullException(nameof(target));
			if (subscriber == null || action == null)
			{
				return;
			}

			var weakSubscriber = new WeakReference(subscriber, false);
			target.PropertyChanged += handler;

			void handler(object? sender, PropertyChangedEventArgs e)
			{
				var s = (T?)weakSubscriber.Target;
				if (s == null)
				{
					target.PropertyChanged -= handler;
					return;
				}

				action(s, sender, e);
			}
		}
	}
}
