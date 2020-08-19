using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class UriValidationBehavior : TextValidationBehavior
	{
		public static readonly BindableProperty UriKindProperty =
			BindableProperty.Create(nameof(UriKind), typeof(UriKind), typeof(UriValidationBehavior), UriKind.RelativeOrAbsolute, propertyChanged: OnValidationPropertyChanged);

		public UriKind UriKind
		{
			get => (UriKind)GetValue(UriKindProperty);
			set => SetValue(UriKindProperty, value);
		}

		protected override bool Validate(object value)
			=> base.Validate(value)
				&& Uri.IsWellFormedUriString(value?.ToString(), UriKind);
	}
}