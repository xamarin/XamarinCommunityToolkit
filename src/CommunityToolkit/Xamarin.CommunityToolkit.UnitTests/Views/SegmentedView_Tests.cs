using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using System.Linq;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class SegmentedView_Tests
	{
		[Test]
		public void Text_Segments_Show()
		{
			var sv = new SegmentedView
			{
				ItemsSource = new string[] { "A", "B", "C" }
			};

			Assert.AreEqual(3, sv.Items.Count());
			Assert.AreEqual(SegmentMode.Text, sv.DisplayMode);
		}

		[Test]
		public void Image_Segments_Show()
		{
			var sv = new SegmentedView
			{
				DisplayMode = SegmentMode.Image,
				ItemsSource = new string[] { "img.png", "img.png", "img.png" }
			};

			Assert.AreEqual(3, sv.Items.Count());
			Assert.AreEqual(SegmentMode.Image, sv.DisplayMode);
		}

		[Test]
		public void CornerRadius_Uniformed_Is_Set()
		{
			var r = 10;
			var expected = new CornerRadius(r);

			var sv = new SegmentedView
			{
				ItemsSource = new string[] { "img.png", "img.png", "img.png" },
				CornerRadius = new CornerRadius(r)
			};
			Assert.AreEqual(expected, sv.CornerRadius);
		}

		[Test]
		public void CornerRadius_Individual_Is_Set()
		{
			var tl = 5;
			var tr = 10;
			var bl = 15;
			var br = 20;

			var expected = new CornerRadius(tl, tr, bl, br);

			var sv = new SegmentedView
			{
				ItemsSource = new string[] { "img.png", "img.png", "img.png" },
				CornerRadius = new CornerRadius(tl, tr, bl, br)
			};
			Assert.AreEqual(expected, sv.CornerRadius);
		}

		[Test]
		public void Collection_Changes_Are_Reflected()
		{
			var og = new string[] { "A", "B", "C" };
			var updated = new string[] { "A", "C" };

			var sv = new SegmentedView
			{
				ItemsSource = og
			};
			Assert.AreEqual(3, sv.Items.Count());

			sv.ItemsSource = updated;
			Assert.AreEqual(2, sv.Items.Count());
		}

		[Test]
		public void Color_Changes_Are_Reflected()
		{
			var og = Color.Red;
			var updated = Color.Blue;

			var sv = new SegmentedView
			{
				Color = og
			};

			Assert.AreEqual(og, sv.Color);

			sv.Color = updated;
			Assert.AreEqual(updated, sv.Color);
		}
	}
}
