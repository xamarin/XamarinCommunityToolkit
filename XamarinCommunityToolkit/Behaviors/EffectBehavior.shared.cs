using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    public class EffectBehavior : BaseBehavior<View>
    {
        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(EffectBehavior));

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            AddEffect(bindable as View);
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            RemoveEffect(bindable as View);
            base.OnDetachingFrom(bindable);
        }

        void AddEffect(View view)
        {
            var effect = GetEffect();

            if (effect == null)
            {
                return;
            }

            view.Effects.Add(GetEffect());
        }

        void RemoveEffect(View view)
        {
            var effect = GetEffect();

            if (effect == null)
            {
                return;
            }

            view.Effects.Remove(GetEffect());
        }

        Effect GetEffect()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                return Effect.Resolve($"{Name}");
            }

            return null;
        }
    }
}
