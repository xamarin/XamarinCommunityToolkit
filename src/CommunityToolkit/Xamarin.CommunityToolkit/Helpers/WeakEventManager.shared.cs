using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

using static System.String;

// Inspired by AsyncAwaitBestPractices.WeakEventManager: https://github.com/brminnick/AsyncAwaitBestPractices
namespace Xamarin.CommunityToolkit.Helpers
{
	/// <summary>
	/// Weak event manager that allows for garbage collection when the EventHandler is still subscribed
	/// </summary>
	/// <typeparam name="TEventArgs">Event args type.</typeparam>
	public class WeakEventManager<TEventArgs>
	{
		readonly Dictionary<string, List<Subscription>> eventHandlers = new Dictionary<string, List<Subscription>>();

		/// <summary>
		/// Adds the event handler
		/// </summary>
		/// <param name="handler">Handler</param>
		/// <param name="eventName">Event name</param>
		public void AddEventHandler(EventHandler<TEventArgs> handler, [CallerMemberName] string eventName = "")
		{
			if (IsNullOrWhiteSpace(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			EventManagerService.AddEventHandler(eventName, handler.Target, handler.GetMethodInfo(), eventHandlers);
		}

		/// <summary>
		/// Adds the event handler
		/// </summary>
		/// <param name="action">Handler</param>
		/// <param name="eventName">Event name</param>
		public void AddEventHandler(Action<TEventArgs> action, [CallerMemberName] string eventName = "")
		{
			if (IsNullOrWhiteSpace(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (action == null)
				throw new ArgumentNullException(nameof(action));

			EventManagerService.AddEventHandler(eventName, action.Target, action.GetMethodInfo(), eventHandlers);
		}

		/// <summary>
		/// Removes the event handler
		/// </summary>
		/// <param name="handler">Handler</param>
		/// <param name="eventName">Event name</param>
		public void RemoveEventHandler(EventHandler<TEventArgs> handler, [CallerMemberName] string eventName = "")
		{
			if (IsNullOrWhiteSpace(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			EventManagerService.RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo(), eventHandlers);
		}

		/// <summary>
		/// Removes the event handler
		/// </summary>
		/// <param name="action">Handler</param>
		/// <param name="eventName">Event name</param>
		public void RemoveEventHandler(Action<TEventArgs> action, [CallerMemberName] string eventName = "")
		{
			if (IsNullOrWhiteSpace(eventName))
				throw new ArgumentNullException(nameof(eventName));

			if (action == null)
				throw new ArgumentNullException(nameof(action));

			EventManagerService.RemoveEventHandler(eventName, action.Target, action.GetMethodInfo(), eventHandlers);
		}

		/// <summary>
		/// Invokes the event EventHandler
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="eventArgs">Event arguments</param>
		/// <param name="eventName">Event name</param>
		public void HandleEvent(object sender, TEventArgs eventArgs, string eventName) => RaiseEvent(sender, eventArgs, eventName);

		/// <summary>
		/// Invokes the event Action
		/// </summary>
		/// <param name="eventArgs">Event arguments</param>
		/// <param name="eventName">Event name</param>
		public void HandleEvent(TEventArgs eventArgs, string eventName) => RaiseEvent(eventArgs, eventName);

		/// <summary>
		/// Invokes the event EventHandler
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="eventArgs">Event arguments</param>
		/// <param name="eventName">Event name</param>
		public void RaiseEvent(object sender, TEventArgs eventArgs, string eventName) =>
			EventManagerService.HandleEvent(eventName, sender, eventArgs, eventHandlers);

		/// <summary>
		/// Invokes the event Action
		/// </summary>
		/// <param name="eventArgs">Event arguments</param>
		/// <param name="eventName">Event name</param>
		public void RaiseEvent(TEventArgs eventArgs, string eventName) =>
			EventManagerService.HandleEvent(eventName, eventArgs, eventHandlers);
	}

	public static class WeakEventManagerExtensions
	{
		/// <summary>
		/// Invokes the event EventHandler
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="eventArgs">Event arguments</param>
		/// <param name="eventName">Event name</param>
		public static void RaiseEvent(this Forms.WeakEventManager weakEventManager, object sender, object eventArgs, string eventName) => weakEventManager.HandleEvent(sender, eventArgs, eventName);
	}
}