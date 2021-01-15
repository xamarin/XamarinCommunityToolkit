using System;
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
		LocalizedString localizedString;

		[Fact]
		public void LocalizedStringTests_Localized_ValidImplementation()
		{
			// Arrange
			var testString = "test";
			var culture2 = new CultureInfo("en");
			localizedString = new LocalizedString(localizationManager, () => localizationManager[testString]);

			string responceOnCultureChanged = null;
			localizedString.PropertyChanged += (sender, args) => responceOnCultureChanged = localizedString.Localized;

			// Act
			var responceCulture1 = localizedString.Localized;
			var responceResourceManagerCulture1 = resourceManager.GetString(testString, initialCulture);
			localizationManager.SetCulture(culture2);
			var responceCulture2 = localizedString.Localized;
			var responceResourceManagerCulture2 = resourceManager.GetString(testString, culture2);

			// Assert
			Assert.Equal(responceResourceManagerCulture1, responceCulture1);
			Assert.Equal(responceResourceManagerCulture2, responceOnCultureChanged);
			Assert.Equal(responceResourceManagerCulture2, responceResourceManagerCulture2);
		}

		[Fact]
		public void LocalizedStringTests_Disposed_IfNoReferences()
		{
			// Arrange
			var testString = "test";
			SetLocalizedString();
			var weaklocalizedString = new WeakReference(localizedString);
			localizedString = null;

			// Act
			GC.Collect();

			// Assert
			Assert.False(weaklocalizedString.IsAlive);

			void SetLocalizedString()
			{
				localizedString = new LocalizedString(localizationManager, () => localizationManager[testString]);
			}
		}
	}
}
