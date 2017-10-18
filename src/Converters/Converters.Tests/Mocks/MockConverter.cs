using System;
using System.Globalization;
using Xamarin.Forms;

namespace Converters.Tests.Mocks
{
    public class MockConverter : IValueConverter
    {
        public const string TagFormat = "c{0}<-";

        public int Id { get; }

        public MockConverter()
        {
            Id = 0;
        }

        public MockConverter(int id)
        {
            Id = id;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(TagFormat, Id) + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
