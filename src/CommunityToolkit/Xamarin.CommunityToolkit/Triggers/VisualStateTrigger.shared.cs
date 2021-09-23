using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Triggers
{
    public class VisualStateTrigger : TriggerAction<VisualElement>
    {
        public string? State { get; set; }

        public VisualElement? Target { get; set; }

        protected override void Invoke(VisualElement sender)
        {
            VisualElement visualElement = Target ?? sender;
            if (!string.IsNullOrEmpty(State))
            {
                VisualStateManager.GoToState(visualElement, State);
            }
        }
    }
}
