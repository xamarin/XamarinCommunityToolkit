using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Triggers
{
    public class AnimateInt : AnimationBaseTrigger<double>
    {
        protected override async void Invoke(VisualElement sender)
        {
            if (TargetProperty == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            if (Delay > 0)
                await Task.Delay(Delay);

            SetDefaultFrom((double)sender.GetValue(TargetProperty));

            sender.Animate($"AnimateInt{TargetProperty.PropertyName}", new Animation((progress) =>
            {
                sender.SetValue(TargetProperty, AnimationHelper.GetIntValue((int)From, (int)To, progress));
            }),
            length: Duration,
            easing: Easing);
        }
    }
}