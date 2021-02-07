using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class MaxLengthReachedBehavior : BaseBehavior<InputView>
	{
		public static readonly BindableProperty CommandProperty
			= BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MaxLengthReachedBehavior));

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public static readonly BindableProperty ShouldDismissKeyboardAutomaticallyProperty
			= BindableProperty.Create(nameof(ShouldDismissKeyboardAutomatically), typeof(bool), typeof(MaxLengthReachedBehavior), false);

		public bool ShouldDismissKeyboardAutomatically
		{
			get => (bool)GetValue(ShouldDismissKeyboardAutomaticallyProperty);
			set => SetValue(ShouldDismissKeyboardAutomaticallyProperty, value);
		}

		readonly WeakEventManager<MaxLengthReachedEventArgs> maxLengthReachedEventManager = new WeakEventManager<MaxLengthReachedEventArgs>();

		public event EventHandler<MaxLengthReachedEventArgs> MaxLengthReached
		{
			add => maxLengthReachedEventManager.AddEventHandler(value);
			remove => maxLengthReachedEventManager.RemoveEventHandler(value);
		}

		protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnViewPropertyChanged(sender, e);
			if (e.PropertyName == InputView.TextProperty.PropertyName)
				OnTextPropertyChanged();
		}

		void OnTextPropertyChanged()
		{
			if (!(View.Text?.Length >= View.MaxLength))
				return;

			if (ShouldDismissKeyboardAutomatically)
				View.Unfocus();

			var newTextValue = View.Text.Substring(0, View.MaxLength);

			maxLengthReachedEventManager.RaiseEvent(this, new MaxLengthReachedEventArgs(newTextValue), nameof(MaxLengthReached));

			if (Command?.CanExecute(newTextValue) ?? false)
				Command.Execute(newTextValue);
		}
	}
}