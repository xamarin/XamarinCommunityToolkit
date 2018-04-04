using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public class TurnstileOutAnimation : AnimationBase
    {
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
                    Target.Animate("TurnstileOut", TurnstileOut(), 16, Convert.ToUInt32(Duration));
                });
            });
        }

        internal Animation TurnstileOut()
        {
            var animation = new Animation();

            animation.WithConcurrent((f) => Target.RotationY = f, 0, -75, Xamarin.Forms.Easing.CubicOut);
            animation.WithConcurrent((f) => Target.Opacity = f, 1, 0, null, 0.9, 1);

            return animation;
        }
    }
}
