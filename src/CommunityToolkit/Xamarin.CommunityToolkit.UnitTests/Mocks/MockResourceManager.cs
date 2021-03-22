using System.Globalization;
using System.Resources;

namespace Xamarin.CommunityToolkit.UnitTests.Mocks
{
	class MockResourceManager : ResourceManager
	{
		public override string GetString(string name, CultureInfo culture) => culture.EnglishName;
	}
}