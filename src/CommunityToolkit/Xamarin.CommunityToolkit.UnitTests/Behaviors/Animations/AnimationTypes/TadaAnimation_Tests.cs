using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors.Animations.AnimationTypes
{
	public class TadaAnimation_Tests
	{
		[SetUp]
		public void SetUp()
		{
			Device.PlatformServices = new MockPlatformServices();
			Ticker.SetDefault(new AsyncTicker(TimeSpan.FromMilliseconds(16)));
		}

		[TearDown]
		public void TearDown()
		{
			Device.PlatformServices = null;
			Ticker.SetDefault(null);
		}

		[Test]
		public async Task AFullAnimationShouldReturnToOriginalValues()
		{
			var animation = new TadaAnimation();
			var view = new MockView();

			await animation.Animate(view);

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.Scale)));
			Assert.AreEqual(1, view.ValuesSet[nameof(View.Scale)].Last());

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.Rotation)));
			Assert.AreEqual(0, view.ValuesSet[nameof(View.Rotation)].Last());
		}

		[Test]
		public async Task AnAbortedAnimationShouldNotReturnToOriginalValues()
		{
			var animation = new TadaAnimation();
			var view = new MockView();
			var cancellationTokenSource = new CancellationTokenSource(100);

			await animation.Animate(cancellationTokenSource.Token, view);

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.Scale)));
			Assert.AreNotEqual(1, view.ValuesSet[nameof(View.Scale)].Last());

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.Rotation)));
			Assert.AreNotEqual(0, view.ValuesSet[nameof(View.Rotation)].Last());
		}
	}
}
