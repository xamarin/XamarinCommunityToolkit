using System.Linq;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors.Animations
{
    public class AnimationBehaviour : EventToCommandBehavior
    {
        public static readonly BindableProperty AnimationTypeProperty =
            BindableProperty.Create(nameof(AnimationType), typeof(AnimationBase), typeof(AnimationBehaviour));

        public AnimationBase AnimationType
        {
            get => (AnimationBase)GetValue(AnimationTypeProperty);
            set => SetValue(AnimationTypeProperty, value);
        }

        bool isAnimating = false;

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            if (string.IsNullOrEmpty(EventName))
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnTriggerHandled;
                View.GestureRecognizers?.Clear();
                View.GestureRecognizers.Add(tapGestureRecognizer);
            }
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            var exists = View.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;

            if (exists != null)
                exists.Tapped -= OnTriggerHandled;

            base.OnDetachingFrom(bindable);
        }

        protected override async void OnTriggerHandled(object sender = null, object eventArgs = null)
        {
            if (isAnimating)
                return;

            isAnimating = true;

            await AnimationType?.Animate((View)sender);

            if (Command?.CanExecute(CommandParameter) ?? false)
            {
                Command.Execute(CommandParameter);
            }

            isAnimating = false;

            base.OnTriggerHandled(sender, eventArgs);
        }
    }
}
