using System.ComponentModel;
using Android.Content;
using Android.Widget;
using XamarinCommunityToolkit.Controls.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CheckBox = XamarinCommunityToolkit.Controls.CheckBox;
using NativeCheckBox = Android.Widget.CheckBox;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace XamarinCommunityToolkit.Controls.Droid
{
    class CheckBoxRenderer : ViewRenderer<CheckBox, NativeCheckBox>
    {
        private bool _disposed;

        public CheckBoxRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var checkBox = CreateNativeControl();
                checkBox.CheckedChange += CheckBox_CheckedChange;

                SetNativeControl(checkBox);
            }

            UpdateChecked();
        }

        protected override NativeCheckBox CreateNativeControl()
        {
            return new NativeCheckBox(Context);
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
                    Control.CheckedChange -= CheckBox_CheckedChange;
                }
            }

            base.Dispose(disposing);
        }

        private void CheckBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Element.IsChecked = e.IsChecked;
        }
    }
}