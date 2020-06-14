using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkit.Interfaces;

namespace XamarinCommunityToolkit.Behaviors
{
    public class EventToCommandBehavior : BaseBehavior<View>
    {
        Delegate eventHandler;

        public static readonly BindableProperty EventNameProperty = 
            BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = 
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));
        public static readonly BindableProperty CommandParameterProperty = 
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));
        public static readonly BindableProperty InputConverterProperty = 
            BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(EventToCommandBehavior));

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
            {
                throw new ArgumentException($"EventToCommandBehavior: Can't register the {EventName} event.");
            }

            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        private void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (eventHandler == null)
            {
                return;
            }

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
            {
                throw new ArgumentException($"EventToCommandBehavior: Can't de-register the {EventName} event.");
            }

            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        private async void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
            {
                return;
            }            

            var resolvedParameter = CommandParameter 
                ?? Converter.Convert(eventArgs, typeof(object), null, null) 
                ?? eventArgs;

            if (Command.CanExecute(resolvedParameter))
            {
                Command.Execute(resolvedParameter);
            }

            foreach (BindableObject bindable in Actions)
            {
                bindable.BindingContext = BindingContext;
                var action = (IAction)bindable;
                await action.Execute(sender, eventArgs);
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;

            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }        
    }
}
