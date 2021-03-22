using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Tizen
{
	class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			LoadApplication(new App());
		}

		static void Main(string[] args)
		{
			var app = new Program();
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			var option = new InitializationOptions(app)
			{
				UseDeviceIndependentPixel = true,
				UseSkiaSharp = true
			};
			global::Xamarin.Forms.Forms.Init(option);
			app.Run(args);
		}
	}
}