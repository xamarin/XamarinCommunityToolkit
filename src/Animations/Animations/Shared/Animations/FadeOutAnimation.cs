using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public class FadeOutAnimation : AnimationBase
    {
        public enum FadeDirection
        {
            Up,
            Down
        }

        public static readonly BindableProperty DirectionProperty =
            BindableProperty.Create(
                nameof(Direction),
                typeof(FadeDirection),
                typeof(FadeOutAnimation),
                FadeDirection.Up,
                BindingMode.TwoWay,
                null);

        public FadeDirection Direction
        {
            get { return (FadeDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Target.Animate("FadeOut", FadeOut(), 16, Convert.ToUInt32(Duration));
                });
            });
        }

        internal Animation FadeOut()
        {
            var animation = new Animation();

            animation.WithConcurrent(
                 (f) => Target.Opacity = f,
                 1,
                 0);

            animation.WithConcurrent(
                  (f) => Target.TranslationY = f,
                  Target.TranslationY,
                  Target.TranslationY + ((Direction == FadeDirection.Up) ? 50 : -50));

            return animation;
        }
    }
}