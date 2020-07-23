using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class FlipHorizontalAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
             BindableProperty.Create(nameof(Rotation), typeof(double), typeof(AnimationBase), 90.0, BindingMode.TwoWay);

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public FlipHorizontalAnimation()
        {
            Duration = 300;
        }

        public override async Task Animate(View view)
        {
            var easing = AnimationHelper.GetEasing(Easing);
            await view.RotateYTo(Rotation, Duration, easing);
            await view.RotateYTo(0, Duration, easing);
        }
    }
}
