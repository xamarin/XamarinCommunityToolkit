using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using static System.String;

namespace Xamarin.CommunityToolkit.Helpers
{
	// Copy from https://github.com/xamarin/Xamarin.Forms/blob/5.0.0/Xamarin.Forms.Core/WeakEventManager.cs
	// TODO: Remove when PR complete: https://github.com/xamarin/Xamarin.Forms/pull/12063
	class WeakEventManager
	{
		readonly Dictionary<string, List<Subscription>> eventHandlers = new Dictionary<string, List<Subscription>>();

		public void AddEventHandler<TEventArgs>(EventHandler<TEventArgs> handler,
			[CallerMemberName] string eventName = null)
			where TEventArgs : EventArgs
		{
			if (IsNullOrEmpty(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			AddEventHandler(eventName, handler.Target, handler.GetMethodInfo());
		}

		public void AddEventHandler(EventHandler handler, [CallerMemberName] string eventName = null)
		{
			if (IsNullOrEmpty(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			AddEventHandler(eventName, handler.Target, handler.GetMethodInfo());
		}

		public void HandleEvent(object sender, object args, string eventName)
		{
			var toRaise = new List<(object subscriber, MethodInfo handler)>();
			var toRemove = new List<Subscription>();

			if (eventHandlers.TryGetValue(eventName, out var target))
			{
				for (var i = 0; i < target.Count; i++)
				{
					var subscription = target[i];
					var isStatic = subscription.Subscriber == null;
					if (isStatic)
					{
						// For a static method, we'll just pass null as the first parameter of MethodInfo.Invoke
						toRaise.Add((null, subscription.Handler));
						continue;
					}

					var subscriber = subscription.Subscriber.Target;

					if (subscriber == null)
						// The subscriber was collected, so there's no need to keep this subscription around
						toRemove.Add(subscription);
					else
						toRaise.Add((subscriber, subscription.Handler));
				}

				for (var i = 0; i < toRemove.Count; i++)
				{
					var subscription = toRemove[i];
					target.Remove(subscription);
				}
			}

			for (var i = 0; i < toRaise.Count; i++)
			{
				(var subscriber, var handler) = toRaise[i];
				handler.Invoke(subscriber, new[] {sender, args});
			}
		}

		public void RemoveEventHandler<TEventArgs>(EventHandler<TEventArgs> handler,
			[CallerMemberName] string eventName = null)
			where TEventArgs : EventArgs
		{
			if (IsNullOrEmpty(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo());
		}

		public void RemoveEventHandler(EventHandler handler, [CallerMemberName] string eventName = null)
		{
			if (IsNullOrEmpty(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo());
		}

		void AddEventHandler(string eventName, object handlerTarget, MethodInfo methodInfo)
		{
			if (!eventHandlers.TryGetValue(eventName, out var targets))
			{
				targets = new List<Subscription>();
				eventHandlers.Add(eventName, targets);
			}

			if (handlerTarget == null)
			{
				// This event handler is a static method
				targets.Add(new Subscription(null, methodInfo));
				return;
			}

			targets.Add(new Subscription(new WeakReference(handlerTarget), methodInfo));
		}

		void RemoveEventHandler(string eventName, object handlerTarget, MemberInfo methodInfo)
		{
			if (!eventHandlers.TryGetValue(eventName, out var subscriptions))
				return;

			for (var n = subscriptions.Count; n > 0; n--)
			{
				var current = subscriptions[n - 1];

				if (current.Subscriber?.Target != handlerTarget || current.Handler.Name != methodInfo.Name)
					continue;

				subscriptions.Remove(current);
				break;
			}
		}

		struct Subscription
		{
			public WeakReference Subscriber { get; }
			public MethodInfo Handler { get; }

			public Subscription(WeakReference subscriber, MethodInfo handler)
			{
				Subscriber = subscriber;
				Handler = handler ?? throw new ArgumentNullException(nameof(handler));
			}
		}
	}
}
