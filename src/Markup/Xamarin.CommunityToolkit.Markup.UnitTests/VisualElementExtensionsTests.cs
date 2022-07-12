﻿using System;
using NUnit.Framework;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class VisualElementExtensionsTests : MarkupBaseTestFixture<BoxView>
	{
		BoxView BoxView => Bindable ?? throw new NullReferenceException();

		[Test]
		public void Height()
		{
			BoxView.HeightRequest = 1;
			BoxView.Height(2);
			Assert.That(BoxView.HeightRequest, Is.EqualTo(2));
		}

		[Test]
		public void Width()
		{
			BoxView.WidthRequest = 1;
			BoxView.Width(2);
			Assert.That(BoxView.WidthRequest, Is.EqualTo(2));
		}

		[Test]
		public void MinHeight()
		{
			BoxView.MinimumHeightRequest = 1;
			BoxView.MinHeight(2);
			Assert.That(BoxView.MinimumHeightRequest, Is.EqualTo(2));
		}

		[Test]
		public void MinWidth()
		{
			BoxView.MinimumWidthRequest = 1;
			BoxView.MinWidth(2);
			Assert.That(BoxView.MinimumWidthRequest, Is.EqualTo(2));
		}

		[Test]
		public void SizeNotUniform()
		{
			BoxView.WidthRequest = BoxView.HeightRequest = 1;
			BoxView.Size(2, 3);
			Assert.That(BoxView.WidthRequest, Is.EqualTo(2));
			Assert.That(BoxView.HeightRequest, Is.EqualTo(3));
		}

		[Test]
		public void SizeUniform()
		{
			BoxView.WidthRequest = BoxView.HeightRequest = 1;
			BoxView.Size(2);
			Assert.That(BoxView.WidthRequest, Is.EqualTo(2));
			Assert.That(BoxView.HeightRequest, Is.EqualTo(2));
		}

		[Test]
		public void MinSizeNotUniform()
		{
			BoxView.MinimumWidthRequest = BoxView.MinimumHeightRequest = 1;
			BoxView.MinSize(2, 3);
			Assert.That(BoxView.MinimumWidthRequest, Is.EqualTo(2));
			Assert.That(BoxView.MinimumHeightRequest, Is.EqualTo(3));
		}

		[Test]
		public void MinSizeUniform()
		{
			BoxView.MinimumWidthRequest = BoxView.MinimumHeightRequest = 1;
			BoxView.MinSize(2);
			Assert.That(BoxView.MinimumWidthRequest, Is.EqualTo(2));
			Assert.That(BoxView.MinimumHeightRequest, Is.EqualTo(2));
		}

		[Test]
		public void Style()
		{
			var style = new Style<BoxView>();
			BoxView.Style = null;
			BoxView.Style(style);
			Assert.That(BoxView.Style, Is.EqualTo(style.FormsStyle));
		}

		[Test]
		public void SupportDerivedFromBoxView()
		{
			Assert.IsInstanceOf<DerivedFromBoxView>(
				new DerivedFromBoxView()
				.Height(2)
				.Width(2)
				.MinHeight(2)
				.MinWidth(2)
				.Size(2, 3)
				.Size(2)
				.MinSize(2, 3)
				.MinSize(2)
				.Style(new Style<DerivedFromBoxView>()));
		}

		class DerivedFromBoxView : BoxView { }
	}
}