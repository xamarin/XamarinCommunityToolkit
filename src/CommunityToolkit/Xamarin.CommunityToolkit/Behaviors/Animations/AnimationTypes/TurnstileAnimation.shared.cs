using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
    public class TurnstileInAnimation : AnimationBase
    {
        protected override uint DefaultDuration { get; set; } = 300;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.Animate("TurnstileIn", TurnstileIn(view), 16, Duration));

        internal Animation TurnstileIn(View view)
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => view.RotationY = f, 75, 0, Easing.CubicOut);
            animation.WithConcurrent((f) => view.Opacity = f, 0, 1, null, 0, 0.01);

            return animation;
        }
    }

    public class TurnstileOutAnimation : AnimationBase
    {
        protected override uint DefaultDuration { get; set; } = 300;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.Animate("TurnstileOut", TurnstileOut(view), 16, Duration));

        internal Animation TurnstileOut(View view)
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => view.RotationY = f, 0, -75, Easing.CubicOut);
            animation.WithConcurrent((f) => view.Opacity = f, 1, 0, null, 0.9, 1);

            return animation;
        }
    }
}
