﻿using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to a <see cref="string"/> local time represenation in the
	/// datetime format provided.
	/// </summary>
	public class UtcDateTimeToLocalStringConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Gets or sets the datetime format value of the provided datetime or datetimeoffset for
		/// the <see cref="UtcDateTimeToLocalStringConverter"/>. This is a bindable property.
		/// </summary>
		public string DateTimeFormat
		{
			get => (string)GetValue(DateTimeFormatProperty);
			set => SetValue(DateTimeFormatProperty, value);
		}

		/// <summary>
		/// Backing BindableProperty for the <see cref="DateTimeFormat"/> property.
		/// </summary>
		public static readonly BindableProperty DateTimeFormatProperty = BindableProperty.Create(nameof(DateTimeFormat), typeof(string), typeof(UtcDateTimeToLocalStringConverter), defaultValue: "g");

		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value is DateTime or DateTimeOffset)
			{
				if (!string.IsNullOrEmpty(DateTimeFormat))
				{
					if (DateTimeFormat is string dateTimeFormat && !IsValidDateFormat(DateTimeFormat))
						throw new ArgumentException("Value must be a valid date time format", nameof(DateTimeFormat));
				}

				if (value is DateTime dateTime)
					return dateTime.ToLocalTime().ToString(DateTimeFormat);

				if (value is DateTimeOffset dateTimeOffset)
					return dateTimeOffset.UtcDateTime.ToLocalTime().ToString(DateTimeFormat);

				return string.Empty;
			}

			throw new ArgumentException("Value must be type DateTime or DateTimeOffset", nameof(value));
		}

		bool IsValidDateFormat(string dateFormat)
		{
			try
			{
				var s = DateTime.Now.ToString(dateFormat, CultureInfo.InvariantCulture);
				DateTime.Parse(s, CultureInfo.InvariantCulture);
				return true;
			}
			catch
			{
				return false;
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
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
	}
}