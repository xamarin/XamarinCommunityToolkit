using Microsoft.Maui.Controls;

namespace Xamarin.CommunityToolkit.Markup.MauiCompat
{
    public class Style<T> where T : BindableObject
    {
        public static implicit operator Style?(Style<T>? style) => style?.FormsStyle;

        public Style FormsStyle { get; }

        public Style(params (BindableProperty Property, object Value)[] setters)
        {
            FormsStyle = new Style(typeof(T)) { };
            Add(setters);
        }

        public Style<T> ApplyToDerivedTypes(bool value)
        {
            FormsStyle.ApplyToDerivedTypes = value;
            return this;
        }

        public Style<T> BasedOn(Style value)
        {
            FormsStyle.BasedOn = value;
            return this;
        }

        public Style<T> Add(params (BindableProperty Property, object Value)[] setters)
        {
            foreach (var (Property, Value) in setters)
                FormsStyle.Setters.Add(Property, Value);

            return this;
        }

        public Style<T> Add(params Behavior[] behaviors)
        {
            foreach (var behavior in behaviors)
                FormsStyle.Behaviors.Add(behavior);

            return this;
        }

        public Style<T> Add(params TriggerBase[] triggers)
        {
            foreach (var trigger in triggers)
                FormsStyle.Triggers.Add(trigger);

            return this;
        }

        public Style<T> CanCascade(bool value)
        {
            FormsStyle.CanCascade = value;
            return this;
        }
    }
}