using System;
using System.Globalization;
using System.Resources;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UnitTests.Mocks;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.LocalizedStringTests
{
	[NonParallelizable]
	public class LocalizedStringTests
	{
		readonly ResourceManager resourceManager = new MockResourceManager();
		readonly CultureInfo initialCulture = CultureInfo.InvariantCulture;
		readonly LocalizationResourceManager localizationManager = LocalizationResourceManager.Current;

		LocalizedString? localizedString;

		[SetUp]
		public void Setup()
		{
			localizationManager.Init(resourceManager, initialCulture);
		}

		[Test]
		public void LocalizedStringTests_Localized_ValidImplementation()
		{
			// Arrange
			var testString = "test";
			var culture2 = new CultureInfo("en");
			localizedString = new LocalizedString(() => resourceManager.GetString(testString, localizationManager.CurrentCulture));

			string? responceOnCultureChanged = null;
			localizedString.PropertyChanged += (sender, args) => responceOnCultureChanged = localizedString.Localized;

			// Act
			var responceCulture1 = localizedString.Localized;
			var responceResourceManagerCulture1 = resourceManager.GetString(testString, initialCulture);
			localizationManager.CurrentCulture = culture2;
			var responceCulture2 = localizedString.Localized;
			var responceResourceManagerCulture2 = resourceManager.GetString(testString, culture2);

			// Assert
			Assert.AreEqual(responceResourceManagerCulture1, responceCulture1);
			Assert.AreEqual(responceResourceManagerCulture2, responceOnCultureChanged);
			Assert.AreEqual(responceResourceManagerCulture2, responceResourceManagerCulture2);
		}

		[Test]
		public void LocalizedStringTests_ImplicitConversion_ValidImplementation()
		{
			// Arrange
			var testString = "test";
			Func<string> generator = () => testString;

			// Act
			localizedString = generator;

			// Assert
			Assert.IsNotNull(localizedString);
		}

		[Test]
		public void LocalizedStringTests_WeekSubscribe_ValidImplementation()
		{
			// Arrange
			var isTrigered = false;
			var culture2 = new CultureInfo("en");
			localizedString = new LocalizedString(() => string.Empty);
			localizedString.PropertyChanged += (_, __) => isTrigered = true;

			// Act
			GC.Collect();
			localizationManager.CurrentCulture = culture2;

			// Assert
			Assert.IsTrue(isTrigered);
		}

#if NET461
#warning Test fails on mono x64 Running on macOS
#else
		[Test]
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
				localizedString = new LocalizedString(() => resourceManager.GetString(testString, localizationManager.CurrentCulture));
			}
		}
#endif
	}
}