using System;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace Xamarin.CommunityToolkit.UnitTests.Extensions
{
    public class EdgeInsetsExtension_Tests

    {
        [TestCase(1, 2, 3)]
        public void Top_ShouldReturnSpecifiedSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Top = value,
                Vertical = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(value, result.Top);
        }
        
        [TestCase(double.MinValue, 2, 3)]
        public void Top_ShouldReturnVerticalSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Top = value,
                Vertical = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative1, result.Top);
        }
        
        [TestCase(double.MinValue, double.MinValue, 3)]
        public void Top_ShouldReturnDefaultSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Top = value,
                Vertical = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative2, result.Top);
        }
        
        [TestCase(1, 2, 3)]
        public void Bottom_ShouldReturnSpecifiedSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Bottom = value,
                Vertical = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(value, sut.Bottom);
            Assert.AreEqual(value, result.Bottom);
        }
        
        [TestCase(double.MinValue, 2, 3)]
        public void Bottom_ShouldReturnVerticalSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Bottom = value,
                Vertical = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative1, result.Bottom);
        }
        
        [TestCase(double.MinValue, double.MinValue, 3)]
        public void Bottom_ShouldReturnDefaultSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Bottom = value,
                Vertical = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative2, result.Bottom);
        }
        
        [TestCase(1, 2, 3)]
        public void Left_ShouldReturnSpecifiedSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Left = value,
                Horizontal = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(value, result.Left);
        }
        
        [TestCase(double.MinValue, 2, 3)]
        public void Left_ShouldReturnVerticalSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Left = value,
                Horizontal = alternative1,
                All = alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative1, result.Left);
        }
        
        [TestCase(double.MinValue, double.MinValue, 3)]
        public void Left_ShouldReturnDefaultSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Left = value,
                Horizontal = alternative1,
                All =  alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative2, result.Left);
        }
        
        [TestCase(1, 2, 3)]
        public void Right_ShouldReturnSpecifiedSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Right = value,
                Horizontal = alternative1,
                All =  alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(value, result.Right);
        }
        
        [TestCase(double.MinValue, 2, 3)]
        public void Right_ShouldReturnVerticalSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Right = value,
                Horizontal = alternative1,
                All =  alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative1, result.Right);
        }
        
        [TestCase(double.MinValue, double.MinValue, 3)]
        public void Right_ShouldReturnDefaultSpacing(double value, double alternative1, double alternative2)
        {
            var sut = new EdgeInsetsExtension
            {
                Right = value,
                Horizontal = alternative1,
                All =  alternative2
            };

            var result = (Thickness)sut.ProvideValue(null);
            
            Assert.AreEqual(alternative2, result.Right);
        }
    }
}