using System;
using System.Threading;
using System.Threading.Tasks;
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

		protected override async Task<bool> ValidateAsync(object value, CancellationToken token)
			=> await base.ValidateAsync(value, token)
				&& Uri.IsWellFormedUriString(value?.ToString(), UriKind);
	}
}