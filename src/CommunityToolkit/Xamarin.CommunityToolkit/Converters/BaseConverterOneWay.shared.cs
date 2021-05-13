using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public abstract class BaseConverterOneWay<TFrom, TTo> : ValueConverterExtension, IValueConverter
	{
		public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not TFrom valueFrom)
				throw new ArgumentException($"value needs to be of type {typeof(TFrom)}");

			if (targetType != typeof(TTo) && !(typeof(TFrom) != typeof(string)))
				throw new ArgumentException($"targetType needs to be typeof {typeof(TTo)}");

			return ConvertFrom(valueFrom);
		}

		public abstract TTo ConvertFrom(TFrom value);

		public virtual object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException("Impossible to revert to original value. Consider setting BindingMode to OneWay.");
	}
}
