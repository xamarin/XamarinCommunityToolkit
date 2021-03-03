﻿using System.Globalization;
using System.Resources;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.LocalizationResourceManagerTests
{
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
		public void LocalizationResourceManager_GetCulture_Equal_Indexer()
		{
			// Arrange
			var testString = "test";
			var culture2 = new CultureInfo("en");

			// Act
			var responceIndexerCulture1 = localizationManager[testString];
			var responceGetValueCulture1 = localizationManager.GetValue(testString);
			var responceResourceManagerCulture1 = resourceManager.GetString(testString, initialCulture);
			localizationManager.CurrentCulture = culture2;
			var responceIndexerCulture2 = localizationManager[testString];
			var responceGetValueCulture2 = localizationManager.GetValue(testString);
			var responceResourceManagerCulture2 = resourceManager.GetString(testString, culture2);

			// Assert
			Assert.Equal(responceResourceManagerCulture1, responceIndexerCulture1);
			Assert.Equal(responceResourceManagerCulture1, responceGetValueCulture1);
			Assert.Equal(responceResourceManagerCulture2, responceIndexerCulture2);
			Assert.Equal(responceResourceManagerCulture2, responceGetValueCulture2);
		}

		[Fact]
		public void LocalizationResourceManager_PropertyChanged_Triggered()
		{
			// Arrange
			var culture2 = new CultureInfo("en");
			localizationManager.CurrentCulture = culture2;
			CultureInfo changedCulture = null;
			localizationManager.PropertyChanged += (s, e) => changedCulture = localizationManager.CurrentCulture;

			// Act, Assert
			localizationManager.Init(resourceManager, initialCulture);
			Assert.Equal(initialCulture, changedCulture);

			localizationManager.CurrentCulture = culture2;
			Assert.Equal(culture2, changedCulture);
		}
	}
}