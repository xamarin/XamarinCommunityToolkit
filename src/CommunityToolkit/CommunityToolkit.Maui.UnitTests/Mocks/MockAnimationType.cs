using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UnitTests.Mocks
{
	public class MockAnimationType : AnimationBase
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