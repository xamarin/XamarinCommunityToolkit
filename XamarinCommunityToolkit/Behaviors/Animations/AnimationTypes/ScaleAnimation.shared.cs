using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class ScaleAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty =
           BindableProperty.Create(nameof(Scale), typeof(double), typeof(AnimationBase), 1.2, BindingMode.TwoWay);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public ScaleAnimation()
        {
            Duration = 170;
        }

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.ScaleTo(Scale, Duration, easing);
            await view.ScaleTo(1, Duration, easing);
        }
    }
}
