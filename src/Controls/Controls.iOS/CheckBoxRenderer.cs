using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinCommunityToolkit.Controls;
using XamarinCommunityToolkit.Controls.iOS;
using XamarinCommunityToolkit.Controls.iOS.Views;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace XamarinCommunityToolkit.Controls.iOS
{
    public class CheckBoxRenderer : ViewRenderer<CheckBox, CheckBoxView>
    {
        protected const double NativeCheckBoxWidth = 20d;

        private bool _disposed;

        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                // Instantiate the native control and assign it to the Control property
                var width = Element.WidthRequest > 0
                    ? Element.WidthRequest
                    : NativeCheckBoxWidth;

                // Use default tint color
                var themeColor = new UIView().TintColor;

                var checkBox = new CheckBoxView(new CGRect(0, 0, width, width))
                {
                    CheckColor = themeColor,
                    CheckboxBackgroundColor = themeColor
                };

                checkBox.ValueChanged += CheckBox_ValueChanged;

                SetNativeControl(checkBox);
            }

            UpdateChecked();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CheckBox.IsCheckedProperty.PropertyName)
                UpdateChecked();

            base.OnElementPropertyChanged(sender, e);
        }

        private void UpdateChecked()
        {
            Control.Checked = Element.IsChecked;
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
                    Control.ValueChanged -= CheckBox_ValueChanged;
                }
            }

            base.Dispose(disposing);
        }

        private void CheckBox_ValueChanged(object sender, EventArgs e)
        {
            Element.IsChecked = Control.Checked;
        }
    }
}
