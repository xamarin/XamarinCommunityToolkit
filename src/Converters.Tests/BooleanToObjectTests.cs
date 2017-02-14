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
    public class BooleanToObjectTests
    {
        public class Person
        {
            public string Name { get; set; }
        }

        BooleanToObjectConverter<Person> converter;
        Person truePerson;
        Person falsePerson;

        [SetUp]
        public void Setup()
        {

            truePerson = new Person { Name = "James" };
            falsePerson = new Person { Name = "Motz" };
            converter = new BooleanToObjectConverter<Person>();
            converter.TrueObject = truePerson;
            converter.FalseObject = falsePerson;
        }

        [Test]
        public void TrueObjectConvert()
        {
            var returnVal = converter.Convert(true, null, null, null);

            // TODO: Add your test code here
            Assert.AreEqual(truePerson, returnVal, "True value was not returned.");
        }

        [Test]
        public void FalseObjectConvert()
        {
            var returnVal = converter.Convert(false, null, null, null);

            // TODO: Add your test code here
            Assert.AreEqual(falsePerson, returnVal, "False value was not returned.");
        }

        [Test]
        public void TrueObjectConvertBack()
        {
            var returnVal = converter.ConvertBack(truePerson, null, null, null);

            // TODO: Add your test code here
            Assert.AreEqual(true, returnVal, "True value was not returned.");
        }

        [Test]
        public void FalseObjectConvertBack()
        {
            var returnVal = converter.ConvertBack(falsePerson, null, null, null);

            // TODO: Add your test code here
            Assert.AreEqual(false, returnVal, "False value was not returned.");
        }
    }
}
