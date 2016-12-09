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
    public class InvertedBooleanTests
    {

        InvertedBooleanConverter converter;

        [SetUp]
        public void Setup()
        {
            converter = new InvertedBooleanConverter();
        }

        [Test]
        public void TrueToFalse()
        {
            var returnVal = converter.Convert(true, null, null, null);

            Assert.AreEqual(false, returnVal);
        }

        [Test]
        public void FalseToTrue()
        {
            var returnVal = converter.Convert(false, null, null, null);

            Assert.AreEqual(true, returnVal);
        }
    }
}
