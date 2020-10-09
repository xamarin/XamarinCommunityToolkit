using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class EventToCommandBehavior : BaseBehavior<View>
	{
		public static readonly BindableProperty EventNameProperty = BindablePropertyBuilder.CreateBuilder()
			.SetPropertyName(nameof(EventName))
			.SetReturnType<string>()
			.SetDeclaringType<EventToCommandBehavior>()
			.SetPropertyChangedDelegate(OnEventNamePropertyChanged)
			.Build();

		public static readonly BindableProperty CommandProperty = BindablePropertyBuilder.CreateBuilder()
			.SetPropertyName(nameof(Command))
			.SetReturnType<ICommand>()
			.SetDeclaringType<EventToCommandBehavior>()
			.Build();

		public static readonly BindableProperty CommandParameterProperty = BindablePropertyBuilder.CreateBuilder()
			.SetPropertyName(nameof(CommandParameter))
			.SetReturnType<object>()
			.SetDeclaringType<EventToCommandBehavior>()
			.Build();

		public static readonly BindableProperty EventArgsConverterProperty = BindablePropertyBuilder.CreateBuilder()
			.SetPropertyName(nameof(EventArgsConverter))
			.SetReturnType<IValueConverter>()
			.SetDeclaringType<EventToCommandBehavior>()
			.Build();

		readonly MethodInfo eventHandlerMethodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnTriggerHandled));

		Delegate eventHandler;

		EventInfo eventInfo;

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

		public IValueConverter EventArgsConverter
		{
			get => (IValueConverter)GetValue(EventArgsConverterProperty);
			set => SetValue(EventArgsConverterProperty, value);
		}

		protected override void OnAttachedTo(View bindable)
		{
			base.OnAttachedTo(bindable);
			RegisterEvent();
		}

		protected override void OnDetachingFrom(View bindable)
		{
			UnregisterEvent();
			base.OnDetachingFrom(bindable);
		}

		static void OnEventNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((EventToCommandBehavior)bindable).RegisterEvent();

		void RegisterEvent()
		{
			UnregisterEvent();

			var eventName = EventName;
			if (View == null || string.IsNullOrWhiteSpace(eventName))
				return;

			eventInfo = View.GetType().GetRuntimeEvent(eventName) ??
				throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Couldn't resolve the event.", nameof(EventName));

			eventHandler = eventHandlerMethodInfo.CreateDelegate(eventInfo.EventHandlerType, this) ??
				throw new ArgumentException($"{nameof(EventToCommandBehavior)}: Couldn't create event handler.", nameof(EventName));

			eventInfo.AddEventHandler(View, eventHandler);
		}

		void UnregisterEvent()
		{
			if (eventInfo != null && eventHandler != null)
				eventInfo.RemoveEventHandler(View, eventHandler);

			eventInfo = null;
			eventHandler = null;
		}

		protected virtual void OnTriggerHandled(object sender = null, object eventArgs = null)
		{
			var parameter = CommandParameter
				?? EventArgsConverter?.Convert(eventArgs, typeof(object), null, null)
				?? eventArgs;

			var command = Command;
			if (command?.CanExecute(parameter) ?? false)
				command.Execute(parameter);
		}
	}
}