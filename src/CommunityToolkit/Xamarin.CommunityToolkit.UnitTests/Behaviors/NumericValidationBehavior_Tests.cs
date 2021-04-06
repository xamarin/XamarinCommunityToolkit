using System.Globalization;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class NumericValidationBehavior_Tests
	{
		[SetUp]
		public void Setup() => Device.PlatformServices = new MockPlatformServices();

		[TestCase("en-US", "15.2", 1.0, 16.0, 0, 16, true)]
		[TestCase("en-US", "15.", 1.0, 16.0, 0, 1, true)]
		[TestCase("en-US", "15.88", 1.0, 16.0, 2, 2, true)]
		[TestCase("en-US", "0.99", 0.9, 2.0, 0, 16, true)]
		[TestCase("en-US", ".99", 0.9, 2.0, 0, 16, true)]
		[TestCase("en-US", "1,115.2", 1.0, 2000.0, 0, 16, true)]
		[TestCase("de-DE", "15,2", 1.0, 16.0, 0, 16, true)]
		[TestCase("de-DE", "15,", 1.0, 16.0, 0, 1, true)]
		[TestCase("de-DE", "15,88", 1.0, 16.0, 2, 2, true)]
		[TestCase("de-DE", "0,99", 0.9, 2.0, 0, 16, true)]
		[TestCase("de-DE", ",99", 0.9, 2.0, 0, 16, true)]
		[TestCase("de-DE", "1.115,2", 1.0, 2000.0, 0, 16, true)]
		[TestCase("en-US", "15.3", 16.0, 20.0, 0, 16, false)]
		[TestCase("en-US", "15.3", 0.0, 15.0, 0, 16, false)]
		[TestCase("en-US", "15.", 1.0, 16.0, 0, 0, false)]
		[TestCase("en-US", ".7", 0.0, 16.0, 0, 0, false)]
		[TestCase("en-US", "15", 1.0, 16.0, 1, 16, false)]
		[TestCase("en-US", "", 0.0, 16.0, 0, 16, false)]
		[TestCase("en-US", " ", 0.0, 16.0, 0, 16, false)]
		[TestCase("en-US", null, 0.0, 16.0, 0, 16, false)]
		[TestCase("en-US", "15,2", 1.0, 16.0, 0, 16, false)]
		[TestCase("en-US", "1.115,2", 1.0, 2000.0, 0, 16, false)]
		[TestCase("de-DE", "15,3", 16.0, 20.0, 0, 16, false)]
		[TestCase("de-DE", "15,3", 0.0, 15.0, 0, 16, false)]
		[TestCase("de-DE", "15,", 1.0, 16.0, 0, 0, false)]
		[TestCase("de-DE", ",7", 0.0, 16.0, 0, 0, false)]
		[TestCase("de-DE", "15", 1.0, 16.0, 1, 16, false)]
		[TestCase("de-DE", "", 0.0, 16.0, 0, 16, false)]
		[TestCase("de-DE", " ", 0.0, 16.0, 0, 16, false)]
		[TestCase("de-DE", null, 0.0, 16.0, 0, 16, false)]
		[TestCase("de-DE", "15.2", 1.0, 16.0, 0, 16, false)]
		[TestCase("de-DE", "1,115.2", 1.0, 2000.0, 0, 16, false)]
		public async Task IsValid(string culture, string value, double minValue, double maxValue, int minDecimalPlaces, int maxDecimalPlaces, bool expectedValue)
		{
			// Arrange
			var origCulture = CultureInfo.CurrentCulture;
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);

			var behavior = new NumericValidationBehavior
			{
				MinimumValue = minValue,
				MaximumValue = maxValue,
				MinimumDecimalPlaces = minDecimalPlaces,
				MaximumDecimalPlaces = maxDecimalPlaces
			};

			var entry = new Entry
			{
				Text = value
			};
			entry.Behaviors.Add(behavior);

			try
			{
				// Act
				await behavior.ForceValidate();

				// Assert
				Assert.AreEqual(expectedValue, behavior.IsValid);
			}
			finally
			{
				CultureInfo.CurrentCulture = origCulture;
			}
		}
	}
}