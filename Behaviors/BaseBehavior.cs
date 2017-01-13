using System;
using System.Linq;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Behaviors
{
    public class BaseBehavior<T> : Behavior<T> where T : BindableObject
    {

        public T AssociatedObject { get; private set; }

        public static readonly BindableProperty AttachBehaviorProperty =
            BindableProperty.CreateAttached("AttachBehavior",
                                            typeof(bool),
                                            typeof(BaseBehavior<T>),
                                            false, propertyChanged: OnAttachBehaviorChanged);

        public static bool GetAttachBehavior(T view)
        {
            return (bool)view.GetValue(AttachBehaviorProperty);
        }

        public static void SetAttachBehavior(T view, bool value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }

        static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            var control = view as Xamarin.Forms.View;
            bool attachBehavior = (bool)newValue;
            if (attachBehavior)
            {
                control.Behaviors.Add(new BaseBehavior<T>());
            }
            else
            {
                var toRemove = control.Behaviors.FirstOrDefault(b => b is BaseBehavior<T>);
                if (toRemove != null)
                {
                    control.Behaviors.Remove(toRemove);
                }
            }
        }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
