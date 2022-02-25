using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class TouchEffectPage
	{
		public TouchEffectPage()
		{
			Command = CommandFactory.Create(() =>
			{
				TouchCount++;
				OnPropertyChanged(nameof(TouchCount));
			});

			LongPressCommand = CommandFactory.Create(() =>
			{
				LongPressCount++;
				OnPropertyChanged(nameof(LongPressCount));
			});

			ParentCommand = CommandFactory.Create(() => DisplayAlert("Parent clicked", null, "Ok"));

			ChildCommand = CommandFactory.Create(() => DisplayAlert("Child clicked", null, "Ok"));

			InitializeComponent();
		}

		public ICommand Command { get; }

		public ICommand LongPressCommand { get; }

		public ICommand ParentCommand { get; }

		public ICommand ChildCommand { get; }

		public int TouchCount { get; private set; }

		public int LongPressCount { get; private set; }

		bool nativeAnimationBorderless;

		public bool NativeAnimationBorderless
		{
			get => nativeAnimationBorderless;
			set
			{
				nativeAnimationBorderless = value;
				OnPropertyChanged();
			}
		}
	}
}