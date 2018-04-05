using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;

namespace Converters.Tests
{
    [TestClass]
    public class LowerTextTests
    {
        LowerTextConverter converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new LowerTextConverter();
        }

        [TestMethod]
        public void LowerText()
        {
            var returnVal = converter.Convert("Hello World", null, null, null);

            Assert.AreEqual("hello world", returnVal);
        }
    }
}
