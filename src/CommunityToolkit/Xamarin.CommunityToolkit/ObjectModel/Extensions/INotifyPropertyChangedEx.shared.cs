using System;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.ObjectModel.Extensions
{
	public static class INotifyPropertyChangedEx
	{
		public static void WeakSubscribe<T>(this INotifyPropertyChanged target, T subscriber, Action<T, object, EventArgs> action)
		{
			var weakSubscriber = new WeakReference(subscriber, false);
			target.PropertyChanged += handler;

			void handler(object sender, PropertyChangedEventArgs e)
			{
				var s = (T)weakSubscriber.Target;
				if (s is null)
				{
					target.PropertyChanged -= handler;
					return;
				}

				action.Invoke(s, sender, e);
			}
		}
	}
}
