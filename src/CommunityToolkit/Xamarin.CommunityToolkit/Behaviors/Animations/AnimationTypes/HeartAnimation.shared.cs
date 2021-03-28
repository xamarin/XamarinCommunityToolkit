using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class HeartAnimation : AnimationBase
    {
        protected override uint DefaultDuration { get; set; } = 500;

        public override Task Animate(View view) =>
            Device.InvokeOnMainThreadAsync(() => view.Animate("Hearth", Hearth(view), 16, Duration));

        internal Animation Hearth(View view)
        {
            var animation = new Animation();

            animation.WithConcurrent(
                (f) => view.Scale = f,
                view.Scale, view.Scale,
                Easing.Linear, 0, 0.1);

            animation.WithConcurrent(
                (f) => view.Scale = f,
                view.Scale, view.Scale * 1.1,
                Easing.Linear, 0.1, 0.4);

            animation.WithConcurrent(
                (f) => view.Scale = f,
                view.Scale * 1.1, view.Scale,
                Easing.Linear, 0.4, 0.5);

            animation.WithConcurrent(
                (f) => view.Scale = f,
                view.Scale, view.Scale * 1.1,
                Easing.Linear, 0.5, 0.8);

            animation.WithConcurrent(
                (f) => view.Scale = f,
                view.Scale * 1.1, view.Scale,
                Easing.Linear, 0.8, 1);

            return animation;
        }
    }
}