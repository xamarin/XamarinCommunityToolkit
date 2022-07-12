using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.PancakeView.Platforms.WPF;
using Xamarin.Forms.Platform.WPF;

namespace Xamarin.CommunityToolkit.Sample.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : FormsApplicationPage
	{
		public MainWindow()
		{
			InitializeComponent();
			Forms.Forms.Init();
			PancakeViewRenderer.Init();
			LoadApplication(new Xamarin.CommunityToolkit.Sample.App());
		}
	}
}