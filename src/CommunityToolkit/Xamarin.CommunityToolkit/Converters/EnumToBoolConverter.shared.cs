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
		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value is not Enum enumValue)
				throw new ArgumentException("The value should be of type Enum", nameof(value));

 			if (TrueValues.Count == 0 && parameter != null && parameter is not Enum)
            {
                var valueType = value.GetType();
				// Check if parameter is an Array
                if (parameter is Array arrayParms)
                {
                    foreach (var arrayParm in arrayParms)
                    {
                        if (arrayParm.GetType() == valueType)
                            TrueValues.Add((Enum)arrayParm);
                    }
                }
                else
                {
                    var parameterEnumArrayType = parameter.GetType().GetGenericArguments().FirstOrDefault();
					// Check if parameter is a List of Enum
                    if (valueType == parameterEnumArrayType)
                    {
                        if (parameter is IEnumerable list)
                        {
                            foreach (var item in list)
                            {
                                TrueValues.Add((Enum)item);
                            }
                        }
                    }
                }
            }

			return TrueValues.Count == 0
				? CompareTwoEnums(enumValue, parameter as Enum)
				: TrueValues.Any(item => CompareTwoEnums(enumValue, item));

			static bool CompareTwoEnums(Enum valueToCheck, object? referenceValue)
			{
				if (referenceValue is not Enum referenceEnumValue)
					return false;

				var valueToCheckType = valueToCheck.GetType();
				if (valueToCheckType != referenceEnumValue.GetType())
					return false;

				if (valueToCheckType.GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
					return referenceEnumValue.HasFlag(valueToCheck);

				return Equals(valueToCheck, referenceEnumValue);
			}
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) =>
			throw new NotImplementedException();
	}
}
