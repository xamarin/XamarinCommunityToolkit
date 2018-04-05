using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;

namespace Converters.Tests
{
    [TestClass]
    public class UpperTextTests
    {
        UpperTextConverter converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new UpperTextConverter();
        }

        [TestMethod]
        public void UpperText()
        {
            var returnVal = converter.Convert("Hello World", null, null, null);

            Assert.AreEqual("HELLO WORLD", returnVal);
        }
    }
}
