using Xamarin.Toolkit.Behaviors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Behaviors.Tests
{
    [TestClass]
    public class EntryEmptyValidationTests
    {
        EntryEmptyValidation behavior;
        Entry entry;
        Color textColor;

        [TestInitialize]
        public void Setup()
        {
            behavior = new EntryEmptyValidation();
            behavior.TextColorInvalid = Color.Red;
            entry = new Entry();
            entry.TextColor = Color.Green;

            textColor = entry.TextColor;
            entry.Behaviors.Add(behavior);
        }

        [TestMethod]
        public void HasText()
        {
            entry.Text = "Hello World";

            Assert.IsTrue(behavior.IsValid, "Value should be valid, but is not.");

            Assert.AreEqual(entry.TextColor, textColor, "Color was changed, but shouldn't have.");
        }

        [TestMethod]
        public void NoText()
        {
            entry.Text = string.Empty;

            Assert.IsFalse(behavior.IsValid, "Value should not be valid, but is.");

            Assert.AreEqual(entry.TextColor, behavior.TextColorInvalid, "Color was not set to invalid text color.");
        }
    }
}
