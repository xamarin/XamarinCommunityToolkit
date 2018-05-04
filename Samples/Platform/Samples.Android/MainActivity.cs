using Android.App;
using Android.OS;
using Xamarin.Toolkit.Droid.SampleApp.SamplePages;

namespace Sample.Android
{
    [Activity(Label = "Xamarin.Toolkit.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MarkdownTextViewSamplePage));
        }
    }
}
