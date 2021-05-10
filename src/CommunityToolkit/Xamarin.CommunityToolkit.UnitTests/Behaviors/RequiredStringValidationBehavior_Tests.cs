using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class RequiredStringValidationBehavior_Tests
	{
		[SetUp]
		public void Setup() => Device.PlatformServices = new MockPlatformServices();

		[Test]
		public void IsValidTrueWhenBothIsNull_Test()
		{
			// Arrange
			var passwordEntry = new Entry();
			var confirmPasswordEntry = new Entry();
			var confirmPasswordBehavior = new RequiredStringValidationBehavior();
			confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnAttaching;

			// Act
			confirmPasswordBehavior.RequiredString = passwordEntry.Text;
			confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);

			// Assert
			Assert.IsTrue(confirmPasswordBehavior.IsValid);
		}

		[Test]
		public void IsValidFalseWhenOneIsNull_Test()
		{
			// Arrange
			var passwordEntry = new Entry();
			var confirmPasswordEntry = new Entry();
			var confirmPasswordBehavior = new RequiredStringValidationBehavior();
			confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnAttaching;

			// Act
			passwordEntry.Text = "123456";
			confirmPasswordBehavior.RequiredString = passwordEntry.Text;
			confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);

			confirmPasswordEntry.Text = null;

			// Assert
			Assert.False(confirmPasswordBehavior.IsValid);
		}

		[Test]
		public void IsValidTrueWhenEnterSameText_Test()
		{
			// Arrange
			var passwordEntry = new Entry();
			var confirmPasswordEntry = new Entry();
			var confirmPasswordBehavior = new RequiredStringValidationBehavior();
			confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnValueChanging;

			// Act
			passwordEntry.Text = "123456";
			confirmPasswordBehavior.RequiredString = passwordEntry.Text;
			confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
			confirmPasswordEntry.Text = "123456";

			// Assert
			Assert.IsTrue(confirmPasswordBehavior.IsValid);
		}

		[Test]
		public void IsValidFalseWhenEnterDifferentText_Test()
		{
			// Arrange
			var passwordEntry = new Entry();
			var confirmPasswordEntry = new Entry();
			var confirmPasswordBehavior = new RequiredStringValidationBehavior();
			confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnValueChanging;

			// Act
			passwordEntry.Text = "123456";
			confirmPasswordBehavior.RequiredString = passwordEntry.Text;
			confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
			confirmPasswordEntry.Text = "1234567";

			// Assert
			Assert.False(confirmPasswordBehavior.IsValid);
		}
	}
}