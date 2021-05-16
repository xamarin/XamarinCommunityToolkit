using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class IsInRangeConverter : BindableObjectExtension, IValueConverter
	{
		public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(object), typeof(IsInRangeConverter));

		public object MinValue
		{
			get => GetValue(MinValueProperty);
			set => SetValue(MinValueProperty, value);
		}

		public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(object), typeof(IsInRangeConverter));

		public object MaxValue
		{
			get => GetValue(MaxValueProperty);
			set => SetValue(MaxValueProperty, value);
		}

		public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is not IComparable comparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(value));

			if (MinValue is not IComparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(MinValue));

			if (MaxValue is not IComparable)
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(MaxValue));

			return comparable.CompareTo(MinValue) >= 0 && comparable.CompareTo(MaxValue) <= 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}
