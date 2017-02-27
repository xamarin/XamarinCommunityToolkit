using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Linq;
using Windows.UI.Xaml.Media.Animation;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.UWP;

[assembly: ExportEffect(typeof(PlatformEffects.SwitchChangeColor), nameof(RoutingEffects.SwitchChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.UWP
{
    [Preserve]
    public class SwitchChangeColor : PlatformEffect
    {
        private Windows.UI.Color _trueColor;
        private Windows.UI.Color _falseColor;

        protected override void OnAttached()
        {
            var color = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.TrueColorProperty);
            _trueColor = ConvertColor(color);

            // currently not supported
            color = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.FalseColorProperty);
            _falseColor = ConvertColor(color);

            var toggleSwitch = Control as ToggleSwitch;
            if (toggleSwitch == null)
                return;
            else
            {
                toggleSwitch.Loaded -= OnSwitchLoaded;
                toggleSwitch.Loaded += OnSwitchLoaded;
            }
        }

        protected override void OnDetached()
        {
            var toggleSwitch = Control as ToggleSwitch;
            if (toggleSwitch == null)
                return;
            else
                toggleSwitch.Loaded -= OnSwitchLoaded;
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
                            frame.Value = new SolidColorBrush(_trueColor) { Opacity = .7 };
                            break;
                        }
                    }
                }
            }

            var rect = toggleSwitch.GetChildByName("SwitchKnobBounds") as Windows.UI.Xaml.Shapes.Rectangle;
            if (rect != null)
                rect.Fill = new SolidColorBrush(_trueColor);

            toggleSwitch.Loaded -= OnSwitchLoaded;
        }

        private Windows.UI.Color ConvertColor(Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
        }
    }
}
