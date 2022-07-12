﻿using NUnit.Framework;
using Xamarin.CommunityToolkit.Markup.RightToLeft;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class ViewExtensionsRightToLeftTests : MarkupBaseTestFixture<BoxView>
	{
		[Test]
		public void Left()
			=> TestPropertiesSet(v => v?.Left(), (View.HorizontalOptionsProperty, LayoutOptions.Start, LayoutOptions.End));

		[Test]
		public void Right()
			=> TestPropertiesSet(v => v?.Right(), (View.HorizontalOptionsProperty, LayoutOptions.End, LayoutOptions.Start));

		[Test]
		public void LeftExpand()
			=> TestPropertiesSet(v => v?.LeftExpand(), (View.HorizontalOptionsProperty, LayoutOptions.Start, LayoutOptions.EndAndExpand));

		[Test]
		public void RightExpand()
			=> TestPropertiesSet(v => v?.RightExpand(), (View.HorizontalOptionsProperty, LayoutOptions.End, LayoutOptions.StartAndExpand));

		[Test]
		public void SupportDerivedFromView()
		{
			Assert.IsInstanceOf<DerivedFromView>(
				new DerivedFromView()
				.Left()
				.Right()
				.LeftExpand()
				.RightExpand());
		}

		class DerivedFromView : BoxView { }
	}
}