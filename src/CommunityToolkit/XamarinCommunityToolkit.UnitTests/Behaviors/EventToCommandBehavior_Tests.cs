using System;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class EventToCommandBehavior_Tests
	{
		public EventToCommandBehavior_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Fact]
		public void ArgumentExceptionIfSpecifiedEventDoesNotExist()
		{
			var listView = new ListView();
			var behavior = new EventToCommandBehavior
			{
				EventName = "Wrong Event Name"
			};
			Assert.Throws<ArgumentException>(() => listView.Behaviors.Add(behavior));
		}

		[Fact]
		public void NoExceptionIfSpecifiedEventExists()
		{
			var listView = new ListView();
			var behavior = new EventToCommandBehavior
			{
				EventName = nameof(ListView.ItemTapped)
			};
			listView.Behaviors.Add(behavior);
		}

		[Fact]
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