﻿using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class UserStoppedTypingBehavior : BaseBehavior<InputView>
	{
		public static readonly BindableProperty CommandProperty
			= BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(UserStoppedTypingBehavior));

		public static readonly BindableProperty StoppedTypingTimeThresholdProperty
			= BindableProperty.Create(nameof(StoppedTypingTimeThreshold), typeof(int), typeof(UserStoppedTypingBehavior), 1000);

		public static readonly BindableProperty ShouldDismissKeyboardAutomaticallyProperty
			= BindableProperty.Create(nameof(ShouldDismissKeyboardAutomatically), typeof(bool), typeof(UserStoppedTypingBehavior), false);

		CancellationTokenSource tokenSource;

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public int StoppedTypingTimeThreshold
		{
			get => (int)GetValue(StoppedTypingTimeThresholdProperty);
			set => SetValue(StoppedTypingTimeThresholdProperty, value);
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
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				tokenSource.Dispose();
			}
			tokenSource = new CancellationTokenSource();

			_ = Task.Delay(StoppedTypingTimeThreshold, tokenSource.Token)
				.ContinueWith(task =>
				{
					if (task.Status == TaskStatus.Canceled)
						return;

					if (ShouldDismissKeyboardAutomatically)
						Device.BeginInvokeOnMainThread(View.Unfocus);

					if (Command?.CanExecute(View.Text) ?? false)
						Command.Execute(View.Text);
				});
		}
	}
}