using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views.Tabs
{
	sealed class NormalTestViewModel : ObservableObject
	{
		public static NormalTestViewModel Current { get; } = new NormalTestViewModel();

		string loadedViews = string.Empty;

		public string LoadedViews
		{
			get => loadedViews;
			set => SetProperty(ref loadedViews, value);
		}
	}
}