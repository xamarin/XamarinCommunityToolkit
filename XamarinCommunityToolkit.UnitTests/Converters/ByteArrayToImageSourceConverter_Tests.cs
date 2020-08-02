using Microsoft.Toolkit.Xamarin.Forms.Converters;
using System.Globalization;
using Xamarin.Forms;
using System.IO;
using System;
using Xunit;

namespace Microsoft.Toolkit.Xamarin.Forms.UnitTests.Converters
{
    public class ByteArrayToImageSourceConverter_Tests
    {
        [Fact]
        public void ByteArrayToImageSourceConverter()
        {
            var byteArray = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            var expectedValue = ImageSource.FromStream(() => new MemoryStream(byteArray));

            var byteArrayToImageSourceConverter = new ByteArrayToImageSourceConverter();

            var result = byteArrayToImageSourceConverter.Convert(byteArray, typeof(ByteArrayToImageSourceConverter), null, CultureInfo.CurrentCulture);
            Assert.Equal(result, expectedValue);
        }

        [Theory]
        [InlineData("Random String Value")]
        public void InvalidConverterValuesReturnsNull(object value)
        {
            var byteArrayToImageSourceConverter = new ByteArrayToImageSourceConverter();
            Assert.Null(byteArrayToImageSourceConverter.Convert(value, typeof(ByteArrayToImageSourceConverter), null, CultureInfo.CurrentCulture));
        }
    }
}
