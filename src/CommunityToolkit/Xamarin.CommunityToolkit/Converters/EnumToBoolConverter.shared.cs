using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions.Internals;
using System.Reflection;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	///     Convert an <see cref="Enum" /> to corresponding <see cref="bool" />
	/// </summary>
	public class EnumToBoolConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		///     Enum values, that converts to <c>true</c> (optional)
		/// </summary>
		public IList<Enum> TrueValues { get; } = new List<Enum>();

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

			static bool CompareTwoEnums(Enum valueToCheck, object referenceValue)
			{
				if (!(referenceValue is Enum referenceEnumValue))
					return false;

				if (valueToCheck.GetType() != referenceEnumValue.GetType())
					return false;

				if (valueToCheck.GetType().GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
					return referenceEnumValue.HasFlag(valueToCheck);

				return Equals(valueToCheck, referenceEnumValue);
			}

			return TrueValues.Count == 0
				? CompareTwoEnums(enumValue, parameter as Enum)
				: TrueValues.Any(item => CompareTwoEnums(enumValue, item));
		}

		/// <inheritdoc/>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}