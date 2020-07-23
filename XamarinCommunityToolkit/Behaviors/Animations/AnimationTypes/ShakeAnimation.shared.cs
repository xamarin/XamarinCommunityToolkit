using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class ShakeAnimation : AnimationBase
    {
        public static readonly BindableProperty StartFactorProperty =
          BindableProperty.Create(nameof(StartFactor), typeof(double), typeof(AnimationBase), 15.0, BindingMode.TwoWay);

        public double StartFactor
        {
            get { return (double)GetValue(StartFactorProperty); }
            set { SetValue(StartFactorProperty, value); }
        }

        public ShakeAnimation()
        {
            Duration = 50;
        }

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);

            for (var i = StartFactor; i > 0; i= i-5)
            {
                await view.TranslateTo(-i, 0, Duration, easing);
                await view.TranslateTo(i, 0, Duration, easing);
            }

            view.TranslationX = 0;
        }
    }
}
