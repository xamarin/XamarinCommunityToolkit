using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts double to integer and vice versa.
	/// </summary>
	[ContentProperty(nameof(Ratio))]
	public class DoubleToIntConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Multiplier / Denominator (Equals 1 by default).
		/// </summary>
		public double Ratio { get; set; } = 1;

		/// <summary>
		/// Converts double to integer.
		/// </summary>
		/// <param name="value">Double value.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Multiplier (Equals 1 by default).</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>Integer value.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is double result
				? (int)Math.Round(result * GetParameter(parameter))
				: throw new ArgumentException("Value is not a valid double", nameof(value));

		/// <summary>
		/// Converts back integer to double.
		/// </summary>
		/// <param name="value">Integer value.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Denominator (Equals 1 by default).</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>Double value.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is int result
				? result / GetParameter(parameter)
				: throw new ArgumentException("Value is not a valid integer", nameof(value));

		double GetParameter(object parameter)
			=> parameter == null
			? Ratio
			: parameter switch
			{
				double d => d,
				int i => i,
				string s => double.TryParse(s, out var result)
					? result
					: throw new ArgumentException("Cannot parse number from the string", nameof(parameter)),
				_ => 1,
			};
	}
}