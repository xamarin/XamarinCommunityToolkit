using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class NumericValidationBehavior_Tests
	{
		public NumericValidationBehavior_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Theory]
		// Positive
		[InlineData("15.2", 1.0, 16.0, 0, 16, true)]
		[InlineData("15.", 1.0, 16.0, 0, 1, true)]
		[InlineData("15.88", 1.0, 16.0, 2, 2, true)]
		[InlineData("0.99", 0.9, 2.0, 0, 16, true)]
		[InlineData(".99", 0.9, 2.0, 0, 16, true)]
		// Negative
		[InlineData("15.3", 16.0, 20.0, 0, 16, false)]
		[InlineData("15.3", 0.0, 15.0, 0, 16, false)]
		[InlineData("15.", 1.0, 16.0, 0, 0, false)]
		[InlineData(".7", 0.0, 16.0, 0, 0, false)]
		[InlineData("15", 1.0, 16.0, 1, 16, false)]
		[InlineData("", 0.0, 16.0, 0, 16, false)]
		[InlineData(" ", 0.0, 16.0, 0, 16, false)]
		[InlineData(null, 0.0, 16.0, 0, 16, false)]
		public void IsValid(string value, double minValue, double maxValue, int minDecimalPlaces, int maxDecimalPlaces, bool expectedValue)
		{
			var behavior = new NumericValidationBehavior
			{
				MinimumValue = minValue,
				MaximumValue = maxValue,
				MinimumDecimalPlaces = minDecimalPlaces,
				MaximumDecimalPlaces = maxDecimalPlaces
			};
			new Entry
			{
				Text = value,
				Behaviors =
				{
					behavior
				}
			};
			behavior.ForceValidate();
			Assert.Equal(expectedValue, behavior.IsValid);
		}
	}
}