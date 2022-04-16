using System;using Microsoft.Extensions.Logging;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Abstract class used to implement converters that do not implement the ConvertBack logic.
	/// </summary>
	/// <typeparam name="TFrom">Type of the input value</typeparam>
	/// <typeparam name="TTo">Type of the output value</typeparam>
	public abstract class BaseConverterOneWay<TFrom, TTo> : ValueConverterExtension, IValueConverter
															where TFrom : notnull
															where TTo : notnull
	{
		/// <summary>
		/// Method that will be called by <see cref="Convert(object, Type, object, CultureInfo)"/>.
		/// </summary>
		/// <param name="value">Value to be converted from <see cref="TFrom"/> to <see cref="TTo"/>.</param>
		/// <returns>An object of type <see cref="TTo"/>.</returns>
		public abstract TTo ConvertFrom(TFrom value);

		/// <summary>
		/// Not implemented, use <see cref="BaseConverter{TFrom, TTo}"/>
		/// </summary>
		public virtual object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotSupportedException("Impossible to revert to original value. Consider setting BindingMode to OneWay.");

		/// <inheritdoc/>
		object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is null)
				throw new ArgumentNullException(nameof(value), $"value needs to be of type {typeof(TFrom)}, but is null. If type {typeof(TFrom)} should be nullable, inherit from {nameof(BaseNullableConverterOneWay<TFrom, TTo>)} instead");

			if (value is not TFrom valueFrom)
				throw new ArgumentException($"value needs to be of type {typeof(TFrom)}");

			if (targetType != typeof(TTo) && !(typeof(TFrom) != typeof(string)))
				throw new ArgumentException($"targetType needs to be typeof {typeof(TTo)}");

			return ConvertFrom(valueFrom);
		}
	}

	/// <summary>
	/// Abstract class used to implement converters that support null and do not implement the ConvertBack logic.
	/// </summary>
	/// <typeparam name="TFrom">Type of the input value</typeparam>
	/// <typeparam name="TTo">Type of the output value</typeparam>
	public abstract class BaseNullableConverterOneWay<TFrom, TTo> : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Method that will be called by <see cref="Convert(object, Type, object, CultureInfo)"/>.
		/// </summary>
		/// <param name="value">Value to be converted from <see cref="TFrom"/> to <see cref="TTo"/>.</param>
		/// <returns>An object of type <see cref="TTo"/>.</returns>
		public abstract TTo? ConvertFrom(TFrom? value);

		/// <summary>
		/// Not implemented, use <see cref="BaseConverter{TFrom, TTo}"/>
		/// </summary>
		public virtual object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotSupportedException("Impossible to revert to original value. Consider setting BindingMode to OneWay.");

		/// <inheritdoc/>
		object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is null)
				return ConvertFrom(default);

			if (value is not TFrom valueFrom)
				throw new ArgumentException($"value needs to be of type {typeof(TFrom)}");

			if (targetType != typeof(TTo) && !(typeof(TFrom) != typeof(string)))
				throw new ArgumentException($"targetType needs to be typeof {typeof(TTo)}");

			return ConvertFrom(valueFrom);
		}
	}
}