using System;
using System.Globalization;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.UI.Views
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