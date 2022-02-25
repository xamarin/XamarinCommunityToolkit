#nullable enable
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;

namespace Xamarin.CommunityToolkit.Sample.Droid
{
	[Activity(Label = "XamarinCommunityToolkitSample", Icon = "@mipmap/icon", Theme = "@style/SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class SplashActivity : AppCompatActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			var intent = new Intent(this, typeof(MainActivity));
			StartActivity(intent);
			Finish();
		}
	}
}
