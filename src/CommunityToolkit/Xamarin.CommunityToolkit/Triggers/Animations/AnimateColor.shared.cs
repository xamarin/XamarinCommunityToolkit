using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Triggers
{
    public class AnimateColor : AnimationBaseTrigger<Color>
    {
        protected override async void Invoke(VisualElement sender)
        {
            if (TargetProperty == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            if (Delay > 0)
                await Task.Delay(Delay);

            SetDefaultFrom((Color)sender.GetValue(TargetProperty));

            sender.Animate($"AnimateColor{TargetProperty.PropertyName}", new Animation((progress) =>
            {
                sender.SetValue(TargetProperty, AnimationHelper.GetColorValue(From, To, progress));
            }),
            length: Duration,
            easing: Easing);
        }
    }
}