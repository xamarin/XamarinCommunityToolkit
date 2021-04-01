using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class DateTimeBetweenConverter : ValueConverterExtension, IValueConverter
	{
		public DateTime MinValue { get; } = DateTime.MinValue;

		public DateTime MaxValue { get; } = DateTime.MaxValue;

		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value is not DateTime datetimeValue)
				throw new ArgumentException("Value cannot be casted to DateTime", nameof(value));

			return MinValue <= datetimeValue && datetimeValue <= MaxValue;
		}

		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) =>
			throw new NotImplementedException();
	}
}
