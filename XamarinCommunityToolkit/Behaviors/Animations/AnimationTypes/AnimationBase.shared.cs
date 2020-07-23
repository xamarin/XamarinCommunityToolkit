using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public abstract class AnimationBase : BindableObject
    {
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(nameof(Duration), typeof(uint), typeof(AnimationBase), uint.MinValue,
                BindingMode.TwoWay);

        public uint Duration
        {
            get { return (uint)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        
        public static readonly BindableProperty EasingTypeProperty =
           BindableProperty.Create(nameof(Easing), typeof(EasingType), typeof(AnimationBase), EasingType.Linear,
               BindingMode.TwoWay);

        public EasingType Easing
        {
            get { return (EasingType)GetValue(EasingTypeProperty); }
            set { SetValue(EasingTypeProperty, value); }
        }

        public abstract Task Animate(View view);
    }
}
