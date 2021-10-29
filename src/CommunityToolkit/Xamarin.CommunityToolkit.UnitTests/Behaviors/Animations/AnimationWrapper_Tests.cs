using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors.Animations
{
	public class AnimationWrapper_Tests
	{
		[SetUp]
		public void SetUp() => Device.PlatformServices = new MockPlatformServices();

		[Test]
		public async Task ShouldCallbackAnExpectedNumberOfTimes()
		{
			var frameCount = 0;

			var animation = new Animation
			{
				{ 0, 1, new Animation(d => frameCount++) }
			};

			var animationWrapper = new AnimationWrapper(
				animation,
				Guid.NewGuid().ToString(),
				new BoxView(),
				16,
				160,
				Easing.Linear,
				(v, t) => { },
				() => false);

			Assert.IsFalse(animationWrapper.IsRunning);

			animationWrapper.Commit();

			await Task.Delay(250);

			// 160 ms length / 1 frame every 16 ms = 10 frames
			Assert.GreaterOrEqual(10, frameCount);
		}

		[Test]
		public void AbortShouldStopAnimationRunning()
		{
			var animation = new Animation
			{
				{ 0, 1, new Animation(d => { }) }
			};

			var animationWrapper = new AnimationWrapper(
				animation,
				Guid.NewGuid().ToString(),
				new BoxView(),
				16,
				100,
				Easing.Linear,
				(v, t) => { },
				() => false);

			Assert.IsFalse(animationWrapper.IsRunning);

			animationWrapper.Commit();
			animationWrapper.Abort();

			Assert.IsFalse(animationWrapper.IsRunning);
		}

		[Test]
		public void IsRunningShouldReportStatus()
		{
			var animation = new Animation
			{
				{ 0, 1, new Animation(d => { }) }
			};

			var animationWrapper = new AnimationWrapper(
				animation,
				Guid.NewGuid().ToString(),
				new BoxView(),
				16,
				100,
				Easing.Linear,
				(v, t) => { },
				() => false);

			Assert.IsFalse(animationWrapper.IsRunning);

			animationWrapper.Commit();

			Assert.IsTrue(animationWrapper.IsRunning);
		}
	}
}
