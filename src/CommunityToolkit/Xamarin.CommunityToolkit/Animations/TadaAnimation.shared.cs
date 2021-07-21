using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Animations
{
    public class TadaAnimation : AnimationBase
    {
        internal const uint DefaultLength = 1000;
        internal const double DefaultMaximumScale = 1.1;
        internal const double DefaultMinimumScale = 0.9;
        internal const double DefaultRotationAngle = 3.0;

        readonly double maximumScale = 1.1;
        readonly double minimumScale = 0.9;
        readonly double rotationAngle = DefaultRotationAngle;

        public TadaAnimation(
            double maximumScale = 1.1,
            double minimumScale = 0.9,
            double rotationAngle = 3.0,
            string? name = null,
            uint length = DefaultLength,
            Easing? easing = null,
            Action<double, bool>? onFinished = null,
            IAnimatable? owner = null,
            uint rate = 16,
            Func<bool>? shouldRepeat = null,
            params View[] views)
            : base(
                  name ?? nameof(TadaAnimation),
                  owner,
                  rate,
                  length,
                  easing,
                  onFinished,
                  shouldRepeat,
                  views)
        {
            this.maximumScale = maximumScale;
            this.minimumScale = minimumScale;
            this.rotationAngle = rotationAngle;
        }

        protected override Animation Create()
        {
            var animation = new Animation();

            foreach (var label in views)
            {
                animation.Add(0, 0.1, new Animation(v => label.Scale = v, 1, minimumScale));
                animation.Add(0.2, 0.3, new Animation(v => label.Scale = v, minimumScale, maximumScale));
                animation.Add(0.9, 1.0, new Animation(v => label.Scale = v, maximumScale, 1));

                animation.Add(0, 0.2, new Animation(v => label.Rotation = v, 0, -rotationAngle));
                animation.Add(0.2, 0.3, new Animation(v => label.Rotation = v, -rotationAngle, rotationAngle));
                animation.Add(0.3, 0.4, new Animation(v => label.Rotation = v, rotationAngle, -rotationAngle));
                animation.Add(0.4, 0.5, new Animation(v => label.Rotation = v, -rotationAngle, rotationAngle));
                animation.Add(0.5, 0.6, new Animation(v => label.Rotation = v, rotationAngle, -rotationAngle));
                animation.Add(0.6, 0.7, new Animation(v => label.Rotation = v, -rotationAngle, rotationAngle));
                animation.Add(0.7, 0.8, new Animation(v => label.Rotation = v, rotationAngle, -rotationAngle));
                animation.Add(0.8, 0.9, new Animation(v => label.Rotation = v, -rotationAngle, rotationAngle));
                animation.Add(0.9, 1.0, new Animation(v => label.Rotation = v, rotationAngle, 0));
            }

            return animation;
        }
    }
}