using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Helpers
{
	class BindablePropertyParameters
	{
		/// <summary>
		/// Indicates the type (usually a control) declaring the <see cref="BindableProperty"/>
		/// </summary>
		public Type DeclaringType { get; set; }

		/// <summary>
		/// The BindingMode to use on SetBinding() if no <see cref="BindingMode"/> is given. This property is optional. Default is <see cref="BindingMode.OneWay"/>.
		/// </summary>
		public BindingMode DefaultBindingMode { get; set; }

		/// <summary>
		/// Indicates the property's default value. This property is optional. Default value is null.
		/// </summary>
		public object DefaultValue { get; set; }

		/// <summary>
		/// Indicates the name of the property that is bound to the <see cref="BindableProperty"/>
		/// </summary>
		public string PropertyName { get; set; }

		/// <summary>
		/// Indicates the type of the value that is bound to the <see cref="BindableProperty"/>
		/// </summary>
		public Type ReturnType { get; set; }

		/// <summary>
		/// A delegate to be run when the value has changed. This property is optional. Default is null.
		/// </summary>
		public BindableProperty.BindingPropertyChangedDelegate BindingPropertyChangedDelegate { get; set; }

		/// <summary>
		/// A delegate to be run when the value will change. This property is optional. Default is null.
		/// </summary>
		public BindableProperty.BindingPropertyChangingDelegate BindingPropertyChangingDelegate { get; set; }

		/// <summary>
		/// A delegate used to coerce the range of a value. This property is optional. Default is null.
		/// </summary>
		public BindableProperty.CoerceValueDelegate CoerceValueDelegate { get; set; }

		/// <summary>
		/// A Func used to initialize default value for reference types. This property is optional. Default is null.
		/// </summary>
		public BindableProperty.CreateDefaultValueDelegate CreateDefaultValueDelegate { get; set; }

		/// <summary>
		/// A delegate to be run when a value is set. This property is optional. Default is null.
		/// </summary>
		public BindableProperty.ValidateValueDelegate ValidateValueDelegate { get; set; }

		public BindablePropertyParameters() => ResetParameters();

		public void ResetParameters()
		{
			DeclaringType = null;
			DefaultBindingMode = BindingMode.OneWay;
			DefaultValue = null;
			PropertyName = null;
			ReturnType = null;

			BindingPropertyChangedDelegate = null;
			BindingPropertyChangingDelegate = null;
			CoerceValueDelegate = null;
			CreateDefaultValueDelegate = null;
			ValidateValueDelegate = null;
		}
	}
}
