using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TouchEffectPage
	{
		public TouchEffectPage()
		{
			Command = new Command(() =>
			{
				Count++;
				OnPropertyChanged(nameof(Count));
			});
			LongPressCommand = new Command(() => Application.Current.MainPage.DisplayAlert("LongPressCommand", "LongPressCommand was executed.", "OK"));
			InitializeComponent();

		}

		public ICommand Command { get; }

		public ICommand LongPressCommand { get; }

		public int Count { get; private set; }
	}
}
