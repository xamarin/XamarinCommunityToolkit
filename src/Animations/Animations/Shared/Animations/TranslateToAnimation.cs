using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public class TranslateToAnimation : AnimationBase
    {
        public static readonly BindableProperty TranslateXProperty =
            BindableProperty.Create(
                nameof(TranslateX),
                typeof(double),
                typeof(TranslateToAnimation),
                default(double),
                BindingMode.TwoWay,
                null);

        public double TranslateX
        {
            get { return (double)GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        public static readonly BindableProperty TranslateYProperty =
            BindableProperty.Create(
                nameof(TranslateY),
                typeof(double),
                typeof(TranslateToAnimation),
                default(double),
                BindingMode.TwoWay,
                null);

        public double TranslateY
        {
            get { return (double)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.TranslateTo(TranslateX, TranslateY, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}
