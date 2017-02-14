using FormsCommunityToolkit.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters.Tests
{
    [TestFixture]
    public class HasDataTests
    {
        HasDataConverter converter;

        [SetUp]
        public void Setup()
        {
            converter = new HasDataConverter();
        }
        [Test]
        public void HasDataValueType()
        {
            var returnVal = converter.Convert(1, null, null, null);

            Assert.AreEqual(true, returnVal);
        }


        [Test]
        public void HasDataList()
        {
            var returnVal = converter.Convert(new List<int> { 1 }, null, null, null);

            Assert.AreEqual(true, returnVal);
        }


        [Test]
        public void HasDataString()
        {
            var returnVal = converter.Convert("Hello World", null, null, null);

            Assert.AreEqual(true, returnVal);
        }

        [Test]
        public void HasNoDataValueType()
        {
            var returnVal = converter.Convert(null, null, null, null);

            Assert.AreEqual(false, returnVal);
        }


        [Test]
        public void HasNoDataList()
        {
            var returnVal = converter.Convert(new List<int> {  }, null, null, null);

            Assert.AreEqual(false, returnVal);
        }


        [Test]
        public void HasNoDataString()
        {
            var returnVal = converter.Convert(string.Empty, null, null, null);

            Assert.AreEqual(false, returnVal);
        }

        [Test]
        public void HasNoDataStringBlankSpace()
        {
            var returnVal = converter.Convert("    ", null, null, null);

            Assert.AreEqual(false, returnVal);
        }
    }
}
