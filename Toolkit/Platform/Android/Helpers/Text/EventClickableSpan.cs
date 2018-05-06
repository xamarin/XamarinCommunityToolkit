using System;
using Android.Text.Style;
using Android.Views;

namespace Xamarin.Toolkit.Droid.Helpers.Text
{
    public class EventClickableSpan : ClickableSpan
    {
        public override void OnClick(View widget)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        public string Url { get; set; }

        public event EventHandler Clicked;
    }
}
