using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors.Internals
{
	/// <summary>
	/// The <see cref="ValidationBehavior"/> allows users to create custom validation behaviors. All of the validation behaviors in the Xamarin Community Toolkit inherit from this behavior, to expose a number of shared properties. Users can inherit from this class to create a custom validation behavior currently not supported through the Xamarin Community Toolkit. This behavios cannot be used directly as it's abstract.
	/// </summary>
	public abstract class ValidationBehavior : BaseBehavior<VisualElement>
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="IsNotValid"/> property.
		/// </summary>
		public static readonly BindableProperty IsNotValidProperty =
			BindableProperty.Create(nameof(IsNotValid), typeof(bool), typeof(ValidationBehavior), false, BindingMode.OneWayToSource);

		/// <summary>
		/// Backing BindableProperty for the <see cref="IsValid"/> property.
		/// </summary>
		public static readonly BindableProperty IsValidProperty =
			BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidationBehavior), true, BindingMode.OneWayToSource, propertyChanged: OnIsValidPropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="ValidStyle"/> property.
		/// </summary>
		public static readonly BindableProperty ValidStyleProperty =
			BindableProperty.Create(nameof(ValidStyle), typeof(Style), typeof(ValidationBehavior), propertyChanged: OnValidationPropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="InvalidStyle"/> property.
		/// </summary>
		public static readonly BindableProperty InvalidStyleProperty =
			BindableProperty.Create(nameof(InvalidStyle), typeof(Style), typeof(ValidationBehavior), propertyChanged: OnValidationPropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="Flags"/> property.
		/// </summary>
		public static readonly BindableProperty FlagsProperty =
			BindableProperty.Create(nameof(Flags), typeof(ValidationFlags), typeof(ValidationBehavior), ValidationFlags.ValidateOnUnfocusing | ValidationFlags.ForceMakeValidWhenFocused, propertyChanged: OnValidationPropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="Value"/> property.
		/// </summary>
		public static readonly BindableProperty ValueProperty =
			BindableProperty.Create(nameof(Value), typeof(object), typeof(ValidationBehavior), propertyChanged: OnValuePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="ValuePropertyName"/> property.
		/// </summary>
		public static readonly BindableProperty ValuePropertyNameProperty =
			BindableProperty.Create(nameof(ValuePropertyName), typeof(string), typeof(ValidationBehavior), defaultValueCreator: GetDefaultValuePropertyName, propertyChanged: OnValuePropertyNamePropertyChanged);

		/// <summary>
		/// Backing BindableProperty for the <see cref="ForceValidateCommand"/> property.
		/// </summary>
		public static readonly BindableProperty ForceValidateCommandProperty =
			BindableProperty.Create(nameof(ForceValidateCommand), typeof(ICommand), typeof(ValidationBehavior), defaultValueCreator: GetDefaultForceValidateCommand, defaultBindingMode: BindingMode.OneWayToSource);

		ValidationFlags currentStatus;

		bool isAttaching;

		BindingBase defaultValueBinding;

		/// <summary>
		/// Indicates whether or not the current value is considered valid. This is a bindable property.
		/// </summary>
		public bool IsValid
		{
			get => (bool)GetValue(IsValidProperty);
			set => SetValue(IsValidProperty, value);
		}

		/// <summary>
		/// Indicates whether or not the current value is considered not valid. This is a bindable property.
		/// </summary>
		public bool IsNotValid
		{
			get => (bool)GetValue(IsNotValidProperty);
			set => SetValue(IsNotValidProperty, value);
		}

		/// <summary>
		/// The <see cref="Style"/> to apply to the element when validation is successful. This is a bindable property.
		/// </summary>
		public Style ValidStyle
		{
			get => (Style)GetValue(ValidStyleProperty);
			set => SetValue(ValidStyleProperty, value);
		}

		/// <summary>
		/// The <see cref="Style"/> to apply to the element when validation fails. This is a bindable property.
		/// </summary>
		public Style InvalidStyle
		{
			get => (Style)GetValue(InvalidStyleProperty);
			set => SetValue(InvalidStyleProperty, value);
		}

		/// <summary>
		/// Provides an enumerated value that specifies how to handle validation. This is a bindable property.
		/// </summary>
		public ValidationFlags Flags
		{
			get => (ValidationFlags)GetValue(FlagsProperty);
			set => SetValue(FlagsProperty, value);
		}

		/// <summary>
		/// The value to validate. This is a bindable property.
		/// </summary>
		public object Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		/// <summary>
		/// Allows the user to override the property that will be used as the value to validate. This is a bindable property.
		/// </summary>
		public string ValuePropertyName
		{
			get => (string)GetValue(ValuePropertyNameProperty);
			set => SetValue(ValuePropertyNameProperty, value);
		}

		/// <summary>
		/// Allows the user to provide a custom <see cref="ICommand"/> that handles forcing validation. This is a bindable property.
		/// </summary>
		public ICommand ForceValidateCommand
		{
			get => (ICommand)GetValue(ForceValidateCommandProperty);
			set => SetValue(ForceValidateCommandProperty, value);
		}

		protected virtual string DefaultValuePropertyName => Entry.TextProperty.PropertyName;

		protected virtual ICommand DefaultForceValidateCommand => new Command(ForceValidate);

		/// <summary>
		/// Forces the behavior to make a validation pass.
		/// </summary>
		public void ForceValidate() => UpdateState(true);

		protected virtual object DecorateValue() => Value;

		protected abstract bool Validate(object value);

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);

			isAttaching = true;
			currentStatus = ValidationFlags.ValidateOnAttaching;

			OnValuePropertyNamePropertyChanged();
			UpdateState(false);
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
				UpdateState(false);
			}
		}

		protected static void OnValidationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ValidationBehavior)bindable).UpdateState(false);

		static void OnIsValidPropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((ValidationBehavior)bindable).OnIsValidPropertyChanged();

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

		void OnIsValidPropertyChanged()
			=> IsNotValid = !IsValid;

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

		void UpdateState(bool isForced)
		{
			if ((View?.IsFocused ?? false) && Flags.HasFlag(ValidationFlags.ForceMakeValidWhenFocused))
				IsValid = true;
			else if (isForced || (currentStatus != ValidationFlags.None && Flags.HasFlag(currentStatus)))
				IsValid = Validate(DecorateValue());

			UpdateStyle();
		}

		void UpdateStyle()
		{
			if (View == null || (ValidStyle ?? InvalidStyle) == null)
				return;

			View.Style = IsValid
				? ValidStyle
				: InvalidStyle;
		}
	}
}