using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Animations
{
	public class RubberBandAnimation : AnimationBase
	{
        internal const uint DefaultLength = 1000;

        public RubberBandAnimation(
            string? name = null,
            uint length = DefaultLength,
            Easing? easing = null,
            Action<double, bool>? onFinished = null,
            IAnimatable? owner = null,
            uint rate = 16,
            Func<bool>? shouldRepeat = null,
            params View[] views)
            : base(
                  name ?? nameof(RubberBandAnimation),
                  owner,
                  rate,
                  length,
                  easing,
                  onFinished,
                  shouldRepeat,
                  views)
        {
        }

        protected override Animation Create()
        {
            var animation = new Animation();

            foreach (var label in views)
            {
                animation.Add(0, 0.3, new Animation(v => label.ScaleX = v, 1, 1.25));
                animation.Add(0, 0.3, new Animation(v => label.ScaleY = v, 1, 0.75));

                animation.Add(0.3, 0.4, new Animation(v => label.ScaleX = v, 1.25, 0.75));
                animation.Add(0.3, 0.4, new Animation(v => label.ScaleY = v, 0.75, 1.25));

                animation.Add(0.4, 0.5, new Animation(v => label.ScaleX = v, 0.75, 1.15));
                animation.Add(0.4, 0.5, new Animation(v => label.ScaleY = v, 1.25, 0.85));

                animation.Add(0.5, 0.65, new Animation(v => label.ScaleX = v, 1.15, 0.95));
                animation.Add(0.5, 0.65, new Animation(v => label.ScaleY = v, 0.85, 1.05));

                animation.Add(0.65, 0.75, new Animation(v => label.ScaleX = v, 0.95, 1.05));
                animation.Add(0.65, 0.75, new Animation(v => label.ScaleY = v, 1.05, 0.95));

                animation.Add(0.75, 1, new Animation(v => label.ScaleX = v, 1.05, 1));
                animation.Add(0.75, 1, new Animation(v => label.ScaleY = v, 0.95, 1));
            }

            return animation;
        }
    }
}
