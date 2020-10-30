using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture(typeof(Button))]
	[TestFixture(typeof(Frame))]
	[TestFixture(typeof(ImageButton))]
	[TestFixture(typeof(Label))]
	[TestFixture(typeof(Page))]
	public class PaddingElementExtensionsTests<TPaddingElement> : MarkupBaseTestFixture<TPaddingElement> where TPaddingElement : Element, IPaddingElement, new()
	{
		[Test]
		public void PaddingThickness()
			=> TestPropertiesSet(l => l.Padding(new Thickness(1)), (PaddingElement.PaddingProperty, new Thickness(0), new Thickness(1)));

		[Test]
		public void PaddingUniform()
			=> TestPropertiesSet(l => l.Padding(1), (PaddingElement.PaddingProperty, new Thickness(0), new Thickness(1)));

		[Test]
		public void PaddingHorizontalVertical()
			=> TestPropertiesSet(l => l.Padding(1, 2), (PaddingElement.PaddingProperty, new Thickness(0), new Thickness(1, 2)));

		[Test]
		public void Paddings()
			=> TestPropertiesSet(l => l.Paddings(left: 1, top: 2, right: 3, bottom: 4), (PaddingElement.PaddingProperty, new Thickness(0), new Thickness(1, 2, 3, 4)));

		[Test]
		public void SupportDerivedFrom() => AssertExperimental(() =>
		{
			DerivedFrom _ =
				new DerivedFrom()
				.Padding(1)
				.Padding(1, 2)
				.Paddings(left: 1, top: 2, right: 3, bottom: 4);
		});

		class DerivedFrom : ContentView { }
	}
}