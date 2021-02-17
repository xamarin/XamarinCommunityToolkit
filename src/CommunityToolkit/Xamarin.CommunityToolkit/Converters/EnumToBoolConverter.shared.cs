using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	///     Convert an <see cref="Enum" /> to corresponding <see cref="bool" />
	/// </summary>
	public class EnumToBoolConverter : ValueConverterExtension, IValueConverter
	{
		IList<Enum> trueValues = new List<Enum>();

		/// <summary>
		///     Enum values, that converts to <c>true</c> (optional)
		/// </summary>
		public IList<Enum> TrueValues
		{
			get => trueValues;
			set => trueValues = value ?? new List<Enum>();
		}

		/// <summary>
		///     Convert an <see cref="Enum" /> to corresponding <see cref="bool" />
		/// </summary>
		/// <param name="value"><see cref="Enum" /> value to convert</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">
		///     Additional parameter for converter. Can be used for comparison instead of
		///     <see cref="TrueValues" />
		/// </param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>
		///     False, if the value is not in <see cref="TrueValues" />. False, if <see cref="TrueValues" /> is empty and
		///     value not equal to parameter.
		/// </returns>
		/// <exception cref="ArgumentException">If value is not an <see cref="Enum" /></exception>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Enum enumValue))
				throw new ArgumentException("The value should be of type Enum", nameof(value));

			if (TrueValues.Count == 0 && !(parameter is Enum enumParam))
            {
                throw new ArgumentException("The parameter should be of type Enum, if TrueList not set",
					nameof(parameter));
            }

			return TrueValues.Count == 0
				? ConvertWithParameter(enumValue, parameter as Enum)
				: ConvertWithProperty(enumValue);
		}

		/// <inheritdoc/>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();

		bool ConvertWithParameter(Enum value, Enum parameter) => Equals(value, parameter);

		bool ConvertWithProperty(Enum value) => TrueValues.Contains(value);
	}
}