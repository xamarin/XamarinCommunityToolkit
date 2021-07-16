using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Triggers
{
    public class AnimateCornerRadius : AnimationBaseTrigger<CornerRadius>
    {
        protected override async void Invoke(VisualElement sender)
        {
            if (TargetProperty == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            if (Delay > 0)
                await Task.Delay(Delay);

            SetDefaultFrom((CornerRadius)sender.GetValue(TargetProperty));

            sender.Animate($"AnimateCornerRadius{TargetProperty.PropertyName}", new Animation((progress) =>
            {
                sender.SetValue(TargetProperty, AnimationHelper.GetCornerRadiusValue(From, To, progress));
            }),
            length: Duration,
            easing: Easing);
        }
    }
}