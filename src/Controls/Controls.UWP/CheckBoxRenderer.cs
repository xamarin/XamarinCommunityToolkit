using System.ComponentModel;
using Windows.UI.Xaml;
using XamarinCommunityToolkit.Controls;
using XamarinCommunityToolkit.Controls.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace XamarinCommunityToolkit.Controls.UWP
{
    public class CheckBoxRenderer : ViewRenderer<CheckBox, Windows.UI.Xaml.Controls.CheckBox>
    {
        private const double NativeCheckBoxMinWidth = 28;

        private bool _disposed;

        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (_disposed) return;

            if (Control == null)
            {
                var checkBox = new Windows.UI.Xaml.Controls.CheckBox();
                checkBox.MinWidth = NativeCheckBoxMinWidth;
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Unchecked;

                SetNativeControl(checkBox);
            }

            UpdateChecked();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Element.IsChecked = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Element.IsChecked = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CheckBox.IsCheckedProperty.PropertyName)
                UpdateChecked();

            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdateChecked()
        {
            if (Element != null && Control != null)
            {
                Control.IsChecked = Element.IsChecked;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                if (Control != null)
                {
                    Control.Checked -= CheckBox_Checked;
                    Control.Unchecked -= CheckBox_Unchecked;
                }
            }

            base.Dispose(disposing);
        }
    }
}