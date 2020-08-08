using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class NumericValidationBehavior : ValidationBehavior
	{
		public static readonly BindableProperty MinimumValueProperty =
			BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(NumericValidationBehavior), double.NegativeInfinity, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MaximumValueProperty =
			BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(NumericValidationBehavior), double.PositiveInfinity, propertyChanged: OnValidationPropertyChanged);

		public double MinimumValue
		{
			get => (double)GetValue(MinimumValueProperty);
			set => SetValue(MinimumValueProperty, value);
		}

		public double MaximumValue
		{
			get => (double)GetValue(MaximumValueProperty);
			set => SetValue(MaximumValueProperty, value);
		}

		protected override bool Validate(object value)
			=> double.TryParse(value?.ToString(), out var numeric)
				&& numeric >= MinimumValue
				&& numeric <= MaximumValue;
	}
}