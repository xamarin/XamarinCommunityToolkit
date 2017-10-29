using System.ComponentModel;
using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;
using FormsCommunityToolkit.Controls;
using FormsCommunityToolkit.Controls.UWP;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace FormsCommunityToolkit.Controls.UWP
{
	using NativeCheckBox = Windows.UI.Xaml.Controls.CheckBox;

	public class CheckBoxRenderer : ViewRenderer<CheckBox, NativeCheckBox>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= ElementOnPropertyChanged;
			}

			if (Control == null)
			{
				var checkBox = new NativeCheckBox();
			    checkBox.Content = string.Empty;
			    checkBox.MinWidth = 20;
				checkBox.Checked += checkBox_Checked;
				checkBox.Unchecked += checkBox_Unchecked;

				SetNativeControl(checkBox);
			}

			Control.IsChecked = e.NewElement.IsChecked;

			Element.CheckedChanged += CheckedChanged;
			Element.PropertyChanged += ElementOnPropertyChanged;
		}

		private void CheckedChanged(object sender, bool e)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{
				Control.IsChecked = e;
			});
		}

		private void checkBox_Checked(object sender, RoutedEventArgs e)
		{
			Element.IsChecked = true;
		}

		private void checkBox_Unchecked(object sender, RoutedEventArgs e)
		{
			Element.IsChecked = false;
		}

		private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "IsChecked":
					Control.IsChecked = Element.IsChecked;
					break;
				default:
					System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
					break;
			}
		}
	}
}
