using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class IsInRangeConverter : BindableObject, IValueConverter
	{
		public bool RevertResult { get; set; } = false;

		public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(object), typeof(IsInRangeConverter), null);

		public object MinValue
		{
			get => GetValue(MinValueProperty);
			set => SetValue(MinValueProperty, value);
		}

		public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(object), typeof(IsInRangeConverter), null);

		public object MaxValue
		{
			get => GetValue(MaxValueProperty);
			set => SetValue(MaxValueProperty, value);
		}

		public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value == null)
				throw new ArgumentException("Value cannot be null", nameof(value));

			if (MinValue == null)
				throw new ArgumentException("Value cannot be null", nameof(MinValue));

			if (MaxValue == null)
				throw new ArgumentException("Value cannot be null", nameof(MaxValue));

			if (value.GetType() != MinValue.GetType())
				throw new ArgumentException("Values cannot be different types", nameof(MinValue));

			if (value.GetType() != MaxValue.GetType())
				throw new ArgumentException("Values cannot be different types", nameof(MinValue));

			if (value is IComparable comparable)
			{
				var result = comparable.CompareTo(MinValue) >= 0 && comparable.CompareTo(MaxValue) <= 0;
				return RevertResult ? !result : result;
			}

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}
