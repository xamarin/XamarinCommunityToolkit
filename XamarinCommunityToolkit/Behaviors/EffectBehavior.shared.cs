using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class EffectBehavior : BaseBehavior
    {
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(EffectBehavior));

        Effect effect;

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            effect = Effect.Resolve(Name);
            bindable.Effects.Add(effect);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Effects.Remove(effect);
            effect = null;
        }
    }
}
