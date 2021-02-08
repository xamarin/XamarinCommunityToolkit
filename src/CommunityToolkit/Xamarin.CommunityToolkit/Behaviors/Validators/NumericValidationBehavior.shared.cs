using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class NumericValidationBehavior : ValidationBehavior
	{
		public static readonly BindableProperty MinimumValueProperty =
			BindableProperty.Create(nameof(MinimumValue), typeof(double), typeof(NumericValidationBehavior), double.NegativeInfinity, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MaximumValueProperty =
			BindableProperty.Create(nameof(MaximumValue), typeof(double), typeof(NumericValidationBehavior), double.PositiveInfinity, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MinimumDecimalPlacesProperty =
			BindableProperty.Create(nameof(MinimumDecimalPlaces), typeof(int), typeof(NumericValidationBehavior), 0, propertyChanged: OnValidationPropertyChanged);

		public static readonly BindableProperty MaximumDecimalPlacesProperty =
			BindableProperty.Create(nameof(MaximumDecimalPlaces), typeof(int), typeof(NumericValidationBehavior), int.MaxValue, propertyChanged: OnValidationPropertyChanged);

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

		public int MinimumDecimalPlaces
		{
			get => (int)GetValue(MinimumDecimalPlacesProperty);
			set => SetValue(MinimumDecimalPlacesProperty, value);
		}

		public int MaximumDecimalPlaces
		{
			get => (int)GetValue(MaximumDecimalPlacesProperty);
			set => SetValue(MaximumDecimalPlacesProperty, value);
		}

		protected override object Decorate(object value)
			=> base.Decorate(value)?.ToString()?.Trim();

		protected override Task<bool> ValidateAsync(object value, CancellationToken token)
		{
			var valueString = value as string;
			if (!(double.TryParse(valueString, out var numeric)
				&& numeric >= MinimumValue
				&& numeric <= MaximumValue))
				return Task.FromResult(false);

			var decimalDelimeterIndex = valueString.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
			var hasDecimalDelimeter = decimalDelimeterIndex >= 0;

			// If MaximumDecimalPlaces equals zero, ".5" or "14." should be considered as invalid inputs.
			if (hasDecimalDelimeter && MaximumDecimalPlaces == 0)
				return Task.FromResult(false);

			var decimalPlaces = hasDecimalDelimeter
				? valueString.Substring(decimalDelimeterIndex + 1, valueString.Length - decimalDelimeterIndex - 1).Length
				: 0;

			return Task.FromResult(
				decimalPlaces >= MinimumDecimalPlaces &&
				decimalPlaces <= MaximumDecimalPlaces);
		}
	}
}