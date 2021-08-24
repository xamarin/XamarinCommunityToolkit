using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using CoreAnimation;
using Xamarin.CommunityToolkit.UI.Views;

[assembly: ExportRenderer(typeof(TextSwitcher), typeof(TextSwitcherRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
    public class TextSwitcherRenderer : LabelRenderer
    {
        TextSwitcher TextSwitcher => (TextSwitcher)Element;

        protected override void OnElementChanged(Microsoft.Maui.Controls.Platform.ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
                e.OldElement.PropertyChanging -= OnElementPropertyChanging;

            if (e.NewElement != null)
                e.NewElement.PropertyChanging += OnElementPropertyChanging;
        }

        void OnElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == Label.TextProperty.PropertyName ||
                e.PropertyName == Label.FormattedTextProperty.PropertyName)
            {
                var transition = new CATransition
                {
                    TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut),
                    Duration = ((double)TextSwitcher.TransitionDuration) / 1000
                };

                switch (TextSwitcher.TransitionType)
				{
                    case TransitionType.Fade:
                        transition.Type = CAAnimation.TransitionFade;
                        break;
                    case TransitionType.MoveInFromLeft:
                        transition.Type = CAAnimation.TransitionMoveIn;
                        transition.Subtype = CAAnimation.TransitionFromLeft;
                        break;
				}

                Layer.AddAnimation(transition, transition.Type);
            }
        }
    }
}