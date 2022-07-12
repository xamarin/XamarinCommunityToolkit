using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views.Tabs
{
	sealed class LazyTestViewModel : ObservableObject
	{
		public static LazyTestViewModel Current { get; } = new LazyTestViewModel();

		string title = string.Empty;

		public string Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}

		bool loaded;

		public bool Loaded
		{
			get => loaded;
			set => SetProperty(ref loaded, value);
		}

		public LazyTestViewModel() => Title = "Lazy Tab Sample";
	}
}