using Xamarin.Toolkit.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
