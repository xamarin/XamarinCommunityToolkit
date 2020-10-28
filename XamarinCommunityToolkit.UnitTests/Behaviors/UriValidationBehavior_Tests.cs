using System;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Behaviors
{
	public class UriValidationBehavior_Tests
	{
		public UriValidationBehavior_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Theory]
		[InlineData(@"http://microsoft.com", UriKind.Absolute, true)]
		[InlineData(@"microsoft/xamarin/news", UriKind.Relative, true)]
		[InlineData(@"http://microsoft.com", UriKind.RelativeOrAbsolute, true)]
		[InlineData(@"microsoftcom", UriKind.Absolute, false)]
		[InlineData(@"microsoft\\\\\xamarin/news", UriKind.Relative, false)]
		[InlineData(@"ht\\\.com", UriKind.RelativeOrAbsolute, false)]
		public void IsValid(string value, UriKind uriKind, bool expectedValue)
		{
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
			behavior.ForceValidate();
			Assert.Equal(expectedValue, behavior.IsValid);
		}
	}
}