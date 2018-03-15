using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public abstract class AnimationBase : BindableObject
    {
        public static readonly BindableProperty TargetProperty =
            BindableProperty.Create(
                nameof(Target),
                typeof(VisualElement),
                typeof(AnimationBase),
                null,
                BindingMode.TwoWay,
                null);

        public VisualElement Target
        {
            get { return (VisualElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create(
                nameof(Duration),
                typeof(string),
                typeof(AnimationBase),
                "1000",
                BindingMode.TwoWay,
                null);

        public string Duration
        {
            get { return (string)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly BindableProperty EasingProperty =
            BindableProperty.Create(
                nameof(Easing),
                typeof(EasingType),
                typeof(AnimationBase),
                EasingType.Linear,
                BindingMode.TwoWay,
                null);

        public EasingType Easing
        {
            get { return (EasingType)GetValue(EasingProperty); }
            set { SetValue(EasingProperty, value); }
        }

        protected abstract Task BeginAnimation();

        public async Task Begin()
        {
            await BeginAnimation();
        }
    }
}
