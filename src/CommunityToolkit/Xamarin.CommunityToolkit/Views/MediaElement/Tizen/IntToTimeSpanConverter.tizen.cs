﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class IntToTimeSpanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return TimeSpan.FromMilliseconds((int)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((TimeSpan)value).Milliseconds;
		}
	}
}