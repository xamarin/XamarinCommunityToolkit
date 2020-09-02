using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class UserStoppedTypingBehavior : BaseBehavior
	{
		CancellationTokenSource tokenSource;

		#region Bindable Properties

		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(UserStoppedTypingBehavior));
		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public static readonly BindableProperty StoppedTypingTimeThresholdProperty = BindableProperty.Create(nameof(StoppedTypingTimeThreshold), typeof(int), typeof(UserStoppedTypingBehavior), 1000);
		public int StoppedTypingTimeThreshold
		{
			get => (int)GetValue(StoppedTypingTimeThresholdProperty);
			set => SetValue(StoppedTypingTimeThresholdProperty, value);
		}

		public static readonly BindableProperty ShouldDismissKeyboardAutomaticallyProperty = BindableProperty.Create(nameof(ShouldDismissKeyboardAutomatically), typeof(bool), typeof(UserStoppedTypingBehavior), false);
		public bool ShouldDismissKeyboardAutomatically
		{
			get => (bool)GetValue(ShouldDismissKeyboardAutomaticallyProperty);
			set => SetValue(ShouldDismissKeyboardAutomaticallyProperty, value);
		}

		#endregion Bindable Properties

		protected override void OnAttachedTo(View view)
		{
			base.OnAttachedTo(view);

			if (!(view is InputView inputView)) return;

			inputView.TextChanged += OnInputViewTextChanged;
		}

		protected override void OnDetachingFrom(View view)
		{
			base.OnDetachingFrom(view);

			if (!(view is InputView inputView)) return;

			inputView.TextChanged -= OnInputViewTextChanged;
		}

		void OnInputViewTextChanged(object sender, TextChangedEventArgs e)
		{
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				tokenSource.Dispose();
			}

			tokenSource = new CancellationTokenSource();

			_ = PerformTextChanged((InputView)sender, e.NewTextValue, tokenSource.Token);
		}

		Task PerformTextChanged(InputView inputView, string newTextValue, CancellationToken token)
			=> Task.Delay(StoppedTypingTimeThreshold, token)
				.ContinueWith(task =>
				{
					if (task.Exception != null || token.IsCancellationRequested) return;

					if (ShouldDismissKeyboardAutomatically)
						inputView.Unfocus();

					if (Command == null || !Command.CanExecute(newTextValue)) return;

					Command.Execute(newTextValue);
				});
	}
}