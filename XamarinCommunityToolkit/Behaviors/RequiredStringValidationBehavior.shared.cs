using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class RequiredStringValidationBehavior : ValidationBehavior
	{
		public static readonly BindableProperty RequiredStringProperty
			= BindableProperty.Create(nameof(RequiredString), typeof(string), typeof(RequiredStringValidationBehavior));

		public string RequiredString
		{
			get => (string)GetValue(RequiredStringProperty);
			set => SetValue(RequiredStringProperty, value);
		}

		protected override bool Validate(object value)
			=> value?.ToString() == RequiredString;
	}
}