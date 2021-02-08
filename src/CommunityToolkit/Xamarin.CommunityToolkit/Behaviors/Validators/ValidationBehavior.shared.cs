using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors.Internals
{
	public abstract class ValidationBehavior : BaseBehavior<VisualElement>
	{
		public static readonly BindableProperty IsValidProperty =
			BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidationBehavior), true, BindingMode.OneWayToSource);

		public static readonly BindableProperty IsRunningProperty =
			BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(ValidationBehavior), false, BindingMode.OneWayToSource);

		public static readonly BindableProperty ValidStyleProperty =
			BindableProperty.Create(nameof(ValidStyle), typeof(Style), typeof(ValidationBehavior), propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty InvalidStyleProperty =
			BindableProperty.Create(nameof(InvalidStyle), typeof(Style), typeof(ValidationBehavior), propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty FlagsProperty =
			BindableProperty.Create(nameof(Flags), typeof(ValidationFlags), typeof(ValidationBehavior), ValidationFlags.ValidateOnUnfocusing | ValidationFlags.ForceMakeValidWhenFocused, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty ValueProperty =
			BindableProperty.Create(nameof(Value), typeof(object), typeof(ValidationBehavior), propertyChanged: OnValuePropertyChanged);

		public static readonly BindableProperty ValuePropertyNameProperty =
			BindableProperty.Create(nameof(ValuePropertyName), typeof(string), typeof(ValidationBehavior), defaultValueCreator: GetDefaultValuePropertyName, propertyChanged: OnValuePropertyNamePropertyChanged);

		public static readonly BindableProperty ForceValidateCommandProperty =
			BindableProperty.Create(nameof(ForceValidateCommand), typeof(ICommand), typeof(ValidationBehavior), defaultValueCreator: GetDefaultForceValidateCommand, defaultBindingMode: BindingMode.OneWayToSource);

		ValidationFlags currentStatus;

		bool isAttaching;

		BindingBase defaultValueBinding;

		CancellationTokenSource validationTokenSource;

		public bool IsValid
		{
			get => (bool)GetValue(IsValidProperty);
			set => SetValue(IsValidProperty, value);
		}

		public bool IsRunning
		{
			get => (bool)GetValue(IsRunningProperty);
			set => SetValue(IsRunningProperty, value);
		}

		public Style ValidStyle
		{
			get => (Style)GetValue(ValidStyleProperty);
			set => SetValue(ValidStyleProperty, value);
		}

		public Style InvalidStyle
		{
			get => (Style)GetValue(InvalidStyleProperty);
			set => SetValue(InvalidStyleProperty, value);
		}

		public ValidationFlags Flags
		{
			get => (ValidationFlags)GetValue(FlagsProperty);
			set => SetValue(FlagsProperty, value);
		}

		public object Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public string ValuePropertyName
		{
			get => (string)GetValue(ValuePropertyNameProperty);
			set => SetValue(ValuePropertyNameProperty, value);
		}

		public ICommand ForceValidateCommand
		{
			get => (ICommand)GetValue(ForceValidateCommandProperty);
			set => SetValue(ForceValidateCommandProperty, value);
		}

		protected virtual string DefaultValuePropertyName => Entry.TextProperty.PropertyName;

		protected virtual ICommand DefaultForceValidateCommand => new Command(ForceValidate);

		public void ForceValidate() => _ = UpdateStateAsync(true);

		internal Task ValidateNestedAsync(CancellationToken token) => UpdateStateAsync(true, token);

		protected virtual object Decorate(object value) => value;

		protected abstract Task<bool> ValidateAsync(object value, CancellationToken token);

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);

			isAttaching = true;
			currentStatus = ValidationFlags.ValidateOnAttaching;

			OnValuePropertyNamePropertyChanged();
			_ = UpdateStateAsync(false);
			isAttaching = false;
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			if (defaultValueBinding != null)
			{
				RemoveBinding(ValueProperty);
				defaultValueBinding = null;
			}

			currentStatus = ValidationFlags.None;
			base.OnDetachingFrom(bindable);
		}

		protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnViewPropertyChanged(sender, e);
			if (e.PropertyName == VisualElement.IsFocusedProperty.PropertyName)
			{
				currentStatus = View.IsFocused
					? ValidationFlags.ValidateOnFocusing
					: ValidationFlags.ValidateOnUnfocusing;
				_ = UpdateStateAsync(false);
			}
		}

		protected static void OnValidationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> _ = ((ValidationBehavior)bindable).UpdateStateAsync(false);

		static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ValidationBehavior)bindable).OnValuePropertyChanged();
			OnValidationPropertyChanged(bindable, oldValue, newValue);
		}

		static void OnValuePropertyNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ValidationBehavior)bindable).OnValuePropertyNamePropertyChanged();

		static object GetDefaultForceValidateCommand(BindableObject bindable)
			=> ((ValidationBehavior)bindable).DefaultForceValidateCommand;

		static object GetDefaultValuePropertyName(BindableObject bindable)
			=> ((ValidationBehavior)bindable).DefaultValuePropertyName;

		void OnValuePropertyChanged()
		{
			if (isAttaching)
				return;

			currentStatus = ValidationFlags.ValidateOnValueChanging;
		}

		void OnValuePropertyNamePropertyChanged()
		{
			if (IsBound(ValueProperty, defaultValueBinding))
			{
				defaultValueBinding = null;
				return;
			}

			defaultValueBinding = new Binding
			{
				Path = ValuePropertyName,
				Source = View
			};
			SetBinding(ValueProperty, defaultValueBinding);
		}

		async Task UpdateStateAsync(bool isForced, CancellationToken? parentToken = null)
		{
			IsRunning = true;

			if ((View?.IsFocused ?? false) && Flags.HasFlag(ValidationFlags.ForceMakeValidWhenFocused))
			{
				ResetValidationTokenSource(null);
				IsValid = true;
			}
			else if (isForced || (currentStatus != ValidationFlags.None && Flags.HasFlag(currentStatus)))
			{
				using var tokenSource = new CancellationTokenSource();
				var token = parentToken ?? tokenSource.Token;
				ResetValidationTokenSource(tokenSource);

				try
				{
					var isValid = await ValidateAsync(Decorate(Value), token);

					if (token.IsCancellationRequested)
						return;

					validationTokenSource = null;
					IsValid = isValid;
				}
				catch (TaskCanceledException)
				{
					return;
				}
			}

			UpdateStyle();
			IsRunning = false;
		}

		void UpdateStyle()
		{
			if (View == null || (ValidStyle ?? InvalidStyle) == null)
				return;

			View.Style = IsValid
				? ValidStyle
				: InvalidStyle;
		}

		void ResetValidationTokenSource(CancellationTokenSource newTokenSource)
		{
			validationTokenSource?.Cancel();
			validationTokenSource = newTokenSource;
		}
	}
}