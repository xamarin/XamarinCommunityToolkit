using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class SegmentedViewPage
	{
		double all = 0.0;
		double tl = 0.0;
		double tr = 0.0;
		double bl = 0.0;
		double br = 0.0;

		public SegmentedViewPage()
		{
			InitializeComponent();
		}

		void Slider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			all = r;
			TextSegments.CornerRadius = new CornerRadius(all);
		}

		void TopLeftSlider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			tl = r;
			UpdateCorners();
		}

		void TopRightSlider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			tr = r;
			UpdateCorners();
		}

		void BottomLeftSlider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			bl = r;
			UpdateCorners();
		}

		void BottomRightSlider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			br = r;
			UpdateCorners();
		}

		void UpdateCorners()
		{
			TextSegments.CornerRadius = new CornerRadius(tl, tr, bl, br);
		}
	}
}
