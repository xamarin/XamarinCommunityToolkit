using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Helpers
{
	public class BindablePropertyBuilder
	{
		readonly BindablePropertyParameters bpParameters;

		public BindablePropertyBuilder() => bpParameters = new BindablePropertyParameters();

		/// <summary>
		/// (Required) Sets the type of the value that is bound to the <see cref="BindableProperty"/>
		/// </summary>
		/// <param name="returnType">The type of the value that is bound to the <see cref="BindableProperty"/></param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetReturnType(Type returnType)
		{
			bpParameters.ReturnType = returnType;
			return this;
		}

		/// <summary>
		/// (Required) Sets the name of the property that is bound to the <see cref="BindableProperty"/>
		/// </summary>
		/// <param name="propertyName">The name of the property that is bound to the <see cref="BindableProperty"/></param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetPropertyName(string propertyName)
		{
			bpParameters.PropertyName = propertyName;
			return this;
		}

		/// <summary>
		/// Sets the property's default value
		/// </summary>
		/// <param name="defaultValue">The property's default value</param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetDefaultValue(object defaultValue)
		{
			bpParameters.DefaultValue = defaultValue;
			return this;
		}

		/// <summary>
		/// Sets the property's default <see cref="BindingMode"/>
		/// </summary>
		/// <param name="bindingMode">The property's default <see cref="BindingMode"/></param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetDefaultBindingMode(BindingMode bindingMode)
		{
			bpParameters.DefaultBindingMode = bindingMode;
			return this;
		}

		/// <summary>
		/// (Required) Sets the type (usually a control) that's declaring the <see cref="BindableProperty"/>
		/// </summary>
		/// <param name="declaringType">The type (usually a control) that's declaring the <see cref="BindableProperty"/></param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetDeclaringType(Type declaringType)
		{
			bpParameters.DeclaringType = declaringType;
			return this;
		}

		/// <summary>
		/// Sets the delegate that runs when a bound value is set.
		/// </summary>
		/// <param name="validateValueDelegate">The delegate that runs when a bound value is set.</param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetValidateValueDelegate(BindableProperty.ValidateValueDelegate validateValueDelegate)
		{
			bpParameters.ValidateValueDelegate = validateValueDelegate;
			return this;
		}

		/// <summary>
		/// Sets the delegate that runs after a bound value has changed.
		/// </summary>
		/// <param name="propertyChangedDelegate">The delegate that runs after a bound value has changed.</param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetPropertyChangedDelegate(BindableProperty.BindingPropertyChangedDelegate propertyChangedDelegate)
		{
			bpParameters.BindingPropertyChangedDelegate = propertyChangedDelegate;
			return this;
		}

		/// <summary>
		/// Sets the delegate that runs before a bound value is changed.
		/// </summary>
		/// <param name="propertyChangingDelegate">The delegate that runs before a bound value is changed.</param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetPropertyChangingDelegate(BindableProperty.BindingPropertyChangingDelegate propertyChangingDelegate)
		{
			bpParameters.BindingPropertyChangingDelegate = propertyChangingDelegate;
			return this;
		}

		/// <summary>
		/// Sets the delegate used to coerce the range of a value.
		/// </summary>
		/// <param name="coerceValueDelegate">A delegate used to coerce the range of a value.</param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetCoerceValueDelegate(BindableProperty.CoerceValueDelegate coerceValueDelegate)
		{
			bpParameters.CoerceValueDelegate = coerceValueDelegate;
			return this;
		}

		/// <summary>
		/// Sets the delegate used to initialize the default value for reference types.
		/// </summary>
		/// <param name="createDefaultValueDelegate">The delegate used to initialize the default value for reference types.</param>
		/// <returns>The <see cref="BindablePropertyBuilder"/> to support the fluent syntax</returns>
		public BindablePropertyBuilder SetCreateDefaultValueDelegate(BindableProperty.CreateDefaultValueDelegate createDefaultValueDelegate)
		{
			bpParameters.CreateDefaultValueDelegate = createDefaultValueDelegate;
			return this;
		}

		/// <summary>
		/// Instantiates the <see cref="BindableProperty"/> based on the values that have been set.
		/// </summary>
		/// <returns>An instance of <see cref="BindableProperty"/> with the values that have been set</returns>
		/// <exception cref="ArgumentNullException">
		/// If any of the following are null:
		/// <see cref="BindableProperty.PropertyName"/>
		/// <see cref="BindableProperty.ReturnType"/>
		/// <see cref="BindableProperty.DeclaringType"/>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// If the type of the <see cref="BindableProperty.DefaultValue"/>
		/// and <see cref="BindableProperty.ReturnType"/> don't match.
		/// </exception>
		public BindableProperty Build()
		{
			if (string.IsNullOrEmpty(bpParameters.PropertyName))
				throw new ArgumentNullException(nameof(bpParameters.PropertyName), $"{nameof(bpParameters.PropertyName)} is required");
			if (bpParameters.ReturnType == default)
				throw new ArgumentNullException(nameof(bpParameters.ReturnType), $"{nameof(bpParameters.ReturnType)} is required");
			if (bpParameters.DeclaringType == default)
				throw new ArgumentNullException(nameof(bpParameters.DeclaringType), $"{nameof(bpParameters.DeclaringType)} is required");

			if (bpParameters.DefaultValue != null && bpParameters.ReturnType != bpParameters.DefaultValue.GetType())
				throw new ArgumentException($"The return type is {bpParameters.ReturnType.Name}, but the default value is of type {bpParameters.DefaultValue.GetType().Name}");

			var bp = BindableProperty.Create(propertyName: bpParameters.PropertyName,
											 returnType: bpParameters.ReturnType,
											 declaringType: bpParameters.DeclaringType,
											 defaultValue: bpParameters.DefaultValue,
											 defaultBindingMode: bpParameters.DefaultBindingMode,
											 validateValue: bpParameters.ValidateValueDelegate,
											 propertyChanged: bpParameters.BindingPropertyChangedDelegate,
											 propertyChanging: bpParameters.BindingPropertyChangingDelegate,
											 coerceValue: bpParameters.CoerceValueDelegate,
											 defaultValueCreator: bpParameters.CreateDefaultValueDelegate);
			return bp;
		}
	}
}
