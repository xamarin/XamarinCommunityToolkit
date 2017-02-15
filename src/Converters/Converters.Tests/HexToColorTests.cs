using FormsCommunityToolkit.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Converters.Tests
{
    [TestFixture]
    public class HexToColorTests
    {
        HexToColorConverter converter;

        [SetUp]
        public void Setup()
        {
            converter = new HexToColorConverter();
        }

        [Test]
        public void HexNull()
        {
            var returnVal = converter.Convert(null, null, null, null);

            Assert.AreEqual(converter.DefaultColor, returnVal);
        }

        [Test]
        public void HexWhite()
        {
            var returnVal = converter.Convert("#FFFFFF", null, null, null);

            Assert.AreEqual(Color.White, returnVal);
        }

        [Test]
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
