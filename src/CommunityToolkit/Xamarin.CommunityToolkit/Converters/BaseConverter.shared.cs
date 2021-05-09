using System;
using System.Globalization;

namespace Xamarin.CommunityToolkit.Converters
{
	public abstract class BaseConverter<TFrom, TTo> : BaseConverterOneWay<TFrom, TTo>
	{
		public sealed override object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not TTo valueFrom)
				throw new ArgumentException($"value needs to be of type {typeof(TTo)}", nameof(value));

			if (targetType != typeof(TFrom))
				throw new ArgumentException($"targetType needs to be typeof {typeof(TFrom)}", nameof(targetType));

			return ConvertBackTo(valueFrom);
		}

		public abstract TFrom ConvertBackTo(TTo value);
	}
}
