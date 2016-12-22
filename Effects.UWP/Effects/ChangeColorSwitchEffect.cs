using Windows.UI.Xaml.Controls;
using FormsCommunityToolkit.Effects.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Media;
using System;
using Windows.UI.Xaml;
using System.Linq;
using Windows.UI.Xaml.Media.Animation;

[assembly: ExportEffect(typeof(ChangeColorSwitchEffect), nameof(ChangeColorSwitchEffect))]

namespace FormsCommunityToolkit.Effects.UWP
{
    [Preserve]
    public class ChangeColorSwitchEffect : PlatformEffect
    {
        Windows.UI.Color trueColor;
        Windows.UI.Color falseColor;

        protected override void OnAttached()
        {
            var color = (Color)Element.GetValue(ChangeColorEffect.TrueColorProperty);
            trueColor = ConvertColor(color);

            // currently not supported
            color = (Color)Element.GetValue(ChangeColorEffect.FalseColorProperty);
            falseColor = ConvertColor(color);

            var toggleSwitch = Control as ToggleSwitch;
            if (toggleSwitch != null)
            {
                toggleSwitch.Loaded -= OnSwitchLoaded;
                toggleSwitch.Loaded += OnSwitchLoaded;
            }
        }

        protected override void OnDetached()
        {
            var toggleSwitch = Control as ToggleSwitch;
            if (toggleSwitch != null)
            {
                toggleSwitch.Loaded -= OnSwitchLoaded;
            }
        }

        private void OnSwitchLoaded(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = Control as ToggleSwitch;
            var grid = toggleSwitch.GetChildOfType<Windows.UI.Xaml.Controls.Grid>();
            var groups = VisualStateManager.GetVisualStateGroups(grid);
            foreach (var group in groups)
            {
                if (group.Name != "CommonStates")
                    continue;

                foreach (var state in group.States)
                {
                    if (state.Name != "PointerOver")
                        continue;

                    foreach (var timeline in state.Storyboard.Children.OfType<ObjectAnimationUsingKeyFrames>())
                    {
                        var property = Storyboard.GetTargetProperty(timeline);
                        var target = Storyboard.GetTargetName(timeline);
                        if ((target == "SwitchKnobBounds") && (property == "Fill"))
                        {
                            var frame = timeline.KeyFrames.First();
                            frame.Value = new SolidColorBrush(trueColor) { Opacity = .7 };
                            break;
                        }
                    }
                }
            }

            var rect = toggleSwitch.GetChildByName("SwitchKnobBounds") as Windows.UI.Xaml.Shapes.Rectangle;
            if (rect != null)
            {
                rect.Fill = new SolidColorBrush(trueColor);
            }
            toggleSwitch.Loaded -= OnSwitchLoaded;
        }

        private Windows.UI.Color ConvertColor(Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
        }
    }
}
