using System;
using Xamarin.CommunityToolkit.Helpers;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.WeakEventManagerTests
{
	public class BaseWeakEventManagerTests
	{
		protected event EventHandler TestEvent
		{
			add => TestWeakEventManager.AddEventHandler(value);
			remove => TestWeakEventManager.RemoveEventHandler(value);
		}

		protected event EventHandler<string> TestStringEvent
		{
			add => TestStringWeakEventManager.AddEventHandler(value);
			remove => TestStringWeakEventManager.RemoveEventHandler(value);
		}

		protected WeakEventManager TestWeakEventManager { get; } = new WeakEventManager();

		protected WeakEventManager<string> TestStringWeakEventManager { get; } = new WeakEventManager<string>();
	}
}