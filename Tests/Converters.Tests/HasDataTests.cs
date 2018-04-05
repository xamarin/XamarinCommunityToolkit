using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;

namespace Converters.Tests
{
    [TestClass]
    public class HasDataTests
    {
        HasDataConverter converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new HasDataConverter();
        }

        [TestMethod]
        public void HasDataValueType()
        {
            var returnVal = converter.Convert(1, null, null, null);

            Assert.AreEqual(true, returnVal);
        }

        [TestMethod]
        public void HasDataList()
        {
            var returnVal = converter.Convert(new List<int> { 1 }, null, null, null);

            Assert.AreEqual(true, returnVal);
        }

        [TestMethod]
        public void HasDataString()
        {
            var returnVal = converter.Convert("Hello World", null, null, null);

            Assert.AreEqual(true, returnVal);
        }

        [TestMethod]
        public void HasNoDataValueType()
        {
            var returnVal = converter.Convert(null, null, null, null);

            Assert.AreEqual(false, returnVal);
        }

        [TestMethod]
        public void HasNoDataList()
        {
            var returnVal = converter.Convert(new List<int> { }, null, null, null);

            Assert.AreEqual(false, returnVal);
        }

        [TestMethod]
        public void HasNoDataString()
        {
            var returnVal = converter.Convert(string.Empty, null, null, null);

            Assert.AreEqual(false, returnVal);
        }

        [TestMethod]
        public void HasNoDataStringBlankSpace()
        {
            var returnVal = converter.Convert("    ", null, null, null);

            Assert.AreEqual(false, returnVal);
        }
    }
}
