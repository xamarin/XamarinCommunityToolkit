using System;
using System.Globalization;
using System.Resources;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UnitTests.Mocks;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.LocalizationResourceManagerTests
{
	[NonParallelizable]
	public class LocalizationResourceManagerTests
	{
		ResourceManager? resourceManager;
		CultureInfo? initialCulture;
		LocalizationResourceManager? localizationManager;

		[SetUp]
		public void Setup()
		{
			resourceManager = new MockResourceManager();
			initialCulture = CultureInfo.InvariantCulture;
			localizationManager = LocalizationResourceManager.Current;

			localizationManager.Init(resourceManager, initialCulture);
		}

		[Test]
		public void LocalizationResourceManager_GetCulture_Equal_Indexer()
		{
			_ = localizationManager ?? throw new NullReferenceException();
			_ = resourceManager ?? throw new NullReferenceException();

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
			Assert.AreEqual(responceResourceManagerCulture1, responceIndexerCulture1);
			Assert.AreEqual(responceResourceManagerCulture1, responceGetValueCulture1);
			Assert.AreEqual(responceResourceManagerCulture2, responceIndexerCulture2);
			Assert.AreEqual(responceResourceManagerCulture2, responceGetValueCulture2);
		}

		[Test]
		public void LocalizationResourceManager_PropertyChanged_Triggered()
		{
			_ = initialCulture ?? throw new NullReferenceException();
			_ = resourceManager ?? throw new NullReferenceException();
			_ = localizationManager ?? throw new NullReferenceException();

			// Arrange
			var culture2 = new CultureInfo("en");
			CultureInfo? changedCulture = null;
			localizationManager.CurrentCulture = culture2;
			localizationManager.PropertyChanged += (s, e) => changedCulture = localizationManager.CurrentCulture;

			// Act, Assert
			localizationManager.Init(resourceManager, initialCulture);
			Assert.AreEqual(initialCulture, changedCulture);

			localizationManager.CurrentCulture = culture2;
			Assert.AreEqual(culture2, changedCulture);
		}
	}
}