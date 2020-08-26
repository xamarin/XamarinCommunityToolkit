using AppKit;

namespace Xamarin.CommunityToolkit.Sample.macOS
{
	static class MainClass
	{
		static void Main(string[] args)
		{
			NSApplication.Init();
			NSApplication.SharedApplication.Delegate = new AppDelegate();
			NSApplication.Main(args);
		}
	}
}
