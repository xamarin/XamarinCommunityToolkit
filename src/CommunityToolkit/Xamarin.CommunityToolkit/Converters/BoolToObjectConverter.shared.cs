using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts boolean to object and vice versa.
	/// </summary>
	public class BoolToObjectConverter : BoolToObjectConverter<object>
	{
	}

	/// <summary>
	/// Converts boolean to object and vice versa.
	/// </summary>
	public class BoolToObjectConverter<TObject> : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// The object that corresponds to True value.
		/// </summary>
		public TObject TrueObject { get; set; }

		/// <summary>
		/// The object that corresponds to False value.
		/// </summary>
		public TObject FalseObject { get; set; }

		/// <summary>
		/// Converts boolean to object.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>TrueObject if value equals True, otherwise FalseObject.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool result)
				return result ? TrueObject : FalseObject;

			throw new ArgumentException("Value is not a valid boolean", nameof(value));
		}

		/// <summary>
		/// Converts back object to boolean.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>True if value equals TrueObject, otherwise False.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is TObject result)
				return result.Equals(TrueObject);

			if (default(TObject) == null && value == null && TrueObject == null)
				return true;

			throw new ArgumentException($"Value is not a valid {typeof(TObject).Name}", nameof(value));
		}
	}
}