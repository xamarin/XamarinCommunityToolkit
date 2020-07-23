using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class FadeAnimation: AnimationBase
    {
        public static readonly BindableProperty FadeProperty =
           BindableProperty.Create(nameof(Fade), typeof(double), typeof(AnimationBase), 0.3, BindingMode.TwoWay);

        public double Fade
        {
            get { return (double)GetValue(FadeProperty); }
            set { SetValue(FadeProperty, value); }
        }

        public FadeAnimation()
        {
            Duration = 300;
        }

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.FadeTo(Fade, Duration, easing);
            await view.FadeTo(1, Duration, easing);
        }
    }
}
