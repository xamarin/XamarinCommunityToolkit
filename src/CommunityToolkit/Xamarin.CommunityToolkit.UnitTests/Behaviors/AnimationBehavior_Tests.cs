using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class AnimationBehavior_Tests
	{
		[SetUp]
		public void SetUp() => Device.PlatformServices = new MockPlatformServices();

		[Test]
		public void CommandIsInvokedOnlyOneTimePerEvent()
		{
			var commandInvokeCount = 0;
			var mockView = new MockEventView
			{
				Behaviors =
				{
					new AnimationBehavior
					{
						EventName = nameof(MockEventView.Event),
						Command = new Command(() => ++commandInvokeCount)
					}
				}
			};
			mockView.Event += (s, e) => { };
			mockView.InvokeEvent();

			Assert.AreEqual(commandInvokeCount, 1);
		}
	}
}
