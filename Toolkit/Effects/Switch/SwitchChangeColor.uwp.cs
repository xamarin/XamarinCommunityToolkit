using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.SwitchChangeColor), nameof(RoutingEffects.SwitchChangeColorEffect))]
namespace Xamarin.Toolkit.Effects.UWP
{
    [Preserve]
    public class SwitchChangeColor : PlatformEffect
    {
        Windows.UI.Color trueColor;
        Windows.UI.Color falseColor;

        protected override void OnAttached()
        {
            var color = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.TrueColorProperty);
            trueColor = ConvertColor(color);

            // currently not supported
            color = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.FalseColorProperty);
            falseColor = ConvertColor(color);

            var toggleSwitch = Control as ToggleSwitch;
            if (toggleSwitch == null)
                return;

            toggleSwitch.Loaded -= OnSwitchLoaded;
            toggleSwitch.Loaded += OnSwitchLoaded;
        }

        protected override void OnDetached()
        {
            var toggleSwitch = Control as ToggleSwitch;
            if (toggleSwitch == null)
                return;

            toggleSwitch.Loaded -= OnSwitchLoaded;
        }

        void OnSwitchLoaded(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = Control as ToggleSwitch;
            var grid = toggleSwitch.GetChildOfType<Windows.UI.Xaml.Controls.Grid>();
            var groups = Windows.UI.Xaml.VisualStateManager.GetVisualStateGroups(grid);
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

            if (toggleSwitch.GetChildByName("SwitchKnobBounds") is Windows.UI.Xaml.Shapes.Rectangle rect)
                rect.Fill = new SolidColorBrush(trueColor);

            toggleSwitch.Loaded -= OnSwitchLoaded;
        }

        Windows.UI.Color ConvertColor(Color color) =>
            Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
    }
}
