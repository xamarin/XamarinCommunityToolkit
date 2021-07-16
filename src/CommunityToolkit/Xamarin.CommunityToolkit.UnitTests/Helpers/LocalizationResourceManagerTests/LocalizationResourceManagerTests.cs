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
		readonly ResourceManager resourceManager = new MockResourceManager();
		readonly CultureInfo initialCulture = CultureInfo.InvariantCulture;
		readonly LocalizationResourceManager localizationManager = LocalizationResourceManager.Current;

		[SetUp]
		public void Setup()
		{
			localizationManager.Init(resourceManager, initialCulture);
		}

		[Test]
		public void LocalizationResourceManager_PropertyChanged_Triggered()
		{
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