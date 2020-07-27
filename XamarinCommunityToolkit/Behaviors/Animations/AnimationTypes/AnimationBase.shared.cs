using System.Threading.Tasks;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public abstract class AnimationBase : BindableObject
    {
        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(nameof(Duration), typeof(uint), typeof(AnimationBase), default(uint),
                BindingMode.TwoWay, defaultValueCreator: GetDefaultDurationProperty);

        public uint Duration
        {
            get => (uint)GetValue(DurationProperty); 
            set => SetValue(DurationProperty, value); 
        }
        
        public static readonly BindableProperty EasingTypeProperty =
           BindableProperty.Create(nameof(Easing), typeof(EasingType), typeof(AnimationBase), EasingType.Linear,
               BindingMode.TwoWay);

        public EasingType Easing
        {
            get => (EasingType)GetValue(EasingTypeProperty); 
            set => SetValue(EasingTypeProperty, value);
        }

        static object GetDefaultDurationProperty(BindableObject bindable)
            => ((AnimationBase)bindable).DefaultDuration;

        protected abstract uint DefaultDuration { get; set; }

        public abstract Task Animate(View view);
    }
}
