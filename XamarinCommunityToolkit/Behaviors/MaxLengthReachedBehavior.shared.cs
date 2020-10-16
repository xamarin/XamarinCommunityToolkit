using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
    public class MaxLengthReachedBehavior : BaseBehavior<InputView>
	{
		public static readonly BindableProperty CommandProperty
			= BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(UserStoppedTypingBehavior));

		public static readonly BindableProperty ShouldDismissKeyboardAutomaticallyProperty
			= BindableProperty.Create(nameof(ShouldDismissKeyboardAutomatically), typeof(bool), typeof(UserStoppedTypingBehavior), false);

		public event EventHandler<MaxLengthReachedEventArgs> MaxLengthReached;

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public bool ShouldDismissKeyboardAutomatically
		{
			get => (bool)GetValue(ShouldDismissKeyboardAutomaticallyProperty);
			set => SetValue(ShouldDismissKeyboardAutomaticallyProperty, value);
		}

		protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnViewPropertyChanged(sender, e);
			if (e.PropertyName == InputView.TextProperty.PropertyName)
				OnTextPropertyChanged();
		}

		void OnTextPropertyChanged()
		{
			if (View.Text == null || View.Text.Length < View.MaxLength)
				return;

			if (ShouldDismissKeyboardAutomatically)
				View.Unfocus();

			MaxLengthReached?.Invoke(View, new MaxLengthReachedEventArgs(View.Text));
			
			if (Command?.CanExecute(View.Text) ?? false)
				Command.Execute(View.Text);
		}
	}
}