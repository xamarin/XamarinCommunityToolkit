﻿using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	/// <summary>
	/// The <see cref="RequiredStringValidationBehavior"/> is a behavior that allows the user to determine if text input is equal to specific text. For example, an <see cref="Entry"/> control can be styled differently depending on whether a valid or an invalid text input is provided. Additional properties handling validation are inherited from <see cref="ValidationBehavior"/>.
	/// </summary>
	public class RequiredStringValidationBehavior : ValidationBehavior
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="RequiredString"/> property.
		/// </summary>
		public static readonly BindableProperty RequiredStringProperty
			= BindableProperty.Create(nameof(RequiredString), typeof(string), typeof(RequiredStringValidationBehavior));

		/// <summary>
		/// The string that will be compared to the value provided by the user. This is a bindable property.
		/// </summary>
		public string RequiredString
		{
			get => (string)GetValue(RequiredStringProperty);
			set => SetValue(RequiredStringProperty, value);
		}

		protected override Task<bool> ValidateAsync(object value, CancellationToken token)
			=> Task.FromResult(value?.ToString() == RequiredString);
	}
}