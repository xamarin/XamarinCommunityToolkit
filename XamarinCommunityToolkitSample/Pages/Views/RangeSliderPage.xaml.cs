using XamarinCommunityToolkit.Views;
using Xamarin.Forms;

namespace XamarinCommunityToolkitSample.Pages.Views
{
    public partial class RangeSliderPage
    {
        public RangeSliderPage()
            => InitializeComponent();

        void OnThumbSizeSwitchToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                RangeSlider.SetBinding(RangeSlider.LowerThumbSizeProperty, GetSliderValueBinding(LowerThumbSizeSlider));
                RangeSlider.SetBinding(RangeSlider.UpperThumbSizeProperty, GetSliderValueBinding(UpperThumbSizeSlider));
                return;
            }
            RangeSlider.LowerThumbSize = (double)RangeSlider.LowerThumbSizeProperty.DefaultValue;
            RangeSlider.UpperThumbSize = (double)RangeSlider.UpperThumbSizeProperty.DefaultValue;
        }

        Binding GetSliderValueBinding(object source)
            => new Binding
            {
                Path = Slider.ValueProperty.PropertyName,
                Source = source
            };

        void OnShapeCircleSwitchToggled(object sender, ToggledEventArgs e)
        {
            var radius = e.Value
                ? -1
                : 0;

            if (sender == ThumbShapeCircleSwitch)
            {
                RangeSlider.ThumbRadius = radius;
                return;
            }

            if (sender == LowerThumbShapeCircleSwitch)
            {
                RangeSlider.LowerThumbRadius = radius;
                return;
            }

            if (sender == UpperThumbShapeCircleSwitch)
            {
                RangeSlider.UpperThumbRadius = radius;
                return;
            }

            if (sender == TrackShapeRoundedSwitch)
            {
                RangeSlider.TrackRadius = radius;
                return;
            }
        }
    }
}
