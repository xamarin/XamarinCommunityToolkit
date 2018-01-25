using Android.App;
using Android.Content.PM;
using Android.OS;
using XamarinCommunityToolkit.Animations;
using System.Collections.Generic;
using System.Reflection;

namespace XamarinCommunityToolkit.Samples.Droid
{
    [Activity(Label = "XamarinCommunityToolkit.Samples", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            var assembliesToInclude =
                typeof(AnimationBase).GetTypeInfo().Assembly;
            
            global::Xamarin.Forms.Forms.Init(this, bundle, assembliesToInclude);
            LoadApplication(new App());
        }
    }
}

