using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;

namespace Converters.Tests
{
    [TestClass]
    public class NegateIntegerConverterTest
    {
        NegateIntegerConverter converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new NegateIntegerConverter();
        }

        [TestMethod]
        public void Convert_ConvertBackReturns_0_IfInvalidValue()
        {
            var invalidValue = "InvalidValue";
            var result = converter.Convert(invalidValue, null, null, null);

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void Convert_ConvertBackNegatesValue()
        {
            var validValue = 5;
            var result = converter.Convert(validValue, null, null, null);

            Assert.AreEqual(result, -5);
        }
    }
}
