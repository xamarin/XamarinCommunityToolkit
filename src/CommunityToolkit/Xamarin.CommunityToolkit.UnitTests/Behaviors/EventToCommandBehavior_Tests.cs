using System;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class EventToCommandBehavior_Tests
	{
		[SetUp]
		public void SetUp() => Device.PlatformServices = new MockPlatformServices();

		[Test]
		public void ArgumentExceptionIfSpecifiedEventDoesNotExist()
		{
			var listView = new ListView();
			var behavior = new EventToCommandBehavior
			{
				EventName = "Wrong Event Name"
			};
			Assert.Throws<ArgumentException>(() => listView.Behaviors.Add(behavior));
		}

		[Test]
		public void NoExceptionIfSpecifiedEventExists()
		{
			var listView = new ListView();
			var behavior = new EventToCommandBehavior
			{
				EventName = nameof(ListView.ItemTapped)
			};
			listView.Behaviors.Add(behavior);
		}

		[Test]
		public void NoExceptionIfAttachedToPage()
		{
			var page = new ContentPage();
			var behavior = new EventToCommandBehavior
			{
				EventName = nameof(Page.Appearing)
			};
			page.Behaviors.Add(behavior);
		}
	}
}