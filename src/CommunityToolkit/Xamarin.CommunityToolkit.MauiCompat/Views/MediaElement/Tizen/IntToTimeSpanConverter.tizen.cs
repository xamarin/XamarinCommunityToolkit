using System;using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class IntToTimeSpanConverter : IValueConverter
	{
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			return TimeSpan.FromMilliseconds((int)value);
		}

		public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			return ((TimeSpan)value).Milliseconds;
		}
	}
}