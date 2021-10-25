using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Checks if the value is between minValue and maxValue, returning true if the value is within the range and false if the value is out of the range.
	/// </summary>
	public class IsInRangeConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Backing BindableProperty for the <see cref="MinValue"/> property.
		/// </summary>
		public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(object), typeof(IsInRangeConverter));

		/// <summary>
		/// Gets or sets the minimum value of the range for the <see cref="IsInRangeConverter"/>. This is a bindable property.
		/// </summary>
		public object MinValue
		{
			get => GetValue(MinValueProperty);
			set => SetValue(MinValueProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="MaxValue"/> property.
		/// </summary>
		public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(object), typeof(IsInRangeConverter));

		/// <summary>
		/// Gets or sets the maximum value of the range for the <see cref="IsInRangeConverter"/>. This is a bindable property.
		/// </summary>
		public object MaxValue
		{
			get => GetValue(MaxValueProperty);
			set => SetValue(MaxValueProperty, value);
		}

		/// <summary>
		/// Checks if the value is between minValue and maxValue, returning true if the value is within the range and false if the value is out of the range.
		/// </summary>
		/// <param name="value">The object to compare.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>True if <paramref name="value"/> and <paramref name="parameter"/> are equal, False if they are not equal.</returns>
		public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is not IComparable comparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(value));

			if (MinValue is not IComparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(MinValue));

			if (MaxValue is not IComparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(MaxValue));

			return comparable.CompareTo(MinValue) >= 0 && comparable.CompareTo(MaxValue) <= 0;
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}