using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converter to perform math operations
	/// </summary>
	public class MathOperationValueConverter : ValueConverterExtension, IValueConverter
	{
		public OperatorEnum Operator { get; set; }
		public double OperatorValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double.TryParse(value?.ToString(), out var actualValue);

			return Operator switch
			{
				OperatorEnum.Add => (int)(actualValue + OperatorValue),
				OperatorEnum.Minus => (int)(actualValue - OperatorValue),
				OperatorEnum.Multiply => (int)(actualValue * OperatorValue),
				OperatorEnum.Divide => actualValue.Equals(0) || OperatorValue.Equals(0) ? 0 : (int)(actualValue / OperatorValue),
				OperatorEnum.Remainder => (int)Math.IEEERemainder(actualValue, OperatorValue),
				OperatorEnum.Exponent => (int)Math.Pow(actualValue, OperatorValue),
				_ => throw new ArgumentException(nameof(Operator)),
			};
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}

	public enum OperatorEnum
	{
		Unknown = 0,
		Add = 1,
		Minus = 2,
		Multiply = 3,
		Divide = 4,
		Remainder = 5,
		Exponent = 6
	}
}