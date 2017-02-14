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
    public class LowerTextTests
    {
        LowerTextConverter converter;

        [SetUp]
        public void Setup()
        {
            converter = new LowerTextConverter();
        }

        [Test]
        public void LowerText()
        {
            var returnVal = converter.Convert("Hello World", null, null, null);

            Assert.AreEqual("hello world", returnVal);
        }        
    }
}
