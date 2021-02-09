using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using System.Linq;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class SegmentedView_Tests
	{
		[Fact]
		public void Text_Segments_Show()
		{
			var sv = new SegmentedView
			{
				ItemsSource = new string[] { "A", "B", "C" }
			};

			Assert.Equal(3, sv.Items.Count());
			Assert.Equal(SegmentMode.Text, sv.DisplayMode);
		}

		[Fact]
		public void Image_Segments_Show()
		{
			var sv = new SegmentedView
			{
				DisplayMode = SegmentMode.Image,
				ItemsSource = new string[] { "img.png", "img.png", "img.png" }
			};

			Assert.Equal(3, sv.Items.Count());
			Assert.Equal(SegmentMode.Image, sv.DisplayMode);
		}

		[Fact]
		public void CornerRadius_Uniformed_Is_Set()
		{
			var r = 10;
			var expected = new CornerRadius(r);

			var sv = new SegmentedView
			{
				ItemsSource = new string[] { "img.png", "img.png", "img.png" },
				CornerRadius = new CornerRadius(r)
			};
			Assert.Equal(expected, sv.CornerRadius);
		}

		[Fact]
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
			Assert.Equal(expected, sv.CornerRadius);
		}

		[Fact]
		public void Collection_Changes_Are_Reflected()
		{
			var og = new string[] { "A", "B", "C" };
			var updated = new string[] { "A", "C" };

			var sv = new SegmentedView
			{
				ItemsSource = og
			};
			Assert.Equal(3, sv.Items.Count());

			sv.ItemsSource = updated;
			Assert.Equal(2, sv.Items.Count());
		}

		[Fact]
		public void Color_Changes_Are_Reflected()
		{
			var og = Color.Red;
			var updated = Color.Blue;

			var sv = new SegmentedView
			{
				Color = og
			};

			Assert.Equal(og, sv.Color);

			sv.Color = updated;
			Assert.Equal(updated, sv.Color);
		}
	}
}
