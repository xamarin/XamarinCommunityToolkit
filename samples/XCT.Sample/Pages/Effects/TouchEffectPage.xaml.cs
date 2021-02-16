using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class TouchEffectPage
	{
		public TouchEffectPage()
		{
			Command = new Command(() =>
			{
				TouchCount++;
				OnPropertyChanged(nameof(TouchCount));
			});
			LongPressCommand = new Command(() =>
			{
				LongPressCount++;
				OnPropertyChanged(nameof(LongPressCount));
			});
			InitializeComponent();

		}

		public ICommand Command { get; }

		public ICommand LongPressCommand { get; }

		public int TouchCount { get; private set; }

		public int LongPressCount { get; private set; }
	}
}