using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Behaviors
{
    /// <summary>
    /// Attaches an effect to any Xamarin.Forms.View
    /// </summary>
    public class EffectBehavior : BaseBehavior<View>
    {
        /// <summary>
        /// Bindable effect name
        /// </summary>
        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(EffectBehavior));

        /// <summary>
        /// Effect name
        /// </summary>
        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        /// <summary>
        /// Adds the effect when the behavior is attached to the view
        /// </summary>
        /// <param name="bindable"></param>
        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
            AddEffect(bindable as View);
        }

        /// <summary>
        /// Removes the effect when the behavior is detached from the view
        /// </summary>
        /// <param name="bindable"></param>
        protected override void OnDetachingFrom(BindableObject bindable)
        {
            RemoveEffect(bindable as View);
            base.OnDetachingFrom(bindable);
        }

        /// <summary>
        /// Adds the effect to the Effects collection of the View
        /// </summary>
        /// <param name="view"></param>
        void AddEffect(View view)
        {
            var effect = GetEffect();

            if (effect == null)
            {
                return;
            }

            view.Effects.Add(GetEffect());
        }

        /// <summary>
        /// Removes the effect from the Effects collection of the View
        /// </summary>
        /// <param name="view"></param>
        void RemoveEffect(View view)
        {
            var effect = GetEffect();

            if (effect == null)
            {
                return;
            }

            view.Effects.Remove(GetEffect());
        }

        /// <summary>
        /// Resolves the effect name based on the Name property and returns the matching effect
        /// </summary>
        /// <returns></returns>
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
