using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors.Animations.AnimationTypes
{
	public class RubberBandAnimation_Tests
	{
		[SetUp]
		public void SetUp() => Device.PlatformServices = new MockPlatformServices();

		[TearDown]
		public void TearDown() => Device.PlatformServices = null;

		[Test]
		public async Task AFullAnimationShouldReturnToOriginalValues()
		{
			var animation = new RubberBandAnimation();
			var view = new MockView();

			await animation.Animate(view);

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.ScaleX)));

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.ScaleY)));
		}

		[Test]
		public async Task AnAbortedAnimationShouldNotReturnToOriginalValues()
		{
			var animation = new RubberBandAnimation();
			var view = new MockView();
			var cancellationTokenSource = new CancellationTokenSource(100);

			await animation.Animate(cancellationTokenSource.Token, view);

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.ScaleX)));
			Assert.AreNotEqual(1, view.ValuesSet[nameof(View.ScaleX)].Last());

			Assert.IsTrue(view.ValuesSet.ContainsKey(nameof(View.ScaleY)));
			Assert.AreNotEqual(1, view.ValuesSet[nameof(View.ScaleY)].Last());
		}
	}
}
