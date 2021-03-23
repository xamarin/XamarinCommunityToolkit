using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class MathExpressionConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Calculate the incoming expression string.
		/// </summary>
		/// <param name="value">The expression to calculate.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>A <see cref="double"/> The result of calculating an expression.</returns>
		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
		{
			if (value is not string expression)

				throw new ArgumentException("The value should be of type String");


			var math = new MathExpression(expression);
			var result = math.Calculate();
			return result;
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
			=> throw new NotImplementedException();
	}
}