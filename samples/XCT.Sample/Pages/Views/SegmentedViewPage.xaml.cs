using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Sample.ViewModels.Views;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class SegmentedViewPage
	{
		double all = 0.0;
		double topLeft = 0.0;
		double topRight = 0.0;
		double bottomLeft = 0.0;
		double bottomRight = 0.0;

		readonly Color myRed = Color.FromHex("#97DC91");
		readonly Color myGreen = Color.FromHex("#688ff4");
		readonly Color myBlue = Color.FromHex("#FFA7A3");

		public SegmentedViewPage()
		{
			InitializeComponent();
		}

		void Slider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			all = r;
			TextSegments.CornerRadius = new CornerRadius(all);
		}

		void TopLeftSlider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			topLeft = r;
			UpdateCorners();
		}

		void TopRightSlider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			topRight = r;
			UpdateCorners();
		}

		void BottomLeftSlider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			bottomLeft = r;
			UpdateCorners();
		}

		void BottomRightSlider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			if (!double.TryParse(e.NewValue.ToString(), out var r))
				return;
			bottomRight = r;
			UpdateCorners();
		}

		void UpdateCorners()
		{
			TextSegments.CornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
		}

		void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (sender is not Picker picker)
				return;

			var color = picker.SelectedIndex switch
			{
				1 => Color.Green,
				2 => Color.Blue,
				_ => Color.Red,
			};
			TextSegments.Color = color;
		}

		void BG_Picker_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (sender is not Picker picker)
				return;

			var color = picker.SelectedIndex switch
			{
				1 => myRed,
				2 => myGreen,
				_ => myBlue,
			};
			TextSegments.BackgroundColor = color;
		}

		void DisplayMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (sender is not Picker picker)
				return;

			var segmentedViewModel = (SegmentedViewModel)BindingContext;

			switch (picker.SelectedIndex)
			{
				case 1:
					TextSegments.DisplayMode = SegmentMode.Image;
					TextSegments.ItemsSource = segmentedViewModel.IconOptions;
					segmentedViewModel.SegmentMode = SegmentMode.Image;
					break;
				case 0:
				default:
					TextSegments.DisplayMode = SegmentMode.Text;
					TextSegments.ItemsSource = segmentedViewModel.Options;
					segmentedViewModel.SegmentMode = SegmentMode.Text;
					break;
			}
		}

		void Text_Picker_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (sender is not Picker picker)
				return;

			TextSegments.NormalTextColor = picker.SelectedIndex switch
			{
				1 => myRed,
				2 => myGreen,
				_ => myBlue,
			};
		}

		void Selected_Text_Picker_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (sender is not Picker picker)
				return;

			TextSegments.SelectedTextColor = picker.SelectedIndex switch
			{
				1 => myRed,
				2 => myGreen,
				_ => myBlue,
			};
		}

		void Add_Clicked(object sender, System.EventArgs e)
		{
			var button = (Button)sender;
			var segmentedViewModel = (SegmentedViewModel)BindingContext;

			segmentedViewModel.AddCommand.Execute(int.Parse(button.CommandParameter.ToString()));
		}

		void Delete_Clicked(object sender, System.EventArgs e)
		{
			var button = (Button)sender;
			var segmentedViewModel = (SegmentedViewModel)BindingContext;

			segmentedViewModel.RemoveCommand.Execute(int.Parse(button.CommandParameter.ToString()));
		}
	}
}