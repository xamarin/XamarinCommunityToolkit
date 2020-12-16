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
			global::Xamarin.Forms.Forms.Init(app, true);
			app.Run(args);
		}
	}
}