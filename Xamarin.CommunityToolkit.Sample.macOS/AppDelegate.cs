using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Xamarin.CommunityToolkit.Sample.macOS
{
	[Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
		NSWindow _window;

		public override NSWindow MainWindow
		{
			get { return _window; }
		}

		public AppDelegate()
		{
			var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

			var rect = new CoreGraphics.CGRect(200, 200, 800, 600);
			_window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
			_window.Title = "Xamarin Community Toolkit for macOS";
		}

		public override void DidFinishLaunching(NSNotification notification)
		{
			// Insert code here to initialize your application
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new Sample.App());
			base.DidFinishLaunching(notification);
		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}