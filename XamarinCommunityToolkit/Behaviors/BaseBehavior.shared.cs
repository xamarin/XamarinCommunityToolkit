using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinCommunityToolkit.Controls;

namespace XamarinCommunityToolkit.Behaviors
{
    /// <summary>
    /// Contains base functionality for all derived behaviors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Preserve(AllMembers = true)]
    public class BaseBehavior<T> : Behavior<T> where T : BindableObject
    {
        public static readonly BindableProperty ActionsProperty =
            BindableProperty.Create(nameof(Actions), typeof(ActionCollection), typeof(BaseBehavior<BindableObject>), null);

        public ActionCollection Actions
            => (ActionCollection)GetValue(ActionsProperty);

        /// <summary>
        /// Control to which the behavior is attached
        /// </summary>
        public T AssociatedObject { get; private set; }

        public BaseBehavior()
            => SetValue(ActionsProperty, new ActionCollection());

        /// <summary>
        /// Function is fired when the behavior is attached to the control.
        /// </summary>
        /// <param name="bindable">Reference to the control to which it is attached</param>
        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
                BindingContext = bindable.BindingContext;

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        /// <summary>
        /// Function is fired when the behavior is removed from the control.
        /// </summary>
        /// <param name="bindable">Reference to the control to which it is attached</param>
        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        /// <summary>
        /// Function handles the event when the BindingContext changes.
        /// </summary>
        /// <param name="sender">Origin of the event</param>
        /// <param name="e">Event arguments</param>
        void OnBindingContextChanged(object sender, EventArgs e)
            => OnBindingContextChanged();

        /// <summary>
        /// Handles the event when the BindingContext changes on the AssociatedObject
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
