using System;
using System.Collections.Specialized;

namespace Xamarin.CommunityToolkit.ObjectModel.Extensions
{
	public static class INotifyCollectionChangedEx
	{
		public static void WeakSubscribe<T>(this INotifyCollectionChanged target, T subscriber, Action<T, object, EventArgs> action)
		{
			_ = target ?? throw new ArgumentNullException(nameof(target));
			if (subscriber == null || action == null)
			{
				return;
			}

			var weakSubscriber = new WeakReference(subscriber, false);
			target.CollectionChanged += handler;

			void handler(object sender, NotifyCollectionChangedEventArgs e)
			{
				var s = (T)weakSubscriber.Target;
				if (s == null)
				{
					target.CollectionChanged -= handler;
					return;
				}

				action(s, sender, e);
			}
		}
	}
}
