using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public class FadeToAnimation : AnimationBase
    {
        public static readonly BindableProperty OpacityProperty =
            BindableProperty.Create(
                nameof(Opacity),
                typeof(double),
                typeof(FadeToAnimation),
                default(double),
                BindingMode.TwoWay,
                null);

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.FadeTo(Opacity, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}