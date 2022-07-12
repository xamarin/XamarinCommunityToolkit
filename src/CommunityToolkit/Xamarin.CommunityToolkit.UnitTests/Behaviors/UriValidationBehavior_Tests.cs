using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class UriValidationBehavior_Tests
	{
		[SetUp]
		public void Setup() => Device.PlatformServices = new MockPlatformServices();

		[TestCase(@"http://microsoft.com", UriKind.Absolute, true)]
		[TestCase(@"microsoft/xamarin/news", UriKind.Relative, true)]
		[TestCase(@"http://microsoft.com", UriKind.RelativeOrAbsolute, true)]
		[TestCase(@"microsoftcom", UriKind.Absolute, false)]
		[TestCase(@"microsoft\\\\\xamarin/news", UriKind.Relative, false)]
		[TestCase(@"ht\\\.com", UriKind.RelativeOrAbsolute, false)]
		public async Task IsValid(string value, UriKind uriKind, bool expectedValue)
		{
			// Arrange
			var behavior = new UriValidationBehavior
			{
				UriKind = uriKind,
			};

			var entry = new Entry
			{
				Text = value,
				Behaviors =
				{
					behavior
				}
			};
			entry.Behaviors.Add(behavior);

			// Act
			await behavior.ForceValidate();

			// Assert
			Assert.AreEqual(expectedValue, behavior.IsValid);
		}
	}
}