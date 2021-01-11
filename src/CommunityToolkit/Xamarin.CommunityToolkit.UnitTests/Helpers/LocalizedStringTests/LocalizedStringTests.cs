using System.Globalization;
using System.Resources;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.LocalizedStringTests
{
	public class LocalizedStringTests
	{
		public LocalizedStringTests()
		{
			resourceManager = new MockResourceManager();
			localizationManager = new LocalizationResourceManager();
			localizationManager.Init(resourceManager);
			localizationManager.SetCulture(initialCulture);
		}

		readonly LocalizationResourceManager localizationManager;
		readonly ResourceManager resourceManager;
		readonly CultureInfo initialCulture = CultureInfo.InvariantCulture;

		[Fact]
		public void LocalizedStringTests_Localized_ValidImplementation()
		{
			// Arrange
			var testString = "test";
			var culture2 = new CultureInfo("en");
			var localizedString = new LocalizedString(() => localizationManager[testString]);

			string responceOnCultureChanged = null;
			localizedString.PropertyChanged += (_, _) => responceOnCultureChanged = localizedString.Localized;

			// Act
			var responceCulture1 = localizedString.Localized;
			var responceResourceManagerCulture1 = resourceManager.GetString(testString, culture2);
			localizationManager.SetCulture(culture2);
			var responceCulture2 = localizedString.Localized;
			var responceResourceManagerCulture2 = resourceManager.GetString(testString, culture2);

			// Assert
			Assert.Equal(responceResourceManagerCulture1, responceCulture1);
			Assert.Equal(responceResourceManagerCulture2, responceOnCultureChanged);
			Assert.Equal(responceResourceManagerCulture2, responceResourceManagerCulture2);
		}
	}
}
