using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Triggers
{
    public abstract class AnimationBaseTrigger<T> : TriggerAction<VisualElement>
    {
        public T From { get; set; } = default;

        public T To { get; set; } = default;

        public uint Duration { get; set; } = 1000;

        public int Delay { get; set; } = 0;

        public Easing Easing { get; set; } = Easing.Linear;

        public BindableProperty TargetProperty { get; set; } = default;

        protected override void Invoke(VisualElement sender) =>
            throw new NotImplementedException("Please Implement Invoke() in derived-class");

        protected void SetDefaultFrom(T property) =>
            From = From.Equals(default(T)) ? property : From;
	}
}