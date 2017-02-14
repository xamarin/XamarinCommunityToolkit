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
    public class UpperTextTests
    {
        UpperTextConverter converter;

        [SetUp]
        public void Setup()
        {
            converter = new UpperTextConverter();
        }

        [Test]
        public void UpperText()
        {
            var returnVal = converter.Convert("Hello World", null, null, null);

            Assert.AreEqual("HELLO WORLD", returnVal);
        }
    }
}
