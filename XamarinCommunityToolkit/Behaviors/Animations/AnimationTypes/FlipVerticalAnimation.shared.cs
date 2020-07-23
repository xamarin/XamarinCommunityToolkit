using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class FlipVerticalAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
             BindableProperty.Create(nameof(Rotation), typeof(double), typeof(AnimationBase), 90.0, BindingMode.TwoWay);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public FlipVerticalAnimation()
        {
            Duration = 200;
        }

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.RotateXTo(Rotation, Duration, easing);
            await view.RotateXTo(0, Duration, easing);
        }
    }
}
