using System.Threading.Tasks;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class FlipVerticalAnimation : RotateAnimation
    {
        protected override double DefaultRotation { get; set; } = 90;

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.RotateXTo(Rotation, Duration, easing);
            await view.RotateXTo(0, Duration, easing);
        }
    }
}
