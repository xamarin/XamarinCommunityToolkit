using Xunit;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Behaviors;

namespace Microsoft.Toolkit.Xamarin.Forms.UnitTests.Behaviors
{
    public class RequiredStringValidationBehavior_Tests
    {
        public RequiredStringValidationBehavior_Tests()
             => Device.PlatformServices = new MockPlatformServices();
        
        [Fact]
        public void IsValidTrueWhenBothIsNull_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new RequiredStringValidationBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnAttaching;
            //act
            //passwordEntry.Text = "123456";
            confirmPasswordBehavior.ComparedToString = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            //confirmPasswordEntry.Text = "123456";
            //assert
            Assert.True(confirmPasswordBehavior.IsValid);
        }
        
        [Fact]
        public void IsValidFalseWhenOneIsNull_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new RequiredStringValidationBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnAttaching;
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.ComparedToString = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = null;
            //assert
            Assert.False(confirmPasswordBehavior.IsValid);
        }

        [Fact]
        public void IsValidTrueWhenEnterSameText_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new RequiredStringValidationBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnValueChanging;
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.ComparedToString = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = "123456";
            //assert
            Assert.True(confirmPasswordBehavior.IsValid);
        }
        
        [Fact]
        public void IsValidFalseWhenEnterDifferentText_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new RequiredStringValidationBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnValueChanging;
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.ComparedToString = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = "1234567";
            //assert
            Assert.False(confirmPasswordBehavior.IsValid);
        }
    }
}
