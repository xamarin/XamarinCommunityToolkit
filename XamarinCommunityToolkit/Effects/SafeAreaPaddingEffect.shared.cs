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

            var hasSafeArea = !((SafeAreaPadding)newValue).IsEmpty;

            if (hasSafeArea)
            {
                view.Effects.Add(new SafeAreaPaddingEffectRouter());
                return;
            }

            var toRemove = view.Effects.FirstOrDefault(e => e is SafeAreaPaddingEffectRouter);

            if (toRemove != null)
                view.Effects.Remove(toRemove);
        }
    }
}