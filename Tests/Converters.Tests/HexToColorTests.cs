using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms;
using Xamarin.Toolkit.Converters;

namespace Converters.Tests
{
    [TestClass]
    public class HexToColorTests
    {
        HexToColorConverter converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new HexToColorConverter();
        }

        [TestMethod]
        public void HexNull()
        {
            var returnVal = converter.Convert(null, null, null, null);

            Assert.AreEqual(converter.DefaultColor, returnVal);
        }

        [TestMethod]
        public void HexWhite()
        {
            var returnVal = converter.Convert("#FFFFFF", null, null, null);

            Assert.AreEqual(Color.White, returnVal);
        }

        [TestMethod]
        public void HexWhiteAlpha()
        {
            var returnVal = (Color)converter.Convert("#80FFFFFF", null, null, null);

            Assert.AreEqual(Color.White.R, returnVal.R);
            Assert.AreEqual(Color.White.G, returnVal.G);
            Assert.AreEqual(Color.White.B, returnVal.B);
            Assert.AreEqual(50, (int)(returnVal.A * 100));
        }
    }
}
