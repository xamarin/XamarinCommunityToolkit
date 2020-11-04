using System.Globalization;
using NUnit.Framework;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	public class BaseTestFixture
	{
		CultureInfo defaultCulture;
		CultureInfo defaultUICulture;

		[SetUp]
		public virtual void Setup()
		{
			defaultCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			defaultUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
			Device.PlatformServices = new MockPlatformServices();
		}

		[TearDown]
		public virtual void TearDown()
		{
			Device.PlatformServices = null;
			System.Threading.Thread.CurrentThread.CurrentCulture = defaultCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = defaultUICulture;
		}
	}
}