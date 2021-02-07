using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts/Extracts the incoming value from ItemTappedEventArgs object and returns the value of Item property from it.
	/// </summary>
	public class ItemTappedEventArgsConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts/Extracts the incoming value from ItemTappedEventArgs object and returns the value of Item property from it.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A Item object from object of type ItemTappedEventArgs.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is ItemTappedEventArgs itemTappedEventArgs
				? itemTappedEventArgs.Item
				: throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}