using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.TestCases;
using Xamarin.CommunityToolkit.Sample.Pages.TestCases.Popups;
using Xamarin.CommunityToolkit.Sample.Pages.Views;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases
{
	public class TestCasesGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(GH_BUG_1761),
				"Popup, GitHub #1761",
				"Popup, GitHub #1761"),

			new SectionModel(
				typeof(TouchEffectButtonPage),
				"TouchEffect + Button",
				"TouchEffect must automatically invoke button's command execution."),

			new SectionModel(
				typeof(TouchEffectCollectionViewPage),
				"TouchEffect + CollectionView",
				"Using TouchEffect's LongPress should allow still use items selection."),

			new SectionModel(
				typeof(MediaElementSourcePage),
				"MediaElement with Source as string",
				"MediaElement should reproduce the video."),

			new SectionModel(
				typeof(TabViewBindingPage),
				"TabView BindingContext",
				"TabView with BindingContext into MainPage and other BindingContext into TabViewItem."),

			new SectionModel(
				typeof(PopupModalPage),
				"Popup into modal",
				"Using Popup into modal page show the popup"),

			new SectionModel(
				typeof(LinkerCameraViewPage),
				"Linker for CameraView",
				"Make sure that Linker is keeping the MediaCaptured and MediaCaptureFailed events if they are used."),

			new SectionModel(
				typeof(SnackBarActionExceptionPage),
				"SnackBar Action Exception",
				"Exception in SnackBar's action doesn't crash the app."),
			
			new SectionModel(
				typeof(Issue1883Page),
				"SnackBar iOS issue GitHub #1883",
				"Snackbar with 1 action button"),

			new SectionModel(
				typeof(DrawingViewInExpanderPage),
				"DrawingView in expander",
				"DrawingView in Expander Page")
		};
	}
}