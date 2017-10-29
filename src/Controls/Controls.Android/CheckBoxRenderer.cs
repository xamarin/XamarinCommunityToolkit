using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Controls;
using FormsCommunityToolkit.Controls.Droid;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace FormsCommunityToolkit.Controls.Droid
{
	using NativeCheckBox = Android.Widget.CheckBox;

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
				var checkBox = new NativeCheckBox(Context);
				checkBox.CheckedChange += CheckBox_CheckedChange;

				SetNativeControl(checkBox);
			}

			Control.Checked = e.NewElement.IsChecked;

			Element.CheckedChanged += CheckedChanged;
			Element.PropertyChanged += ElementOnPropertyChanged;
		}

		private void CheckBox_CheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
		{
			Element.IsChecked = e.IsChecked;
		}

		private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "IsChecked":
					Control.Checked = Element.IsChecked;
					break;
				default:
					System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
					break;
			}
		}

		private void CheckedChanged(object sender, bool e)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{
				Control.Checked = e;
			});
		}
	}
}