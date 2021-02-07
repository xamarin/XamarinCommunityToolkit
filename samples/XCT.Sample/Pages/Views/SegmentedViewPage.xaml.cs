using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using System.Collections;

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

		void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			if (!(sender is Picker picker))
				return;

			var color = Color.Default;

			switch (picker.SelectedIndex)
			{
				case 1:
					color = Color.Green;
					break;
				case 2:
					color = Color.Blue;
					break;
				default:
				case 0:
					color = Color.Red;
					break;
			}

			TextSegments.Color = color;
			//ImageSegments.Color = color;
		}

		void BG_Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			if (!(sender is Picker picker))
				return;

			var color = Color.Default;

			switch (picker.SelectedIndex)
			{
				case 1:
					color = Color.FromHex("#97DC91");
					break;
				case 2:
					color = Color.FromHex("#688ff4");
					break;
				default:
				case 0:
					color = Color.FromHex("#FFA7A3");
					break;
			}

			TextSegments.BackgroundColor = color;
			//ImageSegments.BackgroundColor = color;
		}

		void DisplayMode_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			if (!(sender is Picker picker))
				return;

			switch (picker.SelectedIndex)
			{
				case 1:
					TextSegments.DisplayMode = SegmentMode.Image;
					TextSegments.ItemsSource = (IList)(BindingContext as ViewModels.Views.SegmentedViewModel).IconOptions;
					break;
				case 0:
				default:
					TextSegments.DisplayMode = SegmentMode.Text;
					TextSegments.ItemsSource = (IList)(BindingContext as ViewModels.Views.SegmentedViewModel).Options;
					break;
			}
		}

		void Text_Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			if (!(sender is Picker picker))
				return;

			switch (picker.SelectedIndex)
			{
				case 1:
					TextSegments.NormalTextColor = Color.FromHex("#97DC91");
					break;
				case 2:
					TextSegments.NormalTextColor = Color.FromHex("#688ff4");
					break;
				default:
				case 0:
					TextSegments.NormalTextColor = Color.FromHex("#FFA7A3");
					break;
			}
		}

		void Selected_Text_Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			if (!(sender is Picker picker))
				return;

			switch (picker.SelectedIndex)
			{
				case 1:
					TextSegments.SelectedTextColor = Color.FromHex("#97DC91");
					break;
				case 2:
					TextSegments.SelectedTextColor = Color.FromHex("#688ff4");
					break;
				default:
				case 0:
					TextSegments.SelectedTextColor = Color.FromHex("#FFA7A3");
					break;
			}
		}
	}
}