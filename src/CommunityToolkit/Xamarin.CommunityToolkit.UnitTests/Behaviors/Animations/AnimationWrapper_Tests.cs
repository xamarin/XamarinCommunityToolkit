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
