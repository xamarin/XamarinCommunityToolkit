using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;

namespace Converters.Tests
{
    [TestClass]
    public class InvertedBooleanTests
    {
        InvertedBooleanConverter converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new InvertedBooleanConverter();
        }

        [TestMethod]
        public void TrueToFalse()
        {
            var returnVal = converter.Convert(true, null, null, null);

            Assert.AreEqual(false, returnVal);
        }

        [TestMethod]
        public void FalseToTrue()
        {
            var returnVal = converter.Convert(false, null, null, null);

            Assert.AreEqual(true, returnVal);
        }
    }
}
