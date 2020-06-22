using System;
using System.Linq;
using Xamarin.Forms;
using XamarinCommunityToolkit.Helpers;

namespace XamarinCommunityToolkit.Effects
{
    public static class SafeAreaPaddingEffect
    {
        public static readonly BindableProperty SafeAreaPaddingProperty =
            BindableProperty.CreateAttached("SafeAreaPadding", typeof(SafeAreaPadding), typeof(SafeAreaPaddingEffect), default(SafeAreaPadding),
                propertyChanged: OnSafeAreaPaddingChanged);

        public static SafeAreaPadding GetSafeAreaPadding(BindableObject view)
            => (SafeAreaPadding)view.GetValue(SafeAreaPaddingProperty);

        public static void SetSafeAreaPadding(BindableObject view, SafeAreaPadding value)
            => view.SetValue(SafeAreaPaddingProperty, value);

        static void OnSafeAreaPaddingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view))
                return;

            var oldEffect = view.Effects.FirstOrDefault(e => e is SafeAreaPaddingEffectRouter);

            if (oldEffect != null)
                view.Effects.Remove(oldEffect);

            if (((SafeAreaPadding)newValue).IsEmpty)
                return;

            view.Effects.Add(new SafeAreaPaddingEffectRouter());
        }
    }
}