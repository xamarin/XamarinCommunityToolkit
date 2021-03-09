using System.Globalization;
using System.Resources;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.LocalizationResourceManagerTests
{
	[Collection(nameof(LocalizationResourceManager))]
	public class LocalizationResourceManagerTests
	{
		public LocalizationResourceManagerTests()
		{
			resourceManager = new MockResourceManager();
			localizationManager.Init(resourceManager, initialCulture);
		}

		readonly LocalizationResourceManager localizationManager = LocalizationResourceManager.Current;
		readonly CultureInfo initialCulture = CultureInfo.InvariantCulture;
		readonly ResourceManager resourceManager;

		[Fact]
		public void LocalizationResourceManager_PropertyChanged_Triggered()
		{
			// Arrange
			var culture2 = new CultureInfo("en");
			localizationManager.CurrentCulture = culture2;
			CultureInfo? changedCulture = null;
			localizationManager.PropertyChanged += (s, e) => changedCulture = localizationManager.CurrentCulture;

			// Act, Assert
			localizationManager.Init(resourceManager, initialCulture);
			Assert.Equal(initialCulture, changedCulture);

			localizationManager.CurrentCulture = culture2;
			Assert.Equal(culture2, changedCulture);
		}
	}
}