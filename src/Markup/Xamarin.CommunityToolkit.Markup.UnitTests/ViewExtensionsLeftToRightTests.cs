﻿using NUnit.Framework;
using Xamarin.CommunityToolkit.Markup.LeftToRight;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class ViewExtensionsLeftToRightTests : MarkupBaseTestFixture<BoxView>
	{
		[Test]
		public void Left()
			=> TestPropertiesSet(v => v?.Left(), (View.HorizontalOptionsProperty, LayoutOptions.End, LayoutOptions.Start));

		[Test]
		public void Right()
			=> TestPropertiesSet(v => v?.Right(), (View.HorizontalOptionsProperty, LayoutOptions.Start, LayoutOptions.End));

		[Test]
		public void LeftExpand()
			=> TestPropertiesSet(v => v?.LeftExpand(), (View.HorizontalOptionsProperty, LayoutOptions.End, LayoutOptions.StartAndExpand));

		[Test]
		public void RightExpand()
			=> TestPropertiesSet(v => v?.RightExpand(), (View.HorizontalOptionsProperty, LayoutOptions.End, LayoutOptions.EndAndExpand));

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