using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public class JumpAnimation : AnimationBase
    {
        internal const int Movement = -25;

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
                    Target.Animate("Jump", Jump(), 16, Convert.ToUInt32(Duration));
                });
            });
        }

        internal Animation Jump()
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => Target.TranslationY = f, Target.TranslationY, Target.TranslationX, Xamarin.Forms.Easing.Linear, 0, 0.2);

            animation.WithConcurrent((f) => Target.TranslationY = f, Target.TranslationY + Movement, Target.TranslationX, Xamarin.Forms.Easing.Linear, 0.2, 0.4);

            animation.WithConcurrent((f) => Target.TranslationY = f, Target.TranslationY, Target.TranslationX, Xamarin.Forms.Easing.Linear, 0.5, 1.0);

            return animation;
        }
    }
}
