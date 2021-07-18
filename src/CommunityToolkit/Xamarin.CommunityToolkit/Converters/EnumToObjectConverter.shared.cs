using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class EnumToObjectConverter : IValueConverter
    {
        public object? DefaultObject { get; set; }

        public IList<EnumToObjectConverterParameter> Objects { get; } = new List<EnumToObjectConverterParameter>();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
                return Objects.FirstOrDefault(item => Compare(enumValue, item))?.Object ?? DefaultObject;

            return value;
        }

        bool Compare(Enum value, EnumToObjectConverterParameter item)
            => Compare(value, item.Enum) || (item is List<Enum> enumValues && Compare(value, enumValues));

        bool Compare(Enum value, object? item)
            => item is Enum enumValue && Compare(value, enumValue);

        bool Compare(Enum value, List<Enum> items)
            => items.Any(item => Compare(value, item));

        bool Compare(Enum value, Enum item)
            => Equals(value, item);

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}