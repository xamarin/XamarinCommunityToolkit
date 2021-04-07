using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

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

		[Test]
		public void AnimateCommandStartsAnimation()
		{
			var mockAnimation = new MockAnimationType();

			var behavior = new AnimationBehavior
			{
				AnimationType = mockAnimation
			};

			new MockEventView
			{
				Behaviors =
				{
					behavior
				}
			};

			behavior.AnimateCommand.Execute(null);

			Assert.That(mockAnimation.HasAnimated, Is.True);
		}

		class MockAnimationType : AnimationBase
		{
			public bool HasAnimated { get; private set; }

			protected override uint DefaultDuration { get; set; } = 50;

			public override Task Animate(View? view)
			{
				HasAnimated = true;

				return Task.CompletedTask;
			}
		}
	}
}
