using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class VisualFeedbackEffect : RoutingEffect
	{
		public VisualFeedbackEffect()
			: base(EffectIds.VisualFeedback)
		{
		}

		public static readonly BindableProperty FeedbackColorProperty =
			BindableProperty.CreateAttached("FeedbackColor", typeof(Color), typeof(VisualFeedbackEffect), new Microsoft.Maui.Graphics.Color());

		public static Color GetFeedbackColor(BindableObject view) => (Color)view.GetValue(FeedbackColorProperty);

		public static void SetFeedbackColor(BindableObject view, Color value) => view.SetValue(FeedbackColorProperty, value);

		public static bool IsFeedbackColorSet(BindableObject element) => GetFeedbackColor(element) != new Microsoft.Maui.Graphics.Color();
	}
}